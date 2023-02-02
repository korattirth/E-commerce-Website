using E_commerce_Website.DTOs;
using E_commerce_Website.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Net;
using System.Net.Mime;

namespace E_commerce_Website.Controllers
{
    [Produces(MediaTypeNames.Application.Json,MediaTypeNames.Application.Xml)]
    [Consumes(MediaTypeNames.Application.Json,MediaTypeNames.Application.Xml)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class TestController : BaseApiController
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {

        /// <summary>
        /// This endpoint retrieves all of the users' information (name, adress, contacts, and so on) that matches the query condition
        /// </summary>
        /// <param name="test">Only for testing purpose</param>
        /// <param name="test1">This for parameter</param>
        /// <remarks> ****Get**** /api/Test1?test=:test</remarks>
        /// <returns>IEnumerable of slugs</returns>
        /// <response code="200">If all requested items are found</response>
        /// <response code="400">If test parameter is missing
        /// </response>
        /// <response code="404">If number of records found doesn't equal 
        /// number of records requested</response>
        [HttpGet(Name ="Get Test")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(LoginDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public JsonResult Test1([FromHeader(Name = "TestHeader")] string test,string test1)
        {
            var json_data = string.Empty;
            using (var w = new WebClient())
            {
                // attempt to download JSON data as a string
                json_data = w.DownloadString("http://localhost:7539/swagger/v1/swagger.json");
            }
            return Json(json_data);
        }
    }
}
