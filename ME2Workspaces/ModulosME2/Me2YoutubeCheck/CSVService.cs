using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Me2Workspaces.ModulosME2.Me2InstagramCheck;

namespace ME2Workspaces.ModulosME2.Me2YoutubeCheck
{
    public class CSVService
    {
        public static List<string> LerIdsDoArquivoCsv(string filePath)
        {
            var ids = new List<string>();

            if (File.Exists(filePath))
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    MissingFieldFound = null
                }))
                {
                    csv.Read();
                    csv.ReadHeader();

                    while (csv.Read())
                    {
                        var id = csv.GetField<string>("ID");
                        ids.Add(id);
                    }
                }
            }
            else
            {
                Console.WriteLine("Arquivo não encontrado: " + filePath);
            }

            return ids;
        }

        public static void WriteModelsToCsv(List<ModeloResponseInfosYTAPI> modelos, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(modelos);
            }
        }
        public static string ToCsv(ModeloResponseInfosYTAPI modelo)
        {
            

            var csvBuilder = new System.Text.StringBuilder();
            csvBuilder.AppendLine("ChannelId,ChannelName,VideoCount,LikecountTotal,DislikecountTotal,CommentcountTotal,ViewcountTotal,SubscriberCount,Score");
            csvBuilder.AppendLine($"{modelo.ChannelId},{modelo.ChannelName},{modelo.VideoCount},{modelo.LikecountTotal},{modelo.DislikecountTotal},{modelo.CommentcountTotal},{modelo.ViewcountTotal},{modelo.SubscriberCount},{modelo.Score}");
            return csvBuilder.ToString();
        }

        public static string ToCsv(UserData modelo)
        {
            // Calcula os somatórios das listas
            var totalLikes = modelo.list_like_count.Sum();
            var totalComments = modelo.list_comments_count.Sum();

            var csvBuilder = new System.Text.StringBuilder();
            csvBuilder.AppendLine("id,username,followers_count,follows_count,media_count,profile_picture_url,website,total_likes,total_comments,total_captions,total_timestamps");

            csvBuilder.AppendLine($"{modelo.id},{modelo.username},{modelo.followers_count},{modelo.follows_count},{modelo.media_count},{modelo.profile_picture_url},{modelo.website},{totalLikes},{totalComments},{modelo.list_captions_count.Count},{modelo.list_timestamp_count.Count}");
            return csvBuilder.ToString();
        }

        public static string ListToCsv(List<ModeloResponseInfosYTAPI> lista)
        {


            var csvBuilder = new System.Text.StringBuilder();
            csvBuilder.AppendLine("ChannelId,ChannelName,VideoCount,LikecountTotal,DislikecountTotal,CommentcountTotal,ViewcountTotal,SubscriberCount,Score");

            foreach (var modelo in lista)
            {
                csvBuilder.AppendLine($"{modelo.ChannelId},{modelo.ChannelName},{modelo.VideoCount},{modelo.LikecountTotal},{modelo.DislikecountTotal},{modelo.CommentcountTotal},{modelo.ViewcountTotal},{modelo.SubscriberCount},{modelo.Score}");
            }
            return csvBuilder.ToString();
        }

        public static string ListToCsv(List<UserData> lista)
        {
            var csvBuilder = new System.Text.StringBuilder();
            csvBuilder.AppendLine("id,username,followers_count,follows_count,media_count,profile_picture_url,website,total_likes,total_comments,total_captions,total_timestamps");

            foreach (var modelo in lista)
            {
                // Calcula os somatórios das listas
                var totalLikes = modelo.list_like_count.Sum();
                var totalComments = modelo.list_comments_count.Sum();
                var totalCaptions = modelo.list_captions_count.Count;
                var totalTimestamps = modelo.list_timestamp_count.Count;

                csvBuilder.AppendLine($"{modelo.id},{modelo.username},{modelo.followers_count},{modelo.follows_count},{modelo.media_count},{modelo.profile_picture_url},{modelo.website},{totalLikes},{totalComments},{totalCaptions},{totalTimestamps}");
            }

            return csvBuilder.ToString();
        }

        public static string ToJSON(ModeloResponseInfosYTAPI modelo)
        {
            // Converte a instância do modelo para uma string JSON
            string jsonString = JsonConvert.SerializeObject(modelo, Formatting.Indented);

            var csvBuilder = new System.Text.StringBuilder();
            csvBuilder.AppendLine(jsonString);
            return csvBuilder.ToString();
        }

        public static string ToJSON(UserData modelo)
        {
            // Converte a instância do modelo UserData para uma string JSON
            string jsonString = JsonConvert.SerializeObject(modelo, Formatting.Indented);

            var jsonBuilder = new System.Text.StringBuilder();
            jsonBuilder.AppendLine(jsonString);
            return jsonBuilder.ToString();
        }

        public static string ListToJSON(List<UserData> lista)
        {
            // Converte a lista de UserData para uma string JSON
            string jsonString = JsonConvert.SerializeObject(lista, Formatting.Indented);

            var jsonBuilder = new System.Text.StringBuilder();
            jsonBuilder.AppendLine(jsonString);
            return jsonBuilder.ToString();
        }

        public static string ListToJSON(List<ModeloResponseInfosYTAPI> lista)
        {
            // Converte a instância do modelo para uma string JSON
            string jsonString = JsonConvert.SerializeObject(lista, Formatting.Indented);

            var csvBuilder = new System.Text.StringBuilder();
            csvBuilder.AppendLine(jsonString);
            return csvBuilder.ToString();
        }

    }
}
