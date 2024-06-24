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
using RefundManagementApplication.Exceptions;

namespace RefundManagementTest.ServiceTest
{
    public class PaymentServicesTest
    {
        RefundManagementContext context;
        private ProductRepository _productRepo;
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

            _productRepo = new ProductRepository(context);
            _paymentRepo = new PaymentRepository(context);
            _refundRepo = new RefundRepository(context);
            _orderRepo = new OrderRepository(context);
            _OrderServices = new OrderServices(new OrderRepository(context), _productRepo);
            _services = new PaymentServices(_paymentRepo ,_refundRepo, _orderRepo, _OrderServices);

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

            Payment payment = new Payment()
            {
                RefundId = 1,
                PaymentDate = DateTime.Now,
                TotalPayment = 1000,
                Type = "NEFT",
                TransactionId = new Guid(),
                UserId = 101
            };

            await _paymentRepo.Add(payment);
            await context.SaveChangesAsync();
        }

        [Test]
        public async Task Create_Payment_PassTest()
        {
            var result = await _services.CreatePaymentToRefund(101, 1,new Guid("476517a4-38cb-4c46-90e5-f415b43ef828"));
            Assert.That(result.RefundId, Is.EqualTo(1));
        }

        [Test]
        public async Task Create_Payment_FailTest()
        {
            try
            {
                var result = await _services.CreatePaymentToRefund(101,6,new Guid("lafdjk-12312-saffsa-1324-sadfdas"));
            }
            catch(Exception ex)
            {
                Assert.That(ex.Message, Is.EqualTo("Refund Not Found"));
                Console.WriteLine(ex.Message);  
            }
        }

        [Test]
        public async Task Get_Payment_ById_PassTest()
        {
            //Action
            var result = await _services.GetMyPayment(1);

            //Assert
            Assert.That(result.TotalPayment, Is.EqualTo(1000));
        }

        [Test]
        public async Task Get_Payment_ById_FailTest()
        {
            try
            {
                //Action
                var result = await _services.GetMyPayment(101);
            }
            catch(NotFoundException nfe)
            {
                Assert.That(nfe.Message, Is.EqualTo("Payment Not Found"));
            }
        }
    }
}
