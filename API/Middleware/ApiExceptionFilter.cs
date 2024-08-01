using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Exceptions;
using System.Net;


namespace API.Middleware
{
    public class ApiExceptionFilter(ILogger<ApiExceptionFilter> logger) : ExceptionFilterAttribute
    {
        private ExceptionContext? _context;

        public override void OnException(ExceptionContext context)
        {
            _context = context;

            switch (context.Exception)
            {
                case BadRequestException br:
                    logger.LogError(br, "An exception occured. Message: {Message}. CustomData: {CustomData}.", br.Message, br.CustomData);
                    _context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _context.Result = new JsonResult(new
                    {
                        br.Message,
                        br.CustomData
                    });
                    _context.ExceptionHandled = true;
                    break;

                case NotFoundException nf:
                    logger.LogError(nf, "An exception occured. Message: {Message}. CustomData: {CustomData}.", nf.Message, nf.CustomData);
                    _context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    _context.Result = new JsonResult(new
                    {
                        nf.Message,
                        nf.CustomData
                    });
                    _context.ExceptionHandled = true;
                    break;

                case ForbiddenException fe:
                    logger.LogError(fe, "An exception occured. Message: {Message}. CustomData: {CustomData}.", fe.Message, fe.CustomData);
                    _context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    _context.Result = new JsonResult(new
                    {
                        fe.Message,
                        fe.CustomData
                    });
                    _context.ExceptionHandled = true;
                    break;

                default: // Todo" create a custom InternalServerException and return a unique number that can be used to lookup in logs
                    logger.LogError(_context.Exception, "An exception occured. Message: {Message}.", _context.Exception.Message);
                    _context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    _context.Result = new ForbidResult();
                    _context.ExceptionHandled = true;
                    break;
            }
        }
    }
}
