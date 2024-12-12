using MF_FD_SA_AUTH_MANAGER.DataAccessLayer;
using MF_FD_SA_AUTH_MANAGER.BussinessAccessLayer;
using MF_FD_SA_AUTH_MANAGER.Services;
using MF_FD_SA_AUTH_MANAGER.Filters;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json").Build();

// Add services to the container.

var conn_auth = config["ConnectionStrings:CONN_FDCRM"];
var conn_error_log = config["ExceptionSettings:FDCRM_ERROR_LOG"];
var error_log_file_path = config["ExceptionSettings:ErrorLogFilePath"];



builder.Services.AddTransient<IAuthManager, AuthManagerDAL>(serviceProvider =>
{
    return new AuthManagerDAL(conn_auth);
});

builder.Services.AddTransient<ITokenServices, TokenService>(serviceProvider =>
{
    return new TokenService(new AuthManagerDAL(conn_auth), conn_auth);
});

builder.Services.AddTransient<IExceptionServices, ExceptionUtility>(serviceProvider =>
{
    return new ExceptionUtility(conn_error_log, error_log_file_path);
});

builder.Services.AddControllers(o =>
{
    
    o.Filters.Add<BadRequestFilter>();
    o.Filters.Add<ExceptionFilter>();
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});


//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//app.UseMiddleware<JsonInputSanitizationMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region VAPT

app.UseFileServer(enableDirectoryBrowsing: false);//VAPT 
app.UseMiddleware<JsonInputSanitizationMiddleware>();
app.UseMiddleware<DuplicateParametersDetectionMiddleware>();


app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
    context.Response.Headers.Add("X-Frame-Options", "deny");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("Referrer-Policy", "origin-when-cross-origin");
    context.Response.Headers.Add("Cache-Control", "no-store");
    //context.Response.Headers.Add("X-Rate-Limit-Limit", "1200");
    //context.Response.Headers.Add("X-Rate-Limit-Remaining", "1199");
    //context.Response.Headers.Add("X-Rate-Limit-Reset", "1609459200");

    await next();
});

#endregion


//app.UseHttpsRedirection();

//app.UseAuthorization();


app.MapControllers();

app.Run();





