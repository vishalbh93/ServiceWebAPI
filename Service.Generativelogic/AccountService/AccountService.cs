using Microsoft.Extensions.Configuration;
using Service.Generativelogic.Interfaces;
using Newtonsoft.Json;
using Service.Models;
using Service.DaataAccess.ServiceDomain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service.Generativelogic.AccountService
{
    public class AccountService : IAccountService
    {
        #region Private Variable
        private readonly string _connString;
        private readonly IConfiguration _configuration;
        #endregion

        #region constructor
        public AccountService( IConfiguration configuration)
        {
            this._configuration = configuration;
            this._connString = _configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(_configuration), "Connection string cannot be null."); ;
        }
        #endregion

        public ODCContext CreateDbContext()
        {
            return new ODCContext(this._connString);
        }

        #region Public methods
        public string GetData()
        {
            _ = new SaveReturn<AccountDetails>();
            SaveReturn<AccountDetails>? returnResult;
            try
            {
                dynamic result;
                using var _con = CreateDbContext();
                {
                    _con.Database.EnsureCreated();
                    Console.WriteLine("Connection successful!");
                    result = _con.AccountDetails.ToList();
                }
                returnResult = new SaveReturn<AccountDetails>()
                {
                    IsError = false,
                    ReturnResult = result,
                    ErrorMessage = ""
                };

            }
            catch (Exception ex)
            {
                returnResult = new SaveReturn<AccountDetails>()
                {
                    IsError = true,
                    ErrorMessage = ex.Message
                };
            }
            return JsonConvert.SerializeObject(returnResult);
        }

        public AccountDetails AuthenticateUse(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(userName), "Username and password is incorrect");
            }
            using var context = CreateDbContext();
            return context.AccountDetails
                          .FirstOrDefault(e => e.UserName == userName && e.Password == password) ?? new AccountDetails();
        }

        public string GenerateToken(AccountDetails user)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this._configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: this._configuration["Jwt:Issuer"],
                audience: this._configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            var tokenHandler = new JwtSecurityTokenHandler();

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion
    }
}
