using System.Runtime.Serialization;

namespace RefundManagementApplication.Exceptions
{
    [Serializable]
    internal class UserNotFoundException : Exception
    {
        string message;
        public UserNotFoundException()
        {
            message = "User not Found";
        }

        public UserNotFoundException(string? message) : base(message)
        {
            this.message = message;
        }

        public override string Message => message;

    }
}