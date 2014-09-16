$(function () {

    function MyViewModel() {
        var self = this;

        self.shelterslist = ko.observableArray();

        self.removeShelters = function (shelters) {
            if (confirm('確定要刪除？')) {

                if (shelters.IsDisable == true)
                    return;

                $.ajax({
                    type: 'post',
                    url: '/Manage/Shelters/Delete',
                    data: {
                        id: shelters.Id,
                    },
                    success: function (data) {
                        if (data.IsSuccess) {
                            self.shelterslist.remove(shelters);
                        } else {
                            alert(data.ErrorMessage);
                        }
                    }
                });
            }
        }
    }

    var vm = new MyViewModel();

    // 取得收容所列表
    $.ajax({
        type: 'post',
        url: '/Manage/Shelters/GetSheltersList',
        success: function (data) {
            vm.shelterslist(data);
        }
    });

    ko.applyBindings(vm);
});
