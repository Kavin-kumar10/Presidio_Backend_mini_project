using Microsoft.Extensions.Configuration;
using Moq;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Models.Enums;
using RefundManagementApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iconfiguration = Microsoft.Extensions.Configuration.IConfiguration;


namespace RefundManagementTest.ServiceTest
{
    public class TokenServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TokenTest()
        {
            //Arrange
            Mock<IConfigurationSection> configurationJWTSection = new Mock<IConfigurationSection>();
            configurationJWTSection.Setup(x => x.Value).Returns("This is the dummy key which has to be a bit long for the 512. which should be even more longer for the passing");
            Mock<IConfigurationSection> congigTokenSection = new Mock<IConfigurationSection>();
            congigTokenSection.Setup(x => x.GetSection("JWT")).Returns(configurationJWTSection.Object);
            Mock<Iconfiguration> mockConfig = new Mock<Iconfiguration>();
            mockConfig.Setup(x => x.GetSection("TokenKey")).Returns(congigTokenSection.Object);
            ITokenServices service = new TokenServices(mockConfig.Object);

            //Action
            var token = service.GenerateToken(new Member { Id = 103, Role = MemberRole.User });

            //Assert
            Assert.IsNotNull(token);
        }
    }
}

