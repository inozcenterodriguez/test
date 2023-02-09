using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Service.Dtos;
using System.Net;

namespace Sat.Recruitment.Api.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (context == null)
            {
                return NotFound();
            }

            var businessException = context.Error as BusinessException;

            var _instance = "";
            var _title = "Internal Error.";
            var _detail = "Please, Consult Log";

            var errorResponse = ProcessResponse(_instance, _title, _detail, (int)HttpStatusCode.InternalServerError);



            if (businessException !=null)
            {

                errorResponse = ProcessResponse(null, context.Error.Message, null, (int)HttpStatusCode.Conflict);

            }

            return errorResponse;
        }// end error




        [Route("/error-development")]
        public IActionResult ErrorDev()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (context == null)
            {
                return NotFound();
            }

            var businessException = context.Error as BusinessException;


            var _instance = context.Path;
            var _title = context.Error.Message;
            var _detail = context.Error.StackTrace;

            var errorResponse = ProcessResponse(_instance, _title, _detail, (int)HttpStatusCode.InternalServerError);




            if (businessException != null)
            {
                errorResponse = ProcessResponse(context.Path, context.Error.Message, context.Error.StackTrace, (int)HttpStatusCode.Conflict);

            }

            return errorResponse;
        }// end error





        private ObjectResult ProcessResponse(string instance, string title, string? detail, int statusCode)
        {
            return Problem(
                         instance: instance,
                         title: title,
                         detail: detail,
                         statusCode: statusCode


                          );
        }
    }
}
