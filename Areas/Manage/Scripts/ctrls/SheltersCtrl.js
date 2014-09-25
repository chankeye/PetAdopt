var MyViewModel = function () {
    var self = this;

    self.loading = ko.observable(false);
    self.responseMessage = ko.observable($.commonLocalization.noRecord);
    self.history = ko.observableArray();

    self.areas = ko.observableArray();

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
                        self.history.remove(shelters);
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        }
    }

    self.editShelters = function (shelters) {
        window.location = "/Manage/Shelters/Edit?id=" + shelters.Id;
    }

    //Add PaginationModel
    //from pagination.js
    ko.utils.extend(self, new PaginationModel());

    self.loadHistory = function (page) {
        self.responseMessage($.commonLocalization.loading);
        self.loading(true);
        self.history.removeAll();
        self.pagination(0, 0, 0);
        var limit = 10;
        page = page || 1; // if page didn't send
        $.ajax({
            type: 'post',
            url: '/Manage/Shelters/GetSheltersList',
            data: {
                page: page
            }
        }).done(function (response) {
            self.responseMessage('');
            self.history(response.List);
            self.pagination(page, response.Count, limit);

            if (response.Count == 0)
                self.responseMessage($.commonLocalization.noRecord);

        }).always(function () {
            self.loading(false);
        });
    };
}

$(function () {

    // 取得地區列表
    $.ajax({
        type: 'post',
        url: '/Manage/System/GetAreaList',
        success: function (area) {
            window.vm.areas(area);
            window.vm.areas.unshift({
                "Word": "請選擇",
                "Id": ""
            });
            $("#selOptions option:first").attr("selected", true);
        }
    });

    window.vm = new MyViewModel();
    window.vm.loadHistory();
    ko.applyBindings(window.vm);

    // 新增消息
    $("#btn1").click(
        function () {
            var $btn = $("#btn1");
            var $uploadfile = $(".table-striped .name a");
            var photo = $uploadfile.text();

            if ($("#commentForm").valid() == false) {
                return;
            }

            if ($("#selOptions").val() == "") {
                alert('請選擇地區');
                return;
            }

            $btn.button("loading");

            $.ajax({
                type: 'post',
                url: '/Manage/Shelters/AddShelters',
                data: {
                    Photo: photo,
                    Name: $("#name").val(),
                    Introduction: $("#introduction").val(),
                    Phone: $("#phone").val(),
                    Address: $("#address").val(),
                    AreaId: $("#selOptions").val(),
                    Url: $("#source").val()
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        //vm.history.push(data.ReturnObject);
                        window.vm.loadHistory();
                        $("#name").val('');
                        $("#introduction").val(''),
                        $("#phone").val(''),
                        $("#address").val('');
                        $("#selOptions option:first").attr("selected", true);
                        $("#source").val();

                        alert("新增成功");
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        });
});
