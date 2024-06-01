using Microsoft.EntityFrameworkCore;
using SimpleOnlineStore.Api.DataAccess.Data;
using SimpleOnlineStore.Api.Helper.DI;

namespace SimpleOnlineStore.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreContext>(x=>x.UseSqlServer(builder.Configuration.GetConnectionString("SimpleStoreConnectionString")));
            builder.Services.AddSimpleStoreServices();

            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            
            app.MapControllers();

            app.Run();
        }
    }
}
