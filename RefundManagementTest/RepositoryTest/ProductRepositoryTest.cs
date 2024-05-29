using Microsoft.EntityFrameworkCore;
using RefundManagementApplication.Context;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefundManagementTest.RepositoryTest
{
    public class ProductRepositoryTest
    {
        RefundManagementContext context;

        IRepository<int, Product> _repo;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                .UseInMemoryDatabase("dummyDB");
            context = new RefundManagementContext(optionsBuilder.Options);

            context.Database.EnsureDeletedAsync().Wait();
            context.Database.EnsureCreatedAsync().Wait();

            _repo = new ProductRepository(context);

        }

        [Test]
        public async Task Add_Product_PassTest()
        {
            //Arrange
            Product entity = new Product() { Title = "Boat TWS", Description = "Tws airdopes", Act_price = 1000, Curr_price = 899, Count = 50, Returnable = 0, ReturnableForPrime = 7 };
            //Action
            var result = await _repo.Add(entity);
            //Assert
            Assert.That(result.Count, Is.EqualTo(50));  
            Console.WriteLine(result.ProductId);
        }

        [Test]
        public async Task Add_Product_FailTest()
        {
            try
            {
                //Arrange
                Product entity = new Product() { ProductId = 102, Title = "Boat TWS", Description = "Tws airdopes", Act_price = 1000, Curr_price = 899, Count = 50, Returnable = 0, ReturnableForPrime = 7 };
                //Action
                var result = await _repo.Add(entity);
            }
            catch(Exception ex)
            {
                //Assert
                Assert.That(ex.Message, Is.EqualTo("An item with the same key has already been added. Key: 102"));
            }
        }

        [Test]
        public async Task Get_Products_PassTest()
        {
            var result = await _repo.Get();
            Assert.That(result.Count,Is.GreaterThan(0));
        }

        [Test]
        public async Task Get_Products_byId_PassTest()
        {
            var result = await _repo.Get(101);
            Assert.That(result.ProductId, Is.EqualTo(101));
        }

        [Test]
        public async Task Get_Product_byId_FailTest()
        {
            try
            {
                var result = await _repo.Get(501);
            }
            catch(Exception ex)
            {
                Assert.That(ex.Message, Is.EqualTo("Product Not Found"));
            }
        }

        [Test]
        public async Task Update_Product_PassTest()
        {
            //Arrange
            Product entity = new Product() { ProductId = 101, Title = "Soccor Football Nivia New Name", Description = "Sportsman Products", Act_price = 1200, Curr_price = 1000, Count = 10, Returnable = 0, ReturnableForPrime = 7 };

            //Action
            var result = await _repo.Update(entity);

            //Assert
            Assert.That(result.Returnable, Is.EqualTo(0));
        }

        [Test]
        public async Task Update_Product_FailTest()
        {
            try
            {
                //Arrange
                Product entity = new Product() { ProductId = 120, Title = "Soccor Football Nivia New Name", Description = "Sportsman Products", Act_price = 1200, Curr_price = 1000, Count = 10, Returnable = 0, ReturnableForPrime = 7 };
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
        public async Task Delete_Product_PassTest()
        {
            //Action
            var result = await _repo.Delete(101);
            //Assert
            Assert.That(result.ProductId,Is.EqualTo(101));
        }

        [Test]
        public async Task Delete_Product_FailTest()
        {
            //Action
            var result = await _repo.Delete(250);
            //Assert
            Assert.That(result, Is.Null);
        }

    }
}
