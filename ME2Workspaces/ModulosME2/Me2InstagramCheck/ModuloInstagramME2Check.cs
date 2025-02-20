
using Me2Workspaces.ModulosME2.Database;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Me2Workspaces.ModulosME2.Me2InstagramCheck
{
    public class ModuloInstagramME2Check
    {

        public static readonly HttpClient client = new HttpClient();

        public static async Task<UserData> GetInstagramAPI(string username)
        {
            try
            {
                ConnectionDB tokendbService = new();

                var resultToken = await tokendbService.GetTokenByID(1);

                string apiResult = await GetInstagramData(username, resultToken.token);

                var result = await JSONToModelME2InstagramData(apiResult);

                if (result != null)
                {
                    //Me2InstagramService servicoMe2Insta = new Me2InstagramService();
                    //await servicoMe2Insta.AtualizaBD(result);
                    return result;
                }

                return null;
            }
            catch (Exception ex)
            {
               

                return null;
            }
            
        }

        public static async Task<string> GetInstagramData(string username, string token)
        {
            var requestUrl = $"https://graph.facebook.com/v20.0/17841454088809121?fields=business_discovery.username({username}){{followers_count,follows_count,media_count,biography,id,profile_picture_url,username,website,media{{comments_count,like_count,media_product_type,caption,media_type,media_url,timestamp}}}}&access_token={token}";

            /* var requestUrl = $"https://graph.facebook.com/v20.0/17841454088809121?" +
            $"fields=" +
            $"business_discovery.username({username}){{" +
            $"followers_count," +
            $"follows_count," +
            $"media_count," +
            $"biography," +
            $"id," +
            $"profile_picture_url," +
            $"username," +
            $"website," +
            $"media{{" +
            $"comments_count," +
            $"like_count," +
            $"media_product_type," +
            $"caption," +
            $"media_type," +
            $"media_url," +
            $"timestamp," +
            $"permalink_url," +
            $"insights{{" +
            $"metric(post_impressions,post_engagement)" +
            $"}}" +
            $"}}}}&access_token={token}"; */

            HttpResponseMessage response = await client.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            JObject data = JObject.Parse(responseBody);
            return data.ToString();
        }

        public static async Task<UserData> JSONToModelME2InstagramData(string json)
        {
            var data = JsonConvert.DeserializeObject<RootObject>(json); ;

            var userData = await TratarJSONApiME2(data);

            return userData;

        }

        public static async Task<UserData> TratarJSONApiME2(RootObject data)
        {
            UserData userData = new();
            RootData dataMedias = new();
            if (data == null)
            {
                return null;
            }

            userData.list_captions_count = new List<string>();
            userData.list_comments_count = new List<int>();
            userData.list_like_count = new List<int>();
            userData.list_timestamp_count = new List<string>();

            //Deserilize the JSON object from API
            dataMedias = JsonConvert.DeserializeObject<RootData>(data.business_discovery.media.ToString());

            userData.followers_count = data.business_discovery.followers_count;
            userData.follows_count = data.business_discovery.follows_count;
            userData.website = data.business_discovery.website;
            userData.profile_picture_url = data.business_discovery.profile_picture_url;
            userData.username = data.business_discovery.username;
            userData.id = data.business_discovery.id;
            userData.media_count = data.business_discovery.media_count;

            if (dataMedias != null)
            {
                foreach (var item in dataMedias.data)
                {
                    userData.list_captions_count.Add(item.caption);
                    userData.list_comments_count.Add(item.comments_count);
                    userData.list_like_count.Add(item.like_count);
                    userData.list_timestamp_count.Add(item.Timestamp);
                }
            }

            return userData;
        }


    }



    public class Media
    {
        [JsonProperty("data")]
        List<Data> data { get; set; }
    }

    public class BusinessDiscovery
    {
        public int followers_count { get; set; }
        public int follows_count { get; set; }
        public int media_count { get; set; }
        public string id { get; set; }
        public string profile_picture_url { get; set; }
        public string username { get; set; }
        public string website { get; set; }

        [JsonProperty("media")]
        public object media { get; set; }
    }

    public class RootObject
    {
        public BusinessDiscovery business_discovery { get; set; }
    }

    public class RootData
    {
        public List<Data> data { get; set; }
    }

    public class UserData
    {
        public int followers_count { get; set; }

        public int follows_count { get; set; }

        public int media_count { get; set; }
        public string id { get; set; }
        public string profile_picture_url { get; set; }
        public string username { get; set; }
        public string website { get; set; }

        public List<int> list_like_count { get; set; }

        public List<int> list_comments_count { get; set; }

        public List<string> list_captions_count { get; set; }

        public List<string> list_timestamp_count { get; set; }

        // Propriedades temporárias para strings JSON
        public string list_like_count_json { get; set; }
        public string list_comments_count_json { get; set; }
        public string list_captions_count_json { get; set; }
        public string list_timestamp_count_json { get; set; }
    }


    public class Data
    {

        public int comments_count { get; set; }
        public int like_count { get; set; }
        public string media_product_type { get; set; }
        public string caption { get; set; }
        public string media_type { get; set; }
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
        public string id { get; set; }
        public string media_url { get; set; }
    }
}


//CREATE TABLE me2instagramdata (
//    id VARCHAR(255) PRIMARY KEY,
//    followers_count INT,
//    follows_count INT,
//    media_count INT,
//    profile_picture_url TEXT,
//    username VARCHAR(255),
//    website VARCHAR(255),
//    list_like_count LONGTEXT,
//    list_comments_count LONGTEXT,
//    list_captions_count LONGTEXT,
//    list_timestamp_count LONGTEXT
//);