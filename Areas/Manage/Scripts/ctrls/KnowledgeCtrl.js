var MyViewModel = function () {
    var self = this;

    self.loading = ko.observable(false);
    self.responseMessage = ko.observable($.commonLocalization.noRecord);
    self.history = ko.observableArray();

    self.classes = ko.observableArray();

    self.removeKnowledge = function (knowledge) {
        if (confirm('確定要刪除？')) {

            if (knowledge.IsDisable == true)
                return;

            $.ajax({
                type: 'post',
                url: '/Manage/Knowledge/Delete',
                data: {
                    id: knowledge.Id,
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        self.history.remove(knowledge);
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        }
    }

    self.editKnowledge = function (knowledge) {
        window.location = "/Manage/Knowledge/Edit?id=" + knowledge.Id;
    }

    //Add PaginationModel
    //from pagination.js
    ko.utils.extend(self, new PaginationModel());

    self.loadHistory = function (page) {
        self.responseMessage($.commonLocalization.loading);
        self.loading(true);
        self.history.removeAll();
        self.pagination(0, 0, 0);
        var limit = 10;
        page = page || 1; // if page didn't send
        $.ajax({
            type: 'post',
            url: '/Manage/Knowledge/GetKnowledgeList',
            data: {
                page: page
            }
        }).done(function (response) {
            self.responseMessage('');
            self.history(response.List);
            self.pagination(page, response.Count, limit);

            if (response.Count == 0)
                self.responseMessage($.commonLocalization.noRecord);

        }).always(function () {
            self.loading(false);
        });
    };
}

$(function () {

    // 取得分類列表
    $.ajax({
        type: 'post',
        url: '/Manage/System/GetClassList',
        success: function (classes) {
            window.vm.classes(classes);
            window.vm.classes.unshift({
                "Word": "請選擇",
                "Id": ""
            });
            $("#selOptions option:first").attr("selected", true);
        }
    });

    window.vm = new MyViewModel();
    window.vm.loadHistory();
    ko.applyBindings(window.vm);

    // 新增知識
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

            $btn.button("loading");

            $.ajax({
                type: 'post',
                url: '/Manage/Knowledge/AddKnowledge',
                data: {
                    Title: $("#title").val(),
                    Message: $("#message").val(),
                    ClassId: $("#selOptions").val()
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        //window.vm.history.push(data.ReturnObject);
                        window.vm.loadHistory();
                        $("#title").val('');
                        $("#message").val(''),
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
        $("#message").val('');
        $("#selOptions option:first").attr("selected", true);
    });
});
