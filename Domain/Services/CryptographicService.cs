﻿using Domain.Models;
using Domain.Interfaces;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Services
{
    public class CryptographicService : ICryptographicService
    {
        private readonly PasswordConfig _passwordConfig;

        public CryptographicService(IOptions<PasswordConfig> passwordOptions)
        {
            _passwordConfig = passwordOptions.Value;
        }

        public Password HashPassword(string password, string? salt = null)
        {
            var saltBytes = salt != null ? 
                Convert.FromBase64String(salt) :
                RandomNumberGenerator.GetBytes(16);
            var pepperBytes = Encoding.UTF8.GetBytes(_passwordConfig.Pepper);
            var passwordBytes = Encoding.UTF8
                .GetBytes(password)
                .Concat(saltBytes)
                .Concat(pepperBytes)
                .ToArray();
            var hashedBytes = SHA256.HashData(passwordBytes);

            return new Password
            {
                Hash = Convert.ToBase64String(hashedBytes),
                Salt = Convert.ToBase64String(saltBytes)
            };
        }
    }
}