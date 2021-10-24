using System.Net;
using System.Net.Mime;
using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;

public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
{
    private readonly MediaTypeCollection contextTypes = new MediaTypeCollection { MediaTypeNames.Application.Json };

    public int Order { get; } = int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        switch (context.Exception)
        {
            case FluentValidation.ValidationException validationEx:
                {
                    context.Result = new ObjectResult(new { validationEx.Message, validationEx.Errors })
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        ContentTypes = contextTypes
                    };

                    context.ExceptionHandled = true;
                    break;
                }
            case MyApplicationException appEx:
                {

                    context.Result = new ObjectResult(new { appEx.Message, appEx.Payload })
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        ContentTypes = contextTypes
                    };

                    context.ExceptionHandled = true;
                    break;
                }
            default:
                break;
        }
    }
}