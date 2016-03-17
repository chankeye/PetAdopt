function MyViewModel() {
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

    // 判斷有沒有輸入收容所
    function sheltersInput() {
        if ($("#shelters").val() != "") {
            $("#selOptionsAreas").attr('disabled', true);
            $("#phone").attr('disabled', true);
            $("#address").attr('disabled', true);
        } else {
            $("#selOptionsAreas").attr('disabled', false);
            $("#phone").attr('disabled', false);
            $("#address").attr('disabled', false);
        }
    }

    $("#commentForm").validate({
        rules: {
            title: {
                required: true,
                maxlength: 50
            },
            introduction: {
                required: true,
                maxlength: 500
            },
            startDate: {
                required: true,
                date: true
            },
            selOptionsClasses: {
                required: true
            },
            selOptionsStatuses: {
                required: true
            },
            shelters: {
                maxlength: 30
            },
            address: {
                maxlength: 30
            },
            endDate: {
                date: true
            }
        },
        messages: {
            title: {
                required: "請輸入標題",
                maxlength: "不得大於50個字"
            },
            introduction: {
                required: "請輸入內容",
                maxlength: "不得大於500個字"
            },
            startDate: {
                required: "請輸入開始送養日期",
                date: "請輸入正確的日期格式yyyy/mm/dd"
            },
            selOptionsClasses: "請選擇分類",
            selOptionsStatuses: "請選擇狀態",
            shelters: "不得大於30個字",
            address: "不得大於30個字",
            endDate: "請輸入正確的日期格式yyyy/mm/dd",
        }
    });

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
                        if (photo.slice(0, "http://".length) == "http://" || photo.slice(0, "https://".length) == "https://")
                            $('#coverPhoto').attr('src', photo);
                        else
                            $('#coverPhoto').attr('src', "../../Content/uploads/" + photo);
                    }
                    $("#title").val(data.ReturnObject.Title);
                    var startdate = new Date(data.ReturnObject.StartDate);
                    var startMonth = startdate.getMonth() + 1;
                    var startDay = startdate.getDate();
                    $("#startDate").val(startdate.getFullYear() + '-' + (startMonth < 10 ? '0' : '') + startMonth + '-' + (startDay < 10 ? '0' : '') + startDay);
                    var enddate = new Date(data.ReturnObject.EndDate);
                    var endMonth = enddate.getMonth() + 1;
                    var endDay = enddate.getDate();
                    $("#endDate").val(enddate.getFullYear() + '-' + (endMonth < 10 ? '0' : '') + endMonth + '-' + (endDay < 10 ? '0' : '') + endDay);
                    $("#introduction").val(data.ReturnObject.Introduction);
                    $("#shelters").val(data.ReturnObject.Shelters);
                    $("#phone").val(data.ReturnObject.Phone);
                    $("#address").val(data.ReturnObject.Address);
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
                    sheltersInput();
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
            valueKey: 'Display',
            template: '<p>{{Display}}</p>',
            engine: Hogan,
            limit: 10
        });

    // 有填入收容所，地區、地址、電話就不需要填
    $("#shelters").keyup(sheltersInput);

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
                if ($("#selOptionsAreas").val() == "") {
                    alert('請輸入收容所 或 選擇地區');
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
                    Shelters: $("#shelters").val(),
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
