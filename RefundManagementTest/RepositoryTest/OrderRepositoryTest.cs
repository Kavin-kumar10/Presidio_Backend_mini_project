using Microsoft.EntityFrameworkCore;
using RefundManagementApplication.Context;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Models.Enums;
using RefundManagementApplication.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefundManagementTest.RepositoryTest
{
    public class OrderRepositoryTest
    {
        RefundManagementContext context;

        IRepository<int, Order> _repo;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                .UseInMemoryDatabase("dummyDB");
            context = new RefundManagementContext(optionsBuilder.Options);

            context.Database.EnsureDeletedAsync().Wait();
            context.Database.EnsureCreatedAsync().Wait();

            _repo = new OrderRepository(context);

        }

        [Test]
        public async Task Add_Order_PassTest()
        {
            //Arrange
            Order entity = new Order() { OrderId = 2, MemberID = 101, CreatedDate = DateTime.Now, OrderStatus = OrderStatuses.Refund_Accepted, ProductId = 102, TotalPrice = 700 };
            //Action
            var result = await _repo.Add(entity);
            //Assert
            Assert.That(result.TotalPrice, Is.EqualTo(700));
            Console.WriteLine(result.OrderId);
        }

        [Test]
        public async Task Add_Order_FailTest()
        {
            try
            {
                //Arrange
                Order entity = new Order() { OrderId = 1, MemberID = 101, CreatedDate = DateTime.Now, OrderStatus = OrderStatuses.Refund_Accepted, ProductId = 102, TotalPrice = 700 };
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
        public async Task Get_Orders_PassTest()
        {
            var result = await _repo.Get();
            Assert.That(result.Count, Is.GreaterThan(0));
        }

        [Test]
        public async Task Get_Orders_byId_PassTest()
        {
            var result = await _repo.Get(1);
            Assert.That(result.OrderId, Is.EqualTo(1));
        }

        [Test]
        public async Task Get_Order_byId_FailTest()
        {
            try
            {
                var result = await _repo.Get(501);
            }
            catch (Exception ex)
            {
                Assert.That(ex.Message, Is.EqualTo("Order Not Found"));
            }
        }

        [Test]
        public async Task Update_Order_PassTest()
        {
            //Arrange
            Order entity = new Order() { OrderId = 1, MemberID = 101, CreatedDate = DateTime.Now, OrderStatus = OrderStatuses.Refund_Accepted, ProductId = 102, TotalPrice = 700 };

            //Action
            var result = await _repo.Update(entity);

            //Assert
            Assert.That(result.TotalPrice, Is.EqualTo(700));
        }

        [Test]
        public async Task Update_Order_FailTest()
        {
            try
            {
                //Arrange
                Order entity = new Order() { OrderId = 5, MemberID = 101, CreatedDate = DateTime.Now, OrderStatus = OrderStatuses.Refund_Accepted, ProductId = 102, TotalPrice = 700 };
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
        public async Task Delete_Order_PassTest()
        {
            //Action
            var result = await _repo.Delete(1);
            //Assert
            Assert.That(result.OrderId, Is.EqualTo(1));
        }

        [Test]
        public async Task Delete_Order_FailTest()
        {
            //Action
            var result = await _repo.Delete(250);
            //Assert
            Assert.That(result, Is.Null);
        }

    }
}
