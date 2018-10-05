using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using DAL;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace DemoBlog
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            Mapper.Reset();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            if (configuration["DbConfig:useSql"].ToLower() == "true")
            {
                builder.UseSqlServer(configuration["DbConfig:ConnectionStrings:DefaultConnection"],
                    b => b.MigrationsAssembly("DemoBlog"));
            }
            else if (configuration["DbConfig:usePostgres"].ToLower() == "true")
            {
                builder.UseNpgsql(configuration["DbConfig:ConnectionStrings:PostgresConnection"],
                    b => b.MigrationsAssembly("DemoBlog"));
            }
            else if (configuration["DbConfig:usePostgresDocker"].ToLower() == "true")
            {
                builder.UseNpgsql(configuration["DbConfig:ConnectionStrings:PostgresDockerConnection"],
                    b => b.MigrationsAssembly("DemoBlog"));
            }
            else
            {
                builder.UseSqlite(configuration["DbConfig:ConnectionStrings:SqliteConnection"],
                    b => b.MigrationsAssembly("DemoBlog"));
            }
            builder.UseOpenIddict();

            return new ApplicationDbContext(builder.Options);
        }
    }
}