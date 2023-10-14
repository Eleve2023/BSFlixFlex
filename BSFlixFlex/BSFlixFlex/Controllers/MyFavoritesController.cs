using BSFlixFlex.Client.Shareds.Interfaces;
using BSFlixFlex.Client.Shareds.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        [HttpGet]
        public async Task<ActionResult<ApiListResponse<IDiscovryCommonProperty>>> Get([FromQuery] int? page, [FromQuery] int? pageSize)
        {
            var r = await myFavoriService.FetchUserFavoritesAsync(this.User, page ?? 1, pageSize ?? 10);
            if (r.IsSuccess)
            {
                return Ok(r);
            }
            return NotFound();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{cinematography}")]
        public async Task<bool> Get(Cinematography cinematography, int id)
        {
            return await myFavoriService.IsFavoriteAsync(id, cinematography, User);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task Post(int id, string cinematography)
        {
            var _ = Enum.TryParse<Cinematography>(cinematography, true, out Cinematography result);
            await myFavoriService.AddToFavoritesAsync(id, result, this.User);
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{cinematography}/{id}")]
        public async Task Delete(int id, string cinematography)
        {
            var _ = Enum.TryParse<Cinematography>(cinematography, true, out Cinematography result);
            await myFavoriService.RemoveFromFavoritesAsync(id, result, this.User);
        }
    }
}
