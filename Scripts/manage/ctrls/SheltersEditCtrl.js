function MyViewModel() {
    var self = this;

    self.loading = ko.observable(false);
    self.responseMessage = ko.observable($.commonLocalization.noRecord);
    self.history = ko.observableArray();

    self.areas = ko.observableArray();

    self.removeMessage = function (message) {
        if (confirm('確定要刪除？')) {

            if (message.IsDisable == true)
                return;

            $.ajax({
                type: 'post',
                url: '/Manage/Shelters/DeleteMessage',
                data: {
                    Id: window.id,
                    MessageId: message.Id
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        self.history.remove(message);
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        }
    }

    //Add PaginationModel
    //from pagination.js
    ko.utils.extend(self, new PaginationModel());

    self.loadHistory = function (page, take) {
        self.responseMessage($.commonLocalization.loading);
        self.loading(true);
        self.history.removeAll();
        self.pagination(0, 0, 0);
        page = page || 1; // if page didn't send
        take = take || 10;
        $.ajax({
            type: 'post',
            url: '/Manage/Shelters/GetMessageList',
            data: {
                id: window.id,
                page: page,
                take: take
            }
        }).done(function (response) {
            self.responseMessage('');
            self.history(response.List);
            self.pagination(page, response.Count, take);

            if (response.Count == 0)
                self.responseMessage($.commonLocalization.noRecord);

        }).always(function () {
            self.loading(false);
        });
    };
};

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

    var urlParams = {};
    (function () {
        var e,
            a = /\+/g,  // Regex for replacing addition symbol with a space
            r = /([^&=]+)=?([^&]*)/g,
            d = function (s) { return decodeURIComponent(s.replace(a, " ")); },
            q = window.location.search.substring(1);
        while (e = r.exec(q)) {
            urlParams[d(e[1])] = d(e[2]);
        }
    })();

    // 沒有輸入id直接導回
    if (urlParams["id"] == null)
        window.location = '/Manage/Shelters';
    window.id = urlParams["id"];

    // 取得收容所資訊
    var photo;
    $.ajax({
        type: 'post',
        url: '/Manage/Shelters/EditInit',
        data: {
            id: urlParams["id"]
        },
        success: function (data) {
            if (data.IsSuccess) {
                photo = data.ReturnObject.Photo;
                if (photo != null) {
                    $('#coverPhoto').attr('src', "../../../../Content/uploads/" + photo);
                }
                $("#name").val(data.ReturnObject.Name);
                $("#selOptions").children().each(function () {
                    if ($(this).val() == data.ReturnObject.AreaId) {
                        //jQuery給法
                        $(this).attr("selected", true); //或是給"selected"也可

                        //javascript給法
                        this.selected = true;
                    }
                });
                $("#introduction").val(data.ReturnObject.Introduction);
                $("#address").val(data.ReturnObject.Address);
                $("#phone").val(data.ReturnObject.Phone);
                $("#source").val(data.ReturnObject.url);
            } else {
                alert(data.ErrorMessage);
                window.location = '/Manage/Shelters';
            }
        }
    });

    window.vm = new MyViewModel();
    window.vm.loadHistory();
    ko.applyBindings(window.vm);

    // 修改收容所資訊
    $("#btn1").click(
        function () {
            var $btn = $("#btn1");
            var $uploadfile = $(".table-striped .name a");
            if ($uploadfile.text() != "") {
                $.ajax({
                    type: 'post',
                    url: '/Manage/System/DeletePhoto',
                    data: {
                        Photo: photo
                    }
                });

                photo = $uploadfile.text();
            }

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
                url: '/Manage/Shelters/EditShelters',
                data: {
                    id: urlParams["id"],
                    Photo: photo,
                    Name: $("#name").val(),
                    Introduction: $("#introduction").val(),
                    Address: $("#address").val(),
                    Url: $("#source").val(),
                    Phone: $("#phone").val(),
                    AreaId: $("#selOptions").val(),
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        $("#name").val('');
                        $("#introduction").val('');
                        $("#address").val('');
                        $("#source").val('');
                        $("#phone").val('');
                        $("#selOptions option:first").attr("selected", true);

                        alert("修改完成");
                        window.location = '/Manage/Shelters';
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        });

    // 取消
    $("#btn2").click(
    function () {
        window.location = '/Manage/Shelters';
    });
});
