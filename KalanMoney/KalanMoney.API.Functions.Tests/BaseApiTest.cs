using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;

namespace KalanMoney.API.Functions.Test;

public abstract class BaseApiTest
{
    protected const string DefaultToken = "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJUZXN0SXNzdWVyIiwiaWF0IjoxNjU4MTAxNzIxLCJleHAiOjE2ODk2Mzc3MjEsImF1ZCI6Ind3dy5leGFtcGxlLmNvbSIsInN1YiI6Impyb2NrZXRAZXhhbXBsZS5jb20iLCJHaXZlbk5hbWUiOiJKb2hubnkifQ._tGy-Rh1FpeH1PjuySi4Lh5yyetCMPkhClaBFGZxdtI";

    protected static DefaultHttpRequest CreateHttpRequest(string requestBody, string token = DefaultToken)
    {
        var badRequestBytes = Encoding.ASCII.GetBytes(requestBody);
        var defaultHttpRequest = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(badRequestBytes),
            Headers = { new KeyValuePair<string, StringValues>("Authorization", new StringValues(token)) }
        };
        
        return defaultHttpRequest;
    }
    
    protected static DefaultHttpRequest CreateHttpRequestNotBody(string token = DefaultToken)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Add("Authorization", token);
        
        var httpRequest = new DefaultHttpRequest(httpContext);
        
        return httpRequest;
    }
}