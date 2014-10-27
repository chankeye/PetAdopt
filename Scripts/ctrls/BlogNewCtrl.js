var MyViewModel = function () {
    var self = this;

    self.loading = ko.observable(false);

    self.classes = ko.observableArray();
}

$(function () {

    // 取得分類列表
    window.utils.getClassList();

    window.vm = new MyViewModel();
    ko.applyBindings(window.vm);

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
                    AnimalId: $("#animalId").val(),
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
