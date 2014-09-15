function MyViewModel() {
    var self = this;

    self.asklist = ko.observableArray();

    self.removeAsk = function (ask) {
        if (confirm('確定要刪除？')) {

            if (ask.IsDisable == true)
                return;

            $.ajax({
                type: 'post',
                url: '/Manage/Ask/Delete',
                data: {
                    id: ask.Id,
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        self.asklist.remove(ask);
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

    // 取得問與答列表
    $.ajax({
        type: 'post',
        url: '/Manage/Ask/GetAskList',
        success: function (data) {
            vm.asklist(data);
        }
    });

    ko.applyBindings(vm);
});
