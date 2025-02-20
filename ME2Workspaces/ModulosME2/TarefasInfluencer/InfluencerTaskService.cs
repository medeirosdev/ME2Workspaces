using Dapper;
using Me2Workspaces.ModulosME2.Database;

namespace ME2Workspaces.ModulosME2.TarefasInfluencer
{
    public class InfluencerTaskService
    {
        // Cria a conexão utilizando a classe ConnectionDB
        private readonly ConnectionDB connectionDB = new ConnectionDB();

        /// <summary>
        /// Cria uma nova tarefa e retorna o ID inserido.
        /// </summary>
        public async Task<long> CreateTask(InfluencerTask task)
        {
            using (var connection = await connectionDB.NewConnection())
            {
                var query = @"
                    INSERT INTO influencer_task (
                        ID_Campanha,
                        ID_Influencer,
                        Descricao,
                        Tipo_Tarefa,
                        Prazo,
                        DataCriacao,
                        Feito
                    ) VALUES (
                        @ID_Campanha,
                        @ID_Influencer,
                        @Descricao,
                        @Tipo_Tarefa,
                        @Prazo,
                        @DataCriacao,
                        @Feito
                    );
                    SELECT LAST_INSERT_ID();";
                // O enum Tipo_Tarefa será convertido para inteiro automaticamente.
                return await connection.QueryFirstOrDefaultAsync<long>(query, task);
            }
        }

        /// <summary>
        /// Retorna uma tarefa pelo seu ID.
        /// </summary>
        public async Task<InfluencerTask> GetTaskById(long id)
        {
            using (var connection = await connectionDB.NewConnection())
            {
                var query = "SELECT * FROM influencer_task WHERE id = @Id;";
                return await connection.QueryFirstOrDefaultAsync<InfluencerTask>(query, new { Id = id });
            }
        }

        /// <summary>
        /// Retorna as tarefas filtradas por ID da campanha e ID do influenciador.
        /// </summary>
        public async Task<List<InfluencerTask>> GetTasksByCampaignAndInfluencer(long campaignId, long influencerId)
        {
            using (var connection = await connectionDB.NewConnection())
            {
                var query = "SELECT * FROM influencer_task WHERE ID_Campanha = @CampaignId AND ID_Influencer = @InfluencerId;";
                var result = await connection.QueryAsync<InfluencerTask>(query, new { CampaignId = campaignId, InfluencerId = influencerId });
                return result.AsList();
            }
        }

        /// <summary>
        /// Retorna as tarefas de uma determinada campanha.
        /// </summary>
        public async Task<List<InfluencerTask>> GetTasksByCampaign(long campaignId)
        {
            using (var connection = await connectionDB.NewConnection())
            {
                var query = "SELECT * FROM influencer_task WHERE ID_Campanha = @CampaignId;";
                var result = await connection.QueryAsync<InfluencerTask>(query, new { CampaignId = campaignId });
                return result.AsList();
            }
        }

        /// <summary>
        /// Retorna as tarefas de um determinado influenciador.
        /// </summary>
        public async Task<List<InfluencerTask>> GetTasksByInfluencer(long influencerId)
        {
            using (var connection = await connectionDB.NewConnection())
            {
                var query = "SELECT * FROM influencer_task WHERE ID_Influencer = @InfluencerId;";
                var result = await connection.QueryAsync<InfluencerTask>(query, new { InfluencerId = influencerId });
                return result.AsList();
            }
        }

        /// <summary>
        /// Atualiza os dados de uma tarefa.
        /// </summary>
        public async Task<bool> UpdateTask(InfluencerTask task)
        {
            using (var connection = await connectionDB.NewConnection())
            {
                var query = @"
                    UPDATE influencer_task SET
                        ID_Campanha = @ID_Campanha,
                        ID_Influencer = @ID_Influencer,
                        Descricao = @Descricao,
                        Tipo_Tarefa = @Tipo_Tarefa,
                        Prazo = @Prazo,
                        DataCriacao = @DataCriacao,
                        Feito = @Feito
                    WHERE id = @Id;";
                var rowsAffected = await connection.ExecuteAsync(query, task);
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// Exclui uma tarefa pelo seu ID.
        /// </summary>
        public async Task<bool> DeleteTask(long id)
        {
            using (var connection = await connectionDB.NewConnection())
            {
                var query = "DELETE FROM influencer_task WHERE id = @Id;";
                var rowsAffected = await connection.ExecuteAsync(query, new { Id = id });
                return rowsAffected > 0;
            }
        }
    }
}
