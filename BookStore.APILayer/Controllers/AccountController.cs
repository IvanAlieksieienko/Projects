using BookStore.BusinessLogicLayer.Models;
using BookStore.BusinessLogicLayer.Services;
using BookStore.BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Net.Mail;
using System.Net;
using BookStore.API.Layer;
using Microsoft.Extensions.Options;
using BookStore.API.Layer.Controllers;
using BookStore.BusinessLogicLayer.InputModels;

namespace BookStore.APILayer.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private IUserService _serviceUser;
        private IAdminService _serviceAdmin;
        private readonly MyOptions _options;

        public AccountController(IAdminService serviceAdmin, IUserService serviceUser, IOptionsMonitor<MyOptions> optionsAccessor)
        {
            _serviceUser = serviceUser;
            _serviceAdmin = serviceAdmin;
            _options = optionsAccessor.CurrentValue;
        }

        [Authorize]
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            UserModel user = _serviceUser.GetByEmailPassword(model.Email, model.Password);
            AdminModel admin = _serviceAdmin.GetByEmailPassword(model.Email, model.Password);
            if (user != null)
            {
                model.Role = Roles.Role.user.ToString();
                Authenticate(model.Email, model.Role);
                return Ok(model);
            }
            if (admin != null)
            {
                model.Role = Roles.Role.admin.ToString();
                Authenticate(model.Email, model.Role);
                return Ok(model);
            }
            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            return View(model);
        }

        [HttpGet("Confirm/{id:int}")]
        public IActionResult Confirm(int id)
        {
            _serviceUser.Confirm(id);
            return Ok();
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]
        public IActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            UserModel user = _serviceUser.GetByEmailPassword(model.Email, model.Password);
            if (user == null)
            {
                _serviceUser.AddItem(new UserInputModel { Email = model.Email, Password = model.Password });
                SendEmailAsync(model.Email);
                return RedirectToAction("Index", "User");
            }
            if (user != null)
            {
                ModelState.AddModelError("", "Неккоректные логин и(или) пароль");
            }
            return View(model);
        }

        public void SendEmailAsync(string email)
        {
            var fromAddress = new MailAddress(_options.FromAdress, _options.FromAddressDisplayName);
            var toAddress = new MailAddress(email, _options.ToAddressDisplayName);
            string fromPassword = _options.FromPassword;
            string subject = _options.Subject;
            string body = _options.Body + _serviceUser.GetByEmail(email).ID.ToString();

            var smtp = new SmtpClient
            {
                Host = _options.Host,
                Port = _options.Port,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        private void Authenticate(string userName, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };
            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        [HttpGet("AuthorizeRole")]
        public string IsAdmin()
        {
            bool isAdmin = HttpContext.User.IsInRole(Roles.Role.admin.ToString());
            bool isUser = HttpContext.User.IsInRole(Roles.Role.admin.ToString());
            if (isAdmin)
            {
                return Roles.Role.admin.ToString();
            }
            if (isUser)
            {
                return Roles.Role.user.ToString();
            }
            return Roles.Role.unauthorized.ToString();
        }

        [HttpGet("GetCurrentUser")]
        public IActionResult GetCurrentUser()
        {
            string userEmail = HttpContext.User.Identity.Name;
            if (IsAdmin() == Roles.Role.user.ToString())
            {

                return Ok(new CurrentUser
                {
                    ID = _serviceUser.GetByEmail(userEmail).ID,
                    Email = _serviceUser.GetByEmail(userEmail).Email,
                    Password = _serviceUser.GetByEmail(userEmail).Password,
                    Role = _serviceUser.GetByEmail(userEmail).Role
                });
            }
            if (IsAdmin() == Roles.Role.admin.ToString())
            {
                return Ok(new CurrentUser
                {
                    ID = _serviceAdmin.GetByEmail(userEmail).ID,
                    Email = _serviceAdmin.GetByEmail(userEmail).Email,
                    Password = _serviceAdmin.GetByEmail(userEmail).Password,
                    Role = _serviceAdmin.GetByEmail(userEmail).Role
                });
            }
            return Ok();
        }

        [HttpGet("GetCurrentUserID")]
        public IActionResult GetCurrentUserID(string Email)
        {
            return Ok(_serviceUser.GetByEmail(Email).ID);
        }

        private struct CurrentUser
        {
            public int ID { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
        }
    }
}