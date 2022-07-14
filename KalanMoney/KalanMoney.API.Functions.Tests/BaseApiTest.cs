using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace KalanMoney.API.Functions.Test;

public abstract class BaseApiTest
{
    protected static DefaultHttpRequest CreateHttpRequest(string requestBody)
    {
        var badRequestBytes = Encoding.ASCII.GetBytes(requestBody);
        var defaultHttpRequest = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(badRequestBytes)
        };
        
        return defaultHttpRequest;
    }
}