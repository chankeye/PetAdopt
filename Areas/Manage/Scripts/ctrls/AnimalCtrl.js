var MyViewModel = function () {
    var self = this;

    self.loading = ko.observable(false);
    self.responseMessage = ko.observable($.commonLocalization.noRecord);
    self.history = ko.observableArray();

    self.areas = ko.observableArray();
    self.classes = ko.observableArray();
    self.statuses = ko.observableArray();

    self.removeAnimal = function (animal) {
        if (confirm('確定要刪除？')) {

            if (animal.IsDisable == true)
                return;

            $.ajax({
                type: 'post',
                url: '/Manage/Animal/Delete',
                data: {
                    id: animal.Id,
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        self.history.remove(animal);
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        }
    }

    self.editAnimal = function (animal) {
        window.location = "/Manage/Animal/Edit?id=" + animal.Id;
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
            url: '/Manage/Animal/GetAnimalList',
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
            $("#selOptionsAreas option:first").attr("selected", true);
        }
    });

    // 取得分類列表
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

    // 取得狀態列表
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

    window.vm = new MyViewModel();
    window.vm.loadHistory();
    ko.applyBindings(window.vm);

    // 新增動物
    $("#btn1").click(
        function () {
            var $btn = $("#btn1");
            var $uploadfile = $(".table-striped .name a");
            var photo = $uploadfile.text();

            if ($("#commentForm").valid() == false) {
                return;
            }

            if ($("#selOptionsClasses").val() == "") {
                alert('請選擇分類');
                return;
            }

            if ($("#selOptionsStatuses").val() == "") {
                alert('請選擇狀態');
                return;
            }

            if ($("#shelters").val() == "") {
                if ($("#selOptionsAreas").val() == "" || $("#phone").val() == "" || $("#address").val() == "") {
                    alert('請輸入收容所 或 選擇地區、填入地址、電話');
                    return;
                }
            }

            $btn.button("loading");

            $.ajax({
                type: 'post',
                url: '/Manage/Animal/AddAnimal',
                data: {
                    Photo: photo,
                    Title: $("#title").val(),
                    StartDate: $("#startDate").val(),
                    EndDate: $("#endDate").val(),
                    Introduction: $("#introduction").val(),
                    SheltersId: $("#shelters").val(),
                    Phone: $("#phone").val(),
                    Address: $("#address").val(),
                    AreaId: $("#selOptionsAreas").val(),
                    ClassId: $("#selOptionsClasses").val(),
                    StatusId: $("#selOptionsStatuses").val(),
                    Age: $("#age").val(),
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        //vm.history.push(data.ReturnObject);
                        window.vm.loadHistory();
                        $("#title").val('');
                        $("#startDate").val('');
                        $("#endDate").val('');
                        $("#introduction").val('');
                        $("#address").val('');
                        $("#phone").val('');
                        $("#shelters").val('');
                        $("#age").val('');
                        $("#selOptionsAreas option:first").attr("selected", true);
                        $("#selOptionsClasses option:first").attr("selected", true);
                        $("#selOptionsStatuses option:first").attr("selected", true);

                        alert("新增成功");
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        });
});
