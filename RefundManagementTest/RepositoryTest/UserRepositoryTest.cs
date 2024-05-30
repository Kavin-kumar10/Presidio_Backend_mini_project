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
    public class UserRepositoryTest
    {
        RefundManagementContext context;

        IRepository<int, User> _repo;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                .UseInMemoryDatabase("dummyDB");
            context = new RefundManagementContext(optionsBuilder.Options);

            context.Database.EnsureDeletedAsync().Wait();
            context.Database.EnsureCreatedAsync().Wait();

            _repo = new UserRepository(context);

        }

        [Test]
        public async Task Add_User_PassTest()
        {
            //Arrange
            User entity = new User() { MemberId = 104, Password = Encoding.UTF8.GetBytes("yourPassword"), PasswordHashKey = Encoding.UTF8.GetBytes("yourPassword"), Status = "Active" };
            //Action
            var result = await _repo.Add(entity);
            //Assert
            Assert.That(result.Status, Is.EqualTo("Active"));
            Console.WriteLine(result.Status);
        }

        [Test]
        public async Task Add_User_FailTest()
        {
            try
            {
                //Arrange
                User entity = new User() { MemberId = 101, Password = Encoding.UTF8.GetBytes("yourPassword"), PasswordHashKey = Encoding.UTF8.GetBytes("yourPassword"), Status = "Active" };
                //Action
                var result = await _repo.Add(entity);
            }
            catch (Exception ex)
            {
                //Assert
                Assert.That(ex.Message, Is.EqualTo("An item with the same key has already been added. Key: 101"));
            }
        }

        [Test]
        public async Task Get_Users_PassTest()
        {
            var result = await _repo.Get();
            Assert.That(result.Count, Is.GreaterThan(0));
        }

        [Test]
        public async Task Get_Users_byId_PassTest()
        {
            var result = await _repo.Get(101);
            Assert.That(result.Status, Is.EqualTo("Disabled"));
        }

        [Test]
        public async Task Get_User_byId_FailTest()
        {
            try
            {
                var result = await _repo.Get(501);
            }
            catch (Exception ex)
            {
                Assert.That(ex.Message, Is.EqualTo("User Not Found"));
            }
        }

        [Test]
        public async Task Update_User_PassTest()
        {
            //Arrange
            User entity = new User() { MemberId = 101, Password = Encoding.UTF8.GetBytes("yourPassword"), PasswordHashKey = Encoding.UTF8.GetBytes("yourPassword"), Status = "Active" };

            //Action
            var result = await _repo.Update(entity);

            //Assert
            Assert.That(result.Status, Is.EqualTo("Active"));
        }

        [Test]
        public async Task Update_User_FailTest()
        {
            try
            {
                //Arrange
                User entity = new User() { MemberId = 106, Password = Encoding.UTF8.GetBytes("yourPassword"), PasswordHashKey = Encoding.UTF8.GetBytes("yourPassword"), Status = "Active" };
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
        public async Task Delete_User_PassTest()
        {
            //Action
            var result = await _repo.Delete(101);
            //Assert
            Assert.That(result.MemberId, Is.EqualTo(101));
        }

        [Test]
        public async Task Delete_User_FailTest()
        {
            //Action
            var result = await _repo.Delete(150);
            //Assert
            Assert.That(result, Is.Null);
        }

    }
}
