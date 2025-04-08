function CreateModal(e, el, title="") {
    e.preventDefault();

    $.ajax({
        url: $(el).attr('href'),
        beforeSend: function () {
            // show spiner
        }
    }).done(function (result) {
        $('#globalModalLabel').text(title);
        $('#globalModal .modal-body').html(result);
        $('#globalModal').modal('show');
    }).fail(function () {
        $('#globalModal').modal('hide');
        bootbox.alert("An error occured");
    });

}