using Microsoft.EntityFrameworkCore;
using RefundManagementApplication.Context;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models.Enums;
using RefundManagementApplication.Models;
using RefundManagementApplication.Repositories;
using RefundManagementApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefundManagementTest.ServiceTest
{
    public class PaymentServicesTest
    {
        RefundManagementContext context;
        private IRepository<int,Payment> _paymentRepo;
        private IRepository<int,Refund> _refundRepo;
        private IRepository<int,Order> _orderRepo;
        private OrderServices _OrderServices;
        private PaymentServices _services;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                            .UseInMemoryDatabase("dummyDB");
            context = new RefundManagementContext(optionsBuilder.Options);

            context.Database.EnsureDeletedAsync().Wait();
            context.Database.EnsureCreatedAsync().Wait();

            _paymentRepo = new PaymentRepository(context);
            _refundRepo = new RefundRepository(context);
            _orderRepo = new OrderRepository(context);
            _OrderServices = new OrderServices(new OrderRepository(context));
            _services = new PaymentServices(_paymentRepo ,_refundRepo, _orderRepo, _OrderServices);

            //Member member = new Member() { Id = 101, email = "kavinkumar.prof@gmail.com", Name = "Kavin", Role = MemberRole.User };
            //context.Members.Add(member);

            //Order order = new Order() { OrderId = 1, MemberID = 101, CreatedDate = DateTime.Now, OrderStatus = OrderStatuses.Refund_Accepted, ProductId = 101, TotalPrice = 1000 };
            //context.Orders.Add(order);


            Refund refund = new Refund() { 
                OrderId = 1,
                RefundId = 2,
                RefundAmount = 700,
                Reason = "Damaged",
                RefundStatus = RefundStatuses.PENDING,
                CreatedBy = 101,
                CreatedDate = DateTime.Now,
            };
            context.Refunds.Add(refund);
            await context.SaveChangesAsync();
        }

        [Test]
        public async Task Create_Payment_PassTest()
        {
            var result = await _services.CreatePaymentToRefund(101, 1);
            Assert.That(result.RefundId, Is.EqualTo(1));
        }

        [Test]
        public async Task Create_Payment_FailTest()
        {
            try
            {
                var result = await _services.CreatePaymentToRefund(101,6);
            }
            catch(Exception ex)
            {
                Assert.That(ex.Message, Is.EqualTo("Refund Not Found"));
                Console.WriteLine(ex.Message);  
            }
        }
    }
}
