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
            url: '/Manage/Knowledge/GetKnowledgeList',
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

    // 新增知識
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
                url: '/Manage/Knowledge/AddKnowledge',
                data: {
                    Title: $("#title").val(),
                    Message: $("#message").val(),
                    ClassId: $("#selOptionsClasses").val()
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        //window.vm.history.push(data.ReturnObject);
                        window.vm.loadHistory();
                        $("#title").val('');
                        $("#message").val(''),
                        $("#selOptionsClasses option:first").attr("selected", true);

                        alert("新增成功");
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        });

    // 查詢
    $("#btn3").click(
        window.utils.searchList($("#btn3"))
    );
});
