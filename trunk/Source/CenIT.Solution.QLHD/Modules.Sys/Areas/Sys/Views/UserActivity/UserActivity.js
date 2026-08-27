// Man hinh Giam sat truc tuyen.
// Tu lam moi dinh ky de danh sach luon phan anh thuc te.

var ACTIVITY_REFRESH_MS = 15000;
var _activityTimer = null;
var AVATAR_DEFAULT = "/Contents/Base/imgs/avatar-default.png";

function _escapeHtml(s) {
    if (s === null || s === undefined) return "";
    return String(s)
        .replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;").replace(/'/g, "&#39;");
}

// Doi so giay thanh chuoi doc duoc: "vua xong", "45 giay truoc", "3 phut truoc"
function _agoText(seconds) {
    var s = parseInt(seconds, 10) || 0;
    if (s < 10) return "vừa xong";
    if (s < 60) return s + " giây trước";
    var m = Math.floor(s / 60);
    return m + " phút trước";
}

function _onlineText(minutes) {
    var m = parseInt(minutes, 10) || 0;
    if (m < 60) return m + " phút";
    var h = Math.floor(m / 60);
    var r = m % 60;
    return r === 0 ? (h + " giờ") : (h + " giờ " + r + " phút");
}

function loadActivities() {
    $.ajax({
        type: "POST",
        url: urlGetActivities,
        dataType: "json",
        success: function (res) {
            var rows = (res && res.data) ? res.data : [];
            $("#statOnline").text(rows.length);

            var now = new Date();
            $("#statUpdated").text(
                ("0" + now.getHours()).slice(-2) + ":" +
                ("0" + now.getMinutes()).slice(-2) + ":" +
                ("0" + now.getSeconds()).slice(-2));

            if (rows.length === 0) {
                $("#bodyActivity").html(
                    [
                        '<tr><td colspan="7" class="text-center text-grey-m1 py-4">',
                        '<i class="fa fa-user-slash text-150 d-block mb-2"></i>',
                        'Hiện không có tài khoản nào đang trực tuyến</td></tr>'
                    ].join(""));
                return;
            }

            var html = "";
            $.each(rows, function (i, r) {
                var avatar = r.Avatar
                    ? ("/Contents/imgs/avatars/" + _escapeHtml(r.UserName) + "/" + _escapeHtml(r.Avatar))
                    : AVATAR_DEFAULT;

                // Thong tin nguoi dung
                var user = [
                    '<div class="d-flex align-items-center">',
                    '<img src="', avatar, '" class="radius-round mr-2 w-10" ',
                    'onerror="this.src=', String.fromCharCode(38), '#39;', AVATAR_DEFAULT,
                    String.fromCharCode(38), '#39;">',
                    '<div><div class="text-600 text-blue-d1">', _escapeHtml(r.FullName), '</div>',
                    '<div class="text-90 text-grey-d1">', _escapeHtml(r.UserName), '</div></div></div>'
                ].join("");

                // Don vi va chuc vu
                var unit = "";
                if (r.UnionName) {
                    unit += '<div class="text-600 text-95"><i class="fa fa-sitemap text-grey-m1 mr-1"></i>'
                          + _escapeHtml(r.UnionName) + '</div>';
                }
                if (r.PositionName) {
                    unit += '<div class="text-90 text-grey-d1 mt-1">' + _escapeHtml(r.PositionName) + '</div>';
                }
                if (!unit) unit = '<span class="text-grey-m1 text-90">-</span>';

                // Man hinh dang xem. Ten lay tu bang menu nen trung voi ten nguoi
                // dung thay tren thanh menu. Duong dan de o thuoc tinh title, chi
                // hien khi re chuot - tranh lam roi bang.
                var screen = '<span class="badge bgc-blue-l3 text-blue-d2 px-2 py-1" title="'
                           + _escapeHtml(r.CurrentUrl) + '">'
                           + '<i class="fa fa-desktop mr-1"></i>' + _escapeHtml(r.ScreenName) + '</span>';

                // Hoat dong cuoi - duoi 60 giay thi to xanh cho de nhan
                var fresh = (parseInt(r.SecondsAgo, 10) || 0) < 60;
                var last = '<div class="' + (fresh ? "text-green-d2 text-600" : "text-grey-d1") + '">'
                         + '<i class="fa fa-circle text-70 mr-1"></i>' + _agoText(r.SecondsAgo) + '</div>'
                         + '<div class="text-85 text-grey-m1">' + _escapeHtml(r.LastActivity) + '</div>';

                html += '<tr>'
                     + '<td class="text-center">' + (i + 1) + '</td>'
                     + '<td>' + user + '</td>'
                     + '<td>' + unit + '</td>'
                     + '<td>' + screen + '</td>'
                     + '<td class="text-center text-90">' + _escapeHtml(r.IpAddress) + '</td>'
                     + '<td class="text-center text-90">' + _escapeHtml(r.LoginTime)
                     + '<div class="text-85 text-grey-m1">' + _onlineText(r.MinutesOnline) + '</div></td>'
                     + '<td class="text-center text-90">' + last + '</td>'
                     + '</tr>';
            });
            $("#bodyActivity").html(html);
        },
        error: function () {
            $("#bodyActivity").html(
                [
                    '<tr><td colspan="7" class="text-center text-red-d1 py-4">',
                    'Không tải được danh sách. Sẽ thử lại sau ít giây.</td></tr>'
                ].join(""));
        }
    });
}

$(function () {
    loadActivities();
    _activityTimer = setInterval(loadActivities, ACTIVITY_REFRESH_MS);

    $("#btnRefreshActivity").on("click", function () {
        loadActivities();
    });

    // Dung lam moi khi roi khoi trang de khong goi server vo ich
    $(window).on("unload", function () {
        if (_activityTimer) clearInterval(_activityTimer);
    });
});
