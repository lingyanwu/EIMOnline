$(document).ready(
function () {
    $("#tabs").tabs();
    $("input:submit, a,input:button", ".div_title").button();
    require(['/Scripts/jquery/jquery.validate.unobtrusive.min.js'], function () { });
    $(":text[data-val-required]").attr("class","Required");
});