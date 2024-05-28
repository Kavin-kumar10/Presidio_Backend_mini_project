using System.Runtime.Serialization;

namespace RefundManagementApplication.Exceptions.RefundExceptions
{
    [Serializable]
    public class ReturnableDateExpired : Exception
    {
        string message;
        public ReturnableDateExpired()
        {
            message = "The Returnable Date is Expired, Unable to proceed further.";
        }

        public ReturnableDateExpired(string? message) : base(message)
        {
            this.message = message;
        }

    }
}