var currentPageIndex = 0;
var pagingbarItemLimit = 10;
var PaginationModel = function () {

    var self = this;

    self.pagination = function (page, count, limit) {
        // make the pagination buttons
        $(".pagination").hide();
        $(".pagination .pagi").empty(); //empty li first

        if (count > 0) {
            $(".pagination").show();
            var btnCount = Math.ceil(count / limit);
            for (var i = 0; i < btnCount; i++) {
                //if (IsPageNeededToShow(i)) {
                if (page === (i + 1)) {
                    $(".pagination .pagi").append('<li><a href="#" class="active">' + (i + 1) + '</a></li>');
                } else {
                    $(".pagination .pagi").append('<li><a href="#">' + (i + 1) + '</a></li>');
                }
                //}
            }
        }
    };
};

$(function () {
    $(".pagination").hide();
    // bind the pagination click event
    $(".pagination .pagi").on("click", "a", function (e) {
        e.preventDefault();
        var page = parseInt($(this).html());
        var current = parseInt($(".pagination .pagi .active").html());
        if (page !== current)
            currentPageIndex = page;
        window.vm.loadHistory(page, 10, $("#search").val(), !$("#checkAll").is(':checked'), $("#selOptionsAreas").val(), $("#selOptionsClasses").val(), $("#selOptionsStatuses").val());
    });
    $(".pagination .prev").click(function (e) {
        e.preventDefault();
        //get current active index
        var current = parseInt($(".pagination .pagi .active").html());
        if (current - 1 > 0) {
            currentPageIndex = current - 1;
            window.vm.loadHistory(current - 1, 10, $("#search").val(), !$("#checkAll").is(':checked'), $("#selOptionsAreas").val(), $("#selOptionsClasses").val(), $("#selOptionsStatuses").val());
        }
    });
    $(".pagination .next").click(function (e) {
        e.preventDefault();
        //get current active index
        var current = parseInt($(".pagination .pagi .active").html());
        if (current < $(".pagination .pagi li").length) {
            currentPageIndex = current + 1;
            window.vm.loadHistory(current + 1, 10, $("#search").val(), !$("#checkAll").is(':checked'), $("#selOptionsAreas").val(), $("#selOptionsClasses").val(), $("#selOptionsStatuses").val());
        }
    });
});

var IsPageNeededToShow = function (dataPage) {
    //debugger;
    if (dataPage >= currentPageIndex && dataPage < (currentPageIndex + pagingbarItemLimit)) { return true; }
    return false;
};