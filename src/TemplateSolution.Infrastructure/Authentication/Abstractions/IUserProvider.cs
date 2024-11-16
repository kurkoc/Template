using TemplateSolution.Infrastructure.Authentication.Types;

namespace TemplateSolution.Infrastructure.Authentication.Abstractions;

public interface IUserProvider
{
    UserInfo? GetUserInfo();
}