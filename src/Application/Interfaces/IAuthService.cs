using Application.Commands.Auth;
using Application.DTOs.Auth;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<TokenDto> HandleAsync(RegisterCommand command);
        Task<TokenDto> HandleAsync(LoginCommand command);
    }
}
