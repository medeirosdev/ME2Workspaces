using Dapper;
using Me2Workspaces.ModulosME2.Database;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace ME2Workspaces.ModulosME2.Me2YoutubeCheck
{
    public class Me2YoutubeDatabase
    {

        ConnectionDB connectionDB = new();


        public async Task<bool> SaveResponseINFOYTApi(ModeloResponseInfosYTAPI model)
        {
            bool existInDB = await ExistsAsync(model.ChannelId);

            bool status = false;

            if (existInDB)
            {
                status = await UpdateAsync(model);
            }
            else
            {
                status = await CreateAsync(model);
                
            }

            return status;

        }


        public async Task<bool> CreateAsync(ModeloResponseInfosYTAPI model)
        {
            try
            {
                var query = @"
        INSERT INTO me2youtubedatachannel (
            ChannelId, 
            ChannelName, 
            ChannelImage, 
            VideoCount, 
            LikecountTotal, 
            DislikecountTotal, 
            CommentcountTotal, 
            ViewcountTotal, 
            SubscriberCount, 
            PublicationDates, 
            LikeCounts, 
            DislikeCounts, 
            CommentCounts, 
            ViewCounts, 
            Durations, 
            Categories, 
            Descriptions, 
            Score
        ) VALUES (
            @ChannelId, 
            @ChannelName, 
            @ChannelImage, 
            @VideoCount, 
            @LikecountTotal, 
            @DislikecountTotal, 
            @CommentcountTotal, 
            @ViewcountTotal, 
            @SubscriberCount, 
            @PublicationDates, 
            @LikeCounts, 
            @DislikeCounts, 
            @CommentCounts, 
            @ViewCounts, 
            @Durations, 
            @Categories, 
            @Descriptions, 
            @Score
        )";

                var parameters = new
                {
                    ChannelId = model.ChannelId,
                    ChannelName = model.ChannelName,
                    ChannelImage = model.ChannelImage ?? "",
                    VideoCount = model.VideoCount,
                    LikecountTotal = model.LikecountTotal,
                    DislikecountTotal = model.DislikecountTotal,
                    CommentcountTotal = model.CommentcountTotal,
                    ViewcountTotal = model.ViewcountTotal,
                    SubscriberCount = model.SubscriberCount,
                    PublicationDates = JsonConvert.SerializeObject(model.PublicationDates),
                    LikeCounts = JsonConvert.SerializeObject(model.LikeCounts),
                    DislikeCounts = JsonConvert.SerializeObject(model.DislikeCounts),
                    CommentCounts = JsonConvert.SerializeObject(model.CommentCounts),
                    ViewCounts = JsonConvert.SerializeObject(model.ViewCounts),
                    Durations = JsonConvert.SerializeObject(model.Durations),
                    Categories = JsonConvert.SerializeObject(model.Categories),
                    Descriptions = JsonConvert.SerializeObject(model.Descriptions),
                    Score = model.Score
                };

                using (var connection = await connectionDB.NewConnection())
                {
                    if (connection == null) return false;

                    // Executa a query
                    var result = await connection.ExecuteAsync(query, parameters);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                // Registra o erro
                Console.WriteLine($"Erro: {ex.Message}");
                return false;
            }
        }

        public async Task<ModeloResponseInfosYTAPI> GetByIdAsync(string channelId)
        {
            var query = @"
        SELECT * FROM me2youtubedatachannel WHERE ChannelId = @ChannelId";

            using (var connection = await connectionDB.NewConnection())
            {
                if (connection == null) return null;

                return await connection.QuerySingleOrDefaultAsync<ModeloResponseInfosYTAPI>(query, new { ChannelId = channelId });
            }
        }

        public async Task<bool> ExistsAsync(string channelId)
        {
            try
            {
                // Monta a query SQL para verificar a existência do registro
                var query = @"
        SELECT COUNT(*) 
        FROM me2youtubedatachannel 
        WHERE ChannelId = @ChannelId";

                using (var connection = await connectionDB.NewConnection())
                {
                    if (connection == null) return false;

                    // Executa a query e obtém o número de registros encontrados
                    var count = await connection.ExecuteScalarAsync<int>(query, new { ChannelId = channelId });

                    // Se count for maior que 0, o registro existe
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                // Registra o erro
                Console.WriteLine($"Erro: {ex.Message}");
                return false;
            }
        }


        public async Task<IEnumerable<ModeloResponseInfosYTAPI>> GetModeloResponseInfosYTAPIAsync()
        {
            var query = @"
    SELECT 
        ChannelId, 
        ChannelName, 
        ChannelImage, 
        VideoCount, 
        LikecountTotal, 
        DislikecountTotal, 
        CommentcountTotal, 
        ViewcountTotal, 
        SubscriberCount, 
        PublicationDates as PublicationDatesJson, 
        LikeCounts as LikeCountsJson, 
        DislikeCounts as DislikeCountsJson, 
        CommentCounts as CommentCountsJson, 
        ViewCounts as ViewCountsJson, 
        Durations as DurationsJson, 
        Categories as CategoriesJson, 
        Descriptions as DescriptionsJson, 
        Score 
    FROM me2youtubedatachannel";

            using (var connection = await connectionDB.NewConnection())
            {
                if (connection == null) return null;

                var result = await connection.QueryAsync<ModeloResponseInfosYTAPI>(query);

                // Mapear os JSONs para as listas
                foreach (var item in result)
                {
                    item.PublicationDates = JsonConvert.DeserializeObject<List<DateTime>>(item.PublicationDatesJson);
                    item.LikeCounts = JsonConvert.DeserializeObject<List<ulong>>(item.LikeCountsJson);
                    item.DislikeCounts = JsonConvert.DeserializeObject<List<ulong>>(item.DislikeCountsJson);
                    item.CommentCounts = JsonConvert.DeserializeObject<List<ulong>>(item.CommentCountsJson);
                    item.ViewCounts = JsonConvert.DeserializeObject<List<ulong>>(item.ViewCountsJson);
                    item.Durations = JsonConvert.DeserializeObject<List<long>>(item.DurationsJson);
                    item.Categories = JsonConvert.DeserializeObject<List<string>>(item.CategoriesJson);
                    item.Descriptions = JsonConvert.DeserializeObject<List<string>>(item.DescriptionsJson);
                }

                return result;
            }
        }

        public async Task<bool> UpdateAsync(ModeloResponseInfosYTAPI model)
        {
            try
            {
                // Serializa as listas para JSON
                var publicationDatesJson = JsonConvert.SerializeObject(model.PublicationDates);
                var likeCountsJson = JsonConvert.SerializeObject(model.LikeCounts);
                var dislikeCountsJson = JsonConvert.SerializeObject(model.DislikeCounts);
                var commentCountsJson = JsonConvert.SerializeObject(model.CommentCounts);
                var viewCountsJson = JsonConvert.SerializeObject(model.ViewCounts);
                var durationsJson = JsonConvert.SerializeObject(model.Durations);
                var categoriesJson = JsonConvert.SerializeObject(model.Categories);
                var descriptionsJson = JsonConvert.SerializeObject(model.Descriptions);

                // Monta a query SQL com parâmetros
                var query = @"
        UPDATE me2youtubedatachannel SET
            ChannelName = @ChannelName,
            ChannelImage = @ChannelImage,
            VideoCount = @VideoCount,
            LikecountTotal = @LikecountTotal,
            DislikecountTotal = @DislikecountTotal,
            CommentcountTotal = @CommentcountTotal,
            ViewcountTotal = @ViewcountTotal,
            SubscriberCount = @SubscriberCount,
            PublicationDates = @PublicationDates,
            LikeCounts = @LikeCounts,
            DislikeCounts = @DislikeCounts,
            CommentCounts = @CommentCounts,
            ViewCounts = @ViewCounts,
            Durations = @Durations,
            Categories = @Categories,
            Descriptions = @Descriptions,
            Score = @Score
        WHERE ChannelId = @ChannelId";

                using (var connection = await connectionDB.NewConnection())
                {
                    if (connection == null) return false;

                    // Cria um objeto anônimo com os parâmetros para a query
                    var parameters = new
                    {
                        ChannelId = model.ChannelId,
                        ChannelName = model.ChannelName,
                        ChannelImage = model.ChannelImage ?? "",
                        VideoCount = model.VideoCount,
                        LikecountTotal = model.LikecountTotal,
                        DislikecountTotal = model.DislikecountTotal,
                        CommentcountTotal = model.CommentcountTotal,
                        ViewcountTotal = model.ViewcountTotal,
                        SubscriberCount = model.SubscriberCount,
                        PublicationDates = publicationDatesJson,
                        LikeCounts = likeCountsJson,
                        DislikeCounts = dislikeCountsJson,
                        CommentCounts = commentCountsJson,
                        ViewCounts = viewCountsJson,
                        Durations = durationsJson,
                        Categories = categoriesJson,
                        Descriptions = descriptionsJson,
                        Score = model.Score
                    };

                    // Executa a query com os parâmetros
                    var result = await connection.ExecuteAsync(query, parameters);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                // Registra o erro
                Console.WriteLine($"Erro: {ex.Message}");
                return false;
            }
        }
    }
}
