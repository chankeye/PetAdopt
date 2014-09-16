$(function () {

    function MyViewModel() {
        var self = this;

        self.activities = ko.observableArray();
        self.areas = ko.observableArray();

        self.removeActivity = function (activity) {
            if (confirm('確定要刪除？')) {
                $.ajax({
                    type: 'post',
                    url: '/Manage/Activity/Delete',
                    data: {
                        id: activity.Id,
                    },
                    success: function (data) {
                        if (data.IsSuccess) {
                            self.activities.remove(activity);
                        } else {
                            alert(data.ErrorMessage);
                        }
                    }
                });
            }
        }

        self.editActivity = function (activity) {
            window.location = "/Manage/Activity/Edit?id=" + activity.Id;
        }
    };

    var vm = new MyViewModel();

    // 取得消息列表
    $.ajax({
        type: 'post',
        url: '/Manage/Activity/GetActivities',
        success: function (data) {
            vm.activities(data);
        }
    });

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

    // 新增消息
    $("#btn1").click(
        function () {
            var $btn = $("#btn1");
            var $uploadfile = $(".table-striped .name a");
            var poto = $uploadfile.text();

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
                url: '/Manage/Activity/AddActivity',
                data: {
                    Poto: poto,
                    Title: $("#title").val(),
                    Message: oEditor.getData(),
                    Address: $("#address").val(),
                    AreaId: $("#selOptions").val()
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        vm.activities.push(data.ReturnObject);
                        $("#title").val('');
                        oEditor.setData('');
                        $("#address").val('');
                        $("#selOptions option:first").attr("selected", true);

                        alert("新增成功");
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        });

    // 取消
    $("#btn2").click(
    function () {
        $("#title").val('');
        CKEDITOR.instances.content.setData('');
        $("#address").val('');
        $("#selOptions option:first").attr("selected", true);
    });

    ko.applyBindings(vm);
});
