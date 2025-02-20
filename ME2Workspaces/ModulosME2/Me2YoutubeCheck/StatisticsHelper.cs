
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ME2Workspaces.ModulosME2.Me2YoutubeCheck
{
    public class StatisticsHelper
    {
        // Calcula a média ponderada
        public static double WeightedAverage(List<double> values, List<double> weights)
        {
            double weightedSum = 0;
            double weightSum = 0;

            for (int i = 0; i < values.Count; i++)
            {
                weightedSum += values[i] * weights[i];
                weightSum += weights[i];
            }

            return weightedSum / weightSum;
        }

        // Calcula o desvio padrão
        public static double StandardDeviation(List<double> values)
        {
            double average = values.Average();
            double sumOfSquaresOfDifferences = values.Select(val => (val - average) * (val - average)).Sum();
            return Math.Sqrt(sumOfSquaresOfDifferences / values.Count);
        }

        // Normaliza os valores para a escala 0 a 1
        public static List<double> Normalize(List<double> values)
        {
            if (values == null || values.Count == 0)
            {
                throw new ArgumentException("The list cannot be null or empty.", nameof(values));
            }

            double min = values.Min();
            double max = values.Max();

            // Check if all values are the same
            if (min == max)
            {
                // Return a list of zeros, since all values are the same
                return values.Select(val => 0.0).ToList();
            }

            return values.Select(val => (val - min) / (max - min)).ToList();
        }


        // Calcula o coeficiente de correlação de Pearson
        public static double PearsonCorrelation(List<double> x, List<double> y)
        {
            double avgX = x.Average();
            double avgY = y.Average();

            double sumXY = 0;
            double sumX2 = 0;
            double sumY2 = 0;

            for (int i = 0; i < x.Count; i++)
            {
                sumXY += (x[i] - avgX) * (y[i] - avgY);
                sumX2 += Math.Pow((x[i] - avgX), 2);
                sumY2 += Math.Pow((y[i] - avgY), 2);
            }

            return sumXY / Math.Sqrt(sumX2 * sumY2);
        }

        // Realiza a análise de componentes principais (PCA) de forma simplificada
        public static List<double> PrincipalComponentAnalysis(List<List<double>> data, int numComponents)
        {
            if (data == null || data.Count == 0 || data.Any(list => list.Count != data[0].Count))
            {
                throw new ArgumentException("Data must be a non-empty list of lists with the same number of elements.");
            }

            int numRows = data[0].Count;
            int numCols = data.Count;

            var array = new double[numRows, numCols];
            for (int col = 0; col < numCols; col++)
            {
                for (int row = 0; row < numRows; row++)
                {
                    array[row, col] = data[col][row];
                }
            }

            // Centraliza os dados subtraindo a média de cada coluna
            var columnMeans = new double[numCols];
            for (int col = 0; col < numCols; col++)
            {
                columnMeans[col] = Enumerable.Range(0, numRows).Average(row => array[row, col]);
            }

            for (int col = 0; col < numCols; col++)
            {
                for (int row = 0; row < numRows; row++)
                {
                    array[row, col] -= columnMeans[col];
                }
            }

            // Calcula a matriz de covariância
            var covarianceMatrix = new double[numCols, numCols];
            for (int i = 0; i < numCols; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    covarianceMatrix[i, j] = Enumerable.Range(0, numRows).Average(row => array[row, i] * array[row, j]);
                }
            }

            // Calcula os autovalores e autovetores (simplificado para 2D)
            var eigenValues = new double[numCols];
            var eigenVectors = new double[numCols, numCols];

            // Assumindo que temos uma implementação básica para autovalores e autovetores
            // Isso deve ser substituído por uma implementação real ou uma biblioteca de álgebra linear

            // Placeholder para a função de decomposição
            // Implementar decomposição própria ou utilizar uma biblioteca externa

            // Ordena os componentes principais pelos valores próprios (autovalores) em ordem decrescente
            var sortedIndices = eigenValues.Select((value, index) => new { value, index })
                                           .OrderByDescending(x => x.value)
                                           .Select(x => x.index)
                                           .ToArray();

            // Seleciona os componentes principais
            var principalComponents = new List<double>();
            for (int i = 0; i < numComponents && i < sortedIndices.Length; i++)
            {
                var componentIndex = sortedIndices[i];
                principalComponents.Add(eigenValues[componentIndex]);
            }

            return principalComponents;
        }
    }
}
