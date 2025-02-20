using Google.Apis.YouTube.v3.Data;
using Google.Apis.YouTube.v3;
using System.Xml;
using Google.Apis.Services;
using System.Collections.Generic;
using Me2Workspaces.ModulosME2.Database;

namespace ME2Workspaces.ModulosME2.Me2YoutubeCheck
{
    public class Me2YoutubeAPI
    {
        public async Task<ModeloResponseInfosYTAPI> GetInfoFromYTAPIOld(string channel_id)
        {
            ConnectionDB dbService = new();

            ConnectionDB.Tokens token = await dbService.GetTokenByID(2);

            if (token == null)
            {
                return null;
            }

            YouTubeService youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = token.token,
                ApplicationName = "ME2YoutubeCheck"
            });

            var channelRequest = youtubeService.Channels.List("snippet,contentDetails,statistics");

            channelRequest.Id = channel_id;

            var channelResponse = await channelRequest.ExecuteAsync();

            var playlistId = channelResponse.Items[0].ContentDetails.RelatedPlaylists.Uploads;
            var channelId = channelResponse.Items[0].Id;
            var channelName = channelResponse.Items[0].Snippet.Title;
            var subscriberCount = channelResponse.Items[0].Statistics.SubscriberCount;

            // Extrair a URL da imagem do perfil do canal
            var channelImageUrl = channelResponse.Items[0].Snippet.Thumbnails.High.Url;

            var nextPageToken = "";
            var videoCount = 0;

            ulong? likecountTotal = 0;
            ulong? dislikecountTotal = 0;
            ulong? commentcountTotal = 0;
            ulong? viewcountTotal = 0;

            long countTotal = 0;

            var videoIds = new List<string>();

            var publicationDates = new List<DateTime>();
            var likeCounts = new List<ulong>();
            var dislikeCounts = new List<ulong>();
            var commentCounts = new List<ulong>();
            var viewCounts = new List<ulong>();
            var durations = new List<long>();
            var categories = new List<string>();
            var descriptions = new List<string>(); // Lista para armazenar descrições

            while (nextPageToken != null)
            {
                var playlistItemsRequest = youtubeService.PlaylistItems.List("contentDetails");
                playlistItemsRequest.PlaylistId = playlistId;
                playlistItemsRequest.MaxResults = 50;
                playlistItemsRequest.PageToken = nextPageToken;

                var playlistItemsResponse = await playlistItemsRequest.ExecuteAsync();

                videoIds.AddRange(playlistItemsResponse.Items.Select(item => item.ContentDetails.VideoId));
                videoCount += playlistItemsResponse.Items.Count;

                nextPageToken = playlistItemsResponse.NextPageToken;

                if (videoIds.Count >= 50 || nextPageToken == null)
                {
                    var videoRequest = youtubeService.Videos.List("snippet,statistics,contentDetails");
                    videoRequest.Id = string.Join(",", videoIds);
                    var videoResponse = await videoRequest.ExecuteAsync();

                    foreach (var video in videoResponse.Items)
                    {
                        likecountTotal += video.Statistics.LikeCount;
                        dislikecountTotal += video.Statistics.DislikeCount;
                        viewcountTotal += video.Statistics.ViewCount;
                        commentcountTotal += video.Statistics.CommentCount;

                        publicationDates.Add(video.Snippet.PublishedAt.Value);
                        likeCounts.Add(video.Statistics.LikeCount ?? 0);
                        dislikeCounts.Add(video.Statistics.DislikeCount ?? 0);
                        viewCounts.Add(video.Statistics.ViewCount ?? 0);
                        commentCounts.Add(video.Statistics.CommentCount ?? 0);
                        durations.Add(XmlConvert.ToTimeSpan(video.ContentDetails.Duration).Ticks);
                        categories.Add(video.Snippet.CategoryId);
                        descriptions.Add(video.Snippet.Description); // Adiciona a descrição à lista
                    }

                    videoIds.Clear();
                }

                countTotal += playlistItemsResponse.Items.Count;
                if (countTotal >= 1200)
                {
                    break;
                }
            }

            ModeloResponseInfosYTAPI modeloResponse = new ModeloResponseInfosYTAPI
            {
                ChannelId = channelId,
                ChannelName = channelName,
                ChannelImage = channelImageUrl,
                VideoCount = videoCount,
                ViewcountTotal = viewcountTotal,
                LikecountTotal = likecountTotal,
                DislikecountTotal = dislikecountTotal,
                CommentcountTotal = commentcountTotal,
                SubscriberCount = subscriberCount,
                PublicationDates = publicationDates,
                LikeCounts = likeCounts,
                DislikeCounts = dislikeCounts,
                CommentCounts = commentCounts,
                ViewCounts = viewCounts,
                Durations = durations,
                Categories = categories,
                Descriptions = descriptions // Adiciona a lista de descrições ao objeto de resposta
            };

            var score = await ME2YTScore.GetScoreByYTResponse(modeloResponse);

            modeloResponse.Score = score;

            Me2YoutubeDatabase db = new Me2YoutubeDatabase();

            await db.SaveResponseINFOYTApi(modeloResponse);

            return modeloResponse;
        }


        public async Task<ModeloResponseInfosYTAPI> GetInfoFromYTAPI(string channel_id)
        {
            try
            {

                ConnectionDB dbService = new();

                ConnectionDB.Tokens token = await dbService.GetTokenByID(2);

                if (token == null)
                {
                    return null;
                }

                YouTubeService youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = token.token,
                    ApplicationName = "ME2YoutubeCheck"
                });

                var channelRequest = youtubeService.Channels.List("snippet,contentDetails,statistics");

                channelRequest.Id = channel_id;

                var channelResponse = await channelRequest.ExecuteAsync();

                var playlistId = channelResponse.Items[0].ContentDetails.RelatedPlaylists.Uploads;
                var channelId = channelResponse.Items[0].Id;
                var channelName = channelResponse.Items[0].Snippet.Title;
                var subscriberCount = channelResponse.Items[0].Statistics.SubscriberCount;

                // Extrair a URL da imagem do perfil do canal
                var channelImageUrl = channelResponse.Items[0].Snippet.Thumbnails.High.Url;

                var nextPageToken = "";
                var videoCount = 0;

                ulong? likecountTotal = 0;
                ulong? dislikecountTotal = 0;
                ulong? commentcountTotal = 0;
                ulong? viewcountTotal = 0;

                long countTotal = 0;

                var videoIds = new List<string>();

                var publicationDates = new List<DateTime>();
                var likeCounts = new List<ulong>();
                var dislikeCounts = new List<ulong>();
                var commentCounts = new List<ulong>();
                var viewCounts = new List<ulong>();
                var durations = new List<long>();
                var categories = new List<string>();
                var descriptions = new List<string>(); // Lista para armazenar descrições

                List<Video> lista_Videos = new List<Video>();

                while (nextPageToken != null)
                {
                    var playlistItemsRequest = youtubeService.PlaylistItems.List("contentDetails");
                    playlistItemsRequest.PlaylistId = playlistId;
                    playlistItemsRequest.MaxResults = 50;
                    playlistItemsRequest.PageToken = nextPageToken;

                    var playlistItemsResponse = await playlistItemsRequest.ExecuteAsync();

                    videoIds.AddRange(playlistItemsResponse.Items.Select(item => item.ContentDetails.VideoId));
                    videoCount += playlistItemsResponse.Items.Count;  

                    nextPageToken = playlistItemsResponse.NextPageToken;

                    if (videoIds.Count >= 50 || nextPageToken == null)
                    {
                        var videoRequest = youtubeService.Videos.List("snippet,statistics,contentDetails");
                        videoRequest.Id = string.Join(",", videoIds);
                        var videoResponse = await videoRequest.ExecuteAsync();

                        lista_Videos.AddRange(videoResponse.Items);

                        //foreach (var video in videoResponse.Items)
                        //{
                        //    likecountTotal += video.Statistics.LikeCount;
                        //    dislikecountTotal += video.Statistics.DislikeCount;
                        //    viewcountTotal += video.Statistics.ViewCount;
                        //    commentcountTotal += video.Statistics.CommentCount;

                        //    publicationDates.Add(video.Snippet.PublishedAt.Value);
                        //    likeCounts.Add(video.Statistics.LikeCount ?? 0);
                        //    dislikeCounts.Add(video.Statistics.DislikeCount ?? 0);
                        //    viewCounts.Add(video.Statistics.ViewCount ?? 0);
                        //    commentCounts.Add(video.Statistics.CommentCount ?? 0);
                        //    durations.Add(XmlConvert.ToTimeSpan(video.ContentDetails.Duration).Ticks);
                        //    categories.Add(video.Snippet.CategoryId);
                        //    descriptions.Add(video.Snippet.Description); // Adiciona a descrição à lista
                        //}

                        videoIds.Clear();
                    }


                    countTotal += playlistItemsResponse.Items.Count;
                    if (countTotal >= 1200)
                    {
                        break;
                    }
                }

                foreach (var video in lista_Videos)
                {
                    likecountTotal += video.Statistics.LikeCount;
                    dislikecountTotal += video.Statistics.DislikeCount;
                    viewcountTotal += video.Statistics.ViewCount;
                    commentcountTotal += video.Statistics.CommentCount;

                    publicationDates.Add(video.Snippet.PublishedAt.Value);
                    likeCounts.Add(video.Statistics.LikeCount ?? 0);
                    dislikeCounts.Add(video.Statistics.DislikeCount ?? 0);
                    viewCounts.Add(video.Statistics.ViewCount ?? 0);
                    commentCounts.Add(video.Statistics.CommentCount ?? 0);
                    durations.Add(XmlConvert.ToTimeSpan(video.ContentDetails.Duration).Ticks);
                    categories.Add(video.Snippet.CategoryId);
                    descriptions.Add(video.Snippet.Description); // Adiciona a descrição à lista
                }

                ModeloResponseInfosYTAPI modeloResponse = new ModeloResponseInfosYTAPI
                {
                    ChannelId = channelId,
                    ChannelName = channelName,
                    ChannelImage = channelImageUrl,
                    VideoCount = videoCount,
                    ViewcountTotal = viewcountTotal,
                    LikecountTotal = likecountTotal,
                    DislikecountTotal = dislikecountTotal,
                    CommentcountTotal = commentcountTotal,
                    SubscriberCount = subscriberCount,
                    PublicationDates = publicationDates,
                    LikeCounts = likeCounts,
                    DislikeCounts = dislikeCounts,
                    CommentCounts = commentCounts,
                    ViewCounts = viewCounts,
                    Durations = durations,
                    Categories = categories,
                    Descriptions = descriptions // Adiciona a lista de descrições ao objeto de resposta
                };

                var score = await ME2YTScore.GetScoreByYTResponse(modeloResponse);

                modeloResponse.Score = score;

                Me2YoutubeDatabase db = new Me2YoutubeDatabase();

                await db.SaveResponseINFOYTApi(modeloResponse);

                return modeloResponse;

            }
            catch (Exception ex)
            {

             
                return null;
            }
        }

    }



}

public class ModeloResponseInfosYTAPI
{
    public string ChannelId { get; set; }
    public string ChannelName { get; set; }
    public string ChannelImage { get; set; }
    public int VideoCount { get; set; } = 0;
    public ulong? LikecountTotal { get; set; } = 0;
    public ulong? DislikecountTotal { get; set; } = 0;
    public ulong? CommentcountTotal { get; set; } = 0;
    public ulong? ViewcountTotal { get; set; } = 0;
    public ulong? SubscriberCount { get; set; } = 0;
    public List<DateTime> PublicationDates { get; set; } = new List<DateTime>();
    public List<ulong> LikeCounts { get; set; } = new List<ulong>();
    public List<ulong> DislikeCounts { get; set; } = new List<ulong>();
    public List<ulong> CommentCounts { get; set; } = new List<ulong>();
    public List<ulong> ViewCounts { get; set; } = new List<ulong>();
    public List<long> Durations { get; set; } = new List<long>();
    public List<string> Categories { get; set; } = new List<string>();
    public List<string> Descriptions { get; set; } = new List<string>();

    public double Score { get; set; } = 0;

    // Propriedades temporárias para strings JSON
    public string PublicationDatesJson { get; set; }
    public string LikeCountsJson { get; set; }
    public string DislikeCountsJson { get; set; }
    public string CommentCountsJson { get; set; }
    public string ViewCountsJson { get; set; }
    public string DurationsJson { get; set; }
    public string CategoriesJson { get; set; }
    public string DescriptionsJson { get; set; }
}
