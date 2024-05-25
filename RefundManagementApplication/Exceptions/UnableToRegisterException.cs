using System.Runtime.Serialization;

namespace RefundManagementApplication.Exceptions
{
    [Serializable]
    internal class UnableToRegisterException : Exception
    {
        string message;
        public UnableToRegisterException()
        {
            message = "Unable to Register right now";
        }

        public UnableToRegisterException(string? message) : base(message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}