var wordsByUserTimelineChart = function (modelGeneral) {
    for (var i = 0; i < modelGeneral.length; i++) {
        modelGeneral[i].pointStart = Date.UTC(2012, 0, 29);
        modelGeneral[i].pointInterval = 24 * 3600 * 1000;
    }

    $('#wordsByUserTimeline').highcharts({
        chart: {
            zoomType: 'x'
        },
        title: {
            text: 'Статистика по слову с разбивкой по юзерам'
        },
        xAxis: {
            type: 'datetime',
            minRange: 14 * 24 * 3600000 // fourteen days
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Messages count'
            }
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
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0
        },
        series: modelGeneral
    });
}