namespace Middleware.Models
{

    // we can create this class to send client standard shape (clean way) for
    // error response back to the client
    // instead of returning any random error message format, that we can get every 
    // error of the api

    // here we wrapped in this class, so that the client alwats get the same json stucture


    //json format{
    //"message:email is reauie
    //details=null
    public class ErrorResponse
    {
        public string Message { get; set; }
        public string? Details { get; set; }

        public ErrorResponse(string message, string? details)
        {
            Message = message;
            Details = details;
        }
    }
}
