namespace ProniaProject.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        public readonly RequestDelegate _request;

        public GlobalExceptionHandlerMiddleware(RequestDelegate request)
        {
            _request = request;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _request.Invoke(context);
            }
            catch (Exception ex)
            {
                context.Response.Redirect($"/home/error?errorMessage={ex.Message}");
            }
        }
    }
}
