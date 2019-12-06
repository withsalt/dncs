var nowUrl = window.location.href.toLowerCase();
if (nowUrl.indexOf("scan") != -1) {
    $("#scan").html("返回");
    $("#scan").on('click', function () {
        window.location.href = "/Home";
    });
}
else {
    $("#scan").html("关注");
    $("#scan").on('click', function () {
        window.location.href = "/Home/Scan";
    });
}