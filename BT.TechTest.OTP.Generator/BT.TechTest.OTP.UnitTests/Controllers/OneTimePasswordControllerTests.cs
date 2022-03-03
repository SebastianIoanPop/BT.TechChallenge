using BT.TechTest.OTP.Generator.Controllers;
using BT.TechTest.OTP.Generator.Interfaces;
using BT.TechTest.OTP.Generator.Models;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BT.TechTest.OTP.UnitTests.Controllers
{
    public class OneTimePasswordControllerTests
    {
        [Fact]
        public void Post_ShouldGenerateAndSavePassword()
        {
            // Arrange
            Guid testGuid = Guid.NewGuid();
            Mock<IPasswordGenerationService> passwordGenerationServiceMock = new Mock<IPasswordGenerationService>();
            Mock<IPasswordStore> passwordStoreMock = new Mock<IPasswordStore>();

            OneTimePasswordController oneTimePasswordController = new OneTimePasswordController(passwordGenerationServiceMock.Object, passwordStoreMock.Object);

            // Act
            oneTimePasswordController.Post(testGuid);

            // Assert
            passwordGenerationServiceMock.Verify(mock => mock.GeneratePassword(testGuid, It.IsAny<DateTimeOffset>()), Times.Once);
            passwordStoreMock.Verify(mock => mock.SaveOneTimePassword(It.IsAny<OneTimePassword>(), testGuid));
        }
        
        [Fact]
        public void Put_ShouldGetPasswordFromStore()
        {
            // Arrange
            Guid testGuid = Guid.NewGuid();
            Mock<IPasswordGenerationService> passwordGenerationServiceMock = new Mock<IPasswordGenerationService>();
            Mock<IPasswordStore> passwordStoreMock = new Mock<IPasswordStore>();

            OneTimePasswordController oneTimePasswordController = new OneTimePasswordController(passwordGenerationServiceMock.Object, passwordStoreMock.Object);

            // Act
            oneTimePasswordController.Put(testGuid, string.Empty);

            // Assert
            passwordStoreMock.Verify(mock => mock.GetOneTimePassword(testGuid));
        }

        [Fact]
        public void Put_ShouldReturnFailureMessageIfPasswordNotFound()
        {
            // Arrange
            Guid testGuid = Guid.NewGuid();
            Mock<IPasswordGenerationService> passwordGenerationServiceMock = new Mock<IPasswordGenerationService>();
            Mock<IPasswordStore> passwordStoreMock = new Mock<IPasswordStore>();

            OneTimePasswordController oneTimePasswordController = new OneTimePasswordController(passwordGenerationServiceMock.Object, passwordStoreMock.Object);

            // Act
            string message = oneTimePasswordController.Put(testGuid, string.Empty);

            // Assert
            message.ShouldBe("Unfortunately your one time password has expired, please create a new one.");
        }

        [Fact]
        public void Put_ShouldReturnFailureMessageIfExpired()
        {
            // Arrange
            Guid testGuid = Guid.NewGuid();
            Mock<IPasswordGenerationService> passwordGenerationServiceMock = new Mock<IPasswordGenerationService>();
            Mock<IPasswordStore> passwordStoreMock = new Mock<IPasswordStore>();
            passwordStoreMock.Setup(_ => _.GetOneTimePassword(It.IsAny<Guid>()))
                .Returns(new OneTimePassword { ExpirationDate = DateTimeOffset.UtcNow.AddSeconds(-5) });

            OneTimePasswordController oneTimePasswordController = new OneTimePasswordController(passwordGenerationServiceMock.Object, passwordStoreMock.Object);

            // Act
            string message = oneTimePasswordController.Put(testGuid, string.Empty);

            // Assert
            message.ShouldBe("Unfortunately your one time password has expired, please create a new one.");
        }

        [Fact]
        public void Put_ShouldReturnFailureMessageIfPasswordHasBeenUsed()
        {
            // Arrange
            Guid testGuid = Guid.NewGuid();
            Mock<IPasswordGenerationService> passwordGenerationServiceMock = new Mock<IPasswordGenerationService>();
            Mock<IPasswordStore> passwordStoreMock = new Mock<IPasswordStore>();
            passwordStoreMock.Setup(_ => _.GetOneTimePassword(It.IsAny<Guid>()))
                .Returns(new OneTimePassword { HasBeenUsed = true, ExpirationDate = DateTimeOffset.UtcNow.AddSeconds(2) });

            OneTimePasswordController oneTimePasswordController = new OneTimePasswordController(passwordGenerationServiceMock.Object, passwordStoreMock.Object);

            // Act
            string message = oneTimePasswordController.Put(testGuid, string.Empty);

            // Assert
            message.ShouldBe("This password has already been used, please request a new one");
        }

        [Fact]
        public void Put_ShouldValidatePasswordsMatch()
        {
            // Arrange
            Guid testGuid = Guid.NewGuid();
            string testPassword = "123493233";
            Mock<IPasswordGenerationService> passwordGenerationServiceMock = new Mock<IPasswordGenerationService>();
            Mock<IPasswordStore> passwordStoreMock = new Mock<IPasswordStore>();
            passwordStoreMock.Setup(_ => _.GetOneTimePassword(It.IsAny<Guid>()))
                .Returns(new OneTimePassword { Password = testPassword, ExpirationDate = DateTimeOffset.UtcNow.AddSeconds(2) });

            OneTimePasswordController oneTimePasswordController = new OneTimePasswordController(passwordGenerationServiceMock.Object, passwordStoreMock.Object);

            // Act
            string message = oneTimePasswordController.Put(testGuid, testPassword);

            // Assert
            message.ShouldBe("You've entered the correct one time password!");
            passwordStoreMock.Verify(_ => _.SaveOneTimePassword(It.IsAny<OneTimePassword>(), testGuid), Times.Once);
        }
    }
}
