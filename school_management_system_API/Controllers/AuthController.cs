
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using school_management_system_API.Context;
using school_management_system_API.Models;
using school_management_system_API.Models.Authentication;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace school_management_system_API.Controllers;

[Route("oauth")]
public class AuthController : ODataController
{

    [AllowAnonymous, HttpPost("token")]
    public ActionResult Post([FromBody] AccessCredentialsModel credenciais,
        [FromServices] DataBaseContext dbContext,
        [FromServices] TokenConfigurationsModel tokenConfigurations)
    {
        if (String.IsNullOrWhiteSpace(credenciais?.ClientId) || String.IsNullOrWhiteSpace(credenciais.ClientSecret))
            return BadRequest("ClientId e ClientSecret são obrigatórios");

        if(!Guid.TryParse(credenciais.ClientSecret,out Guid clientSecretGuid))
            return BadRequest("ClientSecret inválido");

        var school = dbContext.Schools.FirstOrDefault(x => x.Id == Int32.Parse(credenciais.ClientId) && x.Identifier == clientSecretGuid);

        if (school == null)
            return Unauthorized("ClientId incorreto");

        return GenerateToken(school, tokenConfigurations);

    }

    private OkObjectResult GenerateToken(School school, TokenConfigurationsModel tokenConfigurations)
    {
        DateTimeOffset creationDate = DateTimeOffset.Now,
           expirationDate = creationDate.AddSeconds(tokenConfigurations.TokenSeconds);

        ClaimsIdentity identity = new(
            new GenericIdentity(school.Id.ToString(), ClaimTypes.Sid),
            new[] {
                new Claim(ClaimTypes.Actor, school.Identifier.ToString()),
                new Claim(ClaimTypes.PrimarySid, Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? String.Empty),
                new Claim(ClaimTypes.System, Request.Headers.UserAgent),
            }
        );

        JwtSecurityTokenHandler handler = new();

        SecurityToken securityToken = handler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = tokenConfigurations.Issuer,
            Audience = tokenConfigurations.Audience,
            SigningCredentials = new SigningCredentials(tokenConfigurations.SecurityKey, SecurityAlgorithms.HmacSha256),
            Subject = identity,
            NotBefore = creationDate.DateTime,
            Expires = expirationDate.DateTime
        });

        return Ok(new
        {
            created = creationDate.ToString("yyyy-MM-dd HH:mm:ss"),
            expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
            accessToken = handler.WriteToken(securityToken),
            identifier = school.Id.ToString(),
        });
    }

}