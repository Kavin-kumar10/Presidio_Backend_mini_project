using Microsoft.EntityFrameworkCore;
using RefundManagementApplication.Context;
using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Repositories;
using RefundManagementApplication.Services;

namespace RefundManagementTest.RepositoryTest
{
    public class ProductServicesTest
    {
        RefundManagementContext context;
        IRepository<int, Product> _Productrepo;
        IServices<int,Product> _Productservices;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                            .UseInMemoryDatabase("dummyDB");
            context = new RefundManagementContext(optionsBuilder.Options);

            context.Database.EnsureDeletedAsync().Wait();
            context.Database.EnsureCreatedAsync().Wait();

            _Productrepo = new ProductRepository(context);
            _Productservices = new ProductServices(_Productrepo);

            await _Productrepo.Add(new Product()
            {
                Title = "Test",
                Description = "Test",
                Count = 1,
                Act_price = 1000,
                Curr_price = 700,
                Returnable = 7,
                ReturnableForPrime = 14
            });
            await context.SaveChangesAsync();
        }

        [Test]
        public async Task CreateMultiple_Product_PassTest()
        {
            //Arrange

            List<Product> products = new List<Product>();

            products.Add(new Product()
            {
                Title = "Test",
                Description = "Test",
                Count = 1,
                Act_price = 1000,
                Curr_price = 700,
                Returnable = 7,
                ReturnableForPrime = 14
            });

            products.Add(new Product()
            {
                Title = "Test",
                Description = "Test",
                Count = 1,
                Act_price = 1000,
                Curr_price = 700,
                Returnable = 7,
                ReturnableForPrime = 14
            });

            //Action
            var result = await _Productservices.CreateMultiple(products);

            //Assert
            Console.WriteLine(result.Count);
            Assert.That(result.Count,Is.EqualTo(2));
        }


        [Test]
        public async Task Create_Product_PassTest()
        {
            //Action
            var result = await _Productservices.Create(new Product()
            {
                 Title = "Test",
                 Description = "Test",
                 Count = 1,
                 Act_price = 1000,
                 Curr_price = 700,
                 Returnable = 7,
                 ReturnableForPrime = 14
            });

            //Assert
            Assert.AreEqual(result.Title,"Test");
        }

        [Test]
        public async Task GetAll_Product_PassTest()
        {
            //Action
            var result = await _Productservices.GetAll();
            //Assert
            Assert.That(result,Is.Not.Null);
        }

        [Test]
        public async Task Get_Product_ById_PassTest()
        {
            //Action
            var result = await _Productservices.GetById(101);
            //Assert
            Assert.That(result,Is.Not.Null);
        }

        [Test]
        public async Task Get_Product_ById_FailTest()
        {
            try
            {
                await _Productservices.GetById(5);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Not Found");
            }
        }

        [Test]
        public async Task Update_Product_PassTest()
        {
            //arrange
            var req = await _Productrepo.Get(101);
            req.Count = 20;

            //Action
            var result = await _Productservices.Update(req);

            //Assert
            Assert.AreEqual(result.Count, 20);
        }

        [Test]
        public async Task Update_Product_ExceptionTest()
        {
            //arrange
            try
            {
                var req = await _Productservices.GetById(5);
                req.Count = 20;
                var result = await _Productservices.Update(req);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Not Found");
            }
        }

        [Test]
        public async Task Delete_Product_PassTest()
        {
            var result = await _Productservices.Delete(101);
            Assert.AreEqual(result.ProductId, 101);
        }

        [Test]
        public async Task Delete_Product_FailTest()
        {
            try
            {
                var req = await _Productservices.GetById(5);
                var result = await _Productservices.Delete(req.ProductId);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Not Found");
            }
        }
    }
}