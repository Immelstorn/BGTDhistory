var usersBySymbolsChart = function (data) {
    $('#usersBySymbols').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: 'Статистика по юзерам не по количеству сообщений а по количеству нахуяченных сюда символов(с)'
        },
        xAxis: {
            crosshair: true,
            categories: data.Categories
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Messages count'
            }
        },
        legend: {
            enabled: false
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            }
        },

        series: [
            {
                name: 'Messages count',
                data: data.Series
            }
        ]
    });
}