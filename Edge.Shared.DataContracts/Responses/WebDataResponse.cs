using Edge.Shared.DataContracts.Enums;

namespace Edge.Shared.DataContracts.Responses
{
    public class WebDataResponse<T> : BaseResponse<EBooleanResponseCode>
    {
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public bool Succeeded { get; set; }
    }
}
