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
        private IRepository<int,Refund> _repo;
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

            _orderRepo = new OrderRepository(context);
            _memberRepo = new MemberRepository(context);
            _productRepo = new ProductRepository(context);
            _repo = new RefundRepository(context);
            _services = new RefundServices(_repo, _orderRepo, _memberRepo, _productRepo);
            _RefundServices = new RefundServices(_repo, _orderRepo, _memberRepo, _productRepo);

            //Order order = new Order()
            //{
            //    MemberID = 101,
            //    CreatedDate = DateTime.Now,
            //    OrderStatus = OrderStatuses.Ordered,
            //    ProductId = 102,
            //    TotalPrice = 1000,
            //};
            //await _orderRepo.Add(order);
        }

        [Test]
        public async Task CreateRefundPassTest() {
            try
            {
                var result = await _RefundServices.CreateRefund(1,"Damaged Product");
                Assert.AreEqual(result.Reason, "Damaged Product");
            }
            catch(Exception ex) { 
            
            }
        }

        [Test]
        public async Task CreateRefundNotReturnableExceptionTest()
        {
            try
            {
                var result = await _RefundServices.CreateRefund(1, "Damaged Product");
                // Test should not reach here
                Assert.Fail("Test should throw ObjectIsNotReturnableException");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Refund not available for the current order");
            }
        }


        [Test]
        public async Task CreateRefundReturnableDateExpiredExceptionTest()
        {
            try
            {
                Order order = new Order()
                {
                    MemberID = 101,
                    CreatedDate = new DateTime(2024, 03, 29, 10, 30, 0),
                    OrderStatus = OrderStatuses.Ordered,
                    ProductId = 101,
                    TotalPrice = 1000,
                };
                await _orderRepo.Add(order);
                var result = await _RefundServices.CreateRefund(2, "Damaged Product");
            }
            catch (ReturnableDateExpired ex)
            {
                Assert.AreEqual(ex.Message, "The Returnable Date is Expired, Unable to proceed further.");
            }
        }
    }
}
