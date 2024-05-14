using GraduationProject.API.DTOs;
using GraduationProject.APIs.DTOs;
using GraduationProject.APIs.Errors;
using GraduationProject.Core.Entities;
using GraduationProject.Core.Models;
using GraduationProject.Core.ServiceInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public AuthenticationController(UserManager<AppUser> userManager
                                       ,SignInManager<AppUser> signInManager
                                       ,ITokenService tokenService
                                       ,IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _emailService = emailService;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var user = new AppUser()
            {
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumner,
                Image = model.Image
            };

            var Result = await _userManager.CreateAsync(user, model.Password);
            var token = await _tokenService.CreateTokenAsync(user, _userManager);
            //var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { token, user.Email });
            //var Message = new Message(new string[] {user.Email!},"Confirmation Email Link",confirmationLink);
            //_emailService.SendEmail(Message);
            if (!Result.Succeeded)
            {
                return BadRequest(Result.Errors.FirstOrDefault());
            }
            else
            {
                var ReturnedUser = new UserDto()
                {
                    Email = model.Email,
                    UserName = model.Email.Split("@")[0],
                    Token = token
                };
                return Ok(ReturnedUser);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);
            if (User == null)
                return Unauthorized(new APIResponse(401, "You are unauthorized , Register First"));
            var Result = await _signInManager.CheckPasswordSignInAsync(User, model.Password, true);
            if (!Result.Succeeded)
                return Unauthorized(new APIResponse(401, "Your Password Isn't Right"));

            var token = await _tokenService.CreateTokenAsync(User, _userManager);
            return Ok(new UserDto()
            {
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                Token = token
            });



        }


        [HttpGet]
        public async Task<IActionResult> TestEmail()
        {
            var message = new Message(new string[] { "maryamgm3323@gmail.com" }, "Test", "uyvuy");

            _emailService.SendEmail(message);
            return StatusCode(StatusCodes.Status200OK, new APIResponse(200, "Success"));
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token,string email)
        {
            var user =await _userManager.FindByEmailAsync(email);
            if(user is not null)
            {
                var result=await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Email Verified Successfully" });
                }

            }
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "The User Dosen't Exist" });

        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([Required] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is not null)
            {
                var token = await _tokenService.CreateTokenAsync(user, _userManager);
                var link = Url.Action(nameof(ResetPassword), "Authentication", new { token, user.Email },Request.Scheme);
                var Message = new Message(new string[] { user.Email! }, "Forget Password Link", link);
                _emailService.SendEmail(Message);
                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Success", Message = $"Password Changed Request Sent to your email{user.Email}"
                    });
            }
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "Couldn't Send Link to your Email ,Please Try Again" });

        }

        [HttpGet("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var model = new ResetPassword {Token= token,Email=  email };
            return Ok(new
            {
                model
            });
        }
        
        
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if(user != null)
            {
                var ResetPassResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
                if (!ResetPassResult.Succeeded)
                {
                    foreach (var error in ResetPassResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                        
                    }
                    return Ok(ModelState);
                }
                return StatusCode(StatusCodes.Status200OK,
                  new Response
                  {
                      Status = "Success",
                      Message ="Password Has Been Changed "
                  });
               

            }
            return StatusCode(StatusCodes.Status500InternalServerError,
                   new Response { Status = "Error", Message = "Couldn't Send Link to your Email ,Please Try Again" });
        }
    }
}
