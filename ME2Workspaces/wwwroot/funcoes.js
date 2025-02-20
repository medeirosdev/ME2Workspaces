function downloadFile(fileName, fileUrl) {
    var link = document.createElement('a');
    link.href = fileUrl;
    link.download = fileName;
    link.click();
}

window.exportFunctions = {
    async exportToPdf(data) {
        const { jsPDF } = window.jspdf;
        const doc = new jsPDF();

        // Title
        doc.setFontSize(20);
        doc.text("Relatório de Campanha", 20, 20);

        // Date
        doc.setFontSize(12);
        doc.text(`Data: ${new Date().toLocaleDateString()}`, 20, 30);

        // KPIs
        doc.setFontSize(16);
        doc.text("Métricas Principais", 20, 45);
        doc.setFontSize(12);
        doc.text(`Total de Visualizações: ${data.totalVisualizacoes.toLocaleString()}`, 25, 55);
        doc.text(`Média de Engajamento: ${(data.mediaEngajamento * 100).toFixed(2)}%`, 25, 65);
        doc.text(`Total de Interações: ${data.totalInteracoes.toLocaleString()}`, 25, 75);
        doc.text(`Cliques em Links: ${data.totalCliquesLinks.toLocaleString()}`, 25, 85);

        // Campaign Details
        if (data.campanha) {
            doc.setFontSize(16);
            doc.text("Detalhes da Campanha", 20, 105);
            doc.setFontSize(12);
            doc.text(`Nome: ${data.campanha.nomeCampanha}`, 25, 115);
            doc.text(`Início: ${new Date(data.campanha.dataInicio).toLocaleDateString()}`, 25, 125);
            doc.text(`Término: ${new Date(data.campanha.dataTermino).toLocaleDateString()}`, 25, 135);
        }

        // Insights Table
        if (data.insights && data.insights.length > 0) {
            doc.addPage();
            doc.setFontSize(16);
            doc.text("Insights", 20, 20);

            const headers = ["Data", "Rede", "Visualizações", "Engajamento"];
            let yPos = 35;

            // Headers
            doc.setFontSize(12);
            doc.setFont(undefined, 'bold');
            headers.forEach((header, index) => {
                doc.text(header, 20 + (index * 45), yPos);
            });

            // Data
            doc.setFont(undefined, 'normal');
            data.insights.forEach((insight, index) => {
                yPos = 45 + (index * 10);
                if (yPos > 280) { // New page if near bottom
                    doc.addPage();
                    yPos = 20;
                }
                doc.text(new Date(insight.dataInclusao).toLocaleDateString(), 20, yPos);
                doc.text(insight.atividadeDoPerfil, 65, yPos);
                doc.text(insight.visualizacoes.toLocaleString(), 110, yPos);
                doc.text(`${((insight.interacoes / insight.visualizacoes) * 100).toFixed(2)}%`, 155, yPos);
            });
        }

        // Save the PDF
        doc.save('relatorio-campanha.pdf');
    },

    async exportToExcel(data) {
        const XLSX = window.XLSX;

        // Prepare data for Excel
        const workbook = XLSX.utils.book_new();

        // Overview sheet
        const overviewData = [
            ["Relatório de Campanha"],
            ["Data:", new Date().toLocaleDateString()],
            [],
            ["Métricas Principais"],
            ["Total de Visualizações", data.totalVisualizacoes],
            ["Média de Engajamento", `${(data.mediaEngajamento * 100).toFixed(2)}%`],
            ["Total de Interações", data.totalInteracoes],
            ["Cliques em Links", data.totalCliquesLinks]
        ];

        if (data.campanha) {
            overviewData.push(
                [],
                ["Detalhes da Campanha"],
                ["Nome", data.campanha.nomeCampanha],
                ["Data Início", new Date(data.campanha.dataInicio).toLocaleDateString()],
                ["Data Término", new Date(data.campanha.dataTermino).toLocaleDateString()]
            );
        }

        const overviewSheet = XLSX.utils.aoa_to_sheet(overviewData);
        XLSX.utils.book_append_sheet(workbook, overviewSheet, "Visão Geral");

        // Insights sheet
        if (data.insights && data.insights.length > 0) {
            const insightsData = [
                ["Data", "Rede Social", "Visualizações", "Interações", "Engajamento", "Cliques", "Compartilhamentos"]
            ];

            data.insights.forEach(insight => {
                insightsData.push([
                    new Date(insight.dataInclusao).toLocaleDateString(),
                    insight.atividadeDoPerfil,
                    insight.visualizacoes,
                    insight.interacoes,
                    `${((insight.interacoes / insight.visualizacoes) * 100).toFixed(2)}%`,
                    insight.alcanceNaoSeguidores,
                    insight.compartilhamentos
                ]);
            });

            const insightsSheet = XLSX.utils.aoa_to_sheet(insightsData);
            XLSX.utils.book_append_sheet(workbook, insightsSheet, "Insights");
        }

        // Save the Excel file
        XLSX.writeFile(workbook, 'relatorio-campanha.xlsx');
    }
};