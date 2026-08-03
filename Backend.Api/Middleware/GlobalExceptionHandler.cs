using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Middleware
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly IHostEnvironment _env;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(
            IHostEnvironment env,
            ILogger<GlobalExceptionHandler> logger)
        {
            _env = env;
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(
            HttpContext context,
            Exception exception,
            CancellationToken ct)
        {
            ProblemDetails problem;

            switch (exception)
            {
                case NotFoundException ex:
                    _logger.LogInformation(ex, ex.Message);
                    problem = CreateProblem(404, ex.Message);
                    break;

                case BadRequestException ex:
                    _logger.LogInformation(ex, ex.Message);
                    problem = CreateProblem(400, ex.Message);
                    break;

                case UnauthorizedException ex:
                    _logger.LogInformation(ex, ex.Message);
                    problem = CreateProblem(401, ex.Message);
                    break;

                case ForbiddenException ex:
                    _logger.LogWarning(ex, ex.Message);
                    problem = CreateProblem(403, ex.Message);
                    break;

                case ConflictException ex:
                    _logger.LogWarning(ex, ex.Message);
                    problem = CreateProblem(409, ex.Message);
                    break;

                default:
                    _logger.LogError(exception, "Unhandled server error");

                    if (_env.IsDevelopment())
                        return false;

                    problem = CreateProblem(500, "حدث خطأ غير متوقع.");
                    break;
            }

            context.Response.StatusCode = problem.Status!.Value;
            await context.Response.WriteAsJsonAsync(problem, ct);
            return true;
        }

        private static ProblemDetails CreateProblem(int status, string detail)
            => new ProblemDetails
            {
                Status = status,
                Detail = detail
            };
    }
}