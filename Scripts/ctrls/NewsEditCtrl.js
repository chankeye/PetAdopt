function MyViewModel() {
    var self = this;

    self.loading = ko.observable(false);
    self.responseMessage = ko.observable($.commonLocalization.noRecord);
    self.history = ko.observableArray();

    self.areas = ko.observableArray();
};


$(function () {

    // 沒有輸入id直接導回
    window.id = window.utils.urlParams("id");
    if (window.id == null) {
        if (history.length > 1)
            history.back();
        else
            window.location = '/News';
    }

    // 取得最新消息
    var photo;
    function init() {
        $.ajax({
            type: 'post',
            url: '/Manage/News/EditInit',
            data: {
                id: window.id
            },
            success: function (data) {
                if (data.IsSuccess) {
                    photo = data.ReturnObject.Photo;
                    if (photo != null) {
                        $('#coverPhoto').attr('src', "../../../../Content/uploads/" + photo);
                    }
                    $("#title").val(data.ReturnObject.Title);
                    $("#selOptionsAreas").children().each(function () {
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
                    if (history.length > 1)
                        history.back();
                    else
                        window.location = '/News';
                }
            }
        });
    }

    // 取得地區列表
    window.utils.getAreaList()
        .done(init());


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
            },
            source: {
                url: true,
                maxlength: 100
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
            },
            source: {
                url: "請輸入正確格式，例:http://loveadopt.somee.com",
                maxlength: "不得大於100個字"
            }
        }
    });

    // 修改消息
    $("#btn1").click(
        function () {
            var $btn = $("#btn1");
            var $uploadfile = $(".table-striped .name a");
            if ($uploadfile.text() != "") {
                $.ajax({
                    type: 'post',
                    url: '/Manage/System/DeletePhoto',
                    data: {
                        Photo: photo
                    }
                });

                photo = $uploadfile.text();
            }

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
                    id: window.id,
                    Photo: photo,
                    Title: $("#title").val(),
                    Message: oEditor.getData(),
                    Url: $("#source").val(),
                    AreaId: $("#selOptionsAreas").val()
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        alert("修改完成");
                        if (history.length > 1)
                            history.back();
                        else
                            window.location = '/News';
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        });

    // 取消
    $("#btn2").click(
    function () {
        if (history.length > 1)
            history.back();
        else
            window.location = '/News';
    });
});
