using BSFlixFlex.Models;
using BSFlixFlex.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<ApiListResponse<IDiscovryCommonProperty>>> Get([FromQuery]int? page)
        {             
            var r = await myFavoriService.FetchUserFavoritesAsync(this.User,page??1 );
            if (r.IsSuccess)
            {
                new ApiListResponse<IDiscovryCommonProperty>() { IsSuccess = true, TotalItems = r.TotalItems, Items = r.Items };
                return Ok(r);
            }
            return NotFound();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async void Post(int id, string cinematography)
        {
            var _ = Enum.TryParse<Cinematography>(cinematography, true,out Cinematography result);
            await myFavoriService.AddToFavoritesAsync(id,result ,this.User);
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async void Delete(int id, string cinematography)
        {
            var _ = Enum.TryParse<Cinematography>(cinematography, true, out Cinematography result);
            await myFavoriService.RemoveFromFavoritesAsync(id, result, this.User);
        }
    }
}
