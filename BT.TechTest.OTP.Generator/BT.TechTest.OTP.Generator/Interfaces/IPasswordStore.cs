using BT.TechTest.OTP.Generator.Models;
using System;

namespace BT.TechTest.OTP.Generator.Interfaces
{
    public interface IPasswordStore
    {
        void SaveOneTimePassword(OneTimePassword password, Guid userId);
        OneTimePassword GetOneTimePassword(Guid userId); 
    }
}
