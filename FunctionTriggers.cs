using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;

namespace AzureFunctionKeyVault
{
    public class FunctionTriggers
    {
        private IConfiguration _config;

        public FunctionTriggers(IConfiguration config)
        {
            _config = config;
        }

        [Function(nameof(Run))]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "settings/{name}")] HttpRequestData req,
                                    string name)
        {
            var response = req.CreateResponse();
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            if (string.IsNullOrWhiteSpace(name))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.WriteString("please provide a setting name.");
            }   
            else
            {
                var setting = _config[name];
                if (string.IsNullOrWhiteSpace(setting))
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.WriteString("please provide a valid setting name.");
                }
                else
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.WriteString($"value for {name} is: {setting}");
                }
            }

            return response;
        }
    }
}
