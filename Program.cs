using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace AzureFunctionKeyVault
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureAppConfiguration((ctx, builder) =>{
                    builder.AddEnvironmentVariables();

                    var tmpConfig = builder.Build();
                    var vaultUri = tmpConfig["VaultUri"];
                    if(string.IsNullOrWhiteSpace(vaultUri)){
                        throw new ArgumentException("please provide a valid VaultUri");
                    }

                    builder.AddAzureKeyVault(new Uri(vaultUri), new Azure.Identity.DefaultAzureCredential());
                })
                .Build();

            host.Run();
        }
    }
}