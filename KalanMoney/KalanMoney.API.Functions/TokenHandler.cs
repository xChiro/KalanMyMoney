using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace KalanMoney.API.Functions;

public class TokenHandler
{
    private readonly HttpRequest _request;

    public TokenHandler(HttpRequest request)
    {
        _request = request;
    }
    
    public bool TryGetSubjectFromToken(out string subject)
    {
        subject = null;
        
        if (!TryGetTokenFromHeaders(out var token)) return false;
        
        var jwtToken = GetSecurityJwtToken(token);
        if (string.IsNullOrEmpty(jwtToken.Subject)) return false;

        subject = jwtToken.Subject;

        return true;
    }

    private static JwtSecurityToken GetSecurityJwtToken(string token)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtToken = jwtHandler.ReadJwtToken(token);
        return jwtToken;
    }

    private bool TryGetTokenFromHeaders(out string token)
    {
        token = null;
        
        if (!_request.Headers.TryGetValue("Authorization", out var values)) return false;        
        if (string.IsNullOrEmpty(values.FirstOrDefault())) return false;

        token = values.First().Remove(0, 7);
        return true;
    }
}