using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        protected IConfiguration configuration;

        public DataContextFactory()
        {

        }

        public DataContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseSqlServer("Server=localhost;Database=AspNetAuth;Trusted_Connection=True");
            return new DataContext(builder.Options);
        }
    }
}
