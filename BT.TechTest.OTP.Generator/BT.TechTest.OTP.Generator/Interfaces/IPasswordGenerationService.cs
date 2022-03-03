using System;

namespace BT.TechTest.OTP.Generator.Interfaces
{
    public interface IPasswordGenerationService
    {
        string GeneratePassword(Guid userId, DateTimeOffset dateTime);
    }
}
