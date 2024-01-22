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
            height: 280,
            type: "radialBar",
        },

        series: [67],
        colors: ["#20E647"],
        plotOptions: {
            radialBar: {
                hollow: {
                    margin: 0,
                    size: "70%",
                    background: "#293450"
                },
                track: {
                    dropShadow: {
                        enabled: true,
                        top: 2,
                        left: 0,
                        blur: 4,
                        opacity: 0.15
                    }
                },
                dataLabels: {
                    name: {
                        offsetY: -10,
                        color: "#fff",
                        fontSize: "13px"
                    },
                    value: {
                        color: "#fff",
                        fontSize: "30px",
                        show: true
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
            lineCap: "round"
        },
        labels: ["Progress"]
    };

    var chart = new ApexCharts(document.querySelector('#'+id), options);

    chart.render();

}