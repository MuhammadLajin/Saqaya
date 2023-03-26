
using System.Threading.Tasks;
using System.Collections.Generic;
using SharedDTO.ControllerDtos;
using SharedDTO;

namespace ServiceLayer.IServices
{
    public interface IUserService
    {
        Task<ApiResponse<List<UserDto>>> GetAllUsers();
        Task<ApiResponse<UserDto>> GetUserById(string id);
        Task<ApiResponse<ReturnCreateUserDto>> CreateUser(CreateUserDto userDto);
    
    }
}