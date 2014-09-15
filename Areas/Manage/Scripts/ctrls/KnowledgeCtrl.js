function MyViewModel() {
    var self = this;

    self.knowledgelist = ko.observableArray();

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
                        self.knowledgelist.remove(knowledge);
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        }
    }
}

$(function () {

    var vm = new MyViewModel();

    // 取得知識列表
    $.ajax({
        type: 'post',
        url: '/Manage/Knowledge/GetKnowledgeList',
        success: function (data) {
            vm.knowledgelist(data);
        }
    });

    ko.applyBindings(vm);
});
