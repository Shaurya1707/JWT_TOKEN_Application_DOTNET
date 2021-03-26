using CommanBusinessLogic;
using JWT_TOKEN_Application.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;

namespace JWT_TOKEN_Application.Controllers
{
    [RoutePrefix("api/jwt")]
    public class JwtController : ApiController
    {



        [HttpPost]
        [Route("Authenticate")]
        [Obsolete]
        public IHttpActionResult Authenticate([FromBody] LoginVM loginVM)
        {
            var loginResponse = new LoginResponseVM();
            var loginrequest = new LoginVM
            {
                Email = loginVM.Email.ToLower(),
                Password = loginVM.Password
            };

            string result = checkCredentials(loginrequest.Email, loginrequest.Password);
            // if credentials are valid
            if (result=="Admin")
            {
                var token = CreateToken(loginrequest.Email, result);
                //return the token
                return Ok(new { Token = token,Role= result, Name= "deepak" });
            }
            else if (result == "User")
            {
                var token = CreateToken(loginrequest.Email, result);
                //return the token
                return Ok(new { Token = token, Role = result, Name = "pushpendra" });
            }
            else
            {
                loginResponse.StatusCode = HttpStatusCode.Unauthorized.ToString();
                return Unauthorized();
            }
        }

        [Obsolete]
        private string checkCredentials(string EMAIL, string password)
        {
            string result = string.Empty;
            try
            {
                if(EMAIL=="deepak" && password=="123")
                {
                    result = "Admin";
                }
                else if (EMAIL == "pushpendra" && password == "123")
                {
                    result = "User";
                }
                else
                {
                    result = "NA";
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string CreateToken(string email, string role)
        {
            //Set issued at date
            DateTime issuedAt = DateTime.Now;
            //set the time when it expires
            DateTime expires = DateTime.Now.AddMinutes(10);

            //http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)

           });

            const string secrectKey = "my_secret_key_12345";
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secrectKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);


            //Create the jwt (JSON Web Token)
            //Replace the issuer and audience with your URL (ex. http:localhost:12345)
            var token =
                (JwtSecurityToken)
                tokenHandler.CreateJwtSecurityToken(
                    issuer: "http://localhost:51969/",
                    audience: "http://localhost:51969/",
                    subject: claimsIdentity,
                    notBefore: issuedAt,
                    expires: expires,
                    signingCredentials: signingCredentials);

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
        private string CreateToken1(string email, string role)
        {
            string key = "my_secret_key_12345";
            var issuer = "http://localhost:51969/";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenHandler = new JwtSecurityTokenHandler();

            var perclaims = new List<Claim>();
            perclaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            perclaims.Add(new Claim(ClaimTypes.Email, email));
            perclaims.Add(new Claim(ClaimTypes.Role, role));
            
            var token = new JwtSecurityToken(
            issuer, issuer, perclaims, expires:DateTime.Now.AddMinutes(5),signingCredentials: signingCredentials);

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        
    }
}
