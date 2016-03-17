var CarouselModel = function () {
    var self = this;

    self.imgList = ko.observableArray();

    self.detail = function (data) {
        window.location = data.Link;
    }

    self.getDyingAnimal = function () {
        $.ajax({
            type: 'post',
            url: '/Animal/GetCarouselList',
            data: {
            }
        }).done(function (response) {
            for (var i = 0; i < response.length; i++) {
                var date = new Date(response[i].EndDate);
                response[i].EndDate = date.getFullYear() + '/' + (date.getMonth() + 1) + '/' + date.getDate();
            }
            self.imgList(response);
        });
    };
}

$(function() {
    window.carousel = new CarouselModel();
    window.carousel.getDyingAnimal();
    ko.applyBindings(window.carousel, $(".carousel-inner")[0]);
});