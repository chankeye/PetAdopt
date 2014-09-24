function MyViewModel() {
    var self = this;

    self.areas = ko.observableArray();
};

$(function () {

    // 取得地區列表
    $.ajax({
        type: 'post',
        url: '/Manage/System/GetAreaList',
        success: function (area) {
            vm.areas(area);
            vm.areas.unshift({
                "Word": "請選擇",
                "Id": ""
            });
            $("#selOptions option:first").attr("selected", true);
        }
    });

    var vm = new MyViewModel();

    var urlParams = {};
    (function () {
        var e,
            a = /\+/g,  // Regex for replacing addition symbol with a space
            r = /([^&=]+)=?([^&]*)/g,
            d = function (s) { return decodeURIComponent(s.replace(a, " ")); },
            q = window.location.search.substring(1);
        while (e = r.exec(q)) {
            urlParams[d(e[1])] = d(e[2]);
        }
    })();

    // 沒有輸入id直接導回
    if (urlParams["id"] == null)
        window.location = '/Manage/Shelters';

    // 取得收容所資訊
    var photo;
    $.ajax({
        type: 'post',
        url: '/Manage/Shelters/EditInit',
        data: {
            id: urlParams["id"]
        },
        success: function (data) {
            if (data.IsSuccess) {
                photo = data.ReturnObject.Photo;
                $("#name").val(data.ReturnObject.Name);
                $("#selOptions").children().each(function () {
                    if ($(this).val() == data.ReturnObject.AreaId) {
                        //jQuery給法
                        $(this).attr("selected", true); //或是給"selected"也可

                        //javascript給法
                        this.selected = true;
                    }
                });
                $("#introduction").val(data.ReturnObject.Introduction);
                $("#address").val(data.ReturnObject.Address);
                $("#phone").val(data.ReturnObject.Phone);
                $("#source").val(data.ReturnObject.url);
            } else {
                alert(data.ErrorMessage);
                window.location = '/Manage/Shelters';
            }
        }
    });

    // 修改收容所資訊
    $("#btn1").click(
        function () {
            var $btn = $("#btn1");
            var $uploadfile = $(".table-striped .name a");
            if ($uploadfile.text() != "") {
                $.ajax({
                    type: 'post',
                    url: '/Manage/System/DeletePhoto',
                    data: {
                        Photo: photo
                    }
                });

                photo = $uploadfile.text();
            }

            if ($("#commentForm").valid() == false) {
                return;
            }

            if ($("#selOptions").val() == "") {
                alert('請選擇地區');
                return;
            }

            $btn.button("loading");

            $.ajax({
                type: 'post',
                url: '/Manage/Shelters/EditShelters',
                data: {
                    id: urlParams["id"],
                    Photo: photo,
                    Name: $("#name").val(),
                    Introduction: $("#introduction").val(),
                    Address: $("#address").val(),
                    Url: $("#source").val(),
                    Phone: $("#phone").val(),
                    AreaId: $("#selOptions").val(),
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        $("#name").val('');
                        $("#introduction").val('');
                        $("#address").val('');
                        $("#source").val('');
                        $("#phone").val('');
                        $("#selOptions option:first").attr("selected", true);

                        alert("修改完成");
                        window.location = '/Manage/Shelters';
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        });

    // 取消
    $("#btn2").click(
    function () {
        window.location = '/Manage/Shelters';
    });

    ko.applyBindings(vm);
});
