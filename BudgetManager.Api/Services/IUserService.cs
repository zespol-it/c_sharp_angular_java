using BudgetManager.Api.DTOs;
using BudgetManager.Api.Models;

namespace BudgetManager.Api.Services
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(UserRegisterDto dto);
        Task<AuthResponseDto> LoginAsync(UserLoginDto dto);
        Task<UserDto> GetByIdAsync(int id);
    }
} 