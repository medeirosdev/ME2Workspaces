using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Me2Workspaces.ModulosME2.Database;
using Me2Workspaces.ModulosME2.Me2InstagramCheck;

public class InsightService
{
    private readonly ConnectionDB _connectionDB;

    public InsightService()
    {
        _connectionDB = new ConnectionDB();
    }

    public async Task CreateInsight(Insight insight)
    {
        using (var connection = await _connectionDB.NewConnection())
        {
            var query = @"
                INSERT INTO insight (
                    ID_Influencer,
                    UsernameInfluencer,
                    ID_Campanha,
                    ID_Usuario,
                    Username,
                    DataInclusao,
                    TipoInsight,
                    JsonData,
                    ListDates,
                    Curtidas,
                    Respostas,
                    Compartilhamentos,
                    Visualizacoes,
                    Interacoes,
                    Avanco,
                    ProximoStory,
                    Voltar,
                    Saiu,
                    VisitasAoPerfil,
                    ComecaramASeguir,
                    ContasAlcancadas,
                    ContasComEngajamento,
                    AtividadeDoPerfil,
                    AlcanceSeguidores,
                    AlcanceNaoSeguidores
                ) VALUES (
                    @ID_Influencer,
                    @UsernameInfluencer,
                    @ID_Campanha,
                    @ID_Usuario,
                    @Username,
                    @DataInclusao,
                    @TipoInsight,
                    @JsonData,
                    @ListDates,
                    @Curtidas,
                    @Respostas,
                    @Compartilhamentos,
                    @Visualizacoes,
                    @Interacoes,
                    @Avanco,
                    @ProximoStory,
                    @Voltar,
                    @Saiu,
                    @VisitasAoPerfil,
                    @ComecaramASeguir,
                    @ContasAlcancadas,
                    @ContasComEngajamento,
                    @AtividadeDoPerfil,
                    @AlcanceSeguidores,
                    @AlcanceNaoSeguidores
                );
            ";

            await connection.ExecuteAsync(query, insight);
        }
    }

    public async Task<List<Insight>> GetInsightsByInfluencerIdAndUsername(long influencerId, string username)
    {
        using (var connection = await _connectionDB.NewConnection())
        {
            var query = "SELECT * FROM insight WHERE ID_Influencer = @InfluencerId AND UsernameInfluencer = @UsernameInfluencer;";
            var insights = await connection.QueryAsync<Insight>(query, new { InfluencerId = influencerId, UsernameInfluencer = username });
            return insights.AsList();
        }
    }

    public async Task<List<Insight>> GetInsightsByParams(long influencerId, long ID_Campanha)
    {
        using (var connection = await _connectionDB.NewConnection())
        {
            var query = "SELECT * FROM insight WHERE ID_Influencer = @InfluencerId AND ID_Campanha = @ID_Campanha;";
            var insights = await connection.QueryAsync<Insight>(query, new { InfluencerId = influencerId, ID_Campanha = ID_Campanha });
            return insights.AsList();
        }
    }

    public async Task<Insight> GetInsightById(long id)
    {
        using (var connection = await _connectionDB.NewConnection())
        {
            var query = "SELECT * FROM insight WHERE id = @Id;";
            return await connection.QueryFirstOrDefaultAsync<Insight>(query, new { Id = id });
        }
    }

    public async Task UpdateInsight(Insight insight)
    {
        using (var connection = await _connectionDB.NewConnection())
        {
            var query = @"
                UPDATE insight SET
                    ID_Influencer = @ID_Influencer,
                    UsernameInfluencer = @UsernameInfluencer,
                    ID_Campanha = @ID_Campanha,
                    ID_Usuario = @ID_Usuario,
                    Username = @Username,
                    DataInclusao = @DataInclusao,
                    TipoInsight = @TipoInsight,
                    JsonData = @JsonData,
                    ListDates = @ListDates,
                    Curtidas = @Curtidas,
                    Respostas = @Respostas,
                    Compartilhamentos = @Compartilhamentos,
                    Visualizacoes = @Visualizacoes,
                    Interacoes = @Interacoes,
                    Avanco = @Avanco,
                    ProximoStory = @ProximoStory,
                    Voltar = @Voltar,
                    Saiu = @Saiu,
                    VisitasAoPerfil = @VisitasAoPerfil,
                    ComecaramASeguir = @ComecaramASeguir,
                    ContasAlcancadas = @ContasAlcancadas,
                    ContasComEngajamento = @ContasComEngajamento,
                    AtividadeDoPerfil = @AtividadeDoPerfil,
                    AlcanceSeguidores = @AlcanceSeguidores,
                    AlcanceNaoSeguidores = @AlcanceNaoSeguidores
                WHERE id = @Id;
            ";

            await connection.ExecuteAsync(query, insight);
        }
    }

    public async Task DeleteInsight(long id)
    {
        using (var connection = await _connectionDB.NewConnection())
        {
            var query = "DELETE FROM insight WHERE id = @Id;";
            await connection.ExecuteAsync(query, new { Id = id });
        }
    }

    public async Task<List<Insight>> GetInsightsByUserId(long userId)
    {
        using (var connection = await _connectionDB.NewConnection())
        {
            var query = "SELECT * FROM insight WHERE ID_Usuario = @UserId;";
            var insights = await connection.QueryAsync<Insight>(query, new { UserId = userId });
            return insights.AsList();
        }
    }

    public async Task<List<Insight>> GetInsights()
    {
        using (var connection = await _connectionDB.NewConnection())
        {
            var query = "SELECT * FROM insight";
            var insights = await connection.QueryAsync<Insight>(query);
            return insights.AsList();
        }
    }

    public async Task<List<Insight>> GetInsightsByCampaignId(long campaignId)
    {
        using (var connection = await _connectionDB.NewConnection())
        {
            var query = "SELECT * FROM insight WHERE ID_Campanha = @CampaignId;";
            var insights = await connection.QueryAsync<Insight>(query, new { CampaignId = campaignId });
            return insights.AsList();
        }
    }
}
