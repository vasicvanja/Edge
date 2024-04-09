using Edge.Dtos;
using Edge.Services.Interfaces;
using Edge.Shared.DataContracts.Enums;
using Edge.Shared.DataContracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Edge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        #region Declarations

        private readonly IStripeService _stripeService;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="stripeService"></param>
        public StripeController(IStripeService stripeService)
        {
            _stripeService = stripeService;
        }

        #endregion

        #region Methods

        [HttpPost]
        [Route("createCheckoutSession")]
        public async Task<IActionResult> CreateCheckOutSession(List<ArtworkDto> artworks)
        {
            try
            {
                var result = await _stripeService.CreateCheckOutSession(artworks);
                return Ok(Conversion<PaymentDetailsDto>.ReturnResponse(result));
            }
            catch (Exception ex)
            {
                var errRet = new DataResponse<PaymentDetailsDto>
                {
                    Data = null,
                    ResponseCode = EDataResponseCode.GenericError,
                    Succeeded = false,
                    ErrorMessage = ex.Message
                };
                return BadRequest(Conversion<PaymentDetailsDto>.ReturnResponse(errRet));
            }
        }

        #endregion
    }
}
