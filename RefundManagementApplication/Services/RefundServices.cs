using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Exceptions.RefundExceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Models.Enums;

namespace RefundManagementApplication.Services
{
    public class RefundServices : BaseServices<Refund>, IRefundServices
    {
        IRepository<int, Order> _orderRepo;
        IRepository<int, Refund> _refundRepo;
        IRepository<int, Member> _memberRepo;
        IRepository<int, Product> _productRepo;
        public RefundServices(IRepository<int, Refund> repo, IRepository<int, Order> orderRepo, IRepository<int, Member> memberRepo, IRepository<int, Product> productRepo) : base(repo)
        {
            _orderRepo = orderRepo;
            _refundRepo = repo;
            _memberRepo = memberRepo;
            _productRepo = productRepo;
        }


        public async Task<Refund> CreateRefund(int OrderId, string Reason)
        {
            Order order = await _orderRepo.Get(OrderId);// To Update Based on result
            Product product = await _productRepo.Get(order.ProductId);// To find Returnable and ReturnableForPrime
            Member member = await _memberRepo.Get(order.MemberID);//To know the member has prime or free 
            bool valid = true;
            if (member.Membership == Plan.Prime)
            {
                valid = await CheckValidReturnDuration(order.CreatedDate, product.ReturnableForPrime);
            }
            else
            {
                valid = await CheckValidReturnDuration(order.CreatedDate, product.Returnable);
            }
            if (valid)
            {
                Refund refund = new Refund()
                {
                    OrderId = order.OrderId,
                    CreatedBy = order.MemberID,
                    CreatedDate = DateTime.Now,
                    Reason = Reason,
                    RefundAmount = order.TotalPrice,
                    RefundStatus = RefundStatuses.PENDING
                };
                var result = await base.Create(refund);
                //order.RefundId = result.RefundId;
                //await _orderRepo.Update(order);
                return result;
            }
            throw new UnableToCreateException();
        }

        public async Task<bool> CheckValidReturnDuration(DateTime created,int DaysValid)
        {
            if (DaysValid == 0)
                throw new ObjectIsNotReturnableException();
            else
            created = created.AddDays(DaysValid);
            if (DateTime.Now <= created)
                return true;
            throw new ReturnableDateExpired();
        }
    }
}
