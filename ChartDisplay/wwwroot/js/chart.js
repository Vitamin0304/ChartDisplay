﻿"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chartHub").build();
var myChart = echarts.init(document.getElementById('main'));

var dataList = [[0,0,0,0,0]];
//for (var i = 0; i < 100; i++) {
//    dataList.push([i, Math.sin(0.05 * 3.1415926 * i)]);
//}

var option = {
    dataset: {
        source: dataList
    },
    title: {
        text: '动态数据 + 时间坐标轴'
    },
    legend: {
        data: ['data1', 'data2', 'data3', 'data4']
    },
    tooltip: {
        trigger: 'axis',
        transitionDuration:0.1
    },
    dataZoom: [
        {
            type: 'slider',
            show: true,
        },
        {
            type: 'inside',
            xAxisIndex: [0]
        },
        {
            type: 'slider',
            show: true,
            yAxisIndex: 0,
            filterMode: 'none',
            showDataShadow: false,
        }
    ],
    xAxis: {
        type: 'value',
    },
    yAxis: {
        type: 'value',
    },
    series: [{
            name: 'data1',
            type: 'line',
            showSymbol: false,
            hoverAnimation: false,
            encode: { x: 0, y: 1 }
        },
        {
            name: 'data2',
            type: 'line',
            showSymbol: false,
            hoverAnimation: false,
            encode: { x: 0, y: 2 }
        },
        {
            name: 'data3',
            type: 'line',
            showSymbol: false,
            hoverAnimation: false,
            encode: { x: 0, y: 3 }
        },
        {
            name: 'data4',
            type: 'line',
            showSymbol: false,
            hoverAnimation: false,
            encode: { x: 0, y: 4 }
        }
    ]
};
myChart.setOption(option);

//connection.on("ReceiveData", function (time, data) {
//    dataList.push([time, data]);
//    dataUpdate();
//});

connection.on("ReceiveChartDisplayData", function (chartDisplayData) {
    var nowData = [];
    nowData.push(chartDisplayData.time);
    nowData = nowData.concat(chartDisplayData.data);
    //var data = eval(chartDisplayData);
    dataList.push(nowData);
    //console.log(dataList);
    dataUpdate();
});

document.getElementById("clear").addEventListener("click", function (event) {
    dataList = [[0, 0, 0, 0, 0]];
    dataUpdate();
});

function dataUpdate() {
    if (document.getElementById('suspend').checked == false) {
        myChart.setOption({
            dataset: {
                source: dataList
            }
        });
    }
}

connection.start().then(function () {


}).catch(function (err) {
    return console.error(err.toString());
});
