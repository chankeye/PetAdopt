function MyViewModel() {
    var self = this;

    self.areas = ko.observableArray();

    self.removeAddress = function (area) {
        if (confirm('確定要刪除？')) {
            $.ajax({
                type: 'post',
                url: '/Manage/System/DeleteArea',
                data: {
                    id: area.Id,
                },
                success: function(data) {
                    if (data.IsSuccess) {
                        self.areas.remove(area);
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        }
    }
};

$(function () {
    var vm = new MyViewModel();

    $.ajax({
        type: 'post',
        url: '/Manage/System/GetAreaList',
        //async:false,
        success: function (data) {
            vm.areas(data);
        }
    });

    $("#btnAddArea").click(function () {
        if ($("#area-form").valid()) {
            $.ajax({
                type: 'post',
                url: '/Manage/System/AddArea',
                data: {
                    word: $("#ares").val(),
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        vm.areas.push(data.ReturnObject);
                        $("#ares").val('');
                    } else {
                        alert(data.ErrorMessage);
                        $("#ares").val('');
                    }
                }
            });
        }
    });

    ko.applyBindings(vm);
});
