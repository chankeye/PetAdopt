var MyViewModel = function () {
    var self = this;

    self.loading = ko.observable(false);
    self.responseMessage = ko.observable($.commonLocalization.noRecord);
    self.history = ko.observableArray([]);

    self.removeShelters = function (shelters) {
        if (confirm('確定要刪除？')) {

            if (shelters.IsDisable == true)
                return;

            $.ajax({
                type: 'post',
                url: '/Manage/Shelters/Delete',
                data: {
                    id: shelters.Id,
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        self.shelterslist.remove(shelters);
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        }
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
            url: '/Manage/Shelters/GetSheltersList',
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

    window.vm = new MyViewModel();
    window.vm.loadHistory();
    ko.applyBindings(window.vm);
});
