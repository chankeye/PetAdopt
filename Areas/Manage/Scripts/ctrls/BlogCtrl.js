function MyViewModel() {
    var self = this;

    self.bloglist = ko.observableArray();

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
                        self.bloglist.remove(blog);
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

    // 取得問與答列表
    $.ajax({
        type: 'post',
        url: '/Manage/Blog/GetBlogList',
        success: function (data) {
            vm.bloglist(data);
        }
    });

    ko.applyBindings(vm);
});
