using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace KalanMoney.API.Functions;

public abstract class BaseRequestFunction
{
    protected static async Task<TRequest> DeserializeRequest<TRequest>(HttpRequest req)
    {
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<TRequest>(requestBody);
        
        return data;
    }
    
    protected static bool TryGetOwnerId(HttpRequest req, out string ownerId)
    {
        var tokenHandler = new TokenHandler(req);

        return tokenHandler.TryGetSubjectFromToken(out ownerId);
    }
}