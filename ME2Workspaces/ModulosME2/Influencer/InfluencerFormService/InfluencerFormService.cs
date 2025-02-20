using Dapper;
using Me2Workspaces.ModulosME2.Database;
using System.Threading.Tasks;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Me2Workspaces.ModulosME2.Influencer.InfluencerFormService
{
    #region Enums

    public enum EnumRegiao
    {
        Norte,
        Nordeste,
        CentroOeste,
        Sudeste,
        Sul
    }

    public enum EnumCategoria
    {
        Moda,
        Tecnologia,
        Beleza,
        Esportes,
        Comida,
        Viagem,
        Educação,
        Entretenimento,
        Saúde,
        Fitness,
        Música,
        Cinema,
        Fotografia,
        Arte,
        Jogos,
        Literatura,
        Ciência,
        Sustentabilidade,
        Carros,
        Negócios,
        Finanças,
        DesenvolvimentoPessoal,
        BemEstar,
        Animais,
        Comédia,
        Política,
        Relacionamentos,
        Culinária,
        DIY,
        TecnologiaVestível,
        Espiritualidade,
        Investimentos,
        CulturaPop,
        Imóveis,
        Empreendedorismo,
        História,
        ModaSustentável,
        FotografiaDrone,
        Humor
    }

    #endregion

    #region Modelo

    public class InfluencerModel
    {
        // ***** CAMPOS NOVOS *****
        public long ID { get; set; }
        public long ID_Campanha { get; set; }
        public string Senha { get; set; }
        public string URLFoto { get; set; }
        public int SeloME2 { get; set; }

        // ***** CAMPOS ANTIGOS *****
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Instagram { get; set; }
        public string TikTok { get; set; }
        public string YouTube { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public EnumRegiao Regiao { get; set; }
        public EnumCategoria CategoriaA { get; set; }
        public EnumCategoria CategoriaB { get; set; }
        public EnumCategoria CategoriaC { get; set; }
        public EnumCategoria CategoriaD { get; set; }
        public string OutrasRedes { get; set; }

        // ***** CAMPO QUE NÃO VAI PARA O BANCO *****
        // public UserData infosInstagram { get; set; }  // preencha em tempo de execução se precisar
    }

    #endregion

    #region Serviço

    public class InfluencerFormService
    {
        private readonly ConnectionDB connectionDB = new ConnectionDB();

        /// <summary>
        /// Cria um novo influenciador no banco, inserindo todos os campos (inclusive os novos).
        /// Retorna true se bem-sucedido.
        /// </summary>
        public async Task<bool> CreateInfluencerAsync(InfluencerModel influencer)
        {
            const string query = @"
                INSERT INTO tabela_influencers_data (
                    ID_Campanha,
                    Senha,
                    URLFoto,
                    SeloME2,
                    Email,
                    Telefone,
                    Instagram,
                    TikTok,
                    YouTube,
                    Nome,
                    Idade,
                    Regiao,
                    CategoriaA,
                    CategoriaB,
                    CategoriaC,
                    CategoriaD,
                    OutrasRedes
                ) VALUES (
                    @ID_Campanha,
                    @Senha,
                    @URLFoto,
                    @SeloME2,
                    @Email,
                    @Telefone,
                    @Instagram,
                    @TikTok,
                    @YouTube,
                    @Nome,
                    @Idade,
                    @Regiao,
                    @CategoriaA,
                    @CategoriaB,
                    @CategoriaC,
                    @CategoriaD,
                    @OutrasRedes
                );
            ";

            using (var conn = await connectionDB.NewConnection())
            {
                if (conn == null)
                    return false;

                await conn.ExecuteAsync(query, influencer);
                return true;
            }
        }

        public async Task<int> ObterNumeroInfluencers(long ID_Campanha)
        {
            using var connection = await connectionDB.NewConnection();
            if (connection == null) return 0;

            try
            {
                string sql = @"SELECT COUNT(*) 
                      FROM tabela_influencers_data 
                      WHERE ID_Campanha = @ID_Campanha;";

                return await connection.ExecuteScalarAsync<int>(sql, new { ID_Campanha = ID_Campanha });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter número de influencers: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Atualiza os dados de um influenciador com base no Email.
        /// Também atualiza os campos recém-adicionados.
        /// </summary>
        public async Task<bool> UpdateInfluencerAsync(InfluencerModel influencer)
        {
            const string query = @"
                UPDATE tabela_influencers_data
                SET
                    ID_Campanha = @ID_Campanha,
                    Senha = @Senha,
                    URLFoto = @URLFoto,
                    SeloME2 = @SeloME2,
                    Telefone = @Telefone,
                    Instagram = @Instagram,
                    TikTok = @TikTok,
                    YouTube = @YouTube,
                    Nome = @Nome,
                    Idade = @Idade,
                    Regiao = @Regiao,
                    CategoriaA = @CategoriaA,
                    CategoriaB = @CategoriaB,
                    CategoriaC = @CategoriaC,
                    CategoriaD = @CategoriaD,
                    OutrasRedes = @OutrasRedes
                WHERE Email = @Email;
            ";

            using (var conn = await connectionDB.NewConnection())
            {
                if (conn == null)
                    return false;

                await conn.ExecuteAsync(query, influencer);
                return true;
            }
        }

        /// <summary>
        /// Exclui um influenciador com base no Email.
        /// </summary>
        public async Task<bool> DeleteInfluencerAsync(string email)
        {
            const string query = @"
                DELETE FROM tabela_influencers_data 
                WHERE Email = @Email;
            ";

            using (var conn = await connectionDB.NewConnection())
            {
                if (conn == null)
                    return false;

                await conn.ExecuteAsync(query, new { Email = email });
                return true;
            }
        }

        /// <summary>
        /// Retorna um influenciador através do Email.
        /// </summary>
        public async Task<InfluencerModel> GetInfluencerByEmailAsync(string email)
        {
            const string query = @"
                SELECT *
                FROM tabela_influencers_data
                WHERE Email = @Email;
            ";

            using (var conn = await connectionDB.NewConnection())
            {
                if (conn == null)
                    return null;

                return await conn.QueryFirstOrDefaultAsync<InfluencerModel>(query, new { Email = email });
            }
        }

        /// <summary>
        /// Retorna um influenciador pelo seu ID (PK).
        /// </summary>
        public async Task<InfluencerModel> GetInfluencerByIdAsync(long id)
        {
            const string query = @"
                SELECT *
                FROM tabela_influencers_data
                WHERE ID = @ID;
            ";

            using (var conn = await connectionDB.NewConnection())
            {
                if (conn == null)
                    return null;

                return await conn.QueryFirstOrDefaultAsync<InfluencerModel>(query, new { ID = id });
            }
        }

        /// <summary>
        /// Retorna uma lista de influenciadores filtrados pelo ID da Campanha.
        /// </summary>
        public async Task<List<InfluencerModel>> GetInfluencersByCampaignIdAsync(long campaignId)
        {
            const string query = @"
                SELECT *
                FROM tabela_influencers_data
                WHERE ID_Campanha = @CampaignId;
            ";

            using (var conn = await connectionDB.NewConnection())
            {
                if (conn == null)
                    return new List<InfluencerModel>();

                var result = await conn.QueryAsync<InfluencerModel>(query, new { CampaignId = campaignId });
                return result.AsList();
            }
        }

        public async Task<List<InfluencerModel>> GetAllInfluencersAsync()
        {
            const string query = @"SELECT * FROM tabela_influencers_data;";

            using (var conn = await connectionDB.NewConnection())
            {
                if (conn == null)
                    return new List<InfluencerModel>();

                var result = await conn.QueryAsync<InfluencerModel>(query);
                return result.AsList();
            }
        }
    }



    #endregion
}
