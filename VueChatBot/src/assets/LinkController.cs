using amazon_web.Common;
using amazon_web.Database.Entities;
using amazon_web.Database.Models;
using amazon_web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace amazon_web.Controllers
{
   // [ApiController, Route("api/[controller]/[Action]")]
    public class LinkController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly LinkService _linkService;

        public LinkController(LinkService linkService, IConfiguration configuration)
        {
            _linkService = linkService;
            _configuration = configuration;
        }

        [HttpPost, Route("api/[controller]/Create")]
        public IActionResult Create([FromBody]Links link)
        {
            var data = _linkService.Create(link, out var msg);
            return Ok(new Response { IsSuccessful = data, Msg = msg });
        }
        [HttpPost, Route("api/[controller]/All")]
        public IActionResult All([FromBody]LinkFilter filter)
        {
            var data = _linkService.All(filter, out var count, out var msg);
            var response = new Response
            {
                IsSuccessful = true,
                Msg = msg,
                Data = data,
                Pagination = new Pagination
                {
                    Fetched = data.Count,
                    PageSize = filter.PageSize,
                    Page = filter.Page,
                    Records = count
                }
            };

            return Ok(response);

        } 
        [HttpGet, Route("/lnk/{key}")]
        public IActionResult redirect(string key)
        {
            var link = _linkService.GetKeyword(key);
            return Redirect(link);

        }
        [HttpPost, Route("api/[controller]/Delete")]
        public IActionResult Delete([FromForm] long linkId)
        {
            var data = _linkService.Delete(linkId, out string msg);
            return Ok(new Response { IsSuccessful = data, Msg = msg });
        }
        [HttpPost, Route("api/[controller]/Update")]
        public IActionResult Update([FromBody] Links link)
        {
            var data = _linkService.Update(link, out string msg);
            return Ok(new Response { IsSuccessful = data, Msg = msg });
        }
    }
}
