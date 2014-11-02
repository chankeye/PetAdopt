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
            window.location = '/Activity';
    }

    // 取得最新活動
    var photo;
    function init() {
        $.ajax({
            type: 'post',
            url: '/Manage/Activity/EditInit',
            data: {
                id: window.id
            },
            success: function (data) {
                if (data.IsSuccess) {
                    photo = data.ReturnObject.Photo;
                    if (photo != null) {
                        $('#coverPhoto').attr('src', "../../Content/uploads/" + photo);
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
                    $("#address").val(data.ReturnObject.Address);
                } else {
                    alert(data.ErrorMessage);
                    if (history.length > 1)
                        history.back();
                    else
                        window.location = '/Activity';
                }
            }
        });
    }

    // 取得地區列表，並將資料填入
    window.utils.getAreaList().done(init());

    window.vm = new MyViewModel();
    ko.applyBindings(window.vm, $("#mainContiner")[0]);

    // 修改活動
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
                url: '/Manage/Activity/EditActivity',
                data: {
                    id: window.id,
                    Photo: photo,
                    Title: $("#title").val(),
                    Message: oEditor.getData(),
                    Address: $("#address").val(),
                    AreaId: $("#selOptionsAreas").val()
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        alert("修改完成");
                        if (history.length > 1)
                            history.back();
                        else
                            window.location = '/Activity';
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
            window.location = '/Activity';
    });
});
