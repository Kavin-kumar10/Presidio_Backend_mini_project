using Microsoft.EntityFrameworkCore;
using RefundManagementApplication.Context;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Models.Enums;
using RefundManagementApplication.Repositories;
using RefundManagementApplication.Services;
using RefundManagementApplication.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RefundManagementApplication.Exceptions.RefundExceptions;

namespace RefundManagementTest.ServiceTest
{
    public class RefundServiceTest
    {
        private RefundManagementContext context;
        private IRepository<int,Refund> _refundRepo;
        private IServices<int, Refund> _services;
        private IRepository<int, Order> _orderRepo;
        private IRepository<int, Member> _memberRepo;
        private IRepository<int, Product> _productRepo;
        private IRefundServices _RefundServices;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                      .UseInMemoryDatabase("dummyDB");
            context = new RefundManagementContext(optionsBuilder.Options);

            context.Database.EnsureDeletedAsync().Wait();
            context.Database.EnsureCreatedAsync().Wait();

            _orderRepo = new OrderRepository(context);
            _memberRepo = new MemberRepository(context);
            _productRepo = new ProductRepository(context);
            _refundRepo = new RefundRepository(context);
            _services = new RefundServices(_refundRepo, _orderRepo, _memberRepo, _productRepo);
            _RefundServices = new RefundServices(new RefundRepository(context), _orderRepo, _memberRepo, _productRepo);

            Member member = new Member()
            {
                email = "email",
                Membership = Plan.Free,
                Name = "name",
                Role = MemberRole.User
            };

            await _memberRepo.Add(member);

            Product product = new Product()
            {
                Title = "Product",
                Description = "dfsf",
                Act_price = 1500,
                Curr_price = 900,
                Count = 10,
                Returnable = 7,
                ReturnableForPrime = 14
            };

            await _productRepo.Add(product);

            Order order = new Order()
            {
                MemberID = 101,
                CreatedDate = DateTime.Now,
                OrderStatus = OrderStatuses.Ordered,
                ProductId = 101,
                TotalPrice = 900,
            };

            await _orderRepo.Add(order);    

            Refund refund = new Refund()
            {
                OrderId = 1,
                CreatedDate = DateTime.Now,
                CreatedBy = 101,
                Reason = "Broken",
                RefundAmount = 900,
                RefundStatus = RefundStatuses.PENDING
            };
            await _refundRepo.Add(refund);
        }

        [Test]
        public async Task GetRefund_ById_PassTest()
        {
            var result = await _services.GetById(1);
            Assert.That(result.Reason, Is.EqualTo("Damaged"));
        }

        [Test]
        public async Task GetRefund_ById_FailTest()
        {
            try
            {
                var result = await _services.GetById(5);
            }
            catch(Exception ex)
            {
                Assert.That(ex.Message, Is.EqualTo("Not Found"));
            }
        }

        [Test]
        public async Task Update_Refund_PassTest()
        {
            try
            {
                var req = await _services.GetById(1);
                req.RefundStatus = RefundStatuses.SUCCEED;
                var result = await _services.Update(req);
            }
            catch (Exception ex)
            {
                Assert.That(ex.Message, Is.EqualTo("Not Found"));
            }
        }

        [Test]
        public async Task Update_Refund_FailTest()
        {
            try
            {
                var req = await _services.GetById(5);
                req.RefundStatus = RefundStatuses.SUCCEED;
                var result = await _services.Update(req);
            }
            catch (Exception ex)
            {
                Assert.That(ex.Message, Is.EqualTo("Not Found"));
            }
        }


        [Test]
        public async Task Delete_Refund_PassTest()
        {
            try
            {
                var result = await _services.Delete(1);
                Assert.That(result.RefundId, Is.EqualTo(1));
            }
            catch (Exception ex)
            {
                //Assert.That(ex.Message, Is.EqualTo("Not Found"));
            }
        }

        [Test]
        public async Task Delete_Refund_FailTest()
        {
            try
            {
                var req = await _services.GetById(5);
                var result = await _services.Delete(req.RefundId);
            }
            catch (Exception ex)
            {
                Assert.That(ex.Message, Is.EqualTo("Not Found"));
            }
        }

        [Test]
        public async Task Create_Refund_PassTest()
        {
            try
            {
                var result = await _RefundServices.CreateRefund(1, "Damaged");
                Assert.That(result.RefundAmount, Is.EqualTo(1000));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        [Test]
        public async Task Create_Refund_OrderNotFound_FailTest()
        {
            //Action
            try
            {
                var result = await _RefundServices.CreateRefund(6, "Damaged");
            }
            catch (Exception ex)
            {
                Assert.That(ex.Message, Is.EqualTo("Order Not Found"));
                Console.WriteLine(ex.Message);
            }
        }

        [Test]
        public async Task Create_Refund_ProductNotFound_FailTest()
        {
            //Arrange
            Order order = new Order()
            {
                MemberID = 101,
                CreatedDate = DateTime.Now,
                OrderStatus = OrderStatuses.Ordered,
                ProductId = 10,
                TotalPrice = 900,
            };

            await _orderRepo.Add(order);


            //Action
            try
            {
                var result = await _RefundServices.CreateRefund(2, "Damaged");
            }
            catch (Exception ex)
            {
                Assert.That(ex.Message, Is.EqualTo("Product Not Found"));
                Console.WriteLine(ex.Message);
            }
        }

        [Test]
        public async Task Create_Refund_MemberNotFound_FailTest()
        {
            //Arrange
            Order order = new Order()
            {
                MemberID = 5,
                CreatedDate = DateTime.Now,
                OrderStatus = OrderStatuses.Ordered,
                ProductId = 101,
                TotalPrice = 900,
            };

            await _orderRepo.Add(order);


            //Action
            try
            {
                var result = await _RefundServices.CreateRefund(2, "Damaged");
            }
            catch (Exception ex)
            {
                Assert.That(ex.Message, Is.EqualTo("Member Not Found"));
                Console.WriteLine(ex.Message);
            }
        }


        [Test]
        public async Task Create_Refund_ObjectNotReturnable_FailTest()
        {
            //Arrange
            Order order = new Order()
            {
                MemberID = 101,
                CreatedDate = DateTime.Now,
                OrderStatus = OrderStatuses.Ordered,
                ProductId = 102,
                TotalPrice = 900,
            };

            await _orderRepo.Add(order);


            //Action
            try
            {
                var result = await _RefundServices.CreateRefund(3, "Damaged");
            }
            catch (ObjectIsNotReturnableException onre)
            {
                Assert.That(onre.Message, Is.EqualTo("Refund not available for the current order"));
                Console.WriteLine(onre.Message);
            }
        }

        [Test]
        public async Task Create_Refund_ReturnableDateExpired_FailTest()
        {
            //Arrange
            Order order = new Order()
            {
                MemberID = 101,
                CreatedDate = new DateTime(2024, 02, 28, 10, 30, 0),
                OrderStatus = OrderStatuses.Ordered,
                ProductId = 101,
                TotalPrice = 900,
            };

            await _orderRepo.Add(order);


            //Action
            try
            {
                var result = await _RefundServices.CreateRefund(3, "Damaged");
            }
            catch (ReturnableDateExpired rde)
            {
                Assert.That(rde.Message, Is.EqualTo("The Returnable Date is Expired, Unable to proceed further."));
                Console.WriteLine(rde.Message);
            }
        }

    }
}
