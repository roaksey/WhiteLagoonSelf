$(document).ready(function () {
    loadTotalBookingRadialChart();
});

function loadTotalBookingRadialChart() {
    $(".chart-spinner").show();

    $.ajax({
        url: "/Dashboard/GetTotalBookingRadialChartData",
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            document.querySelector("#spanTotalBookingCount").innerHTML = data.totalCount;

            var sectionCurrentCount = document.createElement("span");
            if (data.hasIncreased) {
                sectionCurrentCount.className = "text-success me-1";
                sectionCurrentCount.innerHTML = '<i class="bi bi-arrow-down-right-circle me-1"></i> <span> ' + data.countInCurrentMonth + '</span>';
            } else {
                sectionCurrentCount.className = "text-danger me-1";
                sectionCurrentCount.innerHTML = '<i class="bi bi-arrow-down-right-circle me-1"></i> <span> ' + data.countInCurrentMonth + '</span>';
            }

            document.querySelector('#sectionBookingCount').append(sectionCurrentCount);
            document.querySelector('#sectionBookingCount').append('since last month');
            loadRadialBarChart("totalBookingRadialChart", data);
            $('.chart-spinner').hide();
        }
    })
}

function loadRadialBarChart(id, data) {
    var options = {
        chart: {
            height: 90,
            width:90,
            type: "radialBar",
            sparkline: {
                enabled:true
            },
            offsetY:-10
        },

        series: data.series,
        colors: ["#20E647"],
        plotOptions: {
            radialBar: {
                dataLabels: {
                    value: {
                        offsetY: -10,
                    }
                }
            }
        },
        fill: {
            type: "gradient",
            gradient: {
                shade: "dark",
                type: "vertical",
                gradientToColors: ["#87D4F9"],
                stops: [0, 100]
            }
        },
        stroke: {
            
        },
        labels: [""]
    };

    var chart = new ApexCharts(document.querySelector('#'+id), options);

    chart.render();

}