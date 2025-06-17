function debounce(func) {
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = window.setTimeout(() => { func.apply(this, args); }, 250);
    };
}

$(function () {
    /*
     * ALERT FADEOUT SCRIPT
     */
    $(".fade-out-toggle").fadeTo(5000, 500).slideUp(500, function () {
        $(".fade-out-toggle").alert('close');
    });

    $(".toggle-btn").on("click", function () {
        $('.sidebar').toggleClass('nav-collapsed');
        $('.sidebar a').addClass('collapsed');
        $('.sidebar div.collapse.show').removeClass('show');
    })

    $(".sidebar a.nav-link.collapsed").on("click", function () {
        $('.sidebar').removeClass('nav-collapsed');
    });

    $("span.hide-nav-collapsed").each((i, elem) => {
        new bootstrap.Tooltip($(elem).parent()[0], {
            placement: "right",
            title: $(elem).text(),
            trigger: 'hover',
            delay: {
                show: 500,
                hide: 100
            }
        });
    });
});

function notifySuccess(msg, header, autoHide) {
    if (!header) {
        header = 'Success!'
    }
    makeAlert(msg, 'success', header, autoHide);
}

function notifyError(msg, header, autoHide) {
    if (!header) {
        header = 'Error!'
    }
    makeAlert(msg, 'danger', header, autoHide);
}

function notifyInfo(msg, header, autoHide) {
    if (!header) {
        header = 'Info:'
    }
    makeAlert(msg, 'info', header, autoHide);
}

function notifyWarning(msg, header, autoHide) {
    if (!header) {
        header = 'Warning!'
    }
    makeAlert(msg, 'warning', header, autoHide);
}

function makeAlert(msg, type, header, autohide) {
    if (typeof (autohide) === 'undefined') {
        autohide = true;
    }
    const delay = 3000;
    const toast = $(`<div class="toast align-items-center text-white bg-${type} border-0" role="alert">`);

    const toastHeader = $("<div class='toast-header'>");
    toastHeader.append(`<strong class='me-auto'>${header}</strong>`)
    const closeBtn = $('<button type="button" class="btn-close btn-close-white" data-bs-dismiss="toast" aria-label="Close"></button>');
    toastHeader.append(closeBtn)
    toast.append(toastHeader);
    toast.append(`<div class='toast-body'>${msg}</div>`);
    $(".toast-container").append(toast);

    const bsToast = new bootstrap.Toast(toast[0], { autohide, delay });

    const removeFn = () => {
        bsToast.dispose();
        toast.remove();
    }

    if (autohide) {
        setTimeout(removeFn, delay + 1000)
    } else {
        closeBtn.on('click', () => { setTimeout(removeFn, 1000) });
    }

    bsToast.show();
}