var MyViewModel = function () {
    var self = this;

    self.loading = ko.observable(false);

    self.classes = ko.observableArray();
    self.areas = ko.observableArray();
}

$(function () {

    // 取得分類列表
    window.utils.getClassList();

    // 取得地區列表
    window.utils.getAreaList();

    window.vm = new MyViewModel();
    ko.applyBindings(window.vm, $("#mainContiner")[0]);

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

            if ($("#selOptionsAreas").val() == "") {
                alert('請選擇地區');
                return;
            }

            $btn.button("loading");

            $.ajax({
                type: 'post',
                url: '/Manage/Help/AddHelp',
                data: {
                    Photo: photo,
                    Title: $("#title").val(),
                    Message: $("#message").val(),
                    Address: $("#address").val(),
                    ClassId: $("#selOptionsClasses").val(),
                    AreaId: $("#selOptionsAreas").val()
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        alert("新增成功");
                        window.location = '/Help';
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        });
});
