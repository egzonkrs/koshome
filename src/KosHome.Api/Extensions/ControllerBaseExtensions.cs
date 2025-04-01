// using System;
// using System.Collections.Generic;
// using System.Net;
// using FluentResults;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Http.HttpResults;
// using Microsoft.AspNetCore.Mvc;
//
// namespace KosHome.Api.Extensions;
//
// /// <summary>
// /// The <see cref="ControllerBase"/> extensions.
// /// </summary>
// public static class ControllerBaseExtensions
// {
//     /// <summary>
//     /// Creates an <see cref="ActionResult{T}"/> from <see cref="Result"/> setting proper HTTP status code.
//     /// Uses <see cref="Mapster"/> for result mapping.
//     /// </summary>
//     /// <typeparam name="TIn">Type of result.</typeparam>
//     /// <typeparam name="TOut">Type of controller's response.</typeparam>
//     public static ActionResult<TOut> ToActionResult<TIn, TOut>(this ControllerBase controller, Result<TIn> result,
//         HttpStatusCode successCode = HttpStatusCode.OK)
//     {
//         if (result.IsSuccess)
//         {
//             return controller.StatusCode((int)successCode, result.Value!.Adapt<TOut>());
//         }
//
//         if (result.IsFailed && result.HasException<Exception>())
//         {
//             return controller.StatusCode(
//                 (int)HttpStatusCode.InternalServerError,
//                 CreateProblemDetailsInternal(controller.HttpContext, HttpStatusCode.InternalServerError, result.ToResult()));
//         }
//
//         return controller.BadRequest(CreateProblemDetailsInternal(controller.HttpContext, HttpStatusCode.BadRequest, result.ToResult()));
//     }
//     /// <summary>
//     /// Creates an <see cref="ActionResult{T}"/> from <see cref="Result{T}"/> setting proper HTTP status code.
//     /// </summary>
//     public static ActionResult ToActionResult(this ControllerBase controller, Result result)
//     {
//         if (result.IsSuccess)
//         {
//             return controller.NoContent();
//         }
//
//         if (result.IsFailed && result.HasException<Exception>())
//         {
//             return controller.StatusCode((int)InternalServerError,
//                 CreateProblemDetailsInternal(controller.HttpContext, InternalServerError, result));
//         }
//
//         return controller.BadRequest(CreateProblemDetailsInternal(controller.HttpContext, BadRequest, result));
//     }
//
//     private static ProblemDetailsWithErrors CreateProblemDetailsInternal(
//         HttpContext httpContext, HttpStatusCode httpStatusCode, Result result)
//     {
//         var problemDetails = new ProblemDetailsWithErrors
//         {
//             Type = "about:blank",
//             Title = httpStatusCode.ToString(),
//             Instance = httpContext.Request.Path.ToString(),
//             Status = (int) httpStatusCode,
//             Errors = result.Errors.ToDictionary()
//         };
//
//         return problemDetails;
//     }
// }
//
// public class ProblemDetailsWithErrors : ProblemDetails
// {
//     public Dictionary<string, IEnumerable<string>> Errors { get; set; } = new(StringComparer.Ordinal);
// }
