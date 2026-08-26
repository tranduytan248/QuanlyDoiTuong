/* =============================================================================
 *  XEM ẢNH PHÓNG TO
 *  ---------------------------------------------------------------------------
 *  Bấm vào bất kỳ ảnh nào trong trang sẽ mở ảnh ở kích thước lớn ngay tại chỗ,
 *  kèm nút đóng. Không cần mở tab mới.
 *
 *  Cách dùng: chỉ cần nạp file này ở layout dùng chung. Script tự bắt sự kiện
 *  cho mọi ảnh, kể cả ảnh được nạp sau bằng AJAX (dùng delegated event).
 *
 *  Bỏ qua các ảnh KHÔNG nên phóng to: biểu tượng, ảnh đại diện trên thanh menu,
 *  và ảnh nằm trong nút bấm.
 * ========================================================================== */

(function () {
    "use strict";

    var VIEWER_ID = "tsImageViewerOverlay";

    /* Ảnh không nên mở xem lớn: biểu tượng nhỏ, ảnh trong nút, ảnh trên thanh điều hướng */
    function shouldSkip($img) {
        if ($img.closest("#" + VIEWER_ID).length > 0) return true;
        if ($img.closest("button, .btn, .navbar, .dropdown-menu, .nav-link").length > 0) return true;
        if ($img.attr("data-no-zoom") !== undefined) return true;

        var src = $img.attr("src") || "";
        if (src.indexOf("data:image") === 0 && src.length < 200) return true;   // biểu tượng nhúng
        if (/icon|logo|favicon/i.test(src)) return true;

        /* Ảnh quá nhỏ thường là biểu tượng trang trí */
        var w = $img.width(), h = $img.height();
        if (w > 0 && h > 0 && w < 28 && h < 28) return true;

        return false;
    }

    function buildViewer() {
        if ($("#" + VIEWER_ID).length > 0) return;

        var html =
            '<div id="' + VIEWER_ID + '" role="dialog" aria-label="Xem ảnh">' +
                '<button type="button" class="ts-image-viewer-close" title="Đóng (Esc)" aria-label="Đóng">' +
                    '<i class="fas fa-times"></i>' +
                '</button>' +
                '<div class="ts-image-viewer-body">' +
                    '<img src="" alt="Ảnh xem lớn" />' +
                '</div>' +
                '<div class="ts-image-viewer-caption"></div>' +
            '</div>';

        $("body").append(html);

        var css =
            '#' + VIEWER_ID + '{' +
                'position:fixed;top:0;left:0;right:0;bottom:0;z-index:20000;' +
                'background:rgba(15,23,42,.92);display:none;' +
                'align-items:center;justify-content:center;flex-direction:column;padding:24px;' +
            '}' +
            '#' + VIEWER_ID + '.is-open{display:flex;}' +
            '#' + VIEWER_ID + ' .ts-image-viewer-body{' +
                'max-width:100%;max-height:calc(100% - 70px);display:flex;align-items:center;justify-content:center;' +
            '}' +
            '#' + VIEWER_ID + ' .ts-image-viewer-body img{' +
                'max-width:100%;max-height:88vh;object-fit:contain;' +
                'border-radius:4px;box-shadow:0 8px 32px rgba(0,0,0,.5);background:#fff;' +
            '}' +
            '#' + VIEWER_ID + ' .ts-image-viewer-close{' +
                'position:absolute;top:16px;right:20px;width:42px;height:42px;' +
                'border:none;border-radius:50%;background:rgba(255,255,255,.15);color:#fff;' +
                'font-size:20px;line-height:1;cursor:pointer;transition:background .15s ease;' +
            '}' +
            '#' + VIEWER_ID + ' .ts-image-viewer-close:hover{background:rgba(239,68,68,.9);}' +
            '#' + VIEWER_ID + ' .ts-image-viewer-caption{' +
                'margin-top:14px;color:#e2e8f0;font-size:13px;text-align:center;' +
                'max-width:80%;word-break:break-all;' +
            '}' +
            /* Cho biết ảnh bấm được */
            'img.ts-zoomable{cursor:zoom-in;}' +
            '@media (max-width:575.98px){' +
                '#' + VIEWER_ID + '{padding:12px;}' +
                '#' + VIEWER_ID + ' .ts-image-viewer-close{top:8px;right:10px;width:36px;height:36px;font-size:17px;}' +
            '}';

        $("<style>").attr("type", "text/css").html(css).appendTo("head");
    }

    function openViewer(src, caption) {
        buildViewer();
        var $v = $("#" + VIEWER_ID);
        $v.find(".ts-image-viewer-body img").attr("src", src);
        $v.find(".ts-image-viewer-caption").text(caption || "");
        $v.addClass("is-open");
        $("body").css("overflow", "hidden");
    }

    function closeViewer() {
        var $v = $("#" + VIEWER_ID);
        if ($v.length === 0) return;
        $v.removeClass("is-open");
        $v.find(".ts-image-viewer-body img").attr("src", "");
        $("body").css("overflow", "");
    }

    $(function () {
        buildViewer();

        /* Bấm vào ảnh -> mở xem lớn.
           Dùng delegated event nên áp dụng được cho cả ảnh nạp sau bằng AJAX. */
        $(document).on("click", "img", function (e) {
            var $img = $(this);
            if (shouldSkip($img)) return;

            var src = $img.attr("src");
            if (!src) return;

            /* Nếu ảnh nằm trong thẻ <a> thì chặn việc mở tab mới */
            e.preventDefault();
            e.stopPropagation();

            openViewer(src, $img.attr("alt") || $img.attr("title") || "");
        });

        /* Đánh dấu con trỏ chuột cho ảnh bấm được */
        $(document).on("mouseenter", "img", function () {
            var $img = $(this);
            if (!shouldSkip($img)) $img.addClass("ts-zoomable");
        });

        $(document).on("click", "#" + VIEWER_ID + " .ts-image-viewer-close", closeViewer);

        /* Bấm ra vùng nền tối cũng đóng */
        $(document).on("click", "#" + VIEWER_ID, function (e) {
            if (e.target === this) closeViewer();
        });

        /* Phím Esc để đóng */
        $(document).on("keydown", function (e) {
            if (e.key === "Escape" || e.keyCode === 27) closeViewer();
        });
    });
})();
