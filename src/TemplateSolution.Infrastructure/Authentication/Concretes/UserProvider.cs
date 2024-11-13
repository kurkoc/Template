using System.Security.Claims;

using Microsoft.AspNetCore.Http;

using TemplateSolution.Infrastructure.Authentication.Abstractions;

namespace TemplateSolution.Infrastructure.Authentication.Concretes;

public class UserProvider(IHttpContextAccessor httpContextAccessor) : IUserProvider
{
    public UserInfo? GetUserInfo()
    {
        if(httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == false) return null;
        
        var user = httpContextAccessor.HttpContext.User;
        string id = user.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;
        string firstname = user.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
        string lastname = user.Claims.First(claim => claim.Type == ClaimTypes.Surname).Value;
        string email = user.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;
        
        return new UserInfo{ Id = Guid.Parse(id), FirstName = firstname, LastName = lastname, Email = email };
    }
}