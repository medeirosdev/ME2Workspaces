using ME2Workspaces.ModulosME2.Me2YoutubeCheck;

namespace ME2Workspaces.ModulosME2.Me2YoutubeCheck
{
    public class ME2YTScore
    {
        public static async Task<double> GetScoreByYTResponse(ModeloResponseInfosYTAPI dataAPI)
        {
            // Conversão para double para operações estatísticas
            List<double> likeCounts = dataAPI.LikeCounts.Where(x => x != null && !double.IsNaN((double)x)).Select(x => (double)x).ToList();
            List<double> commentCounts = dataAPI.CommentCounts.Where(x => x != null && !double.IsNaN((double)x)).Select(x => (double)x).ToList();
            List<double> viewCounts = dataAPI.ViewCounts.Where(x => x != null && !double.IsNaN((double)x)).Select(x => (double)x).ToList();
            List<double> durations = dataAPI.Durations.Where(x => x != null && !double.IsNaN((double)x)).Select(x => (double)x).ToList();
            double videoCount = (double)dataAPI.VideoCount;
            double subscriberCount = (double)(dataAPI.SubscriberCount ?? 0);

            // Normalização dos dados
            likeCounts = StatisticsHelper.Normalize(likeCounts);
            commentCounts = StatisticsHelper.Normalize(commentCounts);
            viewCounts = StatisticsHelper.Normalize(viewCounts);
            durations = StatisticsHelper.Normalize(durations);
            double normalizedVideoCount = (double)videoCount / 100000;
            double normalizedSubscriberCount = (double)subscriberCount / 100000;

            // Calculando a média ponderada
            List<double> weights = new List<double> { 0.3, 0.2, 0.2, 0.1, 0.1, 0.1 };
            double engagementWeightedAvg = StatisticsHelper.WeightedAverage(new List<double> {
            likeCounts.Average(), commentCounts.Average(),
            viewCounts.Average(), durations.Average(),
            normalizedVideoCount, normalizedSubscriberCount
        }, weights);

            // Calculando o desvio padrão
            double engagementStdDev = StatisticsHelper.StandardDeviation(new List<double> {
            likeCounts.Average(), commentCounts.Average(),
            viewCounts.Average(), durations.Average(),
            normalizedVideoCount, normalizedSubscriberCount
        });

            // Calculando as correlações
            double likeCommentCorrelation = StatisticsHelper.PearsonCorrelation(likeCounts, commentCounts);
            double likeViewCorrelation = StatisticsHelper.PearsonCorrelation(likeCounts, viewCounts);
            double commentViewCorrelation = StatisticsHelper.PearsonCorrelation(commentCounts, viewCounts);

            // Realizando PCA
            List<List<double>> dataMatrix = new List<List<double>>
        {
            likeCounts,
            commentCounts,
            viewCounts,
            durations,
            Enumerable.Repeat(normalizedVideoCount, likeCounts.Count).ToList(),
            Enumerable.Repeat(normalizedSubscriberCount, likeCounts.Count).ToList()
        };

            List<double> pcaComponents = StatisticsHelper.PrincipalComponentAnalysis(dataMatrix, 3);

            // Fórmula final combinando todos os aspectos
            double finalScore = (engagementWeightedAvg * 0.5) +
                                (engagementStdDev * 0.2) +
                                (likeCommentCorrelation * 0.1) +
                                (likeViewCorrelation * 0.1) +
                                (commentViewCorrelation * 0.1);

            // Incorporando os componentes principais se disponíveis
            if (pcaComponents.Count > 0)
            {
                finalScore += pcaComponents.Average() * 0.1;
            }

            return finalScore;
        }





        public static async Task<double> GetEngajamentoYTResponse(ModeloResponseInfosYTAPI dataAPI)
        {
            if (dataAPI.ViewcountTotal == 0 || dataAPI.ViewcountTotal == null)
                return 0;

            ulong totalEngagement = (dataAPI.LikecountTotal ?? 0) + (dataAPI.CommentcountTotal ?? 0);
            double engagementRate = (double)totalEngagement / (double)dataAPI.ViewcountTotal;

            return await Task.FromResult(engagementRate);
        }

        public static async Task<double> GetAlcanceYTResponse(ModeloResponseInfosYTAPI dataAPI)
        {
            if (dataAPI.VideoCount == 0)
                return 0;

            double reach = (double)(dataAPI.ViewcountTotal ?? 0) / dataAPI.VideoCount;

            return await Task.FromResult(reach);
        }

        public static async Task<double> GetAverageViewsPerVideo(ModeloResponseInfosYTAPI dataAPI)
        {
            if (dataAPI.VideoCount == 0)
                return 0;

            double averageViews = (double)(dataAPI.ViewcountTotal ?? 0) / dataAPI.VideoCount;

            return await Task.FromResult(averageViews);
        }

        public static async Task<double> GetLikeCommentRatio(ModeloResponseInfosYTAPI dataAPI)
        {
            if (dataAPI.LikecountTotal == 0 || dataAPI.CommentcountTotal == 0)
                return 0;

            double likeCommentRatio = (double)(dataAPI.LikecountTotal ?? 0) / (dataAPI.CommentcountTotal ?? 0);

            return await Task.FromResult(likeCommentRatio);
        }

        // Função para analisar a lista com base em múltiplas métricas
        public List<ModeloResponseInfosYTAPI> AnalisarPorMetricas(List<ModeloResponseInfosYTAPI> modelos)
        {
            // Verifica se a lista está vazia para evitar erros
            if (modelos == null || !modelos.Any())
                return new List<ModeloResponseInfosYTAPI>();

            // Calcula as métricas médias usando double
            double viewCountMedia = modelos.Average(m => (double)(m.ViewcountTotal ?? 0));
            double likeCountMedia = modelos.Average(m => (double)(m.LikecountTotal ?? 0));
            double commentCountMedia = modelos.Average(m => (double)(m.CommentcountTotal ?? 0));

            // Cria uma lista de modelos com uma pontuação calculada
            var modelosComPontuacao = modelos.Select(modelo => new
            {
                Modelo = modelo,
                Pontuacao = CalcularPontuacao(modelo, viewCountMedia, likeCountMedia, commentCountMedia)
            }).ToList();

            // Ordena os modelos pela pontuação em ordem decrescente
            var modelosOrdenados = modelosComPontuacao
                .OrderByDescending(mp => mp.Pontuacao)
                .Select(mp => mp.Modelo)
                .ToList();

            return modelosOrdenados;
        }

        // Função para calcular a pontuação combinada
        private double CalcularPontuacao(ModeloResponseInfosYTAPI modelo, double viewCountMedia, double likeCountMedia, double commentCountMedia)
        {
            // Define os pesos para cada métrica
            double pesoScore = 0.5;
            double pesoViewCount = 0.2;
            double pesoLikeCount = 0.2;
            double pesoCommentCount = 0.1;

            // Calcula a pontuação baseada nas métricas
            double pontuacao = (modelo.Score * pesoScore) +
                                ((modelo.ViewcountTotal ?? 0) / viewCountMedia * pesoViewCount) +
                                ((modelo.LikecountTotal ?? 0) / likeCountMedia * pesoLikeCount) +
                                ((modelo.CommentcountTotal ?? 0) / commentCountMedia * pesoCommentCount);

            return pontuacao;
        }

    }
}
