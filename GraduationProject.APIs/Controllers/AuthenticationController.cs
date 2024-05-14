using GraduationProject.APIs.DTOs;
using GraduationProject.APIs.Errors;
using GraduationProject.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public AuthenticationController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var Email=_userManager.FindByEmailAsync(model.Email);
            if (Email is not null)
                return BadRequest(new APIResponse(400, "You already Has an account"));
            
            var user = new AppUser()
            {
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumner,
                Image = model.Image
            };

            var Result =await _userManager.CreateAsync(user, model.Password);
            if (!Result.Succeeded)
                return BadRequest(Result.Errors.FirstOrDefault());
            var ReturnedUser = new UserDto()
            {
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                Token = "token"
            };
            return Ok(ReturnedUser);
            

        }
    }
}
