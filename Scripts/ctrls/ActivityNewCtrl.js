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

    $("#commentForm").validate({
        rules: {
            title: {
                required: true,
                maxlength: 50
            },
            content: {
                required: true,
                maxlength: 1000
            }
        },
        messages: {
            title: {
                required: "請輸入標題",
                maxlength: "不得大於50個字"
            },
            content: {
                required: "請輸入內容",
                maxlength: "不得大於1000個字"
            }
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

            var oEditor = CKEDITOR.instances.content;
            if (oEditor.getData() == '') {
                alert('請輸入內容');
                return;
            }

            if (oEditor.getData().length > 10000) {
                alert('內容過長，請重新輸入');
                return;
            }

            $btn.button("loading");

            $.ajax({
                type: 'post',
                url: '/Manage/Activity/AddActivity',
                data: {
                    Photo: photo,
                    Title: $("#title").val(),
                    Message: oEditor.getData(),
                    Address: $("#address").val(),
                    AreaId: $("#selOptionsAreas").val()
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        alert("新增成功");
                        window.location = '/Activity';
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        });
});
