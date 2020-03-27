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
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
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
            await signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost("seedRoles")]
        public async Task<IActionResult> SeedRoles()
        {
            await roleManager.CreateAsync(new IdentityRole("adminLevel"));
            return Ok();
        }

        [HttpPost("addRolesToUser")]
        public async Task<IActionResult> AddRoles()
        {
            var user = await userManager.GetUserAsync(User);
            await userManager.AddToRoleAsync(user, "adminLevel");
            return Ok();
        }

        [HttpGet("secure_role")]
        [Authorize(Roles = "adminLevel")]
        public async Task<IActionResult> SecureRoles()
        {
            return Ok();
        }

        // just to show how the default role based authorization would be working
        [HttpGet("secure_policy")]
        [Authorize(Policy = "adminLevel")]
        public async Task<IActionResult> SecurePolicy()
        {
            return Ok();
        }

    }
}