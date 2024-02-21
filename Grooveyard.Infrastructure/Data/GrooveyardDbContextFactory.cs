using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace Grooveyard.Infrastructure.Data
{
    public class GrooveyardDbContextFactory : IDesignTimeDbContextFactory<GrooveyardDbContext>
    {
        public GrooveyardDbContext CreateDbContext(string[] args)
        {
            //var secretClient = new SecretClient(new Uri("https://grooveyard.vault.azure.net/"), new DefaultAzureCredential());
            //KeyVaultSecret secret = secretClient.GetSecret("grooveyard-connectionstring");
            //string connectionString = secret.Value;
            var optionsBuilder = new DbContextOptionsBuilder<GrooveyardDbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-9U8OMF1;Database=grooveyard-db;Trusted_Connection=True;TrustServerCertificate=true;");

            return new GrooveyardDbContext(optionsBuilder.Options);
        }
    }

}
