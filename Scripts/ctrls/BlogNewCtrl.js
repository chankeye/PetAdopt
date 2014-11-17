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
            valueKey: 'Display',
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
                maxlength: 1000
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
                maxlength: "不得大於1000個字"
            },
            selOptionsClasses: "請選擇分類",
        }
    });

    // 新增Blog
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

            if (oEditor.getData().length > 1000) {
                alert('內容過長，請重新輸入');
                return;
            }

            $btn.button("loading");

            $.ajax({
                type: 'post',
                url: '/Manage/Blog/AddBlog',
                data: {
                    Title: $("#title").val(),
                    Message: oEditor.getData(),
                    AnimalTitle: $("#animalTitle").val(),
                    ClassId: $("#selOptionsClasses").val()
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        alert("新增成功");
                        window.location = '/Blog';
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        });
});
