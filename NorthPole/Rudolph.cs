using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace NorthPole
{
    public class Rudolph
    {
        public static string GenerateHMAC(string input, string secret)
        {
            var enc = Encoding.UTF8;

            var secretBytes = enc.GetBytes(secret);
            var inputBytes = enc.GetBytes(input);

            var hmacsha1 = new HMACSHA1(secretBytes);

            var stream = new MemoryStream(inputBytes);

            return Convert.ToBase64String(hmacsha1.ComputeHash(stream));
        }
    }
}