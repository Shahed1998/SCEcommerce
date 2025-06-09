$(function () {

    if ($('.summernote').next('.note-editor').length) {
        $('.summernote').summernote('destroy');
    }

    $('.summernote').summernote({
        height: 150,
        toolbar: [
            ['style', ['bold', 'italic', 'underline', 'clear']],
            ['color', ['color']],
        ]
    });

    $('.select2Dropdown').select2({
        width: '100%',
        theme: 'bootstrap-5',
        placeholder: '', // Both placeholder and empty option required for allowClear
        allowClear: true,
        dropdownPosition: $(document.body)
    });
});


function sidebarToggle() {
    /* document.getElementById('sidebar').classList.toggle('sidebar-hidden');*/
    $(".main-body").toggleClass("sidebar-collapsed");
}
