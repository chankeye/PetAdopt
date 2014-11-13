var MyViewModel = function () {
    var self = this;

    self.loading = ko.observable(false);

    self.areas = ko.observableArray();
    self.classes = ko.observableArray();
    self.statuses = ko.observableArray();
}

$(function () {
    // 取得地區列表
    window.utils.getAreaList();

    // 取得分類列表
    window.utils.getClassList();

    // 取得狀態列表
    window.utils.getStatusList();

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
    $("#shelters").keyup(
        function () {
            if ($("#shelters").val() != "") {
                $("#selOptionsAreas").attr('disabled', true);
                $("#phone").attr('disabled', true);
                $("#address").attr('disabled', true);
            } else {
                $("#selOptionsAreas").attr('disabled', false);
                $("#phone").attr('disabled', false);
                $("#address").attr('disabled', false);
            }
        });

    $("#commentForm").validate({
        rules: {
            title: {
                required: true,
                maxlength: 50
            },
            introduction: {
                required: true,
                maxlength: 200
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
                maxlength: "不得大於200個字"
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

    // 新增
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
                        alert("新增成功");
                        window.location = '/Animal';
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        });
});