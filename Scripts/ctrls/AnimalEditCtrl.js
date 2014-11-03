﻿function MyViewModel() {
    var self = this;

    self.loading = ko.observable(false);
    self.responseMessage = ko.observable($.commonLocalization.noRecord);
    self.history = ko.observableArray();

    self.areas = ko.observableArray();
    self.classes = ko.observableArray();
    self.statuses = ko.observableArray();
};

$(function () {

    // 沒有輸入id直接導回
    window.id = window.utils.urlParams("id");
    if (window.id == null) {
        if (history.length > 1)
            history.back();
        else
            window.location = '/Animal';
    }

    // 取得認養資訊
    var photo;
    function init() {
        $.ajax({
            type: 'post',
            url: '/Manage/Animal/EditInit',
            data: {
                id: window.id
            },
            success: function (data) {
                if (data.IsSuccess) {
                    photo = data.ReturnObject.Photo;
                    if (photo != null) {
                        $('#coverPhoto').attr('src', "../../Content/uploads/" + photo);
                    }
                    $("#title").val(data.ReturnObject.Title);
                    $("#startDate").val(data.ReturnObject.StartDate),
                        $("#endDate").val(data.ReturnObject.EndDate),
                        $("#introduction").val(data.ReturnObject.Introduction),
                        $("#shelters").val(data.ReturnObject.SheltersId),
                        $("#phone").val(data.ReturnObject.Phone),
                        $("#address").val(data.ReturnObject.Address),
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
                    $("#selOptionsStatuses").children().each(function () {
                        if ($(this).val() == data.ReturnObject.StatusId) {
                            //jQuery給法
                            $(this).attr("selected", true); //或是給"selected"也可

                            //javascript給法
                            this.selected = true;
                        }
                    });
                    $("#age").val(data.ReturnObject.Age);
                    $("#address").val(data.ReturnObject.Address);
                } else {
                    alert(data.ErrorMessage);
                    window.location = '/Manage/Animal';
                }
            }
        });
    }

    // 頁面初始化
    window.utils.getAreaList()
        .then(window.utils.getClassList())
        .then(window.utils.getStatusList())
        .then(init());

    window.vm = new MyViewModel();
    ko.applyBindings(window.vm, $("#mainContiner")[0]);

    // 自動完成
    var timestamp = new Date().getTime();
    $("#shelters")
        .typeahead({
            remote: {
                url: '/Manage/Shelters/GetSheltersSuggestion',
                replace: function (url, uriEncodedQuery) {
                    return url + "?name=" + uriEncodedQuery + "&_=" + timestamp;
                }
            },
            valueKey: 'Value',
            template: '<p>{{Display}}</p>',
            engine: Hogan,
            limit: 10
        });

    // 修改活動
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
                url: '/Manage/Animal/EditAnimal',
                data: {
                    id: window.id,
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
                        alert("修改完成");
                        if (history.length > 1)
                            history.back();
                        else
                            window.location = '/Animal';
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
            window.location = '/Animal';
    });
});