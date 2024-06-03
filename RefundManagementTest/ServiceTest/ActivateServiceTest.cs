using Microsoft.EntityFrameworkCore;
using RefundManagementApplication.Context;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Models.Enums;
using RefundManagementApplication.Repositories;
using RefundManagementApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RefundManagementTest.ServiceTest
{
    public class ActivateServiceTest
    {
        RefundManagementContext context;
        private IRepository<int, Member> _MemberRepo;
        private IRepository<int,User>  _UserRepo;
        IActivateServices _service;
        

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                    .UseInMemoryDatabase("dummyDB");
            context = new RefundManagementContext(optionsBuilder.Options);
            context.Database.EnsureDeletedAsync().Wait();
            context.Database.EnsureCreatedAsync().Wait();
            _MemberRepo = new MemberRepository(context);
            _UserRepo = new UserRepository(context);
            _service = new ActivateServices(_UserRepo,_MemberRepo);


            Member member = new Member()
            {
                email = "Raja@gmail.com",
                Membership = Plan.Free,
                Name = "Raja",
                Role = MemberRole.User,
            };
            await _MemberRepo.Add(member);
            User user = new User()
            {
                MemberId = 1,
                Password = Encoding.UTF8.GetBytes("yourPassword"),
                PasswordHashKey = SHA256.HashData(Encoding.UTF8.GetBytes("yourPassword")),
                Status = "Disable"
            };
            await _UserRepo.Add(user);
        }

        [Test]
        public async Task Activate_User_PassTest()
        {
            var result = await _service.Activate(101,MemberRole.User,Plan.Free);
            Console.Write(result.Status);
            Assert.That(result.Status, Is.EqualTo("Active"));
        }

        [Test]
        public async Task Deactivate_User_PassTest()
        {
            var result = await _service.Deactivate(101);
            Console.Write(result.Status);
            Assert.That(result.Status, Is.EqualTo("Disabled"));
        }

        [Test]
        public async Task  Activate_User_FailTest()
        {
            try
            {
                var result = await _service.Activate(4, MemberRole.User, Plan.Free);
            }
            catch (Exception ex)
            {
                Assert.That(ex.Message, Is.EqualTo("User not Found"));
            }
        }

        [Test]
        public async Task Deactivate_User_FailTest()
        {
            try
            {
                var result = await _service.Deactivate(2);
            }
            catch (Exception ex)
            {
                Assert.That(ex.Message, Is.EqualTo("User not Found"));
            }
        }
    }
}
