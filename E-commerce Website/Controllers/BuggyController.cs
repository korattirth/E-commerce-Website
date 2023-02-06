using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_Website.Controllers
{
    public class BuggyController : BaseApiController
    {
        /// <remarks> ****GET**** /api/Buggy/not-found</remarks>
        //[HttpGet("not-found" ,Name ="Not Found")]
        //public ActionResult GetNotFound()
        //{
        //    return NotFound();
        //}
        /// <remarks> ****GET**** /api/Buggy/bad-request</remarks>
        //[HttpGet("bad-request", Name = "Bad Request")]
        //public ActionResult GetBadRequest()
        //{
        //    return BadRequest(new ProblemDetails { Title = "This is as bad request" });
        //}
        /// <remarks> ****GET**** /api/Buggy/unauthorised</remarks>
        //[HttpGet("unauthorised", Name = "Unauthorised")]
        //public ActionResult GetUnauthorised()
        //{
        //    return Unauthorized();
        //}
        /// <remarks> ****GET**** /api/Buggy/validation-error</remarks>
        //[HttpGet("validation-error", Name = "Validation Error")]
        //public ActionResult GetValidateError()
        //{
        //    ModelState.AddModelError("Problem 1", "This is the first error");
        //    ModelState.AddModelError("Problem 2", "This is the Secound error");
        //    return ValidationProblem();
        //}
        /// <remarks> ****GET**** /api/Buggy/server-error</remarks>
        //[HttpGet("server-error", Name = "Server Error")]
        //public ActionResult GetServerError()
        //{
        //    throw new Exception("This is a server error");
        //}

    }
}
