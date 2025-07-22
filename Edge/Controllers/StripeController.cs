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

        #region CHECKOUT SESSION

        /// <summary>
        /// Creates a Stripe Checkout Session on a stripe-hosted page.
        /// </summary>
        /// <param name="artworks"></param>
        /// <returns></returns>
        /// 
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

        #region WEBHOOK

        /// <summary>
        /// Retrieve information whether a payment was successful or not.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("webhook")]
        public async Task<IActionResult> Webhook()
        {
            try
            {
                var result = await _stripeService.Webhook(HttpContext.Request.Body, Request.Headers["Stripe-Signature"]);
                return Ok(Conversion<bool>.ReturnResponse(result));
            }
            catch (Exception ex)
            {
                var errRet = new DataResponse<bool>
                {
                    Data = false,
                    ResponseCode = EDataResponseCode.GenericError,
                    Succeeded = false,
                    ErrorMessage = ex.Message
                };
                return BadRequest(Conversion<bool>.ReturnResponse(errRet));
            }
        }

        #endregion

        #region GET SESSION ARTWORKS

        [HttpGet]
        [Route("getSessionArtworks/{sessionId}")]
        public async Task<IActionResult> GetSessionArtworks(string sessionId)
        {
            try
            {
                var result = await _stripeService.GetSessionArtworks(sessionId);
                return Ok(Conversion<List<ArtworkDto>>.ReturnResponse(result));
            }
            catch (Exception ex)
            {
                var errRet = new DataResponse<List<ArtworkDto>>
                {
                    Data = null,
                    ResponseCode = EDataResponseCode.GenericError,
                    Succeeded = false,
                    ErrorMessage = ex.Message
                };
                return BadRequest(Conversion<List<ArtworkDto>>.ReturnResponse(errRet));
            }
        }

        #endregion
    }
}
