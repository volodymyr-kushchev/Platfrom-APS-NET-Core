namespace Platform.Middelwares;

public class QueryStringMiddleWareWithTermination
{
    private RequestDelegate? next;
    public QueryStringMiddleWareWithTermination()
    {
        // do nothing
    }
    public QueryStringMiddleWareWithTermination(RequestDelegate nextDelegate)
    {
        next = nextDelegate;
    }
    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Method == HttpMethods.Get
        && context.Request.Query["custom"] == "true")
        {
            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "text/plain";
            }
            await context.Response.WriteAsync("Class-based Middleware \n");
        }

        if (next != null)
        {
            await next(context);
        }
    }
}
