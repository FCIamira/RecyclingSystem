using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RecyclingSystem.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.Account.Commands
{
    public class GenerateTokenCommand : IRequest<string>
    {
        public ApplicationUser user { get; set; } = null!;
        public DateTime Expired { get; set; }
    }

    public class GenerateTokenCommandHandler : IRequestHandler<GenerateTokenCommand, string>
    {
        private readonly IConfiguration config;
        private readonly UserManager<ApplicationUser> userManager;

        public GenerateTokenCommandHandler(IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            this.config = config;
            this.userManager = userManager;
        }

        public async Task<string> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
        {
            var jwtKey = config["JWT:Key"];
            if (string.IsNullOrEmpty(jwtKey))
                throw new Exception("JWT:Key not found in configuration");

            string jti = Guid.NewGuid().ToString();
            string userID = request.user.Id.ToString();
            var userRoles = await userManager.GetRolesAsync(request.user);

            List<Claim> claim = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userID),
                new Claim(ClaimTypes.Name, request.user.UserName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, jti)
            };

            if (userRoles != null)
            {
                foreach (var role in userRoles)
                {
                    Console.WriteLine("✅ ROLE IN TOKEN: " + role);
                    claim.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            SymmetricSecurityKey signKey = new(Encoding.UTF8.GetBytes(jwtKey));
            SigningCredentials signingCredential = new(signKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken myToken = new JwtSecurityToken(
                issuer: config["JWT:Iss"],
                audience: config["JWT:Aud"],
                expires: request.Expired,
                claims: claim,
                signingCredentials: signingCredential
            );

            var handler = new JwtSecurityTokenHandler();
            string writtenToken = handler.WriteToken(myToken);

            return writtenToken;
        }
    }
}

