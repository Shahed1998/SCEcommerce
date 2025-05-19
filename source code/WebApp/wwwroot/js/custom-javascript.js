function CreateModal(e, el, title = "") {
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

        $('.select2Dropdown').select2({
            dropdownParent: $('#globalModal'), 
            width: '100%',
            theme: 'bootstrap-5',
            placeholder: '', // Both placeholder and empty option required for allowClear
            allowClear: true
        });

    }).fail(function (xhr, status, msg) {
        debugger;
        $('#globalModal').modal('hide');

        var message = xhr.responseText;

        bootbox.alert({
            title: "Error",
            message: message
        });

    }).always(function () {
        spinner('hide');
    });
}

function Delete(e, el, id, msg) {
    e.preventDefault();
    bootbox.confirm({
        title: "Confirm",
        message: "Do you want to delete " + msg.trim() + "?",
        callback: function (result) {
            if (!result) return;

            $.ajax({
                method: 'DELETE',
                url: $(el).attr('href'),
                data: { id: id },
                beforeSend: function () {
                    spinner('show');
                },
                success: function (result) {
                    debugger;
                    window.location.href = result.redirectToAction;
                },
                error: function (xhr) {
                    window.location.href = xhr.responseJSON.redirectToAction;
                }
            }).always(function () {
                spinner('hide');
            })
        }
    });
}

function spinner(event) {
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

function save(formElement, type = "save") {
    var form = $(formElement);

    // First check if the form is valid
    if (!form.valid()) {
        return false;
    }

    // if the form is valid hide the form modal
    $('#globalModal').modal('hide');

    // Wait until modal is fully hidden before showing Bootbox
    $('#globalModal').on('hidden.bs.modal', function () {
        // Important: Unbind this event after firing once
        $(this).off('hidden.bs.modal');

        // Now show the confirmation box
        bootbox.confirm({
            title: "Confirm",
            message: `Do you want to ${type} the form?`,
            callback: function (result) {
                if (!result) {
                    // User cancelled, show the form modal again
                    $('#globalModal').modal('show');
                    return;
                }

                $.ajax({
                    type: "POST",
                    url: form.attr("action"),
                    data: form.serialize(),
                    success: function (result) {
                        if (result.hasOwnProperty('success') && result.success) {
                            window.location.href = result.redirectToAction;
                        } else {
                            $('#globalModal .modal-body').html(result);
                            $('#globalModal').modal('show');
                            $.validator.unobtrusive.parse('.modalForm');
                        }
                    },
                    error: function (xhr, status, error) {
                        bootbox.alert("An internal server error occurred.");
                    }
                });
            }
        });
    });

    return false; // prevent normal form submission
}

function ToastrNotification(type, msg = "") {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-center",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    switch (type) {
        case "success":
            toastr.success(msg);
            break;
        case "error":
            toastr.error(msg);
            break;
        default:
            toastr.error('An internal server error occured');
            break;
    }
}
