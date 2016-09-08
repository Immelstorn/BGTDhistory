var generalChart = function(modelGeneral) {
    $('#general').highcharts({
        chart: {
            zoomType: 'x'
        },
        title: {
            text: 'Общая статистика'
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
                type: 'area',
                name: 'Messages count',
                pointInterval: 24 * 3600 * 1000,
                pointStart: Date.UTC(2012, 0, 29),
                turboThreshold:0,
                data: modelGeneral
            }
        ]
    });
}