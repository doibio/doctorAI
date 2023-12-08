using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
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
    //[ApiController, Route("api/[controller]/[Action]")]
    public class QRCodeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly QrCodeService _qrcodeService;

        public QRCodeController(QrCodeService qrcCodeService, IConfiguration configuration)
        {
            _qrcodeService = qrcCodeService;
            _configuration = configuration;
        }

        [HttpGet, Route("api/[controller]/All")]
        public byte[] GetCode([FromBody]string inputText)
        {

            BarcodeWriter barcodeWriter = new BarcodeWriter();
            EncodingOptions encodingOptions = new EncodingOptions()
                {Width = 900, Height = 900, Margin = 0, PureBarcode = false};
            encodingOptions.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);
            barcodeWriter.Renderer = new BitmapRenderer();
            barcodeWriter.Options = encodingOptions;
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            Bitmap bitmap = barcodeWriter.Write(inputText);
            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Jpeg);
            var byteImage = ms.ToArray();
            return byteImage;
        }

        [HttpPost, Route("api/[controller]/CreateURL")]
        public IActionResult CreateURL([FromBody]QrCode qrcode)
        {
            var r = _qrcodeService.createURL(qrcode, out var msg);
            return Ok(r);
        }

        [HttpPost, Route("api/[controller]/Duplicate")]
        public IActionResult Duplicate([FromBody]QrCode qrcode)
        {
            var r = _qrcodeService.duplicate(qrcode, out var msg);
            return Ok(r);
        }

        [HttpGet, Route("/qr/{key}")]
        public IActionResult Get(string key)
        {
            var link = _qrcodeService.getLink(key);

            return Redirect(link);

        }



        [HttpPost, Route("api/[controller]/GetAll")]
        public IActionResult GetAll([FromBody] QRCodeFilter filter)
        {
            var data = _qrcodeService.All(filter, out var count, out var msg);
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

        [HttpPost, Route("api/[controller]/DeleteCode")]
        public IActionResult DeleteCode([FromForm] long codeId)
        {
            var r = _qrcodeService.Delete(codeId, out var msg);
            return Ok(new Response {IsSuccessful = r, Msg = msg});
        }

        [HttpPost, Route("api/[controller]/Update")]
        public IActionResult UpdateCode([FromBody] QrCode qrcode)
        {
            var r = _qrcodeService.Update(qrcode, out var msg);
            return Ok(new Response {IsSuccessful = r, Msg = msg});
        }

    }
}