using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace KalanMoney.API.Functions;

public abstract class BaseFunctions<TRequest>
{
    protected static async Task<TRequest> DeserializeRequest(HttpRequest req)
    {
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<TRequest>(requestBody);
        
        return data;
    }
}