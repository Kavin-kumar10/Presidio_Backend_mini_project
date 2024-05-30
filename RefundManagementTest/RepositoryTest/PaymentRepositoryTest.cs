using Microsoft.EntityFrameworkCore;
using RefundManagementApplication.Context;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models.Enums;
using RefundManagementApplication.Models;
using RefundManagementApplication.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefundManagementTest.RepositoryTest
{
    public class PaymentRepositoryTest
    {
        RefundManagementContext context;

        IRepository<int, Payment> _repo;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                .UseInMemoryDatabase("dummyDB");
            context = new RefundManagementContext(optionsBuilder.Options);

            context.Database.EnsureDeletedAsync().Wait();
            context.Database.EnsureCreatedAsync().Wait();

            _repo = new PaymentRepository(context);
            Payment entity = new Payment() { PaymentId = 5, UserId = 101, RefundId = 1, Type = "Cash", TotalPayment = 250, PaymentDate = DateTime.UtcNow };
            _repo.Add(entity);
        }

        [Test]
        public async Task Add_Payment_PassTest()
        {
            //Arrange
            Payment entity = new Payment() { PaymentId = 1, UserId = 101, RefundId = 1, Type = "Cash", TotalPayment = 250, PaymentDate = DateTime.UtcNow };

            //Action
            var result = await _repo.Add(entity);

            //Assert
            Assert.That(result.TotalPayment, Is.EqualTo(250));
            Console.WriteLine(result.RefundId);
        }


        [Test]
        public async Task Get_Payments_PassTest()
        {
            var result = await _repo.Get();
            Assert.That(result.Count, Is.GreaterThan(0));
        }

        [Test]
        public async Task Get_Payments_byPaymentId_PassTest()
        {
            var result = await _repo.Get(5);
            Assert.That(result.PaymentId, Is.EqualTo(5));
        }

        [Test]
        public async Task Get_Payment_byPaymentId_FailTest()
        {
            try
            {
                var result = await _repo.Get(101);
            }
            catch (Exception ex)
            {
                Assert.That(ex.Message, Is.EqualTo("Payment Not Found"));
            }
        }


        [Test]
        public async Task Update_Payment_FailTest()
        {
            try
            {
                //Arrange
                Payment entity = new Payment() { PaymentId = 20, UserId = 101, RefundId = 1, Type = "Cash", TotalPayment = 250, PaymentDate = DateTime.UtcNow };
                //Action
                var result = await _repo.Update(entity);
            }
            catch (Exception ex)
            {
                //Assert
                Assert.That(ex.Message, Is.EqualTo("Attempted to update or delete an entity that does not exist in the store."));
            }
        }

        [Test]
        public async Task Delete_Payment_PassTest()
        {
            //Action
            var result = await _repo.Delete(5);
            //Assert
            Assert.That(result.PaymentId, Is.EqualTo(5));
        }

        [Test]
        public async Task Delete_Payment_FailTest()
        {
            //Action
            var result = await _repo.Delete(250);
            //Assert
            Assert.That(result, Is.Null);
        }
    }
}
