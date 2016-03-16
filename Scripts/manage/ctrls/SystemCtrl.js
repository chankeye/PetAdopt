function MyViewModel() {
    var self = this;

    // 地區
    self.areas = ko.observableArray();

    self.removeArea = function (area) {
        if (confirm('確定要刪除？')) {
            $.ajax({
                type: 'post',
                url: '/Manage/System/DeleteArea',
                data: {
                    id: area.Id,
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        self.areas.remove(area);
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        }
    }

    // 狀態
    self.statuses = ko.observableArray();

    self.removeStatus = function (status) {
        if (confirm('確定要刪除？')) {
            $.ajax({
                type: 'post',
                url: '/Manage/System/DeleteStatus',
                data: {
                    id: status.Id,
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        self.statuses.remove(status);
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        }
    }

    // 種類
    self.classes = ko.observableArray();

    self.removeClass = function (clas) {
        if (confirm('確定要刪除？')) {
            $.ajax({
                type: 'post',
                url: '/Manage/System/DeleteClass',
                data: {
                    id: clas.Id,
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        self.classes.remove(clas);
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        }
    }
};

$(function () {

    var vm = new MyViewModel();

    // 地區
    $.ajax({
        type: 'post',
        url: '/Manage/System/GetAreaList',
        //async:false,
        success: function (data) {
            vm.areas(data);
        }
    });

    $("#btnAddArea").click(function () {
        if ($("#area-form").valid()) {
            $.ajax({
                type: 'post',
                url: '/Manage/System/AddArea',
                data: {
                    word: $("#ares").val(),
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        vm.areas.push(data.ReturnObject);
                        $("#ares").val('');
                    } else {
                        alert(data.ErrorMessage);
                        $("#ares").val('');
                    }
                }
            });
        }
    });

    // 狀態
    $.ajax({
        type: 'post',
        url: '/Manage/System/GetStatusList',
        success: function (data) {
            vm.statuses(data);
        }
    });

    $("#btnAddStatus").click(function () {
        if ($("#status-form").valid()) {
            $.ajax({
                type: 'post',
                url: '/Manage/System/AddStatus',
                data: {
                    word: $("#status").val(),
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        vm.statuses.push(data.ReturnObject);
                        $("#status").val('');
                    } else {
                        alert(data.ErrorMessage);
                        $("#status").val('');
                    }
                }
            });
        }
    });

    // 種類
    $.ajax({
        type: 'post',
        url: '/Manage/System/GetClassList',
        success: function (data) {
            vm.classes(data);
        }
    });

    $("#btnAddClass").click(function () {
        if ($("#class-form").valid()) {
            $.ajax({
                type: 'post',
                url: '/Manage/System/AddClass',
                data: {
                    word: $("#class").val(),
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        vm.classes.push(data.ReturnObject);
                        $("#class").val('');
                    } else {
                        alert(data.ErrorMessage);
                        $("#class").val('');
                    }
                }
            });
        }
    });

    // init
    $("#btnInitParameter").click(function () {
        $.ajax({
            type: 'post',
            url: '/Manage/System/InitParameter',
            success: function (data) {
                alert("初始化完成");
                location.reload();
            }
        });
    });

    // init
    $("#btnInitAnimal").click(function () {
        $.ajax({
            type: 'post',
            url: '/Manage/System/InitAnimal',
            success: function (count) {
                alert("新增了" + count + "筆資料");
                location.reload();
            }
        });
    });

    // delete animal
    $("#btnDeleteAnimal").click(function () {
        $.ajax({
            type: 'post',
            url: '/Manage/System/DeleteAnimals',
            success: function (count) {
                alert("刪除了" + count + "筆資料");
                location.reload();
            }
        });
    });

    ko.applyBindings(vm);
});
