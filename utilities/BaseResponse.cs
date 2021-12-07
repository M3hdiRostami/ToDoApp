using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoAPI.Uilities.Responses
{
    public class BaseResponse<T> where T:class

    {
        public T Data { get; set; }


        public bool Success { get; set; }
        public string Message { get; set; }


        public BaseResponse(T data=null)
        {
            Data = data;
            Success = (data==null) ?  false:true;
        }
        public BaseResponse(string message)
        {
            Success = false;
            Message = message;
        }
        public BaseResponse(string message,T data,bool suceess)
        {
            data = data;
            Success = suceess;
            Message = message;
        }


    }
}
