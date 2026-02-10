using Application.DTOs.Auth;

namespace Application.Commands.Auth
{
    public class LoginCommand
    {
        public LoginDto LoginDto { get; set; } = new();
    }
}
