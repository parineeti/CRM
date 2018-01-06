// ============================================================== 
// Bar chart option
// ============================================================== 
var url3 = "/json/GetmonthByChart";
var dataPoints6 = [];
var dataPoints7 = [];
var dataPoints8 = [];
var dataPoints9 = [];
$.ajax({
    url: url3,
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    type: "POST",
    success: function (ChartDatas) {
        for (var i = 0; i <= ChartDatas.pending.length - 1; i++) {
            dataPoints6.push(ChartDatas.pending[i].value);
            dataPoints7.push(ChartDatas.Completed[i].value);
            dataPoints8.push(ChartDatas.Problem[i].value);
            dataPoints9.push(ChartDatas.New[i].value);
        }
        var myChart = echarts.init(document.getElementById('bar-chart'));

        // specify chart configuration item and data
        option = {
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data: ['Pending', 'Complete','Problem','New']
            },
            toolbox: {
                show: true,
                feature: {

                    magicType: { show: true, type: ['line', 'bar'] },
                    restore: { show: true },
                    saveAsImage: { show: true }
                }
            }, 
            calculable: true,
            xAxis: [
                {
                    type: 'category',
                    data: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'July', 'Aug', 'Sept', 'Oct', 'Nov', 'Dec']
                }
            ],
            yAxis: [
                {
                    type: 'value'
                }
            ],
            series: [
                {
                    name: 'Pending',
                    type: 'bar',
                    data: dataPoints6,
                    markPoint: {
                        data: [
                            { type: 'max', name: 'Max' },
                            { type: 'min', name: 'Min' }
                        ]
                    }, 
                },
                {
                    name: 'Complete',
                    type: 'bar',
                    data: dataPoints7,
                    markPoint: {
                        data: [
                            { type: 'max', name: 'Max' },
                            { type: 'min', name: 'Min' }
                        ]
                    },
                    
                },
                {
                    name: 'Problem',
                    type: 'bar',
                    data: dataPoints8,
                    markPoint: {
                        data: [
                            { type: 'max', name: 'Max' },
                            { type: 'min', name: 'Min' }
                        ]
                    },
                    
                },
                {
                    name: 'New',
                    type: 'bar',
                    data: dataPoints9,
                    markPoint: {
                        data: [
                            { type: 'max', name: 'Max' },
                            { type: 'min', name: 'Min' }
                        ]
                    },
                    
                }
            ]
        };


        // use configuration item and data specified to show chart
        myChart.setOption(option, true), $(function () {
            function resize() {
                setTimeout(function () {
                    myChart.resize()
                }, 100)
            }
            $(window).on("resize", resize), $(".sidebartoggler").on("click", resize)
        });
    }
});
// ============================================================== 
// Line chart
// ============================================================== 
var url2 = "/json/GetweekByChart";
var dataPoints5 = [];
var dataPoints2 = [];
var dataPoints3 = [];
var dataPoints4 = [];
$.ajax({
    url: url2,
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    type: "POST",
    success: function (ChartDatas) {
        for (var i = 0; i <= ChartDatas.pending.length - 1; i++) {
            dataPoints5.push(ChartDatas.pending[i].value);
            dataPoints2.push(ChartDatas.Completed[i].value);
            dataPoints3.push(ChartDatas.Problem[i].value);
            dataPoints4.push(ChartDatas.New[i].value);
        } 
        var dom = document.getElementById("main");
        var mytempChart = echarts.init(dom);
        var app = {};
        option = null;
        option = {

            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data: ['Pending', 'Complete','Problem','New']
            },
            toolbox: {
                show: true,
                feature: {
                    magicType: { show: true, type: ['line', 'bar'] },
                    restore: { show: true },
                    saveAsImage: { show: true }
                }
            }, 
            calculable: true,
            xAxis: [
                {
                    type: 'category',
                    boundaryGap: false,
                    data: ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday']
                }
            ],
            yAxis: [
                {
                    type: 'value',
                    axisLabel: {
                        formatter: '{value}'
                    }
                }
            ],

            series: [
                {
                    name: 'Pending',
                    type: 'line',
                    color: ['#000'],
                    data: dataPoints5,
                    markPoint: {
                        data: [
                            { type: 'max', name: 'Max' },
                            { type: 'min', name: 'Min' }
                        ]
                    },
                    itemStyle: {
                        normal: {
                            lineStyle: {
                                shadowColor: 'rgba(0,0,0,0.3)',
                                shadowBlur: 10,
                                shadowOffsetX: 8,
                                shadowOffsetY: 8
                            }
                        }
                    },
                     
                },
                {
                    name: 'Complete',
                    type: 'line',
                    data: dataPoints2,
                    markPoint: {
                        data: [
                            { type: 'max', name: 'Max' },
                            { type: 'min', name: 'Min' }
                        ]
                    },
                    itemStyle: {
                        normal: {
                            lineStyle: {
                                shadowColor: 'rgba(0,0,0,0.3)',
                                shadowBlur: 10,
                                shadowOffsetX: 8,
                                shadowOffsetY: 8
                            }
                        }
                    },
                     
                },
                {
                    name: 'Problem',
                    type: 'line',
                    data: dataPoints3,
                    markPoint: {
                        data: [
                            { type: 'max', name: 'Max' },
                            { type: 'min', name: 'Min' }
                        ]
                    },
                    itemStyle: {
                        normal: {
                            lineStyle: {
                                shadowColor: 'rgba(0,0,0,0.3)',
                                shadowBlur: 10,
                                shadowOffsetX: 8,
                                shadowOffsetY: 8
                            }
                        }
                    },
                     
                },
                {
                    name: 'New',
                    type: 'line',
                    data: dataPoints4,
                    markPoint: {
                        data: [
                            { type: 'max', name: 'Max' },
                            { type: 'min', name: 'Min' }
                        ]
                    },
                    itemStyle: {
                        normal: {
                            lineStyle: {
                                shadowColor: 'rgba(0,0,0,0.3)',
                                shadowBlur: 10,
                                shadowOffsetX: 8,
                                shadowOffsetY: 8
                            }
                        }
                    },
                     
                }
            ]
        };

        if (option && typeof option === "object") {
            mytempChart.setOption(option, true), $(function () {
                function resize() {
                    setTimeout(function () {
                        mytempChart.resize()
                    }, 100)
                }
                $(window).on("resize", resize), $(".sidebartoggler").on("click", resize)
            });
        }
    }
});
// ============================================================== 
// Pie chart option
// ============================================================== 
var url1 = "/json/GetStatusByChart"; 
var dataPoints = [];
 $.ajax({
     url: url1,
     contentType: "application/json; charset=utf-8",
     dataType: "json",
     type:"POST",
     success: function (ChartDatas) { 
         for (var i = 0; i <= ChartDatas.data.length - 1; i++) {
             dataPoints.push({ value: parseInt(ChartDatas.data[i].value), name: ChartDatas.data[i].name });
         }
         var pieChart = echarts.init(document.getElementById('pie-chart'));
         option = { 
             tooltip : {
                 trigger: 'item',
                 formatter: "{a} <br/>{b} : {c} ({d}%)"
             },
             legend: {
                 x : 'center',
                 y : 'bottom',
                 data:['Pending','New','Problem','Completed']
             },
             toolbox: {
                 show : true,
                 feature : {
            
                     dataView : {show: true, readOnly: false},
                     magicType : {
                         show: true, 
                         type: ['pie', 'funnel']
                     },
                     restore : {show: true},
                     saveAsImage : {show: true}
                 }
             },
             color: ["#f62d51", "#008000", "#ffbc34", "#7460ee"],
             calculable : true,
             series : [
                 
                 {
                     name:'Job By Status',
                     type:'pie',
                     radius : [30, 110],
                     center : ['50%', 200],
                     roseType : 'area',
                     x: '50%',               // for funnel
                     max: 40,                // for funnel
                     sort : 'ascending',     // for funnel
                     data: dataPoints
                 }
             ]
         };
         // use configuration item and data specified to show chart
         pieChart.setOption(option, true), $(function() {
             function resize() {
                 setTimeout(function() {
                     pieChart.resize()
                 }, 100)
             }
             $(window).on("resize", resize), $(".sidebartoggler").on("click", resize)
         });
     }
     });

 var url = "/json/GettypeByChart";
 var dataPoints1 = [];
 var dataPoints2 = [];
 $.ajax({
     url: url,
     contentType: "application/json; charset=utf-8",
     dataType: "json",
     type: "POST",
     success: function (ChartDatas) {
         for (var i = 0; i <= ChartDatas.data.length - 1; i++) {
             dataPoints1.push({ value: parseInt(ChartDatas.data[i].value), name: ChartDatas.data[i].name });
             dataPoints2.push(ChartDatas.data[i].name ); 
         }  
         if(ChartDatas.data.length==0)
         {
             dataPoints1.push(0);
             dataPoints2.push("No Job Exist");
         }
         var pieChart1 = echarts.init(document.getElementById('pie-chart1'));
         option = {
             tooltip: {
                 trigger: 'item',
                 formatter: "{a} <br/>{b} : {c} ({d}%)"
             },
             legend: {
                 x: 'center',
                 y: 'bottom',
                 data: dataPoints2
             },
             toolbox: {
                 show: true,
                 feature: {

                     dataView: { show: true, readOnly: false },
                     magicType: {
                         show: true,
                         type: ['pie', 'funnel']
                     },
                     restore: { show: true },
                     saveAsImage: { show: true }
                 }
             },
            
             calculable: true,
             series: [

                 {
                     name: 'Job By Type',
                     type: 'pie',
                     radius: [30, 110],
                     center: ['50%', 200],
                     roseType: 'area',
                     x: '50%',               // for funnel
                     max: 40,                // for funnel
                     sort: 'ascending',     // for funnel
                     data: dataPoints1
                 }
             ]
         };
         // use configuration item and data specified to show chart
         pieChart1.setOption(option, true), $(function () {
             function resize() {
                 setTimeout(function () {
                     pieChart1.resize()
                 }, 100)
             }
             $(window).on("resize", resize), $(".sidebartoggler").on("click", resize)
         });
     }
 });


 var url0 = "/json/GetcontactsByChart";
 var dataPoints10 = [];
 var dataPoints20 = [];
 $.ajax({
     url: url0,
     contentType: "application/json; charset=utf-8",
     dataType: "json",
     type: "POST",
     success: function (ChartDatas) {
         for (var i = 0; i <= ChartDatas.data.length - 1; i++) {
             dataPoints10.push({ value: parseInt(ChartDatas.data[i].value), name: ChartDatas.data[i].name });
             dataPoints20.push(ChartDatas.data[i].name);
         }
         if (ChartDatas.data.length == 0) {
             dataPoints10.push(0);
             dataPoints20.push("No Contact Created");
         }
         var pieChart2 = echarts.init(document.getElementById('pie-chart3'));
         option = {
             tooltip: {
                 trigger: 'item',
                 formatter: "{a} <br/>{b} : {c} ({d}%)"
             },
             legend: {
                 x: 'center',
                 y: 'bottom',
                 data: dataPoints20
             },
             toolbox: {
                 show: true,
                 feature: {

                     dataView: { show: true, readOnly: false },
                     magicType: {
                         show: true,
                         type: ['pie', 'funnel']
                     },
                     restore: { show: true },
                     saveAsImage: { show: true }
                 }
             },

             calculable: true,
             series: [

                 {
                     name: 'Contacts By Label',
                     type: 'pie',
                     radius: [30, 110],
                     center: ['70%', 200],
                     roseType: 'area',
                     x: '50%',               // for funnel
                     max: 40,                // for funnel
                     sort: 'ascending',     // for funnel
                     data: dataPoints10
                 }
             ]
         };
         // use configuration item and data specified to show chart
         pieChart2.setOption(option, true), $(function () {
             function resize() {
                 setTimeout(function () {
                     pieChart2.resize()
                 }, 100)
             }
             $(window).on("resize", resize), $(".sidebartoggler").on("click", resize)
         });
     }
 });

  