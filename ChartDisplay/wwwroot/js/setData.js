"use strict";

//引入fly实例
var fly = new Fly();

//设置参数
document.getElementById("setData").addEventListener("click", function (event) {
    var data = [];
    data.push(parseFloat(document.getElementById('data1').value));
    data.push(parseFloat(document.getElementById('data2').value));
    data.push(parseFloat(document.getElementById('data3').value));
    data.push(parseFloat(document.getElementById('data4').value));
    //console.log(data)
    fly.post('api/setdata', { Data:data })
        .then(function (response) {
            if (response.data != null) {
                console.log(response.data);
                var resData = JSON.parse(response.data);
                console.log(resData);
                if (resData.success == true) {
                    alert("数据发送成功！");
                }
                else {
                    alert("数据发送失败！");
                }
            }
        })
        .catch(function (error) {
            console.log(error);
        });
});

function setOneCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toGMTString();
    document.cookie = cname + "=" + cvalue + "; " + expires;
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i].trim();
        if (c.indexOf(name) == 0) { return c.substring(name.length, c.length); }
    }
    return "";
}

function setCookies() {
    var data = [];
    var k = [];
    data.push(parseFloat(document.getElementById('data1').value));
    data.push(parseFloat(document.getElementById('data2').value));
    data.push(parseFloat(document.getElementById('data3').value));
    data.push(parseFloat(document.getElementById('data4').value));
    k.push(document.getElementById('k1').value);
    k.push(document.getElementById('k2').value);
    k.push(document.getElementById('k3').value);
    k.push(document.getElementById('k4').value);

    var cookieValue = JSON.stringify({ k, data });
    //console.log(cookieValue);
    setOneCookie("settingsData", cookieValue, 180);
}

function checkCookie() {
    var settingsDataStr = getCookie("settingsData");
    //console.log(settingsDataStr);
    
    if (settingsDataStr != "") {
        var settinsData = JSON.parse(settingsDataStr);
        document.getElementById('data1').value = settinsData.data[0];
        document.getElementById('data2').value = settinsData.data[1];
        document.getElementById('data3').value = settinsData.data[2];
        document.getElementById('data4').value = settinsData.data[3];
        document.getElementById('k1').value = settinsData.k[0];
        document.getElementById('k2').value = settinsData.k[1];
        document.getElementById('k3').value = settinsData.k[2];
        document.getElementById('k4').value = settinsData.k[3];
    }
}

window.onload = checkCookie;

