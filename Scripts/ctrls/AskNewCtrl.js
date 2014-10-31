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
