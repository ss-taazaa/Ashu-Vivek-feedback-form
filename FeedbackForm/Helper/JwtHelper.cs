using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using global::FeedbackForm.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


namespace FeedbackForm.Helper
    {
        public class JwtHelper
        {
            private readonly JwtSettings _jwtSettings;

            public JwtHelper(IOptions<JwtSettings> jwtSettings)
            {
            Console.WriteLine("JWT Key Length: " + _jwtSettings.Key.Length);

            _jwtSettings = jwtSettings.Value;

            }

            public string GenerateToken(User user)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", user.Id.ToString()) // Custom claim
            };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                    Issuer = _jwtSettings.Issuer,
                    Audience = _jwtSettings.Audience,
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
        }
    }
