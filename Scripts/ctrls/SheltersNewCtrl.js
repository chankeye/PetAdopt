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

    $("#commentForm").validate({
        rules: {
            name: {
                required: true,
                maxlength: 50
            },
            introduction: {
                required: true,
                maxlength: 200
            },
            phone: {
                required: true,
                maxlength: 10
            },
            selOptionsAreas: {
                required: true
            },
            address: {
                required: true,
                maxlength: 30
            },
            source: {
                url: true,
                maxlength: 100
            }
        },
        messages: {
            name: {
                required: "請輸入標題",
                maxlength: "不得大於50個字"
            },
            introduction: {
                required: "請輸入內容",
                maxlength: "不得大於200個字"
            },
            phone: {
                required: "請輸入電話",
                maxlength: "不得大於10個字"
            },
            selOptionsAreas: "請選擇地區",
            address: {
                required: "請輸入地址",
                maxlength: "不得大於30個字"
            },
            source: {
                url: "請輸入正確格式，例:http://loveadopt.somee.com",
                maxlength: "不得大於100個字"
            }
        }
    });

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
                    AreaId: $("#selOptionsAreas").val(),
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
