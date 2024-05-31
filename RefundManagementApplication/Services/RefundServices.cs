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
        IRepository<int, Refund> _Repo;
        IRepository<int, Member> _memberRepo;
        IRepository<int, Product> _productRepo;

        #region Constructor
        public RefundServices(IRepository<int, Refund> repo, IRepository<int, Order> orderRepo, IRepository<int, Member> memberRepo, IRepository<int, Product> productRepo) : base(repo)
        {
            _orderRepo = orderRepo;
            _Repo = repo;
            _memberRepo = memberRepo;
            _productRepo = productRepo;
        }
        #endregion

        #region Create Refund If only applicable
        /// <summary>
        /// Create Refund Only who are all applicable
        /// Returnable time period -> Free subscription
        /// ReturnableForPrime time period -> Prime Subscription
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="Reason"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="UnableToCreateException"></exception>
        public async Task<Refund> CreateRefund(int OrderId, string Reason)
        {
            var order = await _orderRepo.Get(OrderId);// To Update Based on result
            if (order == null) throw new NotFoundException("Order");
            var product = await _productRepo.Get(order.ProductId);// To find Returnable and ReturnableForPrime
            if (product == null) throw new NotFoundException("Product");
            var member = await _memberRepo.Get(order.MemberID);//To know the member has prime or free 
            if (member == null) throw new NotFoundException("Member");
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
                var result = await _Repo.Add(refund);
                order.RefundId = result.RefundId;
                await _orderRepo.Update(order);
                return result;
            }
            throw new UnableToCreateException();
        }
        #endregion

        #region Validate Return Duration (private class)
        private async Task<bool> CheckValidReturnDuration(DateTime created,int DaysValid)
        {
            if (DaysValid == 0)
                throw new ObjectIsNotReturnableException();
            else
            created = created.AddDays(DaysValid);
            if (DateTime.Now <= created)
                return true;
            throw new ReturnableDateExpired();
        }
        #endregion

    }
}
