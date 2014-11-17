var MyViewModel = function () {
    var self = this;

    self.loading = ko.observable(false);

    self.classes = ko.observableArray();
}

$(function () {

    // 取得分類列表
    window.utils.getClassList();

    window.vm = new MyViewModel();
    ko.applyBindings(window.vm, $("#mainContiner")[0]);

    $("#commentForm").validate({
        rules: {
            title: {
                required: true,
                maxlength: 50
            },
            message: {
                required: true,
                maxlength: 200
            },
            selOptionsClasses: {
                required: true
            }
        },
        messages: {
            title: {
                required: "請輸入標題",
                maxlength: "不得大於50個字"
            },
            message: {
                required: "請輸入內容",
                maxlength: "不得大於200個字"
            },
            selOptionsClasses: "請選擇分類",
        }
    });

    // 新增
    $("#btn1").click(
        function () {
            var $btn = $("#btn1");

            if ($("#commentForm").valid() == false) {
                return;
            }

            if ($("#selOptionsClasses").val() == "") {
                alert('請選擇分類');
                return;
            }

            $btn.button("loading");

            $.ajax({
                type: 'post',
                url: '/Manage/Ask/AddAsk',
                data: {
                    Title: $("#title").val(),
                    Message: $("#message").val(),
                    ClassId: $("#selOptionsClasses").val()
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        alert("新增成功");
                        window.location = '/Ask';
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        });
});
