function ajaxCall(target, callback, data=null, method='GET', onerror=null)
{
    if (!(callback instanceof Function)) {
        return;
    }
    if (!(onerror instanceof Function)) {
        onerror = function(jqXHR, textStatus, errorThrown) {
            console.error({jqXHR, textStatus, errorThrown});
            alert("An error occurred. Please see the console for more details.");
        };
    }
    method = method.toUpperCase();
    let ajaxObject = {
        type: method,
        url: target,
        async: true,
        cache: false,
        data: data,
        error: onerror,
        success: callback,
        timeout: 30001
    };
    $.ajax(ajaxObject);
}