using Castle.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using RefundManagementApplication.Context;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Repositories;
using RefundManagementApplication.Services;
using Iconfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RefundManagementApplication.Models.DTOs.RequestDTO.AuthReqDTOs;

namespace RefundManagementTest.ServiceTest
{
    public class UserServicesTest
    {
        RefundManagementContext context;
        ITokenServices _tokenServices;
        IRepository<int, Member> _memRepo;
        IRepository<int,User> _userRepo;
        IUserServices _services;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                .UseInMemoryDatabase("dummyDB");
            context = new RefundManagementContext(optionsBuilder.Options);

            context.Database.EnsureDeletedAsync().Wait();
            context.Database.EnsureCreatedAsync().Wait();

            //Token Services

            Mock<IConfigurationSection> configurationJWTSection = new Mock<IConfigurationSection>();
            configurationJWTSection.Setup(x => x.Value).Returns("This is the dummy key which has to be a bit long for the 512. which should be even more longer for the passing");
            Mock<IConfigurationSection> congigTokenSection = new Mock<IConfigurationSection>();
            congigTokenSection.Setup(x => x.GetSection("JWT")).Returns(configurationJWTSection.Object);
            Mock<Iconfiguration> mockConfig = new Mock<Iconfiguration>();
            mockConfig.Setup(x => x.GetSection("TokenKey")).Returns(congigTokenSection.Object);
            _tokenServices = new TokenServices(mockConfig.Object);

            // Initial Setup
            _memRepo = new MemberRepository(context);
            _userRepo = new UserRepository(context);
            _services = new UserServices(_memRepo,_userRepo,_tokenServices);

            //Demo User
            //Arrange
            RegisterRequestDTO registerDTO = new RegisterRequestDTO();
            registerDTO.email = "Pravinkumar.prof@gmail.com";
            registerDTO.Name = "Pravin";
            registerDTO.password = "Pravin@25";

            //Action
            var result = await _services.Register(registerDTO);
        }

        [Test]
        public async Task Register_PassTest()
        {
            //Arrange
            RegisterRequestDTO registerDTO = new RegisterRequestDTO();
            registerDTO.email = "kavin.prof@gmail.com";
            registerDTO.Name = "Kavin";
            registerDTO.password = "Kavin@10";

            //Action
            var result = await _services.Register(registerDTO);

            //Assert
            Assert.That(result.Id, Is.EqualTo(105));
            Assert.That(result.email, Is.EqualTo("kavin.prof@gmail.com"));
            Assert.That(result.Name, Is.EqualTo("Kavin"));
        }

        [Test]
        public async Task Register_FailTest()
        {
            //Arrange
            try
            {

                RegisterRequestDTO registerDTO = new RegisterRequestDTO();
                registerDTO.email = "kavin.prof@gmail.com";
                registerDTO.Name = "Kavin";
                //registerDTO.password = "Kavin@10";

                //Action
                var result = await _services.Register(registerDTO);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.That(ex.Message, Is.EqualTo("Unable to Register"));
            }
        }

        [Test]
        public async Task Register_duplicateMailId_FailTest()
        {
            //Arrange
            try
            {

                RegisterRequestDTO registerDTO = new RegisterRequestDTO();
                registerDTO.email = "kavinkumar.prof@gmail.com";
                registerDTO.Name = "Kavin";
                registerDTO.password = "Kavin@10";

                //Action
                var result = await _services.Register(registerDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.That(ex.Message, Is.EqualTo("Member with the Id Already Found"));
            }
        }

        [Test]
        public async Task Login_PassTest()
        {
            //Arrange
            try
            {

                UserLoginDTO userLoginDTO = new UserLoginDTO();
                userLoginDTO.UserId = 104;
                userLoginDTO.Password = "Pravin@25";

                //Action
                var result = await _services.Login(userLoginDTO);
            }
            catch (Exception ex)
            {
                Assert.That(ex.Message, Is.EqualTo("Your account is not activated"));
                Console.WriteLine(ex.Message);
            }
        }

        [Test]
        public async Task Login_FailTest()
        {
            //Arrange
            try
            {

                UserLoginDTO userLoginDTO = new UserLoginDTO();
                userLoginDTO.UserId = 1;
                userLoginDTO.Password = "kavin@25";

                //Action
                var result = await _services.Login(userLoginDTO);
            }
            catch (Exception ex)
            {
                Assert.That(ex.Message, Is.EqualTo("Invalid username or password"));
                Console.WriteLine(ex.Message);
            }
        }


    }
}
