using FunRun.HabitTracker;
using FunRun.HabitTracker.Data;
using FunRun.HabitTracker.Services;
using FunRun.HabitTracker.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data;


var host = Host.CreateDefaultBuilder(args)
         .ConfigureServices((context, services) =>
         {

             services.AddSingleton<HabitTrackerApp>();
             services.AddScoped<ISqlOperations,SqlOperations>();
             services.AddScoped<ICrudService, CrudService>();

             services.AddSingleton<SQLiteConnectionFactory>(provider =>
                 new SQLiteConnectionFactory("Data Source=mydatabase.db;")
             );

             services.AddScoped<IDbConnection>(provider =>
             {
                 var factory = provider.GetRequiredService<SQLiteConnectionFactory>();
                 return factory.CreateConnection();
             });

         })
         .ConfigureLogging(logging =>
         {
             
             logging.ClearProviders();

             logging.AddDebug();      
         })
         .Build();




var app = host.Services.GetRequiredService<HabitTrackerApp>();
await app.RunApp();


await host.StopAsync();