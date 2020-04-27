using BookStore.BusinessLogicLayer.Models;
using BookStore.BusinessLogicLayer.Services;
using BookStore.BusinessLogicLayer.Services.Interfaces;
using BookStore.APILayer.JWTAuthorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using BookStore.API.Layer.Controllers;
using BookStore.BusinessLogicLayer.InputModels;

namespace BookStore.APILayer.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class JWTController : Controller
    {
        private IUserService _serviceUser;
        private IAdminService _serviceAdmin;

        public JWTController(IUserService userService, IAdminService adminService)
        {
            _serviceUser = userService;
            _serviceAdmin = adminService;
        }

        [HttpPost("Token")]
        public async Task Token(Person person)
        {
            ClaimsIdentity identity = GetIdentity(person.Email, person.Password, person.Role);
            if (identity == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid email or password");
                return;
            }
            DateTime now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions._issuer,
                    audience: AuthOptions._audience,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions._lifetime)),
                    signingCredentials: new SigningCredentials(
                                AuthOptions.GetSymmetricSecurityKey(),
                                SecurityAlgorithms.HmacSha256
                                )
                    );
            string encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        [Authorize]
        [HttpGet("GetLogin")]
        public IActionResult GetLogin()
        {
            return Ok($"Ваш email: {User.Identity.Name}");
        }

        [Authorize(Roles = "admin")]
        [HttpGet("isAdmin")]
        public IActionResult IsAdmin()
        {
            return Ok("Ваша роль: администратор");
        }

        [Authorize(Roles = "user")]
        [HttpGet("isUser")]
        public IActionResult IsUser()
        {
            return Ok("Ваша роль: пользователь");
        }

        private ClaimsIdentity GetIdentity(string email, string password, string role)
        {
            UserModel user = _serviceUser.GetByEmailPassword(email, password);
            AdminModel admin = _serviceAdmin.GetByEmailPassword(email, password);
            string emailRole = String.Empty;
            if (user != null)
            {
                emailRole = user.Email;
            }
            if (admin != null)
            {
                emailRole = admin.Email;
            }
            if (user != null || admin != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, emailRole),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
                };

                var claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            if (role == Enum.GetName(typeof(Roles.Role), Roles.Role.user) || role == Enum.GetName(typeof(Roles.Role), Roles.Role.admin))
            {
                _serviceUser.AddItem(new UserInputModel { Email = email, Password = password });
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
                };

                var claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }

        public struct Person
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
        }
    }
}