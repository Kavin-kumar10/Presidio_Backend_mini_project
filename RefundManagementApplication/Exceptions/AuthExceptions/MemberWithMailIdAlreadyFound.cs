using System.Runtime.Serialization;

namespace RefundManagementApplication.Exceptions.AuthExceptions
{
    [Serializable]
    public class MemberWithMailIdAlreadyFound : Exception
    {
        string message;
        public MemberWithMailIdAlreadyFound()
        {
            message = "Member with the Id Already Found";
        }

        public MemberWithMailIdAlreadyFound(string? message) : base(message)
        {
            this.message = message;
        }
        public override string Message => message;
    }
}