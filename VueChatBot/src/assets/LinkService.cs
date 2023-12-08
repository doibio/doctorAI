using amazon_web.Database;
using amazon_web.Database.Entities;
using amazon_web.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace amazon_web.Services
{
    public class LinkService
    {
        private readonly AmazonContext _dbo;
        public LinkService(AmazonContext context)
        {
            _dbo = context;
        }
        public bool Create(Links link, out string msg)
        {
            msg = null;
           var e = _dbo.Links.Include(i => i.User).FirstOrDefault(x => x.UserId == link.UserId && x.LinkId == link.LinkId);
             if (e == null)
            {
                // string customURL = "https://localhost:44300/lnk/";
                string customURL = "https://magiclinkz.azurewebsites.net/lnk/";
                string key = "";
                if(link.LinkGuid == null || link.LinkGuid == "") {
                    List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
                    List<char> characters = new List<char>()
{'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '-', '_'};

                    Random rand = new Random();
                    for (int i = 0; i < 11; i++)
                    {
                        int random = rand.Next(0, 3);
                        if (random == 1)
                        {
                            random = rand.Next(0, numbers.Count);
                            key += numbers[random].ToString();
                        }
                        else
                        {
                            random = rand.Next(0, characters.Count);
                            key += characters[random].ToString();
                        }
                    }
                    customURL += key;
                    link.LinkGuid = key;
                    link.Link = customURL;
                    _dbo.Links.Add(link);
                    msg = "Link created up successfully";
                }
                else
                {
                    var slug = _dbo.Campaigns.Include(i => i.User).FirstOrDefault(x => x.UserId == link.UserId && x.LinkGuid == link.LinkGuid);
                    if (slug == null)
                    {
                        customURL += link.LinkGuid;
                        link.Link = customURL;
                        link.LinkGuid = link.LinkGuid;
                        _dbo.Links.Add(link);
                        msg = "Link created up successfully";
                    }
                    else
                    {
                        msg = "Slug already exists!";
                    }
                }
               
            }
            else
            {
                e.User.UserId = link.UserId;
                e.Link = link.Link;
                e.Weights = link.Weights;
                _dbo.Links.Update(e);
                _dbo.SaveChanges();
            }

            return _dbo.SaveChanges() > 0;

        }
        public dynamic All(LinkFilter filter, out int count, out string msg)
        {
            msg = null;
            var query = _dbo.Links.Include(i => i.Weights).Include(u => u.User).OrderByDescending(i => i.LinkGuid).Where(i => i.UserId == filter.UserId && (string.IsNullOrEmpty(filter.FilterText) || i.Title.Contains(filter.FilterText))).ToList();
            count = query.Count;
            return query.Skip(filter.PageSize * (filter.Page - 1)).Take(filter.PageSize).ToList();
        }

        public dynamic GetKeyword(string linkGuid)
        {
            var link = _dbo.Links.Include(i => i.Weights).Include(u => u.User).FirstOrDefault(x => x.LinkGuid == linkGuid);
            if (link == null) return string.Empty;

            if (link.Weights.All(x => x.Weight == x.Count))
            {
                link.Weights.ForEach(x => x.Count = 0);
                _dbo.SaveChanges();
            }

            var keywords = link.Weights.Where(x => x.Weight > x.Count).ToList();

            var random = new Random();

            var index = random.Next(keywords.Count);
            var keyword = keywords[index];
            keyword.Count += 1;
            link.Visits += 1;

            _dbo.SaveChanges();

            return keyword.Keyword;
        }
        public bool Delete(long linkId, out string msg)
        {
            msg = null;
            var e = _dbo.Links.FirstOrDefault(x => x.LinkId == linkId);
            if (e == null) return false;
            _dbo.Links.Remove(e);
            var r = _dbo.SaveChanges() > 0;
            if (r)
                msg = "Link Deleted Succesfully";

            return r;

        }
        public bool Update(Links link, out string msg)
        {
            string customURL = "https://magiclinkz.azurewebsites.net/qr/";
            msg = null;
            var e = _dbo.Links.Include(i => i.Weights).FirstOrDefault(x => x.LinkId == link.LinkId);
            if (e == null)
            {

                _dbo.Links.Add(link);
                msg = "Link Added successfully";
            }
            else
            {
                e.Title = link.Title;
                e.Weights = link.Weights;
                e.Link = customURL += link.LinkGuid;
                e.LinkGuid = link.LinkGuid;
                msg = "Link updated successfully";
                _dbo.Links.Update(e);
            }

            return _dbo.SaveChanges() > 0;

        }
    }
}
