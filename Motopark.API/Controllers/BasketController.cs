using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Motopark.Core.Entities;
using Motopark.Core.IServices;

namespace Motopark.API.Controllers
{
    [Route("basket")]
    [ApiController]
    public class BasketController : Controller
    {
        private IBasketService<Basket> _basketService;

        public BasketController(IBasketService<Basket> basketService)
        {
            _basketService = basketService;
        }

        [HttpGet("get/{id:guid}")]
        public async Task<IActionResult> GetByBasketID(Guid id)
        {
            var baskets = await _basketService.GetByBasketID(id);
            return Ok(baskets);
        }

        [Authorize]
        [HttpGet("getID")]
        public async Task<IActionResult> GetBasketID()
        {
            return Ok(Guid.Parse(HttpContext.User.Identity.Name));
        }

        [HttpPost()]
        public async Task<IActionResult> Add(Basket item)
        {
            if (item.ID == null || item.ID.ToString() == "" || item.ID == Guid.Empty)
                item.ID = Guid.NewGuid();
            else item.ID = Guid.Parse(HttpContext.User.Identity.Name);
            var basket = await _basketService.Add(item);
            await Authenticate(item.ID);
            return Ok(basket);
        }

        [HttpGet("cookiesAuthentication/{idg:guid}")]
        private async Task Authenticate(Guid idg)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, idg.ToString())
            };
            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [HttpPost("update")]
        public async Task<IActionResult> ChangeCount(Basket item)
        {
            var id = item.ID;
            var count = item.Count;
            var basket = await _basketService.ChangeCount(item.ID, item.ProductID, count);
            return Ok(basket);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> SignOut(Guid id)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var newID = Guid.NewGuid();
            await Authenticate(newID);
            return Ok(newID);
        }

        [HttpDelete("product/{id:guid}/{basketId:guid}")]
        public async Task<IActionResult> DeleteByProduct(Guid id, Guid basketId)
        {
            await _basketService.DeleteByProduct(id, basketId);
            return Ok();
        }
    }
}