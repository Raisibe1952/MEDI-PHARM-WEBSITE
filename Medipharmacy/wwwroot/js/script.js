function GetModel(url) {
    $.get(url, function (partial) {
        $("div.modal-content").empty().append(partial);
    });
}

function GetModalContent(url) {
    $.get(url, function (partial) {
        $("div.modal-body").empty().append(partial);
    });
}