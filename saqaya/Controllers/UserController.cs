using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLayer.IServices;
using SharedDTO;
using SharedDTO.ControllerDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Luftborn.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> Logger;
        private readonly IUserService UserService;
        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            Logger = logger;
            UserService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<UserDto>>>> GetAllUsers()
        {
            try
            {
                Logger.LogInformation("begin GetAllUsers");
                var response = await UserService.GetAllUsers();
                return handleStatus(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "GetAllUsers");
                return BadRequest($"Internal server error with exception{ex.InnerException}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUserById([Required] string id)
        {
            try
            {
                Logger.LogInformation("begin GetUserById");
                var response = await UserService.GetUserById(id);
                return handleStatus(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "GetUserById");
                return BadRequest($"Internal server error with exception{ex.InnerException}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ReturnCreateUserDto>>> CreateUser(CreateUserDto userDto)
        {
            try
            {
                Logger.LogInformation("begin CreateUser");
                var response = await UserService.CreateUser(userDto);
                return handleStatus(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CreateUser");
                return BadRequest($"Internal server error with exception{ex.InnerException}");
            }
        }


        #region Private Methods
        private ActionResult<ApiResponse<T>> handleStatus<T>(ApiResponse<T> response)
        {
            if (response.Status == (int)SharedEnums.ApiResponseStatus.Success)
            {
                return Ok(response);
            }
            else if (response.Status == (int)SharedEnums.ApiResponseStatus.Created)
            {
                return Created("Created", response);
            }
            else if (response.Status == (int)SharedEnums.ApiResponseStatus.NotFound)
            {
                return NotFound();
            }
            else
            {
                return BadRequest(response);
            }
        }

        #endregion
    }
}
