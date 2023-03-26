using AutoMapper;
using DomainLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.IRepo;
using RepositoryLayer.UnitOfWork;
using ServiceLayer.IServices;
using SharedDTO;
using SharedDTO.ControllerDtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ServiceLayer.Services
{
    public class UserService : IUserService
    {
        #region Props
        public IUserRepo UserRepo { get; }
        public IMapper Mapper { get; }
        IUnitOfWork UnitofWork { get; }
        IConfiguration Configuration { get; }
        #endregion

        public UserService(IUserRepo userRepo, IMapper mapper, IUnitOfWork unitofWork, IConfiguration configuration)
        {
            UserRepo = userRepo;
            Mapper = mapper;
            UnitofWork = unitofWork;
            Configuration = configuration;
        }

        #region Methods

        public async Task<ApiResponse<List<UserDto>>> GetAllUsers()
        {
            ApiResponse<List<UserDto>> response = new ApiResponse<List<UserDto>>();

            try
            {
                List<User> userList = await UserRepo.GetWhereAsync(x => x.IsDeleted != true);
                if (userList != default)
                {
                    response.IsValidReponse = true;
                    response.CommandMessage = "return list of all users";
                    List<UserDto> userDtoList = new();
                    foreach (User user in userList)
                    {
                        userDtoList.Add(Mapper.Map<UserDto>(user));
                    }
                    response.Datalist = userDtoList;
                    response.TotalCount = userList.Count;
                    response.Status = (int)SharedEnums.ApiResponseStatus.Success;
                }
            }
            catch (Exception ex)
            {
                HandleExceptionResponse(response, ex.InnerException.ToString());
            }

            return response;
        }

        public async Task<ApiResponse<UserDto>> GetUserById(string id)
        {
            ApiResponse<UserDto> response = new ApiResponse<UserDto>();

            try
            {
                User user = await UserRepo.GetById(id);
                if (user != default)
                {
                    response.IsValidReponse = true;
                    response.CommandMessage = "return user";

                    #region Try to deserialize
                    var x = Mapper.Map<UserDto>(user);
                    JsonSerializerOptions options = new()
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    };
                    #endregion
                    string convt =
                        JsonSerializer.Serialize<UserDto>(x, options);

                    response.Datalist = Mapper.Map<UserDto>(user);
                    response.TotalCount = 1;
                    response.Status = (int)SharedEnums.ApiResponseStatus.Success;
                }
                else
                {
                    response.Status = (int)SharedEnums.ApiResponseStatus.NotFound;
                }
            }
            catch (Exception ex)
            {
                HandleExceptionResponse(response, ex.InnerException.ToString());
            }

            return response;
        }

        public async Task<ApiResponse<ReturnCreateUserDto>> CreateUser(CreateUserDto userDto)
        {
            var response = new ApiResponse<ReturnCreateUserDto>();//IsValidUser(userDto);

            try
            {
                User newUser = Mapper.Map<User>(userDto);
                newUser.CreatedAt = DateTime.Now;
                newUser.Id = generateSha1(newUser.email + Configuration["settings:Sha1Key"]);
                newUser.accessToken = GenerateToken(newUser);
                User user = UserRepo.Insert(newUser);
                bool isSuccess = await UnitofWork.SaveChangesAsync();
                if (isSuccess)
                {
                    response.CommandMessage = "new user has been added";
                    response.Datalist = Mapper.Map<ReturnCreateUserDto>(user);
                    response.TotalCount = 1;
                    response.Status = (int)SharedEnums.ApiResponseStatus.Created;
                }
            }
            catch (Exception ex)
            {
                HandleExceptionResponse(response, ex.InnerException.ToString());
            }
            return response;
        }



        #endregion

        #region private Methods


        private void HandleExceptionResponse<T>(ApiResponse<T> response, string message)
        {
            response.IsValidReponse = false;
            response.CommandMessage = $"error raised : {message}";
            response.Datalist = default;
            response.Status = (int)SharedEnums.ApiResponseStatus.BadRequest;
        }
        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.firstName)
            };
            var token = new JwtSecurityToken(Configuration["Jwt:Issuer"],
                Configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        string generateSha1(string source)
        {
            using (SHA1 sha1Hash = SHA1.Create())
            {
                //From String to byte array
                byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
                byte[] hashBytes = sha1Hash.ComputeHash(sourceBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);

                return hash;
            }
        }
        #endregion
    }
}
