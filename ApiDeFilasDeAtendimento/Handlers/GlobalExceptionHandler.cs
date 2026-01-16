using ApiDeFilasDeAtendimento.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ApiDeFilasDeAtendimento.Handlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemsDetails = new ProblemDetails { Instance = httpContext.Request.Path };

            switch (exception)
            {
                case NotFoundException:
                    problemsDetails.Status = StatusCodes.Status404NotFound;
                    problemsDetails.Title = "Não encontrado";
                    problemsDetails.Detail = exception.Message;
                    break;
                case ForbiddenException:
                    problemsDetails.Status = StatusCodes.Status403Forbidden;
                    problemsDetails.Title = "Não autorizado";
                    problemsDetails.Detail = exception.Message;
                    break;
                case ConflictException:
                    problemsDetails.Status = StatusCodes.Status409Conflict;
                    problemsDetails.Title = "Dados Existentes";
                    problemsDetails.Detail = exception.Message;
                    break;
                case UnauthorizedAccessException:
                    problemsDetails.Status = StatusCodes.Status401Unauthorized;
                    problemsDetails.Title = "Usuário sem Cookies";
                    problemsDetails.Detail = exception.Message;
                    break;
                case BusinessException:
                    problemsDetails.Status = StatusCodes.Status400BadRequest;
                    problemsDetails.Title = "Regra de negócios";
                    problemsDetails.Detail = exception.Message;
                    break;
                default:
                    problemsDetails.Status = StatusCodes.Status500InternalServerError;
                    problemsDetails.Title = "Erro Interno";
                    break;
            }
            problemsDetails.Detail = exception.Message;
            httpContext.Response.StatusCode = problemsDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemsDetails, cancellationToken);
            return true;
        }
    }
}
