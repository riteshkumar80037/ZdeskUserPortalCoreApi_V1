using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ZdeskUserPortalApiCore.Common;
using ZdeskUserPortalApiCore.Constant;

namespace ZdeskUserPortalApiCore.Filter
{
    public class ValidateModelHelperAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                List<string> errors = new();
                foreach (var modelState in context.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new ObjectResult(
                    new ResponseMetaData<string>()
                    {
                        Status = System.Net.HttpStatusCode.BadRequest,
                        IsError = true,
                        ErrorDetails = CommonValidationConstants.ValidationFailed,
                        Message = $"[{string.Join(" , ", [.. errors])}]",
                        Result = null
                    }
                );
                base.OnActionExecuting(context);
            }
        }
    }
}
