using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using amazon_web.Database;
using amazon_web.Database.Entities;
using amazon_web.Database.Models;
using Microsoft.EntityFrameworkCore;
using ZXing;
using ZXing.Common;
using ZXing.QrCode.Internal;
using ZXing.Rendering;

namespace amazon_web.Services
{
    public class QrCodeService
    {
        private readonly AmazonContext _dbo;

        public QrCodeService(AmazonContext context)
        {
            _dbo = context;
        }

       
        public bool Delete(long codeId, out string msg)
        {
            msg = null;
            var e = _dbo.QrCodes.Find(codeId);
            if (e == null) return false;
            _dbo.QrCodes.Remove(e);
            var r = _dbo.SaveChanges() > 0;
            if (r)
                msg = "QrCodes Deleted Succesfully";

            return r;

        }
  
        public string getLink(string key) {
            var linkCode = _dbo.QrCodes.FirstOrDefault(x =>x.LinkGuid == key);
            if(linkCode == null) return null;
            if(linkCode.IsActive == false)
            {
                linkCode.TrackedUrl = "Nothing Found";
            }
            if (linkCode.IsActive == true) linkCode.Visits++;
            _dbo.SaveChanges();

            var link = linkCode.TrackedUrl;
            return link;

        }

        public bool createURL(QrCode qrcode, out string msg)
        {

            BarcodeWriter barcodeWriter = new BarcodeWriter();
            EncodingOptions encodingOptions = new EncodingOptions() { Width = 900, Height = 900, Margin = 0, PureBarcode = false };
            encodingOptions.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);
            barcodeWriter.Renderer = new BitmapRenderer();
            barcodeWriter.Options = encodingOptions;
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            msg = null;
            dynamic r ="";
            // List of characters and numbers to be used...  
            string URL = "";
               string customURL = "https://magiclinkz.azurewebsites.net/qr/";
            // string customURL = "https://localhost:44300/qr/";

            if(qrcode.LinkGuid == null || qrcode.LinkGuid == "")
            {
                List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
                List<char> characters = new List<char>()
{'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '-', '_'};

                // Create one instance of the Random  
                Random rand = new Random();
                // run the loop till I get a string of 10 characters  
                for (int i = 0; i < 11; i++)
                {
                    // Get random numbers, to get either a character or a number...  
                    int random = rand.Next(0, 3);
                    if (random == 1)
                    {
                        // use a number  
                        random = rand.Next(0, numbers.Count);
                        URL += numbers[random].ToString();
                    }
                    else
                    {
                        // Use a character  
                        random = rand.Next(0, characters.Count);
                        URL += characters[random].ToString();

                    }

                }
                customURL += URL;
                Bitmap bitmap = barcodeWriter.Write(customURL);
                if (qrcode.Logo != null)
                {
                    byte[] bytes = Convert.FromBase64String(qrcode.Logo);
                    Image logo;
                    using (MemoryStream logoms = new MemoryStream(bytes))
                    {
                        logo = new Bitmap(logoms);
                    }

                    Bitmap resizedlogo = new Bitmap(logo, new Size(300, 300));
                    Graphics g = Graphics.FromImage(bitmap);
                    g.DrawImage(resizedlogo, new Point((bitmap.Width - resizedlogo.Width) / 2, (bitmap.Height - resizedlogo.Height) / 2));
                }
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Jpeg);
                byte[] byteImage = ms.ToArray();
                qrcode.Image = byteImage;
                qrcode.BaseUrl = customURL;
                qrcode.LinkGuid = URL;
                var dbUrl = _dbo.QrCodes.Include(i => i.User).FirstOrDefault(x => x.UserId == qrcode.UserId && x.QrCodeId == qrcode.QrCodeId);
                if (dbUrl == null)
                {
                    _dbo.QrCodes.Add(qrcode);
                    r = _dbo.SaveChanges() > 0;
                    msg = "Record added successfully";
                }
                else
                {
                    dbUrl.QrCodeId = qrcode.QrCodeId;
                    dbUrl.Title = qrcode.Title;
                    dbUrl.Visits = qrcode.Visits;
                    dbUrl.LinkGuid = qrcode.LinkGuid;
                    dbUrl.Image = byteImage;
                    dbUrl.IsActive = qrcode.IsActive;
                    _dbo.QrCodes.Update(dbUrl);
                    r = _dbo.SaveChanges() > 0;
                    msg = "Record updated successfully";
                }
            }
            else
            {
                var slug = _dbo.QrCodes.Include(i => i.User).FirstOrDefault(x => x.UserId == qrcode.UserId && x.LinkGuid == qrcode.LinkGuid);
                if (slug == null)
                {
                    customURL += qrcode.LinkGuid;
                    qrcode.TrackedUrl = qrcode.BaseUrl ;
                    qrcode.LinkGuid = qrcode.LinkGuid;
                    Bitmap bitmap = barcodeWriter.Write(customURL);
                    if (qrcode.Logo != null)
                    {
                        byte[] bytes = Convert.FromBase64String(qrcode.Logo);
                        Image logo;
                        using (MemoryStream logoms = new MemoryStream(bytes))
                        {
                            logo = new Bitmap(logoms);
                        }

                        Bitmap resizedlogo = new Bitmap(logo, new Size(300, 300));
                        Graphics g = Graphics.FromImage(bitmap);
                        g.DrawImage(resizedlogo, new Point((bitmap.Width - resizedlogo.Width) / 2, (bitmap.Height - resizedlogo.Height) / 2));
                    }
                    MemoryStream ms = new MemoryStream();
                    bitmap.Save(ms, ImageFormat.Jpeg);
                    byte[] byteImage = ms.ToArray();
                    qrcode.Image = byteImage;
                    qrcode.BaseUrl = customURL;
                    qrcode.LinkGuid = qrcode.LinkGuid;
                    var dbUrl = _dbo.QrCodes.Include(i => i.User).FirstOrDefault(x => x.UserId == qrcode.UserId && x.QrCodeId == qrcode.QrCodeId);
                    if (dbUrl == null)
                    {
                        _dbo.QrCodes.Add(qrcode);
                        r = _dbo.SaveChanges() > 0;
                        msg = "Record added successfully";
                    }
                    else
                    {
                        dbUrl.QrCodeId = qrcode.QrCodeId;
                        dbUrl.Title = qrcode.Title;
                        dbUrl.Visits = qrcode.Visits;
                        dbUrl.LinkGuid = qrcode.LinkGuid;
                        dbUrl.Image = byteImage;
                        dbUrl.IsActive = qrcode.IsActive;
                        _dbo.QrCodes.Update(dbUrl);
                        r = _dbo.SaveChanges() > 0;
                        msg = "Record updated successfully";
                    }
                }
                else
                {
                    msg = "Slug already exists!";
                }
            }
            return r;
        }

        public bool duplicate(QrCode qrcode, out string msg)
        {
            msg = null;
            var e = _dbo.QrCodes.Include(i => i.User).FirstOrDefault(x => x.UserId == qrcode.UserId && x.QrCodeId == qrcode.QrCodeId);
            if (e != null) return false;
            _dbo.QrCodes.Add(qrcode);
            var r = _dbo.SaveChanges() > 0;
            if (r)
                msg = "QrCodes Duplicated Succesfully";

            return r;

        }
        public bool Update(QrCode qrcode, out string msg)
        {
            msg = null;
            var e = _dbo.QrCodes.Include(i => i.User).FirstOrDefault(x => x.UserId == qrcode.UserId && x.QrCodeId == qrcode.QrCodeId);
            if (e == null) return false;
            string customURL = "https://magiclinkz.azurewebsites.net/qr/";
            if (e != null)
            {
                e.Title = qrcode.Title;
                e.TrackedUrl = qrcode.TrackedUrl;
                e.IsActive = qrcode.IsActive;
                e.LinkGuid = qrcode.LinkGuid;
                e.BaseUrl = customURL += qrcode.LinkGuid;
            }
            _dbo.QrCodes.Update(e);
            var r = _dbo.SaveChanges() > 0;
            if (r)
                msg = "QrCode Updated Succesfully";

            return r;

        }
        public dynamic All(QRCodeFilter filter, out int count, out string msg)
        {
            msg = null;

            var querry = _dbo.QrCodes.Include(i => i.User).Where(x => x.UserId == filter.UserId && (string.IsNullOrEmpty(filter.FilterText) || x.Title.Contains(filter.FilterText)
                                                          || x.TrackedUrl.Contains(filter.FilterText))).ToList();

            //var querry = _dbo.QrCodes.OrderByDescending(i => i.QrCodeId).Where(i =>  (string.IsNullOrEmpty(filter.FilterText) || i.Title.Contains(filter.FilterText)
            //        || i.TrackedUrl.Contains(filter.FilterText))).ToList();
            count = querry.Count;
            return querry.Skip(filter.PageSize * (filter.Page - 1)).Take(filter.PageSize).ToList();
        }
    }
}

