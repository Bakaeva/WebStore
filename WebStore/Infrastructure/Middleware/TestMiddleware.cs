using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebStore.Infrastructure.Middleware
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _next;

        public TestMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            // действия до вызова следующего компонента конвейера
            await _next(context);
            // действия после отработки следующего компонента конвейера
        }
    }
}
