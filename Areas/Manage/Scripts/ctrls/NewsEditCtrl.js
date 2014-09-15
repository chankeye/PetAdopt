function MyViewModel() {
    var self = this;

    self.areas = ko.observableArray();
};


$(function () {

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
        window.location = '/Manage/News';

    // 取得最新消息
    $.ajax({
        type: 'post',
        url: '/Manage/News/EditInit',
        data: {
            id: urlParams["id"]
        },
        success: function (data) {
            if (data.IsSuccess) {
                $("#title").val(data.ReturnObject.Title);
                $("#selOptions").children().each(function () {
                    if ($(this).val() == data.ReturnObject.AreaId) {
                        //jQuery給法
                        $(this).attr("selected", true); //或是給"selected"也可

                        //javascript給法
                        this.selected = true;
                    }
                });
                $("#content").val(data.ReturnObject.Message);
                $("#source").val(data.ReturnObject.Url);
            } else {
                alert(data.ErrorMessage);
            }
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

    // 修改消息
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
                url: '/Manage/News/EditNews',
                data: {
                    id: urlParams["id"],
                    Poto: poto,
                    Title: $("#title").val(),
                    Message: oEditor.getData(),
                    Url: $("#source").val(),
                    AreaId: $("#selOptions").val()
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        $("#title").val('');
                        oEditor.setData('');
                        $("#source").val('');
                        $("#selOptions option:first").attr("selected", true);

                        alert("修改完成");
                        window.location = '/Manage/News/Index';
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        });

    // 取消
    $("#btn2").click(
    function () {
        window.location = '/Manage/News/Index';
    });

    ko.applyBindings(vm);
});
