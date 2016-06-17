console.log("HELLO WORLD");
$("#comment-input").keypress(function (event) {
    console.log("ADSL:KFJ");
    if (event.which == 13) {
        console.log("HELLO!");
        event.preventDefault();
        $("#comment-form").submit();
    }
});