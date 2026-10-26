using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Middleware.CustomExceptions;
using Middleware.Models;
using System.Net;

namespace Middleware
{

    // here we can create this middlware we can request this after build inside program
    // first request piple
    // if caught any excpetion with the help of this exception middleware we will get

    public class ExceptionMiddleware
    {
        //Request delecate represents the next middleare in the pipeling

       // here it call pass the http request to next middleare

        private readonly RequestDelegate _next;


        //ILogger is used to record errors in the appplications logs
        // that will be useful for developer to investigate or to find the 
        //unexpected exception in the appiation

        private readonly ILogger<ExceptionMiddleware> _logger;


        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger
            )
        {
            _next = next;
            _logger = logger;

        }


        // here we can create invoke() that is called when request reach your pipeline
        //


        public async Task Invoke(HttpContext context)
        {
            try
            {
                //it is used to pass the request to next middleare
                await _next(context);
            }
            catch (Exception ex)
            {
                // if any exception occurs that will be handled by this excpeitno
                await HandleExceptionAsync(context, ex);
            }
        }




        //  it will handle the exception
        // this method help to find the exception types and then decides which

        // status code and message should be returned here

        //For example :
        //notfoundexcpetion:404
        //database conflict:409
        //unexpected error: 500
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {

            //here we can set default status code to 500
            // this will be called  or used when exception does nto
            // match the specific case in the switch statement


            var statusCode = HttpStatusCode.InternalServerError;

            // this variable stores the main error message
            // and it will returned to the api client


            string message;


            string? details = null;

            // here we are using switch statement that will check exceptions
            // then decides how to handlied it

            switch (exception)
            {

                //Case 1: REsource not found

                // this case will be run when the exception is  notfoundexception
                case NotFoundException notFoundException:


                    statusCode = HttpStatusCode.NotFound;
                    message = notFoundException.Message;

                    break;


                case BadRequestException badRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = badRequestException.Message;

                    break;

                case ArgumentException argumentException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = argumentException.Message;
                    break;


                case DbUpdateConcurrencyException dbUpdateConcurrencyEx:

                    statusCode = HttpStatusCode.Conflict;
                    message = " a conflict occured while saving changes inside the database";

                    break;

                case DbUpdateException dbUpdateEx:
                    statusCode = HttpStatusCode.Conflict;
                    message = "during the databse update error occured";
                    break;

                //case 6: unexcepecture exception
                //this case will hanle the exception that do not match any of specific
                //case

                default:


                    _logger.LogError(exception, " an unexpected error occurer {Message}", exception.Message);



                    statusCode = HttpStatusCode.InternalServerError;

                    message = " an unexpected error occred please try agina later";

                    break;



                    //Createn a standard error response object

                    var errorResponse = new ErrorResponse(message, details);

                    // we can pass the status code and error response to buildrequest();


                    // this method will the write final json reponse to the client.

                    await BuildResponse(context, statusCode, errorResponse);
            }

        }



        // this method will build and sent the http respone


        // so this method prepares the http resposne and it will sent back to the cline
        public static async Task BuildResponse(HttpContext context, HttpStatusCode statusCode, ErrorResponse errorResponse)
        {

            context.Response.StatusCode=(int)statusCode;


            context.Response.ContentType = "application/json";


            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
        }
    }
}
