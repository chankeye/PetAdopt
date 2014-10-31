var MyViewModel = function () {
    var self = this;

    self.loading = ko.observable(false);

    self.areas = ko.observableArray();
}

$(function () {

    // 取得地區列表
    window.utils.getAreaList();

    window.vm = new MyViewModel();
    ko.applyBindings(window.vm, $("#mainContiner")[0]);

    // 新增
    $("#btn1").click(
        function () {
            var $btn = $("#btn1");
            var $uploadfile = $(".table-striped .name a");
            var photo = $uploadfile.text();

            if ($("#commentForm").valid() == false) {
                return;
            }

            if ($("#selOptionsAreas").val() == "") {
                alert('請選擇地區');
                return;
            }

            $btn.button("loading");

            $.ajax({
                type: 'post',
                url: '/Manage/Shelters/AddShelters',
                data: {
                    Photo: photo,
                    Name: $("#name").val(),
                    Introduction: $("#introduction").val(),
                    Phone: $("#phone").val(),
                    Address: $("#address").val(),
                    AreaId: $("#selOptions").val(),
                    Url: $("#source").val()
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        alert("新增成功");
                        window.location = '/Shelters';
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        });
});
