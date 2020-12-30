using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using DutchTreat.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace DutchTreat.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _config;
        private readonly SignInManager<StoreUser> _signInManager;
        private readonly UserManager<StoreUser> _userManager;
        public AccountController(
            IConfiguration config,
            ILogger<AccountController> logger,
            SignInManager<StoreUser> signInManager, UserManager<StoreUser> userManager)
        {
            _logger = logger;
            _config = config;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "App");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vmodel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager
                .PasswordSignInAsync(
                    vmodel.Username,
                    vmodel.Password,
                    vmodel.RememberMe,
                    false);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }
                    else
                    {
                        return RedirectToAction("Shop", "App");
                    }
                }
            }
            ModelState.AddModelError("", "Failed to login");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "App");
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel vmodel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(vmodel.Username);
                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, vmodel.Password, false);
                    if (result.Succeeded)
                    {
                        // create the token
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                        var signingCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _config["Tokens:Issuer"],
                        _config["Tokens:Issuer"],
                        claims,
                        expires: DateTime.UtcNow.AddDays(3),
                        signingCredentials: signingCreds
                        );
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return Created("", results);
                    }
                }


            }
            return BadRequest();
        }

    }
}
