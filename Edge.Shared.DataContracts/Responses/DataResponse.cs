using Edge.Shared.DataContracts.Enums;

namespace Edge.Shared.DataContracts.Responses
{
    public class DataResponse<T> : BaseResponse<EDataResponseCode>
    {
        public T? Data { get; set; }
        public int Count { get; set; }
        public string ErrorMessage { get; set; }
        public bool Succeeded { get; set; }

    }
}
