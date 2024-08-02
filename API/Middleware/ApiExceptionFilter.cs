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
            var uniqueReference = $"{Guid.NewGuid()}"[..6].ToUpper();

            switch (context.Exception)
            {
                case BadRequestException br:
                    LogError(br, br.CustomData);
                    _context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _context.Result = new JsonResult(new
                    {
                        Message = $"{br.Message}. Unique reference: {uniqueReference}",
                        br.CustomData
                    });
                    _context.ExceptionHandled = true;
                    break;

                case NotFoundException nf:
                    logger.LogError(nf, "An exception occured. Message: {Message}. CustomData: {CustomData}.", nf.Message, nf.CustomData);
                    LogError(nf, nf.CustomData);
                    _context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    _context.Result = new JsonResult(new
                    {
                        Message = $"{nf.Message}. Unique reference: {uniqueReference}",
                        nf.CustomData
                    });
                    _context.ExceptionHandled = true;
                    break;

                case ForbiddenException fe:
                    logger.LogError(fe, "An exception occured. Message: {Message}. CustomData: {CustomData}.", fe.Message, fe.CustomData);
                    LogError(fe, fe.CustomData);
                    _context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    _context.Result = new JsonResult(new
                    {
                        Message = $"{fe.Message}. Unique reference: {uniqueReference}",
                        fe.CustomData
                    });
                    _context.ExceptionHandled = true;
                    break;

                default:
                    var exception = _context.Exception as InternalServerException ?? new InternalServerException(_context.Exception.Message);
                    LogError(_context.Exception, exception.CustomData);
                    _context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _context.Result = new JsonResult(new
                    {
                        Message = $"An error occured. Please contact support. Unique reference: {uniqueReference}"
                    });
                    _context.ExceptionHandled = true;
                    break;
            }

            void LogError(Exception exception, object customData)
            {
                logger.LogError(exception, "An exception occured. Message: {Message}. CustomData: {CustomData}. UniqueReference: {UniqueReference}", exception.Message, customData, uniqueReference);
            }
        }
    }
}
