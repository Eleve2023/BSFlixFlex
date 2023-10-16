using BSFlixFlex.Client.Shareds.Interfaces;
using BSFlixFlex.Client.Shareds.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BSFlixFlex.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "CookiesOrBearer")]
    public class MyFavoritesController(IMyFavoriteService myFavoriService) : ControllerBase
    {
        // GET: api/<ValuesController>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiListResponse<IDiscovryCommonProperty>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<ActionResult<ApiListResponse<IDiscovryCommonProperty>>> Get([FromQuery] int? page, [FromQuery] int? pageSize)
        {
            var r = await myFavoriService.FetchUserFavoritesAsync(this.User, page ?? 1, pageSize ?? 10);
            if (r.IsSuccess)
            {
                return Ok(r);
            }
            return Problem(statusCode: 500);
        }

        // GET api/<ValuesController>/5
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{cinematography}/{id:int}")]
        public async Task<bool> Get([FromRoute]Cinematography cinematography, [FromRoute] int id)
        {
            return await myFavoriService.IsFavoriteAsync(id, cinematography, User);
        }

        // POST api/<ValuesController>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public async Task<IResult> Post([FromForm] int id,[FromForm] string cinematography)
        {
            if (Enum.TryParse(cinematography, true, out Cinematography result))
            {
                try
                {
                    await myFavoriService.AddToFavoritesAsync(id, result, this.User);
                    return Results.Created();
                }
                catch (Exception)
                {
                    return Results.Problem(statusCode: 500);
                }
            }
            else
                return Results.NotFound(cinematography);
        }

        //// PUT api/<ValuesController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<ValuesController>/5
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete]
        public async Task<IResult> Delete([FromForm] int id, [FromForm] string cinematography)
        {
            if (Enum.TryParse(cinematography, true, out Cinematography result))
            {
                try
                {
                    await myFavoriService.RemoveFromFavoritesAsync(id, result, this.User);
                    return Results.Accepted();
                }
                catch (Exception)
                {
                    return Results.Problem(statusCode: 500);
                }
            }
            else
                return Results.NotFound(cinematography);
        }
    }
}
