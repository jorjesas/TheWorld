using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Jorje.TheWorld.Api.Models;
using JWT.Algorithms;
using JWT.Serializers;
using JWT;
using Microsoft.AspNetCore.Http.Authentication;
using Jorje.TheWorld.Domain;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Jorje.TheWorld.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly UserManager<TripIdentityUser> _userManager;
        private readonly SignInManager<TripIdentityUser> _signInManager;
        private readonly JWTSettings _options;

        public AccountController(
          UserManager<TripIdentityUser> userManager,
          SignInManager<TripIdentityUser> signInManager,
          IOptions<JWTSettings> optionsAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _options = optionsAccessor.Value;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Credentials Credentials)
        {
            if (ModelState.IsValid)
            {
                var user = new TripIdentityUser { UserName = Credentials.Email, Email = Credentials.Email };
                var result = await _userManager.CreateAsync(user, Credentials.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return new JsonResult(new Dictionary<string, object>
                    {
                        { "access_token", GetAccessToken(Credentials.Email) },
                        { "id_token", GetIdToken(user) }
                    });
                }
                return Errors(result);
            }

            return Error("Unexpected error");
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] Credentials Credentials)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Credentials.Email, Credentials.Password, false, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(Credentials.Email);
                    

                    return new JsonResult(new Dictionary<string, object>
                        {
                            { "access_token", GetAccessToken(Credentials.Email) },
                            { "id_token", GetIdToken(user) }
                        });
                }
                return new JsonResult("Unable to sign in") { StatusCode = 401 };
            }
            return Error("Unexpected error");
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetToken([FromBody] Credentials credentials)
        {
            var token = CreateToken(credentials);

            if (token != null)
            {
                return Ok(token);
            }
            else
            {
                return BadRequest("Failed to generate token");
            }
        }

        private object CreateToken(Credentials credentials)
        {
            if (!string.IsNullOrWhiteSpace(credentials.Email))
            {
                var claims = new[]
                {
                  new Claim(JwtRegisteredClaimNames.Sub, credentials.Email),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                  new Claim(JwtRegisteredClaimNames.Email, credentials.Email)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                  issuer: _options.Issuer,
                  audience: _options.Audience,
                  claims: claims,
                  expires: DateTime.UtcNow.AddMinutes(15),
                  signingCredentials: creds
                  );

                return new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                };

            }

            return null;
        }

        private string GetIdToken(IdentityUser user)
        {
            var payload = new Dictionary<string, object>
                {
                    { "id", user.Id },
                    { "sub", user.Email },
                    { "email", user.Email },
                    { "emailConfirmed", user.EmailConfirmed },
                };

            return GetToken(payload);
        }

        private string GetAccessToken(string Email)
        {
            var payload = new Dictionary<string, object>
            {
                { "sub", Email },
                { "email", Email }
            };
            return GetToken(payload);
        }

        private string GetToken(Dictionary<string, object> payload)
        {
            var secret = _options.SecretKey;

            payload.Add("iss", _options.Issuer);
            payload.Add("aud", _options.Audience);
            payload.Add("nbf", ConvertToUnixTimestamp(DateTime.Now));
            payload.Add("iat", ConvertToUnixTimestamp(DateTime.Now));
            payload.Add("exp", ConvertToUnixTimestamp(DateTime.Now.AddDays(7)));
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder.Encode(payload, secret);
        }

        private JsonResult Errors(IdentityResult result)
        {
            var items = result.Errors
                .Select(x => x.Description)
                .ToArray();
            return new JsonResult(items) { StatusCode = 400 };
        }

        private JsonResult Error(string message)
        {
            return new JsonResult(message) { StatusCode = 400 };
        }

        private static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}