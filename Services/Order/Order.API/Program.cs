using Middleware;
using MigrationUpdate;
using Order.Data;
using Order.Data.Services;
using RegisterDependency;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.RegisterServices(typeof(OrderService).Assembly.FullName);







builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// this will for update-database

var deployDatabaseChanges = app.Configuration.GetValue<bool>("DisployDatabaseChanges");
if(deployDatabaseChanges)
{
    MigrationUpdateExtensions.UpdateDatabase<OrderDbContext>(app);
}


app.UseMiddleware<ExceptionMiddleware>();

//try{sdfsd}catch{asfds}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
