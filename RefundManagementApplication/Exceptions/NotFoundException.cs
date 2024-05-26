namespace RefundManagementApplication.Exceptions
{
    public class NotFoundException : Exception
    {
        string message;
        public NotFoundException()
        {
            message = "Not Found";
        }

        public NotFoundException(string? message) : base(message)
        {
            this.message = message+ " Not Found";
        }

        public override string Message => message;
    }
}
