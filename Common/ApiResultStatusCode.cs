using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common
{
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
        ListEmpty = 4,
        [Display(Name = "خطای پردازشی")]
        LogicError = 5,
        [Display(Name = "خطای احراز هویت")]
        UnAuthorized = 6
    }
}
