using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MigrationUpdate
{
    public static class MigrationUpdateExtensions
    {

        // this is the generic metho  that can work with dbcontext type

        public static void UpdateDatabase<TContext>(IApplicationBuilder app)  where TContext : DbContext
        {

            // so this method run the migration process in the backend task.


            Task.Run (() =>
            {
                //
                using var serviceScope = app.ApplicationServices.
                GetService<IServiceScopeFactory>().CreateScope();
                // get the dbcontext object with dependency injection


                using var  context = serviceScope.ServiceProvider.GetRequiredService<TContext>();


                // set the maximum time(in second) allowed a database to run commanit
                // it will be usefull when your migraiton will be longer


                context.Database.SetCommandTimeout(1000);


                // applies or implements all the pending migration that is not updated inside 

                // your database
                // then we can update the database 

                context.Database.Migrate();
            });

        }
    }
}
