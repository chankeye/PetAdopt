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
    ko.applyBindings(window.vm);

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
                        alert("新增成功");
                        window.location = '/Animal';
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        });
});