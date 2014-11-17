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
            window.location = '/Blog';
    }

    // 取得文章
    var photo;
    function init() {
        $.ajax({
            type: 'post',
            url: '/Manage/Blog/EditInit',
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
                    $("#animalId").val(data.ReturnObject.AnimalId);
                } else {
                    alert(data.ErrorMessage);
                    if (history.length > 1)
                        history.back();
                    else
                        window.location = '/Blog';
                }
            }
        });
    }

    // 取得分類列表
    window.utils.getClassList()
        .done(init());

    window.vm = new MyViewModel();
    ko.applyBindings(window.vm, $("#mainContiner")[0]);

    // 自動完成
    var timestamp = new Date().getTime();
    $("#animalId")
        .typeahead({
            remote: {
                url: '/Manage/Animal/GetAnimalSuggestion',
                replace: function (url, uriEncodedQuery) {
                    return url + "?title=" + uriEncodedQuery + "&_=" + timestamp;
                }
            },
            valueKey: 'Value',
            template: '<p>{{Display}}</p>',
            engine: Hogan,
            limit: 10
        });

    $("#commentForm").validate({
        rules: {
            title: {
                required: true,
                maxlength: 50
            },
            content: {
                required: true,
                maxlength: 10000
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
            content: {
                required: "請輸入內容",
                maxlength: "不得大於10000個字"
            },
            selOptionsClasses: "請選擇分類",
        }
    });

    // 修改文章
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
                url: '/Manage/Blog/EditBlog',
                data: {
                    id: window.id,
                    Photo: photo,
                    Title: $("#title").val(),
                    Message: oEditor.getData(),
                    AnimalId: $("#animalId").val(),
                    ClassId: $("#selOptionsClasses").val()
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        alert("修改完成");
                        if (history.length > 1)
                            history.back();
                        else
                            window.location = '/Blog';
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
            window.location = '/Blog';
    });
});
