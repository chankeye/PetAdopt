function MyViewModel() {
    var self = this;

    self.loading = ko.observable(false);
    self.responseMessage = ko.observable($.commonLocalization.noRecord);
    self.history = ko.observableArray();

    self.areas = ko.observableArray();
};

$(function () {

    // 沒有輸入id直接導回
    window.id = window.utils.urlParams("id");
    if (window.id == null) {
        if (history.length > 1)
            history.back();
        else
            window.location = '/Shelters';
    }

    // 取得收容所資訊
    var photo;
    function init() {
        $.ajax({
            type: 'post',
            url: '/Manage/Shelters/EditInit',
            data: {
                id: window.id
            },
            success: function (data) {
                if (data.IsSuccess) {
                    photo = data.ReturnObject.Photo;
                    if (photo != null) {
                        $('#coverPhoto').attr('src', "../../../../Content/uploads/" + photo);
                    }
                    $("#name").val(data.ReturnObject.Name);
                    $("#selOptionsAreas").children().each(function () {
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
                    $("#source").val(data.ReturnObject.Url);
                } else {
                    alert(data.ErrorMessage);
                    if (history.length > 1)
                        history.back();
                    else
                        window.location = '/Shelters';
                }
            }
        });
    }

    // 初始化
    window.utils.getAreaList()
        .done(init());

    window.vm = new MyViewModel();
    ko.applyBindings(window.vm, $("#mainContiner")[0]);

    $("#commentForm").validate({
        rules: {
            title: {
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
            title: {
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

            if ($("#selOptionsAreas").val() == "") {
                alert('請選擇地區');
                return;
            }

            $btn.button("loading");

            $.ajax({
                type: 'post',
                url: '/Manage/Shelters/EditShelters',
                data: {
                    id: window.id,
                    Photo: photo,
                    Name: $("#name").val(),
                    Introduction: $("#introduction").val(),
                    Address: $("#address").val(),
                    Url: $("#source").val(),
                    Phone: $("#phone").val(),
                    AreaId: $("#selOptionsAreas").val(),
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        alert("修改完成");
                        if (history.length > 1)
                            history.back();
                        else
                            window.location = '/Shelters';
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        });

    // 取消
    $("#btn2").click(
    function () {
        if (history.length > 1)
            history.back();
        else
            window.location = '/Shelters';
    });
});
