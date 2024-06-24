using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Models.DTOs.RequestDTO.OrderReqDTOs;
using RefundManagementApplication.Models.Enums;
using System.Runtime.CompilerServices;

namespace RefundManagementApplication.Services
{
    public class OrderServices : BaseServices<Order>, IOrderServices
    {
        IRepository<int, Order> _repo;  
        IRepository<int,Product> _productRepository;

        #region Constructor
        public OrderServices(IRepository<int, Order> repo, IRepository<int, Product> productRepository) : base(repo)
        {
            _repo = repo;
            _productRepository = productRepository;
        }
        #endregion

        #region Create New Order from OrderRequestDTO
        /// <summary>
        /// Create Order with orderRequestDTOs
        /// </summary>
        /// <param name="orderRequestDTO"></param>
        /// <returns></returns>
        public async Task<Order> CreateOrder(OrderRequestDTO orderRequestDTO)
        {
            var product = await _productRepository.Get(orderRequestDTO.ProductId);
            if (product == null) throw new NotFoundException("Product");
            product.Count--;
            Order order = new Order()
            {
                MemberID = orderRequestDTO.MemberID,
                ProductId = orderRequestDTO.ProductId,
                TotalPrice = product.Curr_price,
            };
            var result = await Create(order);
            await _productRepository.Update(product);
            return result;
        }
        #endregion

        #region [Collector only] Get All Refund Decision Pending Orders
        /// <summary>
        /// Get all the Decision pending ( Order not returned yet ) -> Only Collector can see
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Order>> GetAllRefundDecisionPendingOrders()
        {
            var orders = await _repo.Get();
            orders = orders.Where(o=>o.OrderStatus == OrderStatuses.Refund_Initiated).ToList();
            return orders;
        }
        #endregion

        #region [Admin only] Get All Refund Decision Accepted Orders to proceed further
        /// <summary>
        /// Get all the Decision Accepted ( Order returned and Accepted by Collector ) -> Only Admin can see
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Order>> GetAllRefundDecisionAcceptedOrders()
        {
            var orders = await _repo.Get();
            orders = orders.Where(o => o.OrderStatus == OrderStatuses.Refund_Accepted).ToList();
            return orders;
        }
        #endregion

        #region Update Order Status Based on Refund Process
        /// <summary>
        /// Update Order Status to Accepted || Rejected || Initiated for Refund
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<Order> UpdateOrderStatus(OrderStatuses Status, int OrderId)
        {
            var reqOrder = await _repo.Get(OrderId);
            if (reqOrder != null) {
                reqOrder.OrderStatus = Status;
                return await _repo.Update(reqOrder);
            }
            throw new NotFoundException("Order");
        }
        #endregion

        #region Get Orders by Member Id
        public async Task<IEnumerable<Order>> GetOrdersByMemberId(int memberId)
        {
            var orders = await _repo.Get();
            orders = orders.Where(o => o.MemberID == memberId).ToList();
            return orders;
        }
        #endregion

    }
}
