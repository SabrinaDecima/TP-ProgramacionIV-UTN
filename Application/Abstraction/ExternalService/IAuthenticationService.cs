using Contracts.Login.Request;
using Contracts.User.Request;

namespace Application.Abstraction.ExternalServices;

public interface IAuthenticationService
{
    string Login(LoginRequest request);

    string Register(CreateUserRequest request);
}
