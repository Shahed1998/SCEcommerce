$(function () {
    $('span.field-validation-error').each(function () {
        if (!$(this).hasClass('has-icon')) {
            const msg = $(this).html();;
            $(this).html('<i class="fa-solid fa-circle-info me-2 mt-2"></i>' + msg).addClass('has-icon');
        }
    });

    if ($('#summernote').next('.note-editor').length) {
        $('#summernote').summernote('destroy');
    }

    $('#summernote').summernote({
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


/* --------------------------------------------------------------------------------------------------- */
/* Rich Text Editor */
/* --------------------------------------------------------------------------------------------------- */
//tinymce.init({
//    selector: 'textarea',
//    plugins: [
//        // Core editing features
//        'anchor', 'autolink', 'charmap', 'codesample', 'emoticons', 'image', 'link', 'lists', 'media', 'searchreplace', 'table', 'visualblocks', 'wordcount',
//        // Your account includes a free trial of TinyMCE premium features
//        // Try the most popular premium features until Jun 16, 2025:
//        'checklist', 'mediaembed', 'casechange', 'formatpainter', 'pageembed', 'a11ychecker', 'tinymcespellchecker', 'permanentpen', 'powerpaste', 'advtable', 'advcode', 'editimage', 'advtemplate', 'ai', 'mentions', 'tinycomments', 'tableofcontents', 'footnotes', 'mergetags', 'autocorrect', 'typography', 'inlinecss', 'markdown', 'importword', 'exportword', 'exportpdf'
//    ],
//    toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table mergetags | addcomment showcomments | spellcheckdialog a11ycheck typography | align lineheight | checklist numlist bullist indent outdent | emoticons charmap | removeformat',
//    tinycomments_mode: 'embedded',
//    tinycomments_author: 'Author name',
//    mergetags_list: [
//        { value: 'First.Name', title: 'First Name' },
//        { value: 'Email', title: 'Email' },
//    ],
//    ai_request: (request, respondWith) => respondWith.string(() => Promise.reject('See docs to implement AI Assistant')),
//})
/* --------------------------------------------------------------------------------------------------- */
/* Rich Text Editor */
/* --------------------------------------------------------------------------------------------------- */