function MyViewModel() {
    var self = this;

    self.classes = ko.observableArray();
};

$(function () {

    // 取得分類列表
    $.ajax({
        type: 'post',
        url: '/Manage/System/GetClassList',
        success: function (classes) {
            vm.classes(classes);
            vm.classes.unshift({
                "Word": "請選擇",
                "Id": ""
            });
            $("#selOptions option:first").attr("selected", true);
        }
    });

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
        window.location = '/Manage/Blog';

    // 取得文章
    var photo;
    $.ajax({
        type: 'post',
        url: '/Manage/Blog/EditInit',
        data: {
            id: urlParams["id"]
        },
        success: function (data) {
            if (data.IsSuccess) {
                $("#title").val(data.ReturnObject.Title);
                $("#selOptions").children().each(function () {
                    if ($(this).val() == data.ReturnObject.ClassId) {
                        //jQuery給法
                        $(this).attr("selected", true); //或是給"selected"也可

                        //javascript給法
                        this.selected = true;
                    }
                });
                $("#content").val(data.ReturnObject.Message);
                $("#animalId").val(data.ReturnObject.AnimalId);
            } else {
                alert(data.ErrorMessage);
                window.location = '/Manage/Blog';
            }
        }
    });

    // 修改文章
    $("#btn1").click(
        function () {
            var $btn = $("#btn1");

            if ($("#commentForm").valid() == false) {
                return;
            }

            if ($("#selOptions").val() == "") {
                alert('請選擇分類');
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
                url: '/Manage/Blog/EditBlog',
                data: {
                    id: urlParams["id"],
                    Photo: photo,
                    Title: $("#title").val(),
                    Message: oEditor.getData(),
                    AnimalId: $("#animalId").val(),
                    ClassId: $("#selOptions").val()
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        $("#title").val('');
                        oEditor.setData('');
                        $("#animalId").val('');
                        $("#selOptions option:first").attr("selected", true);

                        alert("修改完成");
                        window.location = '/Manage/Blog';
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        });

    // 取消
    $("#btn2").click(
    function () {
        window.location = '/Manage/Blog';
    });

    ko.applyBindings(vm);
});
