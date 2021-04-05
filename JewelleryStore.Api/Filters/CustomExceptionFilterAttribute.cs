using FluentValidation;
using JewelleryStore.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net;

namespace JewelleryStore.Api.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.HttpContext.Response.ContentType = "application/json";

            if (context.Exception is ValidationException contextException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new JsonResult(GetErrorMessage(contextException));
                return;
            }

            if (context.Exception is NotFoundException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Result = new JsonResult(new ExceptionMessage
                {
                    ErrorMessage = context.Exception.Message,
                    ErrorType = nameof(NotFoundException)
                });

                return;
            }

            if (context.Exception is UserInputException inputException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new JsonResult(GetErrorMessage(inputException));

                return;
            }

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = new JsonResult(new ExceptionMessage
            {
                ErrorMessage = context.Exception.Message,
                ErrorType = context.Exception.GetType().Name
            });
        }

        private ValidationErrorMessage GetErrorMessage(Exception exception)
            => new ValidationErrorMessage
            {
                ErrorMessage = exception.Message,
                ErrorType = exception.GetType().Name
            };

        private static ValidationErrorMessage GetErrorMessage(ValidationException validationError)
        {
            var errorDetails = new List<ValidationErrorDetailMessage>();

            foreach (var error in validationError.Errors)
            {
                errorDetails.Add(new ValidationErrorDetailMessage
                {
                    FieldName = error.PropertyName,
                    FieldMessage = error.ErrorMessage
                });
            }

            return new ValidationErrorMessage
            {
                ErrorMessage = validationError.Message,
                ErrorType = nameof(ValidationException),
                ErrorDetails = errorDetails.ToArray()
            };
        }
    }

    public class ExceptionMessage
    {
        public string ErrorMessage { get; set; }

        public string ErrorType { get; set; }
    }

    public class ValidationErrorMessage : ExceptionMessage
    {
        public ValidationErrorDetailMessage[] ErrorDetails { get; set; }
    }

    public class ValidationErrorDetailMessage
    {
        public string FieldName { get; set; }

        public string FieldMessage { get; set; }
    }
}
