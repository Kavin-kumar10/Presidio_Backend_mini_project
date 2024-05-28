﻿using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Models.Enums;

namespace RefundManagementApplication.Services
{
    public class PaymentServices : BaseServices<Payment>,IPaymentServices
    {
        IRepository<int, Payment> _repo; // To create new Payment 
        IRepository<int, Refund> _refundRepo; // To get Refund TotalPrice
        IRepository<int, Order> _orderRepo; // To verify current Order Status is Accepted
        IOrderServices _orderServices; //For UpdateOrderStatusMethod
        public PaymentServices(IRepository<int, Payment> repo, IRepository<int, Refund> refundRepo,IRepository<int,Order> orderRepo,IOrderServices orderServices) : base(repo)
        {
            _repo = repo;
            _refundRepo = refundRepo;
            _orderRepo = orderRepo;
            _orderServices = orderServices; 
        }
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
            await _refundRepo.Update(refund); 
            return result;
        }
    }
}