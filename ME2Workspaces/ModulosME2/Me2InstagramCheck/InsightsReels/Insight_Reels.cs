using Microsoft.AspNetCore.Http.HttpResults;
using Dapper;

using Me2Workspaces.ModulosME2.Database;
using Newtonsoft.Json;

namespace Me2Workspaces.ModulosME2.Me2InstagramCheck.InsightsReels
{
    public class Insight_Reels
    {
        public long Id { get; set; }
        public long Id_Campanha { get; set; }
        public long Id_Usuario { get; set; }
        public long Extra { get; set; }
        public string Username { get; set; }              // Nome de usuário
        public UserData JsonData { get; set; }              // JSON grande para armazenar informações extras
       
    }
}


//CREATE TABLE influencer_campanha (
//    id BIGINT AUTO_INCREMENT PRIMARY KEY,
//    id_campanha BIGINT NOT NULL,
//    id_usuario BIGINT NOT NULL,
//    extra BIGINT NOT NULL,
//    username VARCHAR(255) NOT NULL,
//    jsonData LONGTEXT NOT NULL,
//    CONSTRAINT fk_campanha FOREIGN KEY (id_campanha) REFERENCES campanhas(id),
//    CONSTRAINT fk_usuario FOREIGN KEY (id_usuario) REFERENCES usuarios(id)
//);

