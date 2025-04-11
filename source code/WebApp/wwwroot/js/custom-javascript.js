function CreateModal(e, el, title="") {
    e.preventDefault();

    $.ajax({
        url: $(el).attr('href'),
        beforeSend: function () {
            spinner('show');
        }
    }).done(function (result) {
        $('#globalModalLabel').text(title);
        $('#globalModal .modal-body').html(result);
        $('#globalModal').modal('show');
        $.validator.unobtrusive.parse('.modalForm');
    }).fail(function () {
        $('#globalModal').modal('hide');
        bootbox.alert("An error occured");
    }).always(function () {
        spinner('hide');
    });

}

function spinner(event)
{
    debugger;
    if (event === "show") {
        if ($('.loader-container').hasClass('d-none')) {
            $('.loader-container').removeClass('d-none');
        }
    }
    else {
        if (!$('.loader-container').hasClass('d-none')) {
            $('.loader-container').addClass('d-none');
        }
    }
}