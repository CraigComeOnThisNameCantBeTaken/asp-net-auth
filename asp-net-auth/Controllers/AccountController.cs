using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp_net_auth.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace asp_net_auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUser dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return NotFound();
            }

            var signInResult = await signInManager.PasswordSignInAsync(user, dto.Password, true, false);
            if (!signInResult.Succeeded)
            {
                return BadRequest();
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUser dto)
        {
            var registerResult = await userManager.CreateAsync(new IdentityUser
            {
                UserName = dto.Username,
                Email = dto.Email
            }, dto.Password);

            if (!registerResult.Succeeded)
            {
                return BadRequest(registerResult.Errors);
            }

            return CreatedAtAction(nameof(Register), null);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var user = User;
            await signInManager.SignOutAsync();
            return Ok();
        }
    }
}