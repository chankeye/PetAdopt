window.utils = {

    // 取得地區列表
    getAreaList: function () {
        $.ajax({
            type: 'post',
            url: '/Manage/System/GetAreaList',
            success: function (area) {
                window.vm.areas(area);
                window.vm.areas.unshift({
                    "Word": "請選擇",
                    "Id": ""
                });
                $("#selOptionsAreas option:first").attr("selected", true);
            }
        });
    },

    // 取得分類列表
    getClassList: function () {
        $.ajax({
            type: 'post',
            url: '/Manage/System/GetClassList',
            success: function (classes) {
                window.vm.classes(classes);
                window.vm.classes.unshift({
                    "Word": "請選擇",
                    "Id": ""
                });
                $("#selOptionsClasses option:first").attr("selected", true);
            }
        });
    },

    // 取得狀態列表
    getStatusList: function () {
        $.ajax({
            type: 'post',
            url: '/Manage/System/GetStatusList',
            success: function (statuses) {
                window.vm.statuses(statuses);
                window.vm.statuses.unshift({
                    "Word": "請選擇",
                    "Id": ""
                });
                $("#selOptionsStatuses option:first").attr("selected", true);
            }
        });
    },

    // 解析url
    urlParams: function (data) {
        var urlParams = {};
        var e,
            a = /\+/g,  // Regex for replacing addition symbol with a space
            r = /([^&=]+)=?([^&]*)/g,
            d = function (s) { return decodeURIComponent(s.replace(a, " ")); },
            q = window.location.search.substring(1);
        while (e = r.exec(q)) {
            urlParams[d(e[1])] = d(e[2]);
        }

        return urlParams[data];
    },

    // 查詢
    searchList: function (btn) {
        var $btn = btn;

        $btn.button("loading");
        window.vm.loadHistory(1, 10, $("#search").val(), !$("#checkAll").is(':checked'));
        $btn.button("reset");
    }
}