using E_Commerce.Shared.CommonResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ApiBaseController:ControllerBase
    {
        //Handle result Without value 
        // if result is success return no contect 204
        // ~~~~~~~~~~~~ failure ~~~~~~  Prroblem with statue code and error details

        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
                return NoContent();
            else
                return HandleProblem(result.Errors); 
        }      

        //Handle result With value 
        // if result is success return  ok 200 with value 
        // ~~~~~~~~~~~~ failure ~~~~~~  Prroblem with statue code and error details
        protected ActionResult<Tvalue> HandleResult<Tvalue>(Result<Tvalue> result)
        {
            if (result.IsSuccess)
                return Ok(result.value);
            else
                return HandleProblem(result.Errors);

        }


        private ActionResult HandleProblem(IReadOnlyList<Error> errors)
        {
            // if no error =>500
            if (errors.Count == 0)
                return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "An UnExpectedErrorOccured");
            // if only one error=> handle single error problem 
            if (errors.All(e => e.ErrorType == ErrorType.Validation)) return HandleValidationProblem(errors);
            return HandleSngelErrorProblem(errors[0]);

            // if all error are validation error , handle the as valdation problem  
        
        }

        private ActionResult HandleSngelErrorProblem(Error error) 
        {
            return Problem(
                title: error.Code,
                detail:error.Description,
                type:error.ErrorType.ToString(),
                statusCode:MapErrorTypeToStatusCode(error.ErrorType)

                );
        }

        private static int MapErrorTypeToStatusCode(ErrorType errorType) => errorType switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthrized => StatusCodes.Status401Unauthorized,
            ErrorType.forbidden => StatusCodes.Status403Forbidden,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.InvalidCredentials => StatusCodes.Status401Unauthorized,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError

        };

        private  ActionResult HandleValidationProblem(IReadOnlyList<Error> errors)
        {
            var modelState = new ModelStateDictionary();
            foreach (var error in errors)
                modelState.AddModelError(error.Code, error.Description);

            return ValidationProblem(modelState);
        
        }

    }
}
