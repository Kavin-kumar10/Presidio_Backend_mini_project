using Microsoft.EntityFrameworkCore;
using RefundManagementApplication.Context;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Models.DTOs.RequestDTO.OrderReqDTOs;
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
        private IRepository<int, Product> _productRepo;
        private IServices<int,Order> _services;
        private IOrderServices _OrderService;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                            .UseInMemoryDatabase("dummyDB");
            context = new RefundManagementContext(optionsBuilder.Options);

            context.Database.EnsureDeletedAsync().Wait();
            context.Database.EnsureCreatedAsync().Wait();

            _productRepo = new ProductRepository(context);
            _repo = new OrderRepository(context);
            _services = new OrderServices(_repo,_productRepo);
            _OrderService = new OrderServices(new OrderRepository(context),_productRepo);
            Order order = new Order()
            {
                MemberID = 101,
                CreatedDate = DateTime.Now,
                OrderStatus = OrderStatuses.Refund_Initiated,
                ProductId = 101,
                TotalPrice = 1000,
            };

            await _repo.Add(order);
        }

        [Test]
        public async Task CreateOrder_From_Register_PassTest()
        {
            //Arrange
            OrderRequestDTO orderRequestDTO = new OrderRequestDTO() { 
                MemberID = 101,
                ProductId = 101
            };

            //Action
            var result = await _OrderService.CreateOrder(orderRequestDTO);

            //Assert
            Assert.That(result.MemberID, Is.EqualTo(101));
            Console.WriteLine(result.TotalPrice);
        }

        [Test]
        public async Task CreateOrder_From_Register_FailTest()
        {
            try
            {
                //Arrange
                OrderRequestDTO orderRequestDTO = new OrderRequestDTO()
                {
                    MemberID = 101,
                    ProductId = 200
                };
                //Action
                var result = await _OrderService.CreateOrder(orderRequestDTO);
            }
            catch (Exception ex) {            
                //Assert
                Assert.That(ex.Message, Is.EqualTo("Product Not Found"));
            }
        }


        [Test]
        public async Task Create_Order_PassTest()
        {

            Order order = new Order() {
                MemberID = 101,
                CreatedDate = DateTime.Now,
                OrderStatus = OrderStatuses.Ordered,
                ProductId = 101,
                TotalPrice = 1000,
            };
            var result = await _services.Create(order);

            Assert.AreEqual(result.OrderId, 3);
        }

        [Test]
        public async Task Create_Order_FailTest()
        {
            try
            {
                Order order = new Order()
                {
                    MemberID = 101,
                    CreatedDate = DateTime.Now,
                    OrderStatus = OrderStatuses.Refund_Initiated,
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
        public async Task GetAll_Order_PassTest()
        {
            //Action
            var result = _services.GetAll();

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Get_Order_ById_PassTest()
        {
            //Action
            var result = _services.GetById(1);

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Get_Order_ById_FailTest()
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
        public async Task Update_Order_PassTest()
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
        public async Task Update_Order_FailTest()
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
        public async Task Delete_Order_PassTest()
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
        public async Task Delete_Order_FailTest()
        {
            try
            {
                var req = await _services.GetById(5);
                var result = await _services.Delete(req.OrderId);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Not Found");
            }
        }

        [Test]
        public async Task GetAll_Refund_DecisionPending_OrdersTest()
        {
            var result = await _OrderService.GetAllRefundDecisionPendingOrders();
            Assert.That(result.Count,Is.EqualTo(1));
        }

        [Test]
        public async Task GetAll_Refund_DecisionAccepted_OrdersTest()
        {
            var result = await _OrderService.GetAllRefundDecisionAcceptedOrders();
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        [TestCase(OrderStatuses.Refund_Initiated,1)]
        public async Task Update_OrderStatus_PassTest(OrderStatuses Status, int OrderId)
        {
            var result = await _OrderService.UpdateOrderStatus(Status, OrderId);
            Assert.AreEqual(result.OrderStatus,Status);
        }

        [Test]
        [TestCase(OrderStatuses.Refund_Initiated, 5)]
        public async Task Update_OrderStatus_FailTest(OrderStatuses Status, int OrderId)
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
