//getCaret taken from http://stackoverflow.com/questions/20742392/textarea-shiftenter-for-next-line-and-enter-to-submit-the-form
function getCaret(el) {
    if (el.selectionStart) {
        return el.selectionStart;
    } else if (document.selection) {
        el.focus();
        var r = document.selection.createRange();
        if (r === null) {
            return 0;
        }
        var re = el.createTextRange(),
            rc = re.duplicate();
        re.moveToBookmark(r.getBookmark());
        rc.setEndPoint('EndToStart', re);
        return rc.text.length;
    }
    return 0;
}

$("#comment-input").keypress(function (event) {
    if (event.shiftKey && event.which === 13) {
        console.log("DETECTED");
        var content = this.value;
        var caret = getCaret(this);
        this.value = content.substring(0, caret) + "\n" + content.substring(caret, content.length - 1);
        event.stopPropagation();
    } else if (event.keyCode === 13) {
        console.log("REGULAR SUBMIT");
        $('#comment-form').submit();
    }
    console.log("EERYD");
});