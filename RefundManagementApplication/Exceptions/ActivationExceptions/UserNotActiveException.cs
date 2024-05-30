using System.Runtime.Serialization;

namespace RefundManagementApplication.Exceptions.ActivationExceptions
{
    [Serializable]
    public class UserNotActiveException : Exception
    {
        string message;
        public UserNotActiveException()
        {
            message = "User Not Active";
        }

        public UserNotActiveException(string? message) : base(message)
        {
            this.message = message;
        }

        public override string Message => message;

    }
}