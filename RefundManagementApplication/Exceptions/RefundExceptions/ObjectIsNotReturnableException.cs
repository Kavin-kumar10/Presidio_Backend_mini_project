using System.Runtime.Serialization;

namespace RefundManagementApplication.Exceptions.RefundExceptions
{
    [Serializable]
    public class ObjectIsNotReturnableException : Exception
    {
        string message;
        public ObjectIsNotReturnableException()
        {
            message = "Refund not available for the current order";
        }

        public ObjectIsNotReturnableException(string? message) : base(message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}