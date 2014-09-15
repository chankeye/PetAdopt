function MyViewModel() {
    var self = this;

    self.helplist = ko.observableArray();

    self.removeHelp = function (help) {
        if (confirm('確定要刪除？')) {

            if (help.IsDisable == true)
                return;

            $.ajax({
                type: 'post',
                url: '/Manage/Help/Delete',
                data: {
                    id: help.Id,
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        self.helplist.remove(help);
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        }
    }
}

$(function () {

    var vm = new MyViewModel();

    // 取得即刻救援列表
    $.ajax({
        type: 'post',
        url: '/Manage/Help/GetHelpList',
        success: function (data) {
            vm.helplist(data);
        }
    });

    ko.applyBindings(vm);
});
