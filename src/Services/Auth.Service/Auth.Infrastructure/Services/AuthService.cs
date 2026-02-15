using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Auth.Core.Entities;
using Auth.Core.Interfaces;

namespace Auth.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<TokenDto> RegisterAsync(RegisterDto dto)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(dto.Username) || dto.Username.Length < 3 || dto.Username.Length > 50)
            throw new ArgumentException("Username must be between 3-50 characters");

        if (string.IsNullOrWhiteSpace(dto.Email) || !dto.Email.Contains("@"))
            throw new ArgumentException("Invalid email format");

        if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
            throw new ArgumentException("Password must be at least 6 characters");

        // Check if user exists
        if (await _userRepository.UsernameExistsAsync(dto.Username))
            throw new InvalidOperationException("Username already exists");

        if (await _userRepository.EmailExistsAsync(dto.Email))
            throw new InvalidOperationException("Email already exists");

        // Create user
        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "User",
            RefreshToken = _tokenService.GenerateRefreshToken(),
            RefreshTokenExpiry = DateTime.UtcNow.AddDays(7)
        };

        await _userRepository.CreateAsync(user);

        // Generate tokens
        var accessToken = _tokenService.GenerateAccessToken(user);

        return new TokenDto
        {
            Token = accessToken,
            RefreshToken = user.RefreshToken!,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };
    }

    public async Task<TokenDto> LoginAsync(LoginDto dto)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
            throw new ArgumentException("Username and password are required");

        // Get user
        var user = await _userRepository.GetByUsernameAsync(dto.Username);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid credentials");

        // Verify password
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials");

        // Update refresh token
        user.RefreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _userRepository.UpdateAsync(user);

        // Generate access token
        var accessToken = _tokenService.GenerateAccessToken(user);

        return new TokenDto
        {
            Token = accessToken,
            RefreshToken = user.RefreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };
    }

    public async Task<TokenDto> RefreshTokenAsync(RefreshTokenDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.RefreshToken))
            throw new ArgumentException("Refresh token is required");

        // Find user by refresh token
        var user = await _userRepository.GetByRefreshTokenAsync(dto.RefreshToken);
        if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            throw new UnauthorizedAccessException("Invalid or expired refresh token");

        // Generate new tokens
        user.RefreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _userRepository.UpdateAsync(user);

        var accessToken = _tokenService.GenerateAccessToken(user);

        return new TokenDto
        {
            Token = accessToken,
            RefreshToken = user.RefreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        return await Task.FromResult(_tokenService.ValidateToken(token));
    }
}
