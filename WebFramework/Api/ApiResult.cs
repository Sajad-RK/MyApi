﻿using Common.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace WebFramework.Api
{
    public class ApiResult
    {
        public bool IsSuccess { get; set; }
        public ApiResultStatusCode StatusCode { get; set; }
        public string Message { get; set; }

        public ApiResult(bool success, ApiResultStatusCode statusCode, string message = null)
        {
            IsSuccess = success;
            StatusCode = statusCode;
            Message = message ?? statusCode.ToDisplay();
        }

        #region Implicit Operators
        public static implicit operator ApiResult(OkResult result)
        {
            return new ApiResult(true, ApiResultStatusCode.Success);
        }

        public static implicit operator ApiResult(BadRequestResult result)
        {
            return new ApiResult(false, ApiResultStatusCode.BadRequest);
        }

        public static implicit operator ApiResult(BadRequestObjectResult result)
        {
            var message = result.Value.ToString();
            if (result.Value is SerializableError errors)
            {
                var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
                message = string.Join(" | ", errorMessages);
            }
            return new ApiResult(false, ApiResultStatusCode.BadRequest, message);
        }

        public static implicit operator ApiResult(ContentResult result)
        {
            return new ApiResult(true, ApiResultStatusCode.Success, result.Content);
        }

        public static implicit operator ApiResult(NotFoundResult result)
        {
            return new ApiResult(false, ApiResultStatusCode.NotFound);
        }
        #endregion
    }
    public class ApiResult<TData> : ApiResult where TData : class
    {
        public TData Data { get; set; }


        public ApiResult(bool success, ApiResultStatusCode statusCode, TData data, string message = null) 
            : base(success, statusCode, message)
        {
            Data = data;
        }

        #region Implicit Operators
        public static implicit operator ApiResult<TData>(TData data)
        {
            return new ApiResult<TData>(true, ApiResultStatusCode.Success, data);
        }

        public static implicit operator ApiResult<TData>(OkResult result)
        {
            return new ApiResult<TData>(true, ApiResultStatusCode.Success, null);
        }

        public static implicit operator ApiResult<TData>(OkObjectResult result)
        {
            return new ApiResult<TData>(true, ApiResultStatusCode.Success, (TData)result.Value);
        }

        public static implicit operator ApiResult<TData>(BadRequestResult result)
        {
            return new ApiResult<TData>(false, ApiResultStatusCode.BadRequest, null);
        }

        public static implicit operator ApiResult<TData>(BadRequestObjectResult result)
        {
            var message = result.Value.ToString();
            if (result.Value is SerializableError errors)
            {
                var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
                message = string.Join(" | ", errorMessages);
            }
            return new ApiResult<TData>(false, ApiResultStatusCode.BadRequest, null, message);
        }

        public static implicit operator ApiResult<TData>(ContentResult result)
        {
            return new ApiResult<TData>(true, ApiResultStatusCode.Success, null, result.Content);
        }

        public static implicit operator ApiResult<TData>(NotFoundResult result)
        {
            return new ApiResult<TData>(false, ApiResultStatusCode.NotFound, null);
        }

        public static implicit operator ApiResult<TData>(NotFoundObjectResult result)
        {
            return new ApiResult<TData>(false, ApiResultStatusCode.NotFound, (TData)result.Value);
        }
        #endregion
        //public static implicit operator ApiResult<TData>(TData value)
        //{
        //    return new ApiResult<TData>()
        //    {
        //        IsSuccess = true,
        //        StatusCode = ApiResultStatusCode.Success,
        //        Message = "Done",
        //        Data = value
        //    };
        //}
        //public static implicit operator ApiResult<TData>(NotFoundResult value)
        //{
        //    return new ApiResult<TData>()
        //    {
        //        IsSuccess = true,
        //        StatusCode = ApiResultStatusCode.Success,
        //        Message = "Done",
        //        Data = value
        //    };
        //}
    }
    public enum ApiResultStatusCode
    {
        [Display(Name = "عملیات موفق")]
        Success = 0,
        [Display(Name = "خطایی در سرور رخ داده")]
        ServerError = 1,
        [Display(Name = "پارامترهای ارسالی معتبر نیستند")]
        BadRequest = 2,
        [Display(Name = "یاف نشد")]
        NotFound = 3,
        [Display(Name = "لیست خالی است")]
        ListEmpty = 4
    }
}