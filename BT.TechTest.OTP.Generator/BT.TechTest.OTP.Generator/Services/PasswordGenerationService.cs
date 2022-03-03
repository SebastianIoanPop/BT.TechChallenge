using BT.TechTest.OTP.Generator.Extensions;
using BT.TechTest.OTP.Generator.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BT.TechTest.OTP.Generator.Services
{
    public class PasswordGenerationService : IPasswordGenerationService
    {
        public string GeneratePassword(Guid userId, DateTimeOffset dateTime)
        {
            StringBuilder password = new();
            string preHashSalt = CreateSalt(userId, dateTime);

            using SHA512 crypto = SHA512.Create();

            byte[] hashedSalt = crypto.ComputeHash(Encoding.UTF8.GetBytes(preHashSalt));
            int numericalSalt = BitConverter.ToInt32(hashedSalt, 0);
            string absolutValue = Math.Abs(numericalSalt).ToString();

            password.Append(absolutValue);

            Random random = new();

            while (password.Length < 9)
            {
                password.Append(random.Next(0, 10));
            }

            return password.ToString().Substring(0, 9);
        }

        private string CreateSalt(Guid userId, DateTimeOffset dateTime)
        {
            // Adding more elements here increases the number of permutations and decreases the predictability 
            // of the algorithm.
            List<string> saltDeck = new()
            {
                dateTime.Year.ToString(),
                dateTime.Month.ToString(),
                dateTime.Day.ToString(),
                dateTime.Hour.ToString(),
                dateTime.Minute.ToString(),
                dateTime.Second.ToString(),
                userId.ToString()
            };

            saltDeck.FisherYatesShuffle();

            string salt = string.Empty;

            foreach(string value in saltDeck)
            {
                salt += value;
            }

            return salt;
        }
    }
}
