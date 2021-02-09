using Data.Cotracts;
using ElmahCore;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebFramework.Api;
using WebFramework.Filters;

namespace MyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiResultFilter]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<UserController> logger;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger)
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }

        [HttpGet]
        [ApiResultFilter]
        public async Task<List<User>> Get(CancellationToken cancellationToken)
        {
            var users = await userRepository.TableNoTracking.ToListAsync(cancellationToken);
            return users;
        }

        [HttpGet("{id:int}")]
        public async Task<ApiResult<User>> Get(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            if (user == null)
                return NotFound();
            return user;
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
                Username = userDto.Username
            };
            //await userRepository.AddAsync(user, cancellationToken);
            await userRepository.AddAsync(user, userDto.Password, cancellationToken);
            //return new ApiResult(true, ApiResultStatusCode.Success);
            return user;
        }
        [HttpPut]
        public async Task<ApiResult> Update(int id, User user, CancellationToken cancellationToken)
        {
            var updateUser = await userRepository.GetByIdAsync(cancellationToken, id);

            updateUser.Username = user.Username;
            updateUser.PasswordHash = user.PasswordHash;
            updateUser.FullName = user.FullName;
            updateUser.Age = user.Age;
            updateUser.Gender = user.Gender;
            updateUser.IsActive = user.IsActive;
            updateUser.LastLoginDate = user.LastLoginDate;

            await userRepository.UpdateAsync(updateUser, cancellationToken);
            return Ok();
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
