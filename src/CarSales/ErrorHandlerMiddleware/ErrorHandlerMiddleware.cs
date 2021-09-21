using CarSales.Domain.CustomExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CarSales.Repository.ErrorHandlerMiddleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _request;
        private readonly ILogger _logger;
        public ErrorHandlerMiddleware(RequestDelegate request, ILogger<ErrorHandlerMiddleware> logger)
        {
            _request = request;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _request(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                ProcessingResponse(error, response);

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }

        private void ProcessingResponse(Exception error, HttpResponse response)
        {
            if (error is BaseCustomException)
                response.StatusCode = (int)HttpStatusCode.BadRequest;
            else
                response.StatusCode = (int)HttpStatusCode.InternalServerError;

            _logger.LogError(error.Message);
        }
    }
}
