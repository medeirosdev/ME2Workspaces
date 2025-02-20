using Dapper;

using Me2Workspaces.ModulosME2.Database;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Me2Workspaces.ModulosME2.Me2InstagramCheck.InsightsReels
{
    public class Me2InstagramInsightReelsService
    {
        ConnectionDB connectionDB = new();

        // Método para inserir um novo Insight_Reels
        public async Task Create(Insight_Reels insight)
        {
            using (var conn = await connectionDB.NewConnection())
            {
                string query = @"INSERT INTO influencer_campanha (id_campanha, id_usuario, extra, username, jsonData) 
                                 VALUES (@Id_Campanha, @Id_Usuario, @Extra, @Username, @JsonData)";

                var parameters = new
                {
                    Id_Campanha = insight.Id_Campanha,
                    Id_Usuario = insight.Id_Usuario,
                    Extra = insight.Extra,
                    Username = insight.Username,
                    JsonData = JsonConvert.SerializeObject(insight.JsonData)
                };

                await conn.ExecuteAsync(query, parameters);
            }
        }

        // Método para buscar um Insight_Reels por ID
        public async Task<Insight_Reels> Read(long id)
        {
            using (var conn = await connectionDB.NewConnection())
            {
                string query = "SELECT * FROM influencer_campanha WHERE id = @Id";
                var result = await conn.QueryFirstOrDefaultAsync<dynamic>(query, new { Id = id });

                if (result != null)
                {
                    return new Insight_Reels
                    {
                        Id = result.id,
                        Id_Campanha = result.id_campanha,
                        Id_Usuario = result.id_usuario,
                        Extra = result.extra,
                        Username = result.username,
                        JsonData = JsonConvert.DeserializeObject<UserData>(result.jsonData)
                    };
                }
            }
            return null;
        }

        public async Task<List<Insight_Reels>> GetByCampanha(long id_Campanha)
        {
            using (var conn = await connectionDB.NewConnection())
            {
                string query = "SELECT * FROM influencer_campanha WHERE id_campanha = @id_Campanha";
                var results = await conn.QueryAsync<dynamic>(query, new { id_Campanha });

                List<Insight_Reels> insightsList = new List<Insight_Reels>();

                UserData userData = new();

                foreach (var result in results)
                {
                    Insight_Reels novoInsight = new Insight_Reels
                    {
                        Id = result.id,
                        Id_Campanha = result.id_campanha,
                        Id_Usuario = result.id_usuario,
                        Extra = result.extra,
                        Username = result.username
                    };

                    // Obter userData usando o username
                    var userDataAtualizado = await ModuloInstagramME2Check.GetInstagramAPI(novoInsight.Username);

                    novoInsight.JsonData = userDataAtualizado;

                    insightsList.Add(novoInsight);  
                }

                return insightsList;
            }
        }

        public async Task<Insight_Reels> GetByID(long ID)
        {
            using (var conn = await connectionDB.NewConnection())
            {
                string query = "SELECT * FROM influencer_campanha WHERE id = @id";
                var result = await conn.QueryFirstOrDefaultAsync<dynamic>(query, new { ID });

                Insight_Reels insightsList = new Insight_Reels();

                UserData userData = new();
                if (result != null)
                {
                    insightsList = new Insight_Reels
                    {
                        Id = result.id,
                        Id_Campanha = result.id_campanha,
                        Id_Usuario = result.id_usuario,
                        Extra = result.extra,
                        Username = result.username,
                        JsonData = JsonConvert.DeserializeObject<UserData>(result.jsonData)
                    };

                    return insightsList;
                }

                return new();

            }
        }

        // Método para atualizar um Insight_Reels existente
        public async Task Update(Insight_Reels insight)
        {
            using (var conn = await connectionDB.NewConnection())
            {
                string query = @"UPDATE influencer_campanha SET 
                                    id_campanha = @Id_Campanha,
                                    id_usuario = @Id_Usuario,
                                    extra = @Extra,
                                    username = @Username, 
                                    jsonData = @JsonData
                                 WHERE id = @Id";

                var parameters = new
                {
                    Id = insight.Id,
                    Id_Campanha = insight.Id_Campanha,
                    Id_Usuario = insight.Id_Usuario,
                    Extra = insight.Extra,
                    Username = insight.Username,
                    JsonData = JsonConvert.SerializeObject(insight.JsonData)
                };

                await conn.ExecuteAsync(query, parameters);
            }
        }

        // Método para deletar um Insight_Reels pelo ID
        public async Task Delete(long id)
        {
            using (var conn = await connectionDB.NewConnection())
            {
                string query = "DELETE FROM influencer_campanha WHERE id = @Id";
                await conn.ExecuteAsync(query, new { Id = id });
            }
        }

        // Método para obter todos os Insights_Reels
        public async Task<List<Insight_Reels>> GetAll()
        {
            using (var conn = await connectionDB.NewConnection())
            {
                string query = "SELECT * FROM influencer_campanha";
                var results = await conn.QueryAsync<dynamic>(query);

                List<Insight_Reels> insightsList = new List<Insight_Reels>();

                foreach (var result in results)
                {
                    insightsList.Add(new Insight_Reels
                    {
                        Id = result.id,
                        Id_Campanha = result.id_campanha,
                        Id_Usuario = result.id_usuario,
                        Extra = result.extra,
                        Username = result.username,
                        JsonData = JsonConvert.DeserializeObject<UserData>(result.jsonData)
                    });
                }

                return insightsList;
            }
        }
    }
}
