using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAPI.Common.ConstantHelper;

namespace DataAPI.Common
{
    public class ResponseModel
    {
        public APIResponseCode Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public ResponseModel() 
        {
            Code = APIResponseCode.CODE_SUCCESS;
            Message = string.Empty;
            Data = null;
        }
    }
}
