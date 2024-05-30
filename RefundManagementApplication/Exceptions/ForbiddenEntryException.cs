using System.Runtime.Serialization;

namespace RefundManagementApplication.Exceptions
{
    [Serializable]
    public class ForbiddenEntryException : Exception
    {
        string message;
        public ForbiddenEntryException()
        {
            message = "The requested operation is forbidden";
        }

        public ForbiddenEntryException(string? message) : base(message)
        {
            this.message = message;
        }
        public override string Message => message;
    }
}