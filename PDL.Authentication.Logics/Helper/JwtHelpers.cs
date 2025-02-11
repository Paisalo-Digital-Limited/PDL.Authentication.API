using Microsoft.IdentityModel.Tokens;
using PDL.Authentication.Entites.VM;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PDL.Authentication.Logics.Helper
{
    public static class JwtHelpers
    {
        public static IEnumerable<Claim> GetClaims(this AccountTokens userAccounts, Int64 Id)
        {
            if (userAccounts == null) throw new ArgumentNullException(nameof(userAccounts));

            var claims = new List<Claim>
        {
            new Claim("Id", userAccounts.Id.ToString()),
            new Claim(ClaimTypes.Name, userAccounts.Name ?? string.Empty),
            new Claim(ClaimTypes.Email, userAccounts.Email ?? string.Empty),
            new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
            new Claim("Creator", userAccounts.Creator ?? string.Empty),
            new Claim("EmpCode", userAccounts.EmpCode ?? string.Empty),
            new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddDays(1).ToString("MMM ddd dd yyyy HH:mm:ss tt"))
        };

            return claims;
        }

        public static IEnumerable<Claim> GetClaims(this AccountTokens userAccounts, out Int64 Id)
        {
            if (userAccounts == null) throw new ArgumentNullException(nameof(userAccounts));

            Id = userAccounts.Id;
            return GetClaims(userAccounts, Id);
        }

        public static AccountTokens GenTokenkey(AccountTokens model, JwtSettings jwtSettings)
        {
            try
            {
                if (model == null) throw new ArgumentNullException(nameof(model));
                if (jwtSettings == null) throw new ArgumentNullException(nameof(jwtSettings));

                var AccountToken = new AccountTokens();

                // Get secret key
                var key = System.Text.Encoding.ASCII.GetBytes(jwtSettings.IssuerSigningKey ?? throw new ArgumentNullException(nameof(jwtSettings.IssuerSigningKey)));

                Int64 Id = 0;
                DateTime expireTime = DateTime.UtcNow.AddDays(1);
                AccountToken.Validity = expireTime.TimeOfDay;

                var JWToken = new JwtSecurityToken(
                    issuer: jwtSettings.ValidIssuer ?? throw new ArgumentNullException(nameof(jwtSettings.ValidIssuer)),
                    audience: jwtSettings.ValidAudience ?? throw new ArgumentNullException(nameof(jwtSettings.ValidAudience)),
                    claims: GetClaims(model, out Id),
                    notBefore: DateTime.UtcNow,
                    expires: expireTime,
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                );

                AccountToken.Token = new JwtSecurityTokenHandler().WriteToken(JWToken);

                // Assigning properties back to AccountToken
                AccountToken.Name = model.Name;
                AccountToken.Id = model.Id;
                AccountToken.Email = model.Email;
                AccountToken.RoleId = model.RoleId;
                AccountToken.Creator = model.Creator;
                AccountToken.EmpCode = model.EmpCode;

                return AccountToken;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating token: {ex.Message}", ex);
            }
        }
    }

}
