using Common.Exceptions;
using Data.Cotracts;
using ElmahCore;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyApi.Models;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebFramework.Api;
using WebFramework.Filters;
using Common.Utilities;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace MyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiResultFilter]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<UserController> logger;
        private readonly IJWTService jwtService;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly SignInManager<User> signInManager;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger,
            IJWTService jwtService, UserManager<User> userManager, RoleManager<Role> roleManager,
            SignInManager<User> signInManager)
        {
            this.userRepository = userRepository;
            this.logger = logger;
            this.jwtService = jwtService;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [ApiResultFilter]
        [Authorize(Roles = "Admin")]
        public async Task<List<User>> Get(CancellationToken cancellationToken)
        {
            var t = HttpContext;
            //var userName = HttpContext.User.Identity.GetUserName();
            //userName = HttpContext.User.Identity.Name;
            //var userId = HttpContext.User.Identity.GetUserId();
            //var userIdINT = HttpContext.User.Identity.GetUserId<int>();
            //var phone = HttpContext.User.Identity.FindFirstValue(ClaimTypes.MobilePhone);
            //var roles = HttpContext.User.Identity.FindFirstValue(ClaimTypes.Role);
            var users = await userRepository.TableNoTracking.ToListAsync(cancellationToken);
            return users;
        }

        [HttpGet("{id:int}")]
        public async Task<ApiResult<User>> Get(string id, CancellationToken cancellationToken)
        {
            var user2 = await userManager.FindByIdAsync(id.ToString());

            //var user = await userRepository.GetByIdAsync(cancellationToken, id);
            if (user2 == null)
                return NotFound();
            return user2;
        }
        [HttpPost]
        public async Task<ApiResult<User>> Create(UserDto userDto, CancellationToken cancellationToken)
        {
            logger.LogError("UserController/Create Invoked");
            HttpContext.RiseError(new Exception("UserController/Create Invoked"));//elmah
            //var exist = await userRepository.TableNoTracking.AnyAsync(w => w.Username == userDto.Username);
            var user = new User()
            {
                Age = userDto.Age,
                FullName = userDto.FullName,
                Gender = userDto.Gender,
                UserName = userDto.Username,
                Email = userDto.Username
            };
            var result = await userManager.CreateAsync(user, userDto.Password);
            if (result.Succeeded)
            {
                var result3 = await userManager.AddToRoleAsync(user, "Admin");
                if (result3.Succeeded)
                    return user;
            }
            //var result2 = await roleManager.CreateAsync(new Role()
            //{
            //    Name = "Admin",
            //    Description = "amin role"
            //});

            ////await userRepository.AddAsync(user, cancellationToken);
            //await userRepository.AddAsync(user, userDto.Password, cancellationToken);
            ////return new ApiResult(true, ApiResultStatusCode.Success);
            return user;
        }
        [HttpPut]
        public async Task<ApiResult> Update(int id, User user, CancellationToken cancellationToken)
        {
            var updateUser = await userRepository.GetByIdAsync(cancellationToken, id);

            updateUser.UserName = user.UserName;
            updateUser.PasswordHash = user.PasswordHash;
            updateUser.FullName = user.FullName;
            updateUser.Age = user.Age;
            updateUser.Gender = user.Gender;
            updateUser.IsActive = user.IsActive;
            updateUser.LastLoginDate = user.LastLoginDate;

            await userRepository.UpdateAsync(updateUser, cancellationToken);
            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<string> Token(string username, string password, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(username);
            //var user = await userRepository.GetByUserAndPass(username, password, cancellationToken);
            if (user == null)
                throw new BadRequestException("invalid credentilas");

            var jwt = jwtService.Generate(user);
            return jwt;
        }
        [HttpDelete]
        public async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            await userRepository.DeleteAsync(user, cancellationToken);
            return Ok();
        }
        
    }
}
