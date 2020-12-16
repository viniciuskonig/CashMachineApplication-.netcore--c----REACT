
createRemarkable = () => {
    var remarkable = 'undefined' != typeof global && global.Remarkable
        ? global.Remarkable
        : window.Remarkable;

    return new remarkable();
}

createErrorMessage = (message) => {
    $('.applicationContainer').append('<div id="errorMessage"> \
            <div>\
                <div class="alert a' +
        'lert-danger fade in alert-dismissible show" role="alert">\
                <butt' +
        'on id="alertButton" type="button" class="close" data-dismiss="alert">&times;</bu' +
        'tton>\
                ' + message + '\
                </div>\
            </div>\
        </div>');

    $(errorMessage).each(function () {
        $(this).fadeTo(3000, 0)
            .slideUp(500, () => {
                $(this).remove();
            });
    });
}

createSucessMessage = (message) => {
    $('.applicationContainer').append('<div id="successMessage"> \
            <div>\
                <div class="alert a' +
        'lert-success fade in alert-dismissible show" role="alert">\
                <but' +
        'ton id="alertButton" type="button" class="close" data-dismiss="alert">&times;</b' +
        'utton>\
                ' + message + '\
                </div>\
            </div>\
        </div>');

    $(successMessage).each(function () {
        $(this).fadeTo(3000, 0)
            .slideUp(500, () => {
                $(this).remove();
            });
    });
}