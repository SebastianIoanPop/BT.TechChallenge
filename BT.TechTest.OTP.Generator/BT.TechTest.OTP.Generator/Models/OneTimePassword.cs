using System;

namespace BT.TechTest.OTP.Generator.Models
{
    public class OneTimePassword
    {
        public string Password { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
        public bool HasBeenUsed { get; set; }
    }
}
