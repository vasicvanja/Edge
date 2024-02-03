using Edge.Shared.DataContracts.Enums;
using System.Collections;

namespace Edge.Shared.DataContracts.Responses
{
    public static class Conversion<T>
    {
        public static WebDataResponse<T> ReturnResponse(DataResponse<T> response)
        {
            var resp = new WebDataResponse<T>
            {
                Data = response.Data,
                ErrorMessage = response.ErrorMessage,
                ResponseCode = EBooleanResponseCode.False,
                Succeeded = false
            };


            if (IsList(response.Data))
            {
                if (response.ResponseCode == EDataResponseCode.Success ||
                    response.ResponseCode == EDataResponseCode.NoDataFound)
                {
                    resp.ResponseCode = EBooleanResponseCode.True;
                    resp.Succeeded = true;
                }

                return resp;
            }

            if (response.ResponseCode == EDataResponseCode.Success)
            {
                resp.ResponseCode = EBooleanResponseCode.True;
                resp.Succeeded = true;
            }


            return resp;
        }

        private static bool IsList(object o)
        {
            if (o == null) return false;

            return o is IList && o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }
    }
}
