using System;
using System.Collections.Generic;
using System.Linq;
using amazon_web.Database;
using amazon_web.Database.Entities;
using amazon_web.Database.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace amazon_web.Services
{
    public class CampaignService
    {
        private readonly AmazonContext _dbo;

        public CampaignService(AmazonContext context)
        {
            _dbo = context;
        }

       
        public bool Delete(long campaignId, out string msg)
        {
            msg = null;
            var e = _dbo.Campaigns.FirstOrDefault(x => x.CampaignId == campaignId);
            if (e == null) return false;
            _dbo.Campaigns.Remove(e);
            var r = _dbo.SaveChanges() > 0;
            if (r)
                msg = "Campaign Deleted Succesfully";

            return r;

        }

        public bool Update(Campaign campaign, out string msg)
        {

            msg = null;
            string customURL = "https://magiclinkz.azurewebsites.net/cmp/";
            var e = _dbo.Campaigns.Include(i => i.Weights).FirstOrDefault(x => x.CampaignId == campaign.CampaignId);
            if (e == null)
            {

                _dbo.Campaigns.Add(campaign);
                msg = "Campaign Added successfully";
            }
            else
            {
                e.CampaignLink = campaign.CampaignLink;
                e.Title = campaign.Title;
                e.Asin = campaign.Asin;
                e.CampaignLink = customURL += campaign.LinkGuid;
                e.MarketPlace = campaign.MarketPlace;
                e.LinkGuid = campaign.LinkGuid;
                e.Weights = campaign.Weights;
                msg = "Campaign updated successfully";
                _dbo.Campaigns.Update(e);
                _dbo.SaveChanges();
            }
     
            return _dbo.SaveChanges() > 0;

        }
          public bool Create(Campaign campaign, out string msg)
        {
            msg = null;
            var e = _dbo.Campaigns.Include(i => i.User).FirstOrDefault(x=> x.UserId == campaign.UserId && x.CampaignId ==  campaign.CampaignId);

            if (e == null)
            {
                //string customURL = "https://localhost:44300api/Campaign/Redirect?key=";
                // string customURL = "https://localhost:44300/cmp/";
                string customURL = "https://magiclinkz.azurewebsites.net/cmp/";
                // string customURL = "https://magiclinkz.azurewebsites.netapi/Campaign/Redirect?key=";
                if (campaign.LinkGuid == null || campaign.LinkGuid == "")
                {
                    string key = "";
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
                            key += numbers[random].ToString();
                        }
                        else
                        {
                            // Use a character  
                            random = rand.Next(0, characters.Count);
                            key += characters[random].ToString();

                        }

                    }
                    customURL += key;
                    campaign.CampaignLink = customURL;
                    campaign.LinkGuid = key;
                    _dbo.Campaigns.Add(campaign);
                    msg = "Campaign created up successfully";
                }
                else
                {
                    var slug = _dbo.Campaigns.Include(i => i.User).FirstOrDefault(x => x.UserId == campaign.UserId && x.LinkGuid == campaign.LinkGuid);
                    if (slug == null)
                    {
                        customURL += campaign.LinkGuid;
                        campaign.CampaignLink = customURL;
                        campaign.LinkGuid = campaign.LinkGuid;
                        _dbo.Campaigns.Add(campaign);
                        msg = "Campaign created up successfully";
                    }
                    else
                    {
                        msg = "Slug already exists!";
                    }
                }
            }
            else
            {
                e.User.UserId = campaign.UserId;
                e.CampaignLink = campaign.CampaignLink;
                e.Title = campaign.Title;
                e.Asin = campaign.Asin;
                e.MarketPlace = campaign.MarketPlace;
                e.Weights = campaign.Weights;
                _dbo.Campaigns.Update(e);
                _dbo.SaveChanges();
            }
     
            return _dbo.SaveChanges() > 0;

        }



        public dynamic All(CampaignFiter filter ,out int count, out string msg)
        {
            msg = null;
            var query = _dbo.Campaigns.Include(i => i.Weights).Include(u => u.User).OrderByDescending(i => i.CampaignId).Where(i => i.UserId == filter.UserId && (string.IsNullOrEmpty(filter.FilterText) || i.Title.Contains(filter.FilterText)
                                                             || i.MarketPlace.Contains(filter.FilterText))).ToList();
            count = query.Count;
            return query.Skip(filter.PageSize * (filter.Page -1)).Take(filter.PageSize).ToList();
        }


        public dynamic GetKeyword(string linkGuid)
        {
            var campaign = _dbo.Campaigns.Include(i => i.Weights).Include(u => u.User).FirstOrDefault(x =>  x.LinkGuid == linkGuid);
            if (campaign == null) return string.Empty;

            if (campaign.Weights.All(x => x.Weight == x.Count))
            {
                campaign.Weights.ForEach(x => x.Count = 0);
                _dbo.SaveChanges();
            }

            var keywords = campaign.Weights.Where(x => x.Weight > x.Count).ToList();

            var random = new Random();

            var index = random.Next(keywords.Count);
            var keyword = keywords[index];
            keyword.Count += 1;
            campaign.Visits += 1;

            _dbo.SaveChanges();

            return new {campaign.Asin, keyword.Keyword, campaign.MarketPlace};
        }
    }
}

