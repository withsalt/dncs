var tools = {
    //去掉字符串中空格
    excludeSpecial: function (s) {
        // 去掉转义字符
        s = s.replace(/\s*/g, "");
        return s;
    },
    //判断是否为电脑
    isPc: function () {
        var userAgentInfo = navigator.userAgent;
        var Agents = ["Android", "iPhone",
            "SymbianOS", "Windows Phone",
            "iPad", "iPod"
        ];
        var flag = true;
        for (var v = 0; v < Agents.length; v++) {
            if (userAgentInfo.indexOf(Agents[v]) > 0) {
                flag = false;
                break;
            }
        }
        if (flag && ($(window).height() < 600 || $(window).width() < 400)) {
            if ($(window).width() > 600) {
                flag = true;
            } else {
                flag = false;
            }
        }
        return flag;
    },
    //格式化时间戳为标准时间
    formatTimeStamp: function (timestamp) {
        var v = timestamp;
        if (v == 0) {
            return "1970-01-01 08:00:00";
        }

        if (/^(-)?\d{1,10}$/.test(v)) {
            v = v * 1000;
        } else if (/^(-)?\d{1,13}$/.test(v)) {
            v = v * 1000;
        } else if (/^(-)?\d{1,14}$/.test(v)) {
            v = v * 100;
        } else if (/^(-)?\d{1,15}$/.test(v)) {
            v = v * 10;
        } else if (/^(-)?\d{1,16}$/.test(v)) {
            v = v * 1;
        } else {
            //alert("错误：时间戳格式不正确!");
            return "1970-01-01 08:00:00";
        }
        var dateObj = new Date(v);
        //if (dateObj.format('yyyy') == "NaN") { /*alert("时间戳格式不正确");*/return; }
        //var UnixTimeToDate = dateObj.getFullYear() + '/' + (dateObj.getMonth() + 1) + '/' + dateObj.getDate() + ' ' + dateObj.getHours() + ':' + dateObj.getMinutes() + ':' + dateObj.getSeconds();
        var year = dateObj.getFullYear();
        var month = (dateObj.getMonth() + 1) < 10 ? "0" + (dateObj.getMonth() + 1) : (dateObj.getMonth() + 1);
        var day = dateObj.getDate() < 10 ? "0" + dateObj.getDate() : dateObj.getDate();
        var hour = dateObj.getHours() < 10 ? "0" + dateObj.getHours() : dateObj.getHours();
        var min = dateObj.getMinutes() < 10 ? "0" + dateObj.getMinutes() : dateObj.getMinutes();
        var sec = dateObj.getSeconds() < 10 ? "0" + dateObj.getSeconds() : dateObj.getSeconds();

        var UnixTimeToDate = year + '-' + month + '-' + day + " " + hour + ":" + min + ":" + sec;
        return UnixTimeToDate;
    }
}

var request = {
    queryString: function (val) {
        var uri = window.location.search;
        var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
        return ((uri.match(re)) ? decodeURIComponent((uri.match(re)[0].substr(val.length + 1))) : null);
    },
    queryStrings: function () {
        var uri = window.location.search;
        var re = /\w*\=([^\&\?]*)/ig;
        var retval = [];
        while ((arr = re.exec(uri)) != null)
            retval.push(arr[0]);
        return retval;
    },
    setQuery: function (val1, val2) {
        var a = this.QueryStrings();
        var retval = "";
        var seted = false;
        var re = new RegExp("^" + val1 + "\=([^\&\?]*)$", "ig");
        for (var i = 0; i < a.length; i++) {
            if (re.test(a[i])) {
                seted = true;
                a[i] = val1 + "=" + val2;
            }
        }
        retval = a.join("&");
        return "?" + retval + (seted ? "" : (retval ? "&" : "") + val1 + "=" + val2);
    }
};

var server = {
    post: function (url, data, callback) {
        if (url == null || url == "") {
            layer.msg("请求接口URL不能为空！", { icon: 5 });
            return;
        }
        if (data == null) {
            layer.msg("请求数据不能为空！", { icon: 5 });
            return;
        }
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            dataType: "json",
            success: function (data) {
                if (data != "" && data != undefined && data != null) {
                    var dataObj;
                    if (typeof (data) == "string") {
                        var start = data.indexOf("{");
                        var end = data.lastIndexOf("}") + 1;
                        var clearJson = data.substring(start, end);
                        dataObj = eval('(' + clearJson + ')');
                    }
                    else {
                        dataObj = data;
                    }
                    if (callback != null) {
                        callback(dataObj);
                    }
                } else {
                    if (callback != null) {
                        callback(null);
                    }
                }
            },
            error: function () {
                if (callback != null) {
                    callback(null);
                }
            }
        });
    }
}

String.prototype.format = function (args) {
    var result = this;
    if (arguments.length > 0) {
        if (arguments.length == 1 && typeof (args) == "object") {
            for (var key in args) {
                if (args[key] != undefined) {
                    var reg = new RegExp("({" + key + "})", "g");
                    result = result.replace(reg, args[key]);
                }
            }
        }
        else {
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i] != undefined) {
                    var reg = new RegExp("({)" + i + "(})", "g");
                    result = result.replace(reg, arguments[i]);
                }
            }
        }
    }
    return result;
}

String.prototype.removeBlankLines = function () {
    if (this == null) {
        return this;
    }
    return this.replace(/(\n[\s\t]*\r*\n)/g, '\n').replace(/^[\n\r\n\t]*|[\n\r\n\t]*$/g, '')
}

String.prototype.is_null = function () {
    if (this == null || this == "" || this.replace(/(^s*)|(s*$)/g, "").length == 0) {
        return true;
    }
    else {
        return false;
    }
}