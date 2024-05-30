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
    public class RefundRepositoryTest
    {
        RefundManagementContext context;

        IRepository<int, Refund> _repo;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                .UseInMemoryDatabase("dummyDB");
            context = new RefundManagementContext(optionsBuilder.Options);

            context.Database.EnsureDeletedAsync().Wait();
            context.Database.EnsureCreatedAsync().Wait();

            _repo = new RefundRepository(context);

        }

        [Test]
        public async Task Add_Refund_PassTest()
        {
            //Arrange
            Refund entity = new Refund() { OrderId = 1, RefundAmount = 1000, Reason = "Damaged", RefundStatus = RefundStatuses.PENDING, CreatedBy = 101, CreatedDate = DateTime.Now };

            //Action
            var result = await _repo.Add(entity);

            //Assert
            Assert.That(result.RefundStatus, Is.EqualTo(RefundStatuses.PENDING));
            Console.WriteLine(result.RefundId);
        }

        [Test]
        public async Task Add_Refund_FailTest()
        {
            try
            {
                //Arrange
                Refund entity = new Refund() { RefundId = 1, OrderId = 1, RefundAmount = 1000, Reason = "Damaged", RefundStatus = RefundStatuses.PENDING, CreatedBy = 101, CreatedDate = DateTime.Now };
                //Action
                var result = await _repo.Add(entity);
            }
            catch (Exception ex)
            {
                //Assert
                Assert.That(ex.Message, Is.EqualTo("An item with the same key has already been added. Key: 1"));
            }
        }

        [Test]
        public async Task Get_Refunds_PassTest()
        {
            var result = await _repo.Get();
            Assert.That(result.Count, Is.GreaterThan(0));
        }

        [Test]
        public async Task Get_Refunds_byRefundId_PassTest()
        {
            var result = await _repo.Get(1);
            Assert.That(result.RefundId, Is.EqualTo(1));
        }

        [Test]
        public async Task Get_Refund_byRefundId_FailTest()
        {
            try
            {
                var result = await _repo.Get(501);
            }
            catch (Exception ex)
            {
                Assert.That(ex.Message, Is.EqualTo("Refund Not Found"));
            }
        }

        [Test]
        public async Task Update_Refund_PassTest()
        {
            //Arrange
            Refund entity = new Refund() { RefundId = 1, OrderId = 1, RefundAmount = 1000, Reason = "Damaged", RefundStatus = RefundStatuses.PENDING, CreatedBy = 101, CreatedDate = DateTime.Now };

            //Action
            var result = await _repo.Update(entity);

            //Assert
            Assert.That(result.RefundAmount, Is.EqualTo(1000));
        }

        [Test]
        public async Task Update_Refund_FailTest()
        {
            try
            {
                //Arrange
                Refund entity = new Refund() { RefundId = 10, OrderId = 1, RefundAmount = 1000, Reason = "Damaged", RefundStatus = RefundStatuses.PENDING, CreatedBy = 101, CreatedDate = DateTime.Now };
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
        public async Task Delete_Refund_PassTest()
        {
            //Action
            var result = await _repo.Delete(1);
            //Assert
            Assert.That(result.RefundId, Is.EqualTo(1));
        }

        [Test]
        public async Task Delete_Refund_FailTest()
        {
            //Action
            var result = await _repo.Delete(250);
            //Assert
            Assert.That(result, Is.Null);
        }
    }
}
