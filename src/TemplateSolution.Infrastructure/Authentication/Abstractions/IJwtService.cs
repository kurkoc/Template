namespace TemplateSolution.Infrastructure.Authentication.Abstractions;

public interface IJwtService
{
    Task<dynamic> GenerateToken(Guid userId, string firstname, string lastname, string email);
}