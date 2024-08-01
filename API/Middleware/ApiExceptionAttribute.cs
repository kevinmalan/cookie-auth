using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Exceptions;
using System.Net;


namespace API.Middleware
{
    public class ApiExceptionAttribute : ExceptionFilterAttribute
    {
        private ExceptionContext? _context;

        public override void OnException(ExceptionContext context)
        {
            _context = context;
            // TODO: Log Exception
            switch (context.Exception)
            {
                case BadRequestException br:
                    _context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _context.Result = new JsonResult(new
                    {
                        br.Message,
                        br.CustomData
                    });
                    _context.ExceptionHandled = true;
                    break;

                case NotFoundException nf:
                    _context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    _context.Result = new JsonResult(new
                    {
                        nf.Message,
                        nf.CustomData
                    });
                    _context.ExceptionHandled = true;
                    break;

                case ForbiddenException fe:
                    _context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    _context.Result = new JsonResult(new
                    {
                        fe.Message,
                        fe.CustomData
                    });
                    _context.ExceptionHandled = true;
                    break;

                default:
                    _context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    _context.Result = new ForbidResult();
                    _context.ExceptionHandled = true;
                    break;
            }
        }
    }
}
