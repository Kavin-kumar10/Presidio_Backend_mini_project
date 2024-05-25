namespace RefundManagementApplication.Exceptions
{
    public class UnableToCreateException : Exception
    {
        string message;
        public UnableToCreateException()
        {
            message = "Unable to create the requirement";
        }

        public UnableToCreateException(string? message) : base(message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}
