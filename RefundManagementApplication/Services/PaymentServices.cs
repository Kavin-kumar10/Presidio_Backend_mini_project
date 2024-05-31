using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Models.Enums;

namespace RefundManagementApplication.Services
{
    public class PaymentServices : IPaymentServices
    {
        IRepository<int, Payment> _repo; // To create new Payment 
        IRepository<int, Refund> _refundRepo; // To get Refund TotalPrice
        IRepository<int, Order> _orderRepo; // To verify current Order Status is Accepted
        IOrderServices _orderServices; //For UpdateOrderStatusMethod

        #region Constructor
        public PaymentServices(IRepository<int, Payment> repo, IRepository<int, Refund> refundRepo,IRepository<int,Order> orderRepo,IOrderServices orderServices) 
        {
            _repo = repo;
            _refundRepo = refundRepo;
            _orderRepo = orderRepo;
            _orderServices = orderServices; 
        }
        #endregion

        #region Get Payment Details by Id
        /// <summary>
        /// Get my Payment Details for User
        /// </summary>
        /// <param name="PaymentId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<Payment> GetMyPayment(int PaymentId)
        {
            var result = await _repo.Get(PaymentId);
            if(result == null)
            {
                throw new NotFoundException("Payment");
            }
            return result;
        }
        #endregion

        #region Create Payment To Refund
        /// <summary>
        /// Create Payment only for those already accepted by the Collector
        /// </summary>
        /// <param name="AdminId"></param>
        /// <param name="refundId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="ForbiddenEntryException"></exception>
        public async Task<Payment> CreatePaymentToRefund(int AdminId,int refundId)
        {
            //Get Refund
            var refund = await _refundRepo.Get(refundId);
            if (refund == null)
                throw new NotFoundException("Refund");

            //Get Order
            var order = await _orderRepo.Get(refund.OrderId);
            if (order == null)
                throw new NotFoundException("Order");

            //Check for Status
            if(order.OrderStatus != OrderStatuses.Refund_Accepted)
            {
                throw new ForbiddenEntryException();
            }

            //Payment
            Payment payment = new Payment()
            {
                RefundId = refundId,
                TotalPayment = refund.RefundAmount,
                Type = "NEFT",
                UserId = order.MemberID,
                PaymentDate = DateTime.Now
            };
            var result = await _repo.Add(payment);

            //UpdateStatus
            await _orderServices.UpdateOrderStatus(OrderStatuses.Refund_Completed, order.OrderId);
            refund.ClosedBy = AdminId;
            refund.PaymentId = result.PaymentId;
            await _refundRepo.Update(refund); 
            return result;
        }
        #endregion

    }
}
