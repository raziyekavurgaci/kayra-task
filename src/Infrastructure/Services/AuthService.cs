using Application.Commands.Auth;
using Application.DTOs.Auth;
using Application.Interfaces;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<TokenDto> HandleAsync(RegisterCommand command)
        {
            var dto = command.RegisterDto;

            // Validasyon
            if (string.IsNullOrWhiteSpace(dto.Username) || dto.Username.Length < 3 || dto.Username.Length > 50)
                throw new ArgumentException("Kullanıcı adı 3-50 karakter arasında olmalıdır");
            
            if (string.IsNullOrWhiteSpace(dto.Email) || !dto.Email.Contains("@"))
                throw new ArgumentException("Geçerli bir email adresi giriniz");
            
            if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                throw new ArgumentException("Şifre en az 6 karakter olmalıdır");
            
            // Username kontrolü
            if (await _userRepository.UsernameExistsAsync(dto.Username))
                throw new ArgumentException($"Kullanıcı adı '{dto.Username}' zaten kullanılıyor");
            
            // Email kontrolü
            if (await _userRepository.EmailExistsAsync(dto.Email))
                throw new ArgumentException($"Email '{dto.Email}' zaten kullanılıyor");
            
            // User oluştur ve kaydet
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "User"
            };
            
            var createdUser = await _userRepository.CreateAsync(user);
            var token = _tokenService.GenerateToken(createdUser);
            
            return new TokenDto
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }

        public async Task<TokenDto> HandleAsync(LoginCommand command)
        {
            var dto = command.LoginDto;

            // Validasyon
            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new ArgumentException("Kullanıcı adı boş olamaz");
            
            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Şifre boş olamaz");
            
            // Kullanıcıyı bul
            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            if (user == null)
                throw new ArgumentException("Kullanıcı adı veya şifre hatalı");
            
            // Şifre kontrolü
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new ArgumentException("Kullanıcı adı veya şifre hatalı");
            
            // Token üret
            var token = _tokenService.GenerateToken(user);
            
            return new TokenDto
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }
    }
}
