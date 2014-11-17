function MyViewModel() {
    var self = this;

    self.loading = ko.observable(false);
    self.responseMessage = ko.observable($.commonLocalization.noRecord);
    self.history = ko.observableArray();

    self.classes = ko.observableArray();
};


$(function () {

    // 沒有輸入id直接導回
    window.id = window.utils.urlParams("id");
    if (window.id == null) {
        if (history.length > 1)
            history.back();
        else
            window.location = '/Knowledge';
    }

    // 取得問與答
    function init() {
        $.ajax({
            type: 'post',
            url: '/Manage/Knowledge/EditInit',
            data: {
                id: window.id
            },
            success: function (data) {
                if (data.IsSuccess) {
                    $("#title").val(data.ReturnObject.Title);
                    $("#selOptionsClasses").children().each(function () {
                        if ($(this).val() == data.ReturnObject.ClassId) {
                            //jQuery給法
                            $(this).attr("selected", true); //或是給"selected"也可

                            //javascript給法
                            this.selected = true;
                        }
                    });
                    $("#content").val(data.ReturnObject.Message);
                } else {
                    alert(data.ErrorMessage);
                    if (history.length > 1)
                        history.back();
                    else
                        window.location = '/Knowledge';
                }
            }
        });
    }

    // 取得分類列表
    window.utils.getClassList()
        .done(init());

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
                maxlength: 500
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
                maxlength: "不得大於500個字"
            },
            selOptionsClasses: "請選擇分類"
        }
    });

    // 修改問與答
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
                url: '/Manage/Knowledge/EditKnowledge',
                data: {
                    id: window.id,
                    Title: $("#title").val(),
                    Message: $("#content").val(),
                    ClassId: $("#selOptionsClasses").val()
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        alert("修改完成");
                        if (history.length > 1)
                            history.back();
                        else
                            window.location = '/Knowledge';
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
            window.location = '/Knowledge';
    });
});
