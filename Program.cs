using Assignment4;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


app.UseRouting();

app.UseMyCustomMiddleWare();

app.MapGet("/", () => "Hello World!");

app.Run();
