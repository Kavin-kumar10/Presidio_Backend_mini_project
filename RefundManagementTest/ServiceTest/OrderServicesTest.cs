using Microsoft.EntityFrameworkCore;
using RefundManagementApplication.Context;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Models.Enums;
using RefundManagementApplication.Repositories;
using RefundManagementApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefundManagementTest.ServiceTest
{
    public class OrderServicesTest
    {
        RefundManagementContext context;
        private IRepository<int,Order> _repo;
        private IServices<int,Order> _services;
        private IOrderServices _OrderService;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                            .UseInMemoryDatabase("dummyDB");
            context = new RefundManagementContext(optionsBuilder.Options);

            _repo = new OrderRepository(context);
            _services = new OrderServices(_repo);

            Order order = new Order()
            {
                MemberID = 101,
                CreatedDate = DateTime.Now,
                OrderStatus = OrderStatuses.Ordered,
                ProductId = 101,
                TotalPrice = 1000,
            };

            await _repo.Add(order);
        }

        [Test]
        public async Task CreateOrderPassTest()
        {

            Order order = new Order() {
                MemberID = 101,
                CreatedDate = DateTime.Now,
                OrderStatus = OrderStatuses.Ordered,
                ProductId = 101,
                TotalPrice = 1000,
            };
            var result = await _services.Create(order);

            Assert.AreEqual(result.OrderId, 2);
        }

        [Test]
        public async Task CreateOrderFailTest()
        {
            try
            {
                Order order = new Order()
                {
                    MemberID = 101,
                    CreatedDate = DateTime.Now,
                    OrderStatus = OrderStatuses.Ordered,
                    ProductId = 101,
                    TotalPrice = 1000,
                };
                var result = await _services.Create(order);
            }
            catch(Exception ex)
            {
                Assert.AreEqual(ex.Message, "Unable to create the requirement");
            }
        }

        [Test]
        public async Task GetAllOrderPassTest()
        {
            //Action
            var result = _services.GetAll();

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetByIdPassTest()
        {
            //Action
            var result = _services.GetById(1);

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetByIdFailTest()
        {
            try
            {
                var result = _services.GetById(5);
            }
            catch(Exception ex)
            {
                Assert.AreEqual(ex.Message, "Not Found");
            }            
        }

        [Test]
        public async Task UpdatePassTest()
        {
            try
            {
                var req = await _repo.Get(1);
                req.TotalPrice = 700;
                var result = await _services.Update(req);
                Assert.AreEqual(result.TotalPrice, 700);
            }
            catch(Exception ex)
            {

            }
        }

        [Test]
        public async Task UpdateFailTest()
        {
            try
            {
                var req = await _services.GetById(5);
                req.TotalPrice = 700;
                var result = await _services.Update(req);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Not Found");
            }
        }

        [Test]
        public async Task DeletePassTest()
        {
            try
            {
                var result = await _services.Delete(1);
                Assert.AreEqual(result.TotalPrice, 1000);
            }
            catch (Exception ex)
            {

            }
        }

        [Test]
        public async Task DeleteFailTest()
        {
            try
            {
                var result = await _services.Delete(5);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Not Found");
            }
        }

        [Test]
        public async Task GetAllRefundDecisionPendingOrdersTest()
        {
            var result = _OrderService.GetAllRefundDecisionPendingOrders();
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllRefundDecisionAcceptedOrdersTest()
        {
            var result = _OrderService.GetAllRefundDecisionAcceptedOrders();
            Assert.IsNotNull(result);
        }

        [Test]
        [TestCase(OrderStatuses.Refund_Initiated,1)]
        public async Task UpdateOrderStatusPassTest(OrderStatuses Status, int OrderId)
        {
            var result = await _OrderService.UpdateOrderStatus(Status, OrderId);
            Assert.AreEqual(result.OrderStatus,Status);
        }

        [Test]
        [TestCase(OrderStatuses.Refund_Initiated, 5)]
        public async Task UpdateOrderStatusFailTest(OrderStatuses Status, int OrderId)
        {
            try
            {
                var result = await _OrderService.UpdateOrderStatus(Status, OrderId);
            }
            catch(Exception ex)
            {
                Assert.AreEqual(ex.Message,"Order Not Found");
            }
        }

    }
}
