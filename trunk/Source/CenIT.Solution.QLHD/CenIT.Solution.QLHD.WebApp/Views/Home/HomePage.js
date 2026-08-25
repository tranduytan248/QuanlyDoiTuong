
$(document).ready(function () {
    generateCaptcha();
});
function clean() {
    $("#Search input[type='text']").val('');
}

//window.addEventListener('DOMContentLoaded', (event) => {
//    setTimeout(function () {
//        var bodyContent = document.querySelector('.slide-body');
//        bodyContent.style.opacity = '1';
//        bodyContent.style.transform = 'translateY(0)';
//    }, 500); // Độ trễ 500ms (0.5 giây)
//});

function generateCaptcha() {
    $('#captchaInput').val('');
    var chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXTZabcdefghiklmnopqrstuvwxyz";
    var captchaLength = 6;
    var captcha = '';

    for (var i = 0; i < captchaLength; i++) {
        var randomIndex = Math.floor(Math.random() * chars.length);
        captcha += chars.substring(randomIndex, randomIndex + 1);
    }

    $('#captchaCode').text(captcha);

    $('#captchaCode').on('copy', function (event) {
        event.preventDefault();
    });

    var popoverContent = $('#captchaInput').data('content');
    if (popoverContent != null && popoverContent.length > 0) {

        html2canvas(document.getElementById("captchaCode"), {
            allowTaint: true, useCORS: true
        }).then(function (canvas) {
            var imgageData = canvas.toDataURL("image/png");
            popoverContent = popoverContent.format(imgageData);
            $('#captchaInput').attr('data-content', popoverContent);
        });
    }
}

function checkCaptcha() {
    var userInput = $('#captchaInput').val();
    var captchaCode = $('#captchaCode').text();

    if (userInput === captchaCode) {
        return true;
    } else {
        generateCaptcha();
        return false;
    }
}

var faqItems = document.querySelectorAll('.qb_faq_detailds');

faqItems.forEach(function (item) {
    // Đặt sự kiện click cho mỗi phần tử
    item.addEventListener('click', function () {
        // Tìm phần tử tg_textwidget tương ứng
        var textWidget = this.querySelector('.tg_textwidget');

        // Kiểm tra trạng thái hiển thị của phần tử
        if (textWidget.style.display === 'block') {
            // Nếu đang hiển thị, ẩn nó đi
            textWidget.style.display = 'none';
        } else {
            // Nếu đang ẩn, hiển thị nó lên
            textWidget.style.display = 'block';
        }
    });
});
