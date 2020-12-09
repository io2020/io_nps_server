using DotNetCore.Security;
using Microsoft.Extensions.Logging;
using Nps.Application.Account.Dtos;
using Nps.Core.Aop.Attributes;
using Nps.Infrastructure;
using Nps.Infrastructure.Exceptions;
using Nps.Infrastructure.Helpers;
using Nps.Core.Repositories;
using Nps.Core.Security;
using Nps.Core.Services;
using Nps.Data.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Nps.Application.Account.Services
{
    /// <summary>
    /// Jwt认证服务
    /// </summary>
    public class JwtTokenService : DomainService, ITokenService
    {
        private readonly IFreeSqlRepository<User> _userRepository;

        private readonly IJsonWebTokenService _jsonWebTokenService;

        private readonly ILogger<JwtTokenService> _logger;

        public JwtTokenService(
            IFreeSqlRepository<User> userRepository,
            IJsonWebTokenService jsonWebTokenService,
            ILogger<JwtTokenService> logger)
        {
            _userRepository = userRepository;
            _jsonWebTokenService = jsonWebTokenService;
            _logger = logger;
        }

        [Caching(AbsoluteExpiration = 15, ExpirationType = ExpirationType.Day)]
        public async Task<Tokens> LoginAsync(LoginInput input)
        {
            _logger.LogInformation("Login With Jwt Begin;");

            var user = await _userRepository
                .Where(x => x.UserName == input.UserName || x.Email == input.UserName)
                .ToOneAsync();

            if (user == null)
            {
                throw new NpsException("用户不存在", StatusCode.NotFound);
            }

            bool valid = EncryptHelper.Md5By32(input.Password) == user.Password;

            if (!valid)
            {
                throw new NpsException("请输入正确密码", StatusCode.ParameterError);
            }

            _logger.LogInformation($"用户{input.UserName}，登录成功");

            Tokens tokens = await CreateTokenAsync(user);
            return tokens;
        }

        public async Task<Tokens> GetRefreshTokenAsync(string refreshToken)
        {
            var user = await _userRepository
                .Where(x => x.RefreshToken == refreshToken)
                .ToOneAsync();

            if (user.IsNull())
            {
                throw new NpsException("该refreshToken无效!");
            }

            if (DateTime.Compare(user.LastLoginTime, DateTime.Now) > new TimeSpan(30, 0, 0, 0).Ticks)
            {
                throw new NpsException("请重新登录", StatusCode.RefreshTokenError);
            }

            Tokens tokens = await CreateTokenAsync(user);
            _logger.LogInformation($"用户{user.UserName},JwtRefreshToken 刷新-登录成功");

            return tokens;
        }

        private async Task<Tokens> CreateTokenAsync(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim (ClaimTypes.Name, user.UserName?? ""),
                new Claim (ClaimTypes.Email, user.Email?? ""),
                new Claim (ClaimTypes.GivenName, user.NikeName?? ""),
                new Claim (ClaimTypes.MobilePhone, user.Mobile?? "")
            };
            //添加角色信息
            //TODO

            string token = _jsonWebTokenService.Encode(claims);

            string refreshToken = GenerateToken();
            user.LastLoginTime = DateTime.Now;
            user.RefreshToken = refreshToken;
            await _userRepository.UpdateAsync(user);

            var jwtToken = _jsonWebTokenService.Decode(token);
            return new Tokens(token, refreshToken, jwtToken["exp"]?.ToString());
        }

        private static string GenerateToken(int size = 32)
        {
            var randomNumber = new byte[size];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return randomNumber.ToBase64String();
        }
    }
}