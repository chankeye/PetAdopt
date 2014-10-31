var MyViewModel = function () {
    var self = this;

    self.loading = ko.observable(false);

    self.areas = ko.observableArray();
};

$(function () {

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

            var oEditor = CKEDITOR.instances.content;
            if (oEditor.getData() == '') {
                alert('請輸入內容');
                return;
            }

            if (oEditor.getData().length > 1000) {
                alert('內容過長，請重新輸入');
                return;
            }

            $btn.button("loading");

            $.ajax({
                type: 'post',
                url: '/Manage/News/AddNews',
                data: {
                    Photo: photo,
                    Title: $("#title").val(),
                    Message: oEditor.getData(),
                    Url: $("#source").val(),
                    AreaId: $("#selOptionsAreas").val()
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        alert("新增成功");
                        window.location = '/News';
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        });
});
