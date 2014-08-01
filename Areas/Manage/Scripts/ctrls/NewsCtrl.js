function MyViewModel() {
    var self = this;

    self.newslist = ko.observableArray();
    self.areas = ko.observableArray();

    self.removeNews = function (news) {
        if (confirm('確定要刪除？')) {
            $.ajax({
                type: 'post',
                url: '/Manage/News/Delete',
                data: {
                    id: news.Id,
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        self.newslist.remove(news);
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        }
    }

    self.editNews = function (news) {
        window.location = "/Manage/News/Edit?id="+news.Id+"";
        //$.ajax({
        //    type: 'get',
        //    url: '/Manage/News/Edit',
        //    data: {
        //        id: news.Id
        //    }
        //});
    }
};

$(function () {

    var vm = new MyViewModel();

    // 取得消息列表
    $.ajax({
        type: 'post',
        url: '/Manage/News/GetNewsList',
        success: function (data) {
            vm.newslist(data);
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
                url: '/Manage/News/AddNews',
                data: {
                    Poto: poto,
                    Title: $("#title").val(),
                    Message: oEditor.getData(),
                    Url: $("#source").val(),
                    AreaId: $("#selOptions").val()
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        vm.newslist.push(data.ReturnObject);
                        $("#title").val('');
                        oEditor.setData('');
                        $("#source").val('');
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
        $("#source").val('');
        $("#selOptions option:first").attr("selected", true);
    });

    ko.applyBindings(vm);
});
