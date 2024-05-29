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
    public class MemberRepositoryTest
    {
        RefundManagementContext context;

        IRepository<int, Member> _repo;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                .UseInMemoryDatabase("dummyDB");
            context = new RefundManagementContext(optionsBuilder.Options);

            context.Database.EnsureDeletedAsync().Wait();
            context.Database.EnsureCreatedAsync().Wait();

            _repo = new MemberRepository(context);

        }

        [Test]
        public async Task Add_Member_PassTest()
        {
            //Arrange
            Member entity = new Member() { email = "rajan@gmail.com", Name = "Rajan", Role = MemberRole.Collector };

            //Action
            var result = await _repo.Add(entity);

            //Assert
            Assert.That(result.Role, Is.EqualTo(MemberRole.Collector));
            Console.WriteLine(result.Id);
        }

        [Test]
        public async Task Add_Member_FailTest()
        {
            try
            {
                //Arrange
                Member entity = new Member() { Id = 102, email = "raju@gmail.com", Name = "Raju", Role = MemberRole.Collector };
                //Action
                var result = await _repo.Add(entity);
            }
            catch (Exception ex)
            {
                //Assert
                Assert.That(ex.Message, Is.EqualTo("An item with the same key has already been added. Key: 102"));
            }
        }

        [Test]
        public async Task Get_Members_PassTest()
        {
            var result = await _repo.Get();
            Assert.That(result.Count, Is.GreaterThan(0));
        }

        [Test]
        public async Task Get_Members_byId_PassTest()
        {
            var result = await _repo.Get(101);
            Assert.That(result.Id, Is.EqualTo(101));
        }

        [Test]
        public async Task Get_Member_byId_FailTest()
        {
            try
            {
                var result = await _repo.Get(501);
            }
            catch (Exception ex)
            {
                Assert.That(ex.Message, Is.EqualTo("Member Not Found"));
            }
        }

        [Test]
        public async Task Update_Member_PassTest()
        {
            //Arrange
            Member entity = new Member() { Id = 103, email = "rohan@gmail.com", Name = "Rohan", Role = MemberRole.Collector };

            //Action
            var result = await _repo.Update(entity);

            //Assert
            Assert.That(result.Name, Is.EqualTo("Rohan"));
        }

        [Test]
        public async Task Update_Member_FailTest()
        {
            try
            {
                //Arrange
                Member entity = new Member() { Id = 120, email = "raju@gmail.com", Name = "Raju", Role = MemberRole.Collector };
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
        public async Task Delete_Member_PassTest()
        {
            //Action
            var result = await _repo.Delete(101);
            //Assert
            Assert.That(result.Id, Is.EqualTo(101));
        }

        [Test]
        public async Task Delete_Member_FailTest()
        {
            //Action
            var result = await _repo.Delete(250);
            //Assert
            Assert.That(result, Is.Null);
        }
    }
}
