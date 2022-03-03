using BT.TechTest.OTP.Generator.Services;
using Shouldly;
using System;
using Xunit;

namespace BT.TechTest.OTP.UnitTests
{
    public class PasswordGenerationServiceTests
    {
        [Fact]
        public void GeneratePassword_EnsureOnlyNumericalCharactersAreReturned()
        {
            // Arrange
            PasswordGenerationService passwordGenerationService = new PasswordGenerationService();

            // Act
            string result = passwordGenerationService.GeneratePassword(Guid.NewGuid(), DateTimeOffset.UtcNow);

            // Assert
            result.ShouldAllBe(character => character >= '0' && character <= '9');
        }

        [Fact]
        public void GeneratePassword_EnsurePasswordHasTheCorrectLength()
        {
            // Arrange
            PasswordGenerationService passwordGenerationService = new PasswordGenerationService();

            // Act
            string result = passwordGenerationService.GeneratePassword(Guid.NewGuid(), DateTimeOffset.UtcNow);

            // Assert
            result.ShouldNotBeNullOrEmpty();
            result.Length.ShouldBe(9);
        }
    }
}
