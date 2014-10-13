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
    }
}