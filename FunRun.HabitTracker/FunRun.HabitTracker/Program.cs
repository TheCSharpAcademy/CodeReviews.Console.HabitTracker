

using FunRun.HabitTracker;
using FunRun.HabitTracker.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Threading.Tasks;


var host = Host.CreateDefaultBuilder(args)
         .ConfigureServices((context, services) =>
         {

             services.AddTransient<HabitTrackerApp>();
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
             logging.AddConsole();   
             logging.AddDebug();      
         })
         .Build();



var app = host.Services.GetRequiredService<HabitTrackerApp>();
await app.RunApp();


await host.StopAsync();