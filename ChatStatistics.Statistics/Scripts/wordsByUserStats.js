var wordsByUserChart = function (data) {
    $('#wordsByUser').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: 'Использование одного слова, разбивка по юзерам'
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
            area: {
                fillColor: {
                    linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                    stops: [
                        [0, Highcharts.getOptions().colors[0]],
                        [1, Highcharts.Color(Highcharts.getOptions().colors[0]).setOpacity(0).get('rgba')]
                    ]
                },
                marker: {
                    radius: 2
                },
                lineWidth: 1,
                states: {
                    hover: {
                        lineWidth: 1
                    }
                },
                threshold: null
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