$(function () {

    function MyViewModel() {
        var self = this;

        self.animallist = ko.observableArray();

        self.removeAnimal = function (animal) {
            if (confirm('確定要刪除？')) {

                if (animal.IsDisable == true)
                    return;

                $.ajax({
                    type: 'post',
                    url: '/Manage/Animal/Delete',
                    data: {
                        id: animal.Id,
                    },
                    success: function (data) {
                        if (data.IsSuccess) {
                            self.animallist.remove(animal);
                        } else {
                            alert(data.ErrorMessage);
                        }
                    }
                });
            }
        }
    }

    var vm = new MyViewModel();

    // 取得動物列表
    $.ajax({
        type: 'post',
        url: '/Manage/Animal/GetAnimalList',
        success: function (data) {
            vm.animallist(data);
        }
    });

    ko.applyBindings(vm);
});
