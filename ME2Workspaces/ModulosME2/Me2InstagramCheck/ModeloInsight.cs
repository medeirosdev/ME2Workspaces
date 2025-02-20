using ME2Workspaces.ModulosME2.TarefasInfluencer;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Me2Workspaces.ModulosME2.Me2InstagramCheck
{
    public class Insight
    {
        public long Id { get; set; }  // Campo ID único
        public long ID_Influencer { get; set; }  // ID do Influenciador
        public string UsernameInfluencer { get; set; }  // Nome de usuário do Influenciador
        public long ID_Campanha { get; set; }  // ID da Campanha
        public long ID_Usuario { get; set; }  // ID do Usuário QUE CRIO, vem ddo dadossessaoUsuario
        public string Username { get; set; }  // Nome de usuário
        public DateTime DataInclusao { get; set; }  // Data de inclusão

        public string Mes => DataInclusao.ToString("MMMM yyyy");
        public TipoTarefa TipoInsight { get; set; }  // Tipo de Insight (inteiro)
        public int JsonData { get; set; }  // Dados extras (alterado para inteiro)
        public int ListDates { get; set; }  // Lista de datas (alterado para inteiro)

        // Interações
        public int Curtidas { get; set; }  // Número de curtidas
        public int Respostas { get; set; }  // Número de respostas
        public int Compartilhamentos { get; set; }  // Número de compartilhamentos

        // Novos campos adicionados
        public long Visualizacoes { get; set; }  // Número de visualizações
        public long Interacoes { get; set; }  // Número de interações

        // Navegação
        public int Avanco { get; set; }  // Número de avanços
        public int ProximoStory { get; set; }  // Número de próximos stories
        public int Voltar { get; set; }  // Número de retornos
        public int Saiu { get; set; }  // Número de saídas do story

        // Atividade do perfil
        public int VisitasAoPerfil { get; set; }  // Número de visitas ao perfil
        public int ComecaramASeguir { get; set; }  // Número de novos seguidores

        // Alcance e engajamento
        public int ContasAlcancadas { get; set; }  // Número de contas alcançadas
        public int ContasComEngajamento { get; set; }  // Número de contas com engajamento
        public TipoRedeSocial AtividadeDoPerfil { get; set; }  // Número de atividades do perfil

        // Toques na Figurinha
        public int AlcanceSeguidores { get; set; }  // Alcance de seguidores

        //Cliques no Link
        public int AlcanceNaoSeguidores { get; set; }  // Alcance de não seguidores

        
    }


    public enum TipoRedeSocial
    {
        Instagram,
        TikTok,
        Youtube,
        Facebook,
        Linkedin
    }

}


//CREATE TABLE insight (
//    id BIGINT AUTO_INCREMENT PRIMARY KEY,   -- Campo ID único
//    ID_Influencer BIGINT,                   -- ID do Influenciador
//    UsernameInfluencer VARCHAR(255),        -- Nome de usuário do Influenciador
//    ID_Campanha BIGINT,                     -- ID da Campanha
//    ID_Usuario BIGINT,                      -- ID do Usuário
//    username VARCHAR(255),                  -- Nome de usuário
//    DataInclusao DATETIME,                  -- Data de inclusão
//    TipoInsight INT,                        -- Tipo de Insight (inteiro)
//    JsonData INT,                           -- Dados extras (inteiro)
//    ListDates INT,                          -- Lista de datas (inteiro)
    
//    -- Interações
//    Curtidas INT,                           -- Número de curtidas
//    Respostas INT,                          -- Número de respostas
//    Compartilhamentos INT,                  -- Número de compartilhamentos

//    -- Novos campos adicionados
//    Visualizacoes BIGINT,                   -- Número de visualizações
//    Interacoes BIGINT,                      -- Número de interações
    
//    -- Navegação
//    Avanco INT,                             -- Número de avanços
//    ProximoStory INT,                       -- Número de próximos stories
//    Voltar INT,                             -- Número de retornos
//    Saiu INT,                               -- Número de saídas do story
    
//    -- Atividade do perfil
//    VisitasAoPerfil INT,                    -- Número de visitas ao perfil
//    ComecaramASeguir INT,                   -- Número de novos seguidores
    
//    -- Alcance e engajamento
//    ContasAlcancadas INT,                   -- Número de contas alcançadas
//    ContasComEngajamento INT,               -- Número de contas com engajamento
//    AtividadeDoPerfil INT,                  -- Número de atividades do perfil
    
//    -- Alcance de seguidores e não seguidores
//    AlcanceSeguidores INT,                  -- Alcance de seguidores
//    AlcanceNaoSeguidores INT                -- Alcance de não seguidores
//);

