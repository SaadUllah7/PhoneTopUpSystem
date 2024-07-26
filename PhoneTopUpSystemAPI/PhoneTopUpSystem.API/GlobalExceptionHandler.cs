using System.Data;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class GlobalExceptionHandler : ExceptionFilterAttribute 
{
    public override void OnException(ExceptionContext context)
    {
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        if(context.Exception is KeyNotFoundException){
            statusCode = HttpStatusCode.NotFound;
        }

        if(context.Exception is BadHttpRequestException){
            statusCode = HttpStatusCode.BadRequest;
        }

          if(context.Exception is DuplicateNameException){
            statusCode = HttpStatusCode.NotAcceptable;
        }


        context.HttpContext.Response.StatusCode = (int)statusCode;
        context.Result = new JsonResult(new {
            StatusCode = statusCode,
            Message = context.Exception.Message
        });
        
        base.OnException(context);

    }
}