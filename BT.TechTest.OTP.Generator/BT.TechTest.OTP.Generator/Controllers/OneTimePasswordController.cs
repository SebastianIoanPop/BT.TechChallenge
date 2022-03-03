using BT.TechTest.OTP.Generator.Interfaces;
using BT.TechTest.OTP.Generator.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BT.TechTest.OTP.Generator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OneTimePasswordController : ControllerBase
    {
        private const int OTP_EXPIRATION_IN_MS = 30000;

        private readonly IPasswordGenerationService _passwordGenerationService;
        private readonly IPasswordStore _passwordStore;

        public OneTimePasswordController(IPasswordGenerationService passwordGenerationService,
            IPasswordStore passwordStore)
        {
            _passwordGenerationService = passwordGenerationService;
            _passwordStore = passwordStore;
        }

        /// <summary>
        /// I've taken the freedom to simplify the request as I believed the date time input was not necessary
        /// for the endpoint, however below there's an override which takes the date time as per requirement.
        /// 
        /// The requirement does not specifiy if the expiry should be 30 seconds after the input date therefor
        /// it has little relevance towards how the actual implementation works.
        /// 
        /// Unless you'd like to test that the password generated with the same datetime is always the same,
        /// which is not due to the algorithm I've used, however using the same datetime will eventually give you
        /// the same results.
        /// 
        /// See the Fisher Yates Shuffle in ListExtensions.
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        public OneTimePassword Post(Guid userId)
        {
            return Post(userId, DateTimeOffset.UtcNow);
        }

        [HttpPost]
        [Route("withDate")]
        public OneTimePassword Post(Guid userId, DateTimeOffset dateTime)
        {
            string password = _passwordGenerationService.GeneratePassword(userId, dateTime);
            OneTimePassword otp = new()
            {
                Password = password,
                ExpirationDate = DateTimeOffset.UtcNow.AddMilliseconds(OTP_EXPIRATION_IN_MS),
                HasBeenUsed = false
            };

            _passwordStore.SaveOneTimePassword(otp, userId);

            return otp;
        }

        [HttpPut]
        public string Put(Guid userId, string passwordInput)
        {
            OneTimePassword otp = _passwordStore.GetOneTimePassword(userId);

            if (otp == null || DateTimeOffset.Compare(DateTimeOffset.UtcNow, otp.ExpirationDate) > 0)
            {
                return "Unfortunately your one time password has expired, please create a new one.";
            }

            if (otp.HasBeenUsed)
            {
                return "This password has already been used, please request a new one";
            }

            if (otp.Password.Equals(passwordInput))
            {
                // A more optimised alternative would be to invalidate the cache for the userId
                // However a missing password from the cache would provide less information on the current state
                // For either logging purposes or user feedback.
                otp.HasBeenUsed = true;
                _passwordStore.SaveOneTimePassword(otp, userId);
                return "You've entered the correct one time password!";
            }

            return "Please request a new one time password";
        }
    }
}
