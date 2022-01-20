using System.Text.Json;

namespace Assignment4;
public class MyCustomMiddleWare{
    private readonly RequestDelegate _next;

    public MyCustomMiddleWare(RequestDelegate next){
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context){
        
        var headers = new Dictionary<string,string>() ;
        foreach(var item in context.Request.Headers){
            headers.Add(item.Key,item.Value.ToString());
        }

        var stream = new StreamReader(context.Request.Body);
        var body = await stream.ReadToEndAsync();

        var requestData = new {
            Schema = context.Request.Scheme,
            Host = context.Request.Host.ToString(),
            Path = context.Request.Path.ToString(),
            Query = context.Request.QueryString.ToString(),
            Body = body,
            Header = headers
        };
    
        using (StreamWriter writer = File.AppendText("file.txt")){
            var data = JsonSerializer.Serialize(requestData);
            await writer.WriteLineAsync("================================================================");
            await writer.WriteLineAsync(data);
        }
        await _next(context);
    }
}

public static class RequestMyCustomMiddleWareExtensions{
    public static IApplicationBuilder UseMyCustomMiddleWare(this IApplicationBuilder builder){
        return builder.UseMiddleware<MyCustomMiddleWare>();
    }
}