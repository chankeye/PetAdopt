function MyViewModel() {
    var self = this;

    self.areas = ko.observableArray();
    self.classes = ko.observableArray();
    self.statuses = ko.observableArray();
};

$(function () {

    // 取得地區列表
    $.ajax({
        type: 'post',
        url: '/Manage/System/GetAreaList',
        success: function (area) {
            vm.areas(area);
            vm.areas.unshift({
                "Word": "請選擇",
                "Id": ""
            });
            $("#selOptions option:first").attr("selected", true);
        }
    });

    // 取得分類列表
    $.ajax({
        type: 'post',
        url: '/Manage/System/GetClassList',
        success: function (classes) {
            vm.classes(classes);
            vm.classes.unshift({
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
            vm.statuses(statuses);
            vm.statuses.unshift({
                "Word": "請選擇",
                "Id": ""
            });
            $("#selOptionsStatuses option:first").attr("selected", true);
        }
    });

    var vm = new MyViewModel();

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
        window.location = '/Manage/Animal';

    // 取得認養資訊
    var photo;
    $.ajax({
        type: 'post',
        url: '/Manage/Animal/EditInit',
        data: {
            id: urlParams["id"]
        },
        success: function (data) {
            if (data.IsSuccess) {
                photo = data.ReturnObject.Photo;
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
                    id: urlParams["id"],
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

                        alert("修改完成");
                        window.location = '/Manage/Animal';

                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        });

    // 取消
    $("#btn2").click(
    function () {
        window.location = '/Manage/Animal';
    });

    ko.applyBindings(vm);
});
