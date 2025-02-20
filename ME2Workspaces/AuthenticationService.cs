using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Me2Workspaces.ModulosME2.DBUser;
using Microsoft.AspNetCore.Http;
using Me2Workspaces.ModulosME2.DBUser;

public class AuthenticationService
{
    private readonly UserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationService(UserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Tenta autenticar o usuário com base no email e senha fornecidos.
    /// Retorna verdadeiro se a autenticação for bem-sucedida.
    /// </summary>
    public async Task<bool> LoginAsync2(string email, string senha)
    {
        try
        {

      
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return false;

        // Valida usuário via UserService
        var user = await _userService.GetUserByEmailAndPassword(email, senha);
        if (user == null) return false;

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.ADMIN ? "Admin" : "User")
        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

        // Executa o login e gera o cookie
            await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                IsPersistent = true,
                AllowRefresh = true
            }
        );

        return true;
        }catch(Exception ex)
        {
            string var = ex.ToString();
            return false;
        }
    }

    public async Task<bool> LoginAsync(string email, string senha)
    {
        try
        {
            // Valida usuário via UserService
            var user = await _userService.GetUserByEmailAndPassword(email, senha);
            if (user == null) return false;

            // Retorna sucesso se o usuário for válido
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }


    /// <summary>
    /// Realiza o logout do usuário atual, encerrando a sessão.
    /// </summary>
    public async Task LogoutAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return;

        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
