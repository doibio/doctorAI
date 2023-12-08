using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using amazon_web.Common;
using amazon_web.Database.Entities;
using amazon_web.Database.Models;
using amazon_web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ZXing;
using ZXing.Common;
using ZXing.QrCode.Internal;
using ZXing.Rendering;

namespace amazon_web.Controllers
{
   // [ApiController, Route("api/[controller]/[Action]")]
    public class CampaignController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly CampaignService _campaignService;

        public CampaignController(CampaignService qrcCodeService, IConfiguration configuration)
        {
            _campaignService = qrcCodeService;
            _configuration = configuration;
        }

        [HttpPost, Route("api/[controller]/GetAll")]
        public IActionResult GetAll([FromBody]CampaignFiter filter)
        {
            var data = _campaignService.All(filter, out var count, out var msg);
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

        [HttpPost, Route("api/[controller]/Create")]
        public IActionResult Create([FromBody]Campaign campaign)
        {
            var data = _campaignService.Create(campaign, out var msg);
            return Ok(new Response {IsSuccessful = data, Msg = msg});
        }


        [HttpGet, Route("/cmp/{key}")]
        public IActionResult redirect(string key)
            // public IActionResult Redirect(string key)
        {
            var word = _campaignService.GetKeyword(key);

            var domain = "";
            switch (word.MarketPlace)
            {
                case "United States":
                    domain = "com";
                    break;
                case "Canada":
                    domain = "ca";
                    break;
                case "Mexico":
                    domain = "com.mx";
                    break;
                case "United Kingdom":
                    domain = "co.uk";
                    break;
                case "Germany":
                    domain = "de";
                    break;
                case "Spain":
                    domain = "es";
                    break;
                case "France":
                    domain = "fs";
                    break;
                case "India":
                    domain = "co.in";
                    break;
                case "Italy":
                    domain = "it";
                    break;
                case "Japan":
                    domain = "co.jp";
                    break;

            }
            var guid1 = Guid.NewGuid().ToString().Replace("-", string.Empty);
            var guid2 = Guid.NewGuid().ToString().Replace("-", string.Empty);
            dynamic id = ""+guid1 + guid2;
            string gclid = id.Substring(1, 55);
            var amazonlink = "https://www.amazon." + domain + "/s?k=" + word.Keyword + "%26rh=p_78%3A" + word.Asin + "%26gclid=" + gclid;

            var googlelink = "https://www.google.com/url?sa=t&url=" + amazonlink;

            return Redirect(googlelink);

        }

        [HttpPost, Route("api/[controller]/Delete")]
        public IActionResult Delete([FromForm] long campaignId)
        {
            var data = _campaignService.Delete(campaignId, out string msg);
            return Ok(new Response {IsSuccessful = data, Msg = msg});
        }

        [HttpPost, Route("api/[controller]/Update")]
        public IActionResult Update([FromBody]Campaign campaign)
        {
            var data = _campaignService.Update(campaign, out string msg);
            return Ok(new Response {IsSuccessful = data, Msg = msg});
        }
    }
}