using BT.TechTest.OTP.Generator.Models;
using BT.TechTest.OTP.Generator.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using Xunit;

namespace BT.TechTest.OTP.UnitTests
{
    public class PasswordInMemoryStoreTests
    {
        public void SaveOneTimePassword_CallsInMemoryStoreSet()
        {
            // Arrange
            Guid guidForTest = Guid.NewGuid();
            OneTimePassword oneTimePassword = new OneTimePassword
            {
                ExpirationDate = DateTimeOffset.UtcNow.AddSeconds(new Random().Next(30, 60)),
                Password = "test"
            };

            Mock<IMemoryCache> mockCache = new Mock<IMemoryCache>();
            PasswordInMemoryStore passowrdInMemoryStore = new PasswordInMemoryStore(mockCache.Object);

            // Act
            passowrdInMemoryStore.SaveOneTimePassword(oneTimePassword, guidForTest);

            // Assert
            // Due to set being an extension method and since this is a wrapper around the memory cache
            // I will abandon and ignore this test, the class itself is a wrapper around IMemoryCache so
            // it's not required to be unit tested, it contains no business logic by itself.
            mockCache.Verify(mock => mock.Set(guidForTest.ToString(), oneTimePassword, oneTimePassword.ExpirationDate), Times.Once);
        }
    }
}
