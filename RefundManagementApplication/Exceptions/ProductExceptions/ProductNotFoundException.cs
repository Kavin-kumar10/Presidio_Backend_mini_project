namespace RefundManagementApplication.Exceptions.ProductExceptions
{
    public class ProductNotFoundException : Exception
    {
        string message;
        public ProductNotFoundException()
        {
            message = "Product not found";
        }

        public ProductNotFoundException(string? message) : base(message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}
