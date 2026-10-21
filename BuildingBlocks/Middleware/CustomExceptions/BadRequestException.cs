namespace Middleware.CustomExceptions
{

    // this is the custom exception that will thrown when the cliend invalid or incorrect data
    public class BadRequestException: Exception
    {

        public BadRequestException(string message) : base(message)
        {
        }
    }
}
