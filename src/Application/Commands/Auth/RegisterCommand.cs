using Application.DTOs.Auth;

namespace Application.Commands.Auth
{
    public class RegisterCommand
    {
        public RegisterDto RegisterDto { get; set; } = new();
    }
}
