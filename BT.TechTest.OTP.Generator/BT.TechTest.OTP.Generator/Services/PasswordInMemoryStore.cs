using BT.TechTest.OTP.Generator.Interfaces;
using BT.TechTest.OTP.Generator.Models;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace BT.TechTest.OTP.Generator.Services
{
    /// <summary>
    /// Memory store ideal only for prototype purposes.
    /// </summary>
    public class PasswordInMemoryStore : IPasswordStore
    {
        private readonly IMemoryCache _memoryCache;

        public PasswordInMemoryStore(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Assumption: User can only have 1 OTP active at a time.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public OneTimePassword GetOneTimePassword(Guid userId)
        {
            _memoryCache.TryGetValue(userId.ToString(), out OneTimePassword password);
            return password;
        }

        public void SaveOneTimePassword(OneTimePassword password, Guid userId)
        {
            _memoryCache.Set(userId.ToString(), password, password.ExpirationDate);
        }
    }
}
