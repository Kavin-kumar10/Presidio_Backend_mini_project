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
        }

        [Test]
        public async Task CreateProduct()
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
        public async Task GetAllPassTest()
        {
            //Action
            var result = await _Productservices.GetAll();
            //Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetByIdPassTest()
        {
            //Action
            var result = await _Productservices.GetById(1);
            //Assert
            Assert.That(result,Is.Not.Null);
        }

        [Test]
        public async Task GetByIdFailTest()
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
        public async Task UpdatePassTest()
        {
            //arrange
            var req = await _Productrepo.Get(1);
            req.Count = 20;

            //Action
            var result = await _Productservices.Update(req);

            //Assert
            Assert.AreEqual(result.Count, 20);
        }

        [Test]
        public async Task UpdateExceptionTest()
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
        public async Task DeletePassTest()
        {
            var result = await _Productservices.Delete(1);
            Assert.AreEqual(result.ProductId, 1);
        }

        [Test]
        public async Task DeleteFailTest()
        {
            try
            {
                var req = await _Productservices.GetById(5);
                req.Count = 20;
                var result = await _Productservices.Delete(req.ProductId);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Not Found");
            }
        }
    }
}