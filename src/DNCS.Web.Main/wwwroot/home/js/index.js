$(function () {
    document.onkeyup = function (e) {
        var ev = document.all ? window.event : e;
        if (ev.keyCode == 13) {
            app.Init();
        }
    }
});

var app = new Vue({
    el: '#sentence',
    data: {
        content: '',
        source: '',
        hassource: false
    },
    created() {
        this.Init();
    },
    mounted: function () {
        //this.Init();
    },
    methods: {
        Init: function () {
            var self = this;
            var type = request.queryString("type");
            if (type == null || type.is_null()) {
                type = "1";
            }
            server.post("/Home/DoChange", {
                Type: type
            }, function (data) {
                layer.closeAll();
                if (data == null) {
                    layer.alert('加载失败，请重试。', { icon: 5 });
                    return;
                }
                if (data.Code != 0) {
                    layer.alert('加载失败({0})，请重试。'.format(data.Code), { icon: 5 });
                    return;
                }
                self.content = data.Data.Content;
                if (data.Data.Source != null && !data.Data.Source.is_null()) {
                    self.hassource = true;
                    self.source = "——" + data.Data.Source;
                }
                else {
                    self.hassource = false;
                    self.source = "";
                }
            });
        }
    }
})