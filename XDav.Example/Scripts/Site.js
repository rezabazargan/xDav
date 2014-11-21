$(function () {
    $('.fileLink').click(function (e) {
        $('#SelectedFileName').text($(this).text());
        $('#EditBtn').attr('href', 'ms-word:ofe|u|http://localhost:32351/xdav/' + $(this).text());
    });
});