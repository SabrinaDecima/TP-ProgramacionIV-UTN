namespace Application.Abstraction.ExternalServices;

public interface IAuthenticationService
{
    string Login(string email, string password);
}
