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
    public class MyFavoritesController(MyFavoriService myFavoriService) : ControllerBase
    {

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<ListResponse<IDiscovryCommonProperty>>> Get()
        {             
            var r = await myFavoriService.GetMyListFavorisAsync(this.User);
            if (r != null)
            {
                new ListResponse<IDiscovryCommonProperty>() { IsSuccess = true, TotalItems = r.Count(), Items = r };
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
        public void Post(int id, string cinematography)
        {
            var _ = Enum.TryParse<Cinematography>(cinematography, true,out Cinematography result);
             myFavoriService.AddFavori(id,result ,this.User);
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id, string cinematography)
        {
            var _ = Enum.TryParse<Cinematography>(cinematography, true, out Cinematography result);
            myFavoriService.Remove(id, result, this.User);
        }
    }
}
