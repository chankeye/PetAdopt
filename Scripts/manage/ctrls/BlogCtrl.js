var MyViewModel = function () {
    var self = this;

    self.loading = ko.observable(false);
    self.responseMessage = ko.observable($.commonLocalization.noRecord);
    self.history = ko.observableArray();

    self.classes = ko.observableArray();

    self.removeBlog = function (blog) {
        if (confirm('確定要刪除？')) {

            if (blog.IsDisable == true)
                return;

            $.ajax({
                type: 'post',
                url: '/Manage/Blog/Delete',
                data: {
                    id: blog.Id,
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        self.history.remove(blog);
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        }
    }

    self.editBlog = function (blog) {
        window.location = "/Manage/Blog/Edit?id=" + blog.Id;
    }

    //Add PaginationModel
    //from pagination.js
    ko.utils.extend(self, new PaginationModel());

    self.loadHistory = function (page, take, query, isLike) {
        self.responseMessage($.commonLocalization.loading);
        self.loading(true);
        self.history.removeAll();
        self.pagination(0, 0, 0);
        page = page || 1; // if page didn't send
        take = take || 10;
        query = query || "";
        if (isLike == null)
            isLike = true;
        $.ajax({
            type: 'post',
            url: '/Manage/Blog/GetBlogList',
            data: {
                page: page,
                take: take,
                query: query,
                isLike: isLike
            }
        }).done(function (response) {
            self.responseMessage('');
            self.history(response.List);
            self.pagination(page, response.Count, take);

            if (response.Count == 0)
                self.responseMessage($.commonLocalization.noRecord);

        }).always(function () {
            self.loading(false);
        });
    };
}

$(function () {

    // 取得分類列表
    window.utils.getClassList();

    window.vm = new MyViewModel();
    window.vm.loadHistory();
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
            valueKey: 'Display',
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

            if (oEditor.getData().length > 10000) {
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
                        //vm.history.push(data.ReturnObject);
                        window.vm.loadHistory();
                        $("#title").val('');
                        oEditor.setData('');
                        $("#animalTitle").val('');
                        $("#selOptionsClasses option:first").attr("selected", true);

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
            CKEDITOR.instances.content.setData('');
        });

    // 查詢
    $("#btn3").click(function () {
        var $btn = $("#btn3");

        $btn.button("loading");
        window.vm.loadHistory(1, 10, $("#search").val(), !$("#checkAll").is(':checked'));
        $btn.button("reset");
    });
});
