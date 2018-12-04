using System.IO;
using AutoMapper;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DemoBlog
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            Mapper.Reset();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", true)
                .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            if (configuration["DbConfig:useSql"].ToLower() == "true")
                builder.UseSqlServer(configuration["DbConfig:ConnectionStrings:DefaultConnection"],
                    b => b.MigrationsAssembly("DemoBlog"));
            else if (configuration["DbConfig:usePostgres"].ToLower() == "true")
                builder.UseNpgsql(configuration["DbConfig:ConnectionStrings:PostgresConnection"],
                    b => b.MigrationsAssembly("DemoBlog"));
            else if (configuration["DbConfig:usePostgresDocker"].ToLower() == "true")
                builder.UseNpgsql(configuration["DbConfig:ConnectionStrings:PostgresDockerConnection"],
                    b => b.MigrationsAssembly("DemoBlog"));
            else
                builder.UseSqlite(configuration["DbConfig:ConnectionStrings:SqLiteConnection"],
                    b => b.MigrationsAssembly("DemoBlog"));
            builder.UseOpenIddict();

            return new ApplicationDbContext(builder.Options);
        }
    }
}