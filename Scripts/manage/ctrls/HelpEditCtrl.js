﻿function MyViewModel() {
    var self = this;

    self.loading = ko.observable(false);
    self.responseMessage = ko.observable($.commonLocalization.noRecord);
    self.history = ko.observableArray();

    self.classes = ko.observableArray();
    self.areas = ko.observableArray();

    self.removeMessage = function (message) {
        if (confirm('確定要刪除？')) {

            if (message.IsDisable == true)
                return;

            $.ajax({
                type: 'post',
                url: '/Manage/Help/DeleteMessage',
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
            url: '/Manage/Help/GetMessageList',
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

    // 取得分類列表
    window.utils.getClassList();

    // 取得地區列表
    window.utils.getAreaList();

    // 沒有輸入id直接導回
    window.id = window.utils.urlParams("id");
    if (window.id == null)
        window.location = '/Manage/Help';

    // 取得最新救援
    var photo;
    $.ajax({
        type: 'post',
        url: '/Manage/Help/EditInit',
        data: {
            id: window.id
        },
        success: function (data) {
            if (data.IsSuccess) {
                photo = data.ReturnObject.Photo;
                $("#title").val(data.ReturnObject.Title);
                $("#selOptionsAreas").children().each(function () {
                    if ($(this).val() == data.ReturnObject.AreaId) {
                        //jQuery給法
                        $(this).attr("selected", true); //或是給"selected"也可

                        //javascript給法
                        this.selected = true;
                    }
                });
                $("#selOptionsClasses").children().each(function () {
                    if ($(this).val() == data.ReturnObject.ClassId) {
                        //jQuery給法
                        $(this).attr("selected", true); //或是給"selected"也可

                        //javascript給法
                        this.selected = true;
                    }
                });
                $("#message").val(data.ReturnObject.Message);
                $("#address").val(data.ReturnObject.Address);
            } else {
                alert(data.ErrorMessage);
                window.location = '/Manage/Help';
            }
        }
    });

    window.vm = new MyViewModel();
    window.vm.loadHistory();
    ko.applyBindings(window.vm);

    // 修改救援
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

            if ($("#selOptionsClasses").val() == "") {
                alert('請選擇分類');
                return;
            }

            if ($("#selOptionsAreas").val() == "") {
                alert('請選擇地區');
                return;
            }

            $btn.button("loading");

            $.ajax({
                type: 'post',
                url: '/Manage/Help/EditHelp',
                data: {
                    id: window.id,
                    Photo: photo,
                    Title: $("#title").val(),
                    Message: $("#message").val(),
                    Address: $("#address").val(),
                    AreaId: $("#selOptionsAreas").val(),
                    ClassId: $("#selOptionsClasses").val()
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        $("#title").val('');
                        $("#message").val('');
                        $("#address").val('');
                        $("#selOptionsClasses option:first").attr("selected", true);
                        $("#selOptionsAreas option:first").attr("selected", true);

                        alert("修改完成");
                        window.location = '/Manage/Help';
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        });

    // 取消
    $("#btn2").click(
    function () {
        if (history.length > 1)
            history.back();
        else
            window.location = '/Manage/Help';
    });
});
