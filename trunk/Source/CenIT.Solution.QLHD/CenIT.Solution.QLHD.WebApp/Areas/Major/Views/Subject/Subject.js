var _SubjectActionURLs = {
    Subject_GetData: "/Major/Subject/Get",
    Subject_UploadFile: "/Major/Subject/UploadFile",
    Subject_LookupByCard: "/Major/Subject/LookupByIdentityCard",
    Subject_SaveSubject: "/Major/Subject/SaveSubject",
    Subject_SaveViolation: "/Major/Subject/SaveViolation",
    Subject_GetChangeLog: "/Major/Subject/GetChangeLog"
};
var _tableSubject;
$(document).ready(function () {
    initTableSubject();
});

function initTableSubject() {
    _tableSubject = $("#DSSubject").DataTable({
        "responsive": true,
        "language": {
            "processing": "<div class='overlay'><i class='fas fa-cog fa-spin'></i></div>",
            "emptyTable": "Không có dữ liệu đối tượng",
            "info": "Hiển thị _START_ đến _END_ của _TOTAL_ đối tượng",
            "infoEmpty": "Hiển thị 0 đến 0 của 0 đối tượng",
            "lengthMenu": "Hiển thị _MENU_ đối tượng",
            "paginate": {
                "first": "Đầu",
                "last": "Cuối",
                "next": "Sau",
                "previous": "Trước"
            }
        },
        "lengthChange": true,
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": _SubjectActionURLs.Subject_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": function (d) {
                d.IdentityCardNumber = $("#Search #IdentityCardNumber").val();
                d.FullName = $("#Search #FullName").val();
                d.Gender = $("#Search #Gender").val();
                // Danh sách hành vi vi phạm được chọn, ghép thành chuỗi phân tách bởi dấu phẩy
                var behaviors = $("#Search #BehaviorIds").val();
                d.BehaviorIds = (behaviors && behaviors.length > 0) ? behaviors.join(",") : "";
            }
        },
        "columns": [
            {
                "data": null,
                "defaultContent": "",
                "className": "text-center align-middle",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    return meta.settings._iDisplayStart + meta.row + 1;
                }
            },
            {
                "data": "IdentityCardNumber",
                "defaultContent": "",
                "className": "align-middle",
                "render": function (data, type, row) {
                    var html = '<a href="/Major/Subject/Detail/' + row.SubjectId + '" class="text-primary font-bolder d-block">' +
                        '<i class="far fa-id-card mr-1"></i>' + (data || "") + "</a>";
                    html += '<span class="font-bold text-dark-m1">' + (row.FullName || "") + "</span>";
                    if (row.OtherName) {
                        html += '<br><small class="text-secondary">(Tên gọi khác: ' + row.OtherName + ")</small>";
                    }
                    return html;
                }
            },
            {
                "data": "Gender",
                "defaultContent": "",
                "className": "text-center align-middle"
            },
            {
                "data": "DateOfBirthStr",
                "defaultContent": "",
                "className": "text-center align-middle"
            },
            {
                "data": "PlaceOfOrigin",
                "defaultContent": "",
                "className": "align-middle"
            },
            {
                "data": "TrackingUnitCount",
                "defaultContent": "1",
                "className": "align-middle",
                "orderable": false,
                "render": function (data, type, row) {
                    if (type !== "display") return data;
                    var count = parseInt(data, 10) || 1;
                    var unitsSummary = row.TrackingUnits || row.ReporterUnit || "";
                    var html = "";

                    if (count > 1) {
                        html += _renderButton(true,
                            "MonitoringUnits",
                            "btn btn-xs btn-outline-danger radius-round px-2 font-bolder text-85 shadow-xs d-inline-flex align-items-center mb-1",
                            "/Major/Subject/MonitoringUnits/" + row.SubjectId,
                            '<i class="fa fa-exclamation-triangle mr-1"></i> ' + count + ' đơn vị giám sát',
                            "Đơn vị quản lý & Giám sát đối tượng", "1150px");
                    } else {
                        html += _renderButton(true,
                            "MonitoringUnits",
                            "btn btn-xs btn-outline-default radius-round px-2 text-secondary-d2 font-600 text-85 d-inline-flex align-items-center mb-1",
                            "/Major/Subject/MonitoringUnits/" + row.SubjectId,
                            '<i class="fa fa-building mr-1"></i> 1 đơn vị',
                            "Đơn vị quản lý & Giám sát đối tượng", "1150px");
                    }

                    if (unitsSummary) {
                        html += '<div class="text-80 text-secondary text-truncate" style="max-width:180px;" title="' + unitsSummary.replace(/"/g, '&quot;') + '">' +
                            '<i class="fa fa-sitemap text-grey-m1 mr-1"></i>' + unitsSummary + '</div>';
                    }

                    return html;
                }
            },
            {
                "data": "AvatarUrl",
                "defaultContent": "",
                "className": "text-center align-middle",
                "orderable": false,
                "render": function (data) {
                    var src = data ? data : "/Contents/Base/imgs/avatar-default.png";
                    return '<img src="' + src + '" onerror="this.src=\'/Contents/Base/imgs/avatar-default.png\';" ' +
                        'class="radius-round border-1 brc-primary-m3" style="width:42px;height:42px;object-fit:cover;" />';
                }
            },
            {
                "data": "SubjectId",
                "className": "text-center align-middle",
                "orderable": false,
                "render": function (data, type, row) {
                    if (type !== "display") return "";

                    // Dang menu tha xuong cho gon, giong man hinh Nguoi dung
                    var html = '<div class="dropdown d-inline-block">' +
                        '<button class="btn px-3 btn-lighter-primary v-hover dropdown-toggle" type="button" ' +
                        'data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
                        '<i class="fa fa-ellipsis-h text-120"></i></button>' +
                        '<div class="dropdown-menu dropdown-menu-right">';

                    html += _renderButton(true,
                        "EditSubject",
                        "btn btn-outline-primary mr-1 dropdown-item",
                        "/Major/Subject/Edit/" + data,
                        '<i class="far fa-edit text-120"></i> Cập nhật thông tin',
                        "Cập nhật thông tin", "1350px");

                    html += _renderButton(true,
                        "MonitoringUnits",
                        "btn btn-outline-purple mr-1 dropdown-item",
                        "/Major/Subject/MonitoringUnits/" + data,
                        '<i class="fas fa-sitemap text-120"></i> Đơn vị theo dõi (' + (row.TrackingUnitCount || 1) + ')',
                        "Đơn vị quản lý & Giám sát đối tượng", "1150px");

                    html += _renderButton(true,
                        "ViolationHistory",
                        "btn btn-outline-warning mr-1 dropdown-item",
                        "/Major/Subject/ViolationHistory/" + data,
                        '<i class="fas fa-exclamation-triangle text-120"></i> Lịch sử vi phạm',
                        "Lịch sử vi phạm", "1100px");

                    html += _renderButton(true,
                        "SubjectChangeLog",
                        "btn btn-outline-info mr-1 dropdown-item",
                        "/Major/Subject/ChangeLog/" + data,
                        '<i class="fas fa-history text-120"></i> Log cập nhật',
                        "Log cập nhật", "1000px");

                    html += _renderButton(true,
                        "DeleteSubject",
                        "btn btn-outline-danger mr-1 dropdown-item",
                        "/Major/Subject/Delete/" + data,
                        '<i class="far fa-trash-alt text-120"></i> Xoá đối tượng',
                        "Xoá đối tượng");

                    html += "</div></div>";
                    return html;
                }
            }
        ]
    });
}

function Subject_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableSubject.ajax.reload(null, false);
                response.status = undefined;
                var urlAction = $("#ModalContent #modal_" + formId + " form").attr("action");
                $("#ModalContent #modal_" + formId + " #modal-content").load(urlAction, function () {
                    _initElements(this);
                });
            }
        } else {
            $("#ModalContent #modal_" + formId).modal("hide");
            $("#ModalContent #modal_" + formId).on("hidden.bs.modal", function () {
                if (response.status != undefined) {
                    eval(response.message);
                    if (typeof _tableSubject !== "undefined") {
                        _tableSubject.ajax.reload(null, false);
                    }
                    response.status = undefined;
                }
            });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}

function handleSubjectFileUpload(input, targetInputId, previewImgId) {
    if (input.files && input.files[0]) {
        var formData = new FormData();
        formData.append("file", input.files[0]);
        $.ajax({
            url: _SubjectActionURLs.Subject_UploadFile,
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (res) {
                if (res.status) {
                    $("#" + targetInputId).val(res.url);
                    if (previewImgId) {
                        $("#" + previewImgId).attr("src", res.url).removeClass("d-none");
                    }
                } else {
                    alert(res.message || "Tải ảnh thất bại.");
                }
            },
            error: function () {
                alert("Lỗi kết nối máy chủ tải tệp.");
            }
        });
    }
}

function handleSubjectMultipleViolationUpload(input) {
    if (input.files && input.files.length > 0) {
        for (var i = 0; i < input.files.length; i++) {
            var formData = new FormData();
            formData.append("file", input.files[i]);
            $.ajax({
                url: _SubjectActionURLs.Subject_UploadFile,
                type: "POST",
                data: formData,
                processData: false,
                contentType: false,
                async: false,
                success: function (res) {
                    if (res.status) {
                        var currentVal = $("#InitialImages").val();
                        var newVal = currentVal ? currentVal + ";" + res.url : res.url;
                        $("#InitialImages").val(newVal);
                        $("#previewViolationImages").append(
                            '<div class="d-inline-block position-relative mr-2 mb-2">' +
                            '<img src="' + res.url + '" class="radius-1 border-1 brc-danger-m2" style="width:70px;height:70px;object-fit:cover;" />' +
                            '<button type="button" class="btn btn-xs btn-danger position-absolute" style="top:-6px;right:-6px;border-radius:50%;padding:1px 5px;" onclick="removeSubjectViolationImage(this, \'' + res.url + '\')">&times;</button>' +
                            '</div>'
                        );
                    }
                }
            });
        }
    }
}

function removeSubjectViolationImage(btn, url) {
    $(btn).parent().remove();
    var currentVal = $("#InitialImages").val();
    if (currentVal) {
        var arr = currentVal.split(";").filter(function (x) { return x && x !== url; });
        $("#InitialImages").val(arr.join(";"));
    }
}

/* =========================================================================
 *  TRA CỨU ĐỐI TƯỢNG THEO SỐ CCCD
 *  - Nhấn Enter tại ô CCCD hoặc bấm nút tra cứu bên cạnh.
 *  - Nếu tìm thấy: đổ toàn bộ thông tin định danh + lịch sử vi phạm lên form.
 * ========================================================================= */

function lookupSubjectByIdentityCard() {
    var $card = $("#ModalContent #IdentityCardNumber");
    if ($card.length === 0) $card = $("#IdentityCardNumber");

    var cardNumber = $.trim($card.val() || "");
    var $result = $("#lookupSubjectResult");
    var $button = $("#btnLookupSubject");

    if (cardNumber === "") {
        showLookupSubjectMessage("warning", "fa-exclamation-circle", "Vui lòng nhập số CCCD/CMND cần tra cứu.");
        $card.focus();
        return;
    }

    $button.prop("disabled", true).html('<i class="fas fa-spinner fa-spin"></i>');
    $result.removeClass("d-none").html('<span class="text-secondary"><i class="fas fa-spinner fa-spin mr-1"></i>Đang tra cứu...</span>');

    $.ajax({
        url: _SubjectActionURLs.Subject_LookupByCard,
        type: "GET",
        dataType: "json",
        data: { identityCardNumber: cardNumber },
        success: function (res) {
            $button.prop("disabled", false).html('<i class="fas fa-search"></i>');

            if (res && res.status && res.data) {
                fillSubjectFormFromLookup(res.data);
            } else {
                clearLookupSubjectState();
                var icon = (res && res.isNotFound) ? "fa-info-circle" : "fa-times-circle";
                var type = (res && res.isNotFound) ? "info" : "danger";
                showLookupSubjectMessage(type, icon, (res && res.message) || "Không tìm thấy dữ liệu.");
            }
        },
        error: function () {
            $button.prop("disabled", false).html('<i class="fas fa-search"></i>');
            showLookupSubjectMessage("danger", "fa-times-circle", "Lỗi kết nối máy chủ khi tra cứu CCCD.");
        }
    });
}

function showLookupSubjectMessage(type, icon, message) {
    $("#lookupSubjectResult")
        .removeClass("d-none")
        .html('<span class="text-' + type + ' font-bold"><i class="fas ' + icon + ' mr-1"></i>' + message + '</span>');
}

/* Đổ dữ liệu tra cứu được lên các control của form */
function fillSubjectFormFromLookup(data) {
    setLookupFieldValue("SubjectId", data.subjectId);
    setLookupFieldValue("FullName", data.fullName);
    setLookupFieldValue("OtherName", data.otherName);
    setLookupFieldValue("DateOfBirth", data.dateOfBirth);
    setLookupFieldValue("Gender", data.gender);
    setLookupFieldValue("Ethnicity", data.ethnicity);
    setLookupFieldValue("Religion", data.religion);
    setLookupFieldValue("Nationality", data.nationality);
    setLookupFieldValue("PhoneNumber", data.phoneNumber);
    setLookupFieldValue("PlaceOfOrigin", data.placeOfOrigin);
    setLookupFieldValue("CurrentResidence", data.currentResidence);

    setLookupImage("AvatarUrl", "previewAvatar", data.avatarUrl, "/Contents/Base/imgs/avatar-default.png");
    setLookupImage("IdentityCardFrontUrl", "previewFront", data.identityCardFrontUrl, "/Contents/Base/imgs/no-image.png");
    setLookupImage("IdentityCardBackUrl", "previewBack", data.identityCardBackUrl, "/Contents/Base/imgs/no-image.png");

    // KHÔNG nạp lại lịch sử vi phạm cũ: màn hình này để ghi nhận vi phạm MỚI.
    // Phần nhập vi phạm vẫn giữ nguyên trạng thái khoá cho tới khi bấm "Lưu đối tượng".
    showLookupSubjectMessage(
        "success",
        "fa-check-circle",
        "Đã tìm thấy: <u>" + escapeLookupHtml(data.fullName) + "</u>. Bấm \"Lưu đối tượng\" để tiếp tục ghi nhận vi phạm."
    );
}

function setLookupFieldValue(fieldId, value) {
    var $el = $("#ModalContent #" + fieldId);
    if ($el.length === 0) $el = $("#" + fieldId);
    if ($el.length === 0) return;
    $el.val(value == null ? "" : value);
}

function setLookupImage(hiddenId, previewId, url, defaultUrl) {
    setLookupFieldValue(hiddenId, url);
    var $img = $("#ModalContent #" + previewId);
    if ($img.length === 0) $img = $("#" + previewId);
    if ($img.length === 0) return;
    $img.attr("src", url ? url : defaultUrl).removeClass("d-none");
}

/* Xoá trạng thái tra cứu trước đó (khi không tìm thấy hoặc đổi CCCD) */
function clearLookupSubjectState() {
    // Chỉ reset ở chế độ thêm mới (form AddSubject), tránh phá dữ liệu form sửa.
    if ($("#ModalContent #AddSubject").length > 0 || $("#AddSubject").length > 0) {
        var $subjectId = $("#ModalContent #SubjectId");
        if ($subjectId.length === 0) $subjectId = $("#SubjectId");
        $subjectId.val("");
    }
    // Đối tượng không còn được xác định -> khoá lại phần nhập vi phạm
    lockViolationPanel();
}

function escapeLookupHtml(text) {
    if (text == null) return "";
    return $("<div/>").text(text).html();
}

/* Gắn sự kiện bằng delegated event để hoạt động với nội dung modal nạp bằng AJAX */
$(document).on("keydown", "#IdentityCardNumber", function (e) {
    if (e.which === 13 || e.keyCode === 13) {
        e.preventDefault();
        lookupSubjectByIdentityCard();
    }
});

$(document).on("click", "#btnLookupSubject", function (e) {
    e.preventDefault();
    lookupSubjectByIdentityCard();
});

/* =========================================================================
 *  BỘ LỌC TRA CỨU NGOÀI DANH SÁCH ĐỐI TƯỢNG
 *  Tra cứu theo: số CCCD, họ tên, hành vi vi phạm.
 * ========================================================================= */

/* Thu gọn danh sách hành vi theo lĩnh vực đang chọn */
function filterSearchBehaviorsByField() {
    var fieldId = $("#Search #SearchFieldId").val();
    var $behaviors = $("#Search #BehaviorIds");

    $behaviors.find("option").each(function () {
        var $opt = $(this);
        var match = !fieldId || $opt.attr("data-fieldid") === fieldId;
        $opt.toggle(match);
        // Bỏ chọn những hành vi không còn thuộc lĩnh vực đang lọc
        if (!match) $opt.prop("selected", false);
    });
}

/* Xoá toàn bộ điều kiện tra cứu và tải lại danh sách */
function resetSubjectSearch() {
    $("#Search #IdentityCardNumber").val("");
    $("#Search #FullName").val("");
    $("#Search #Gender").val("");
    $("#Search #SearchFieldId").val("");
    $("#Search #BehaviorIds").val([]);
    filterSearchBehaviorsByField();

    if (typeof _tableSubject !== "undefined") {
        _tableSubject.ajax.reload(null, false);
    }
}

$(document).on("change", "#Search #SearchFieldId", function () {
    filterSearchBehaviorsByField();
});

/* Cho phép nhấn Enter tại ô CCCD / họ tên để tra cứu ngay */
$(document).on("keydown", "#Search #IdentityCardNumber, #Search #FullName", function (e) {
    if (e.which === 13 || e.keyCode === 13) {
        e.preventDefault();
        if (typeof _tableSubject !== "undefined") {
            _tableSubject.ajax.reload(null, false);
        }
    }
});

/* =========================================================================
 *  LUỒNG THÊM MỚI 2 BƯỚC
 *  Bước 1: Lưu thông tin đối tượng  -> mở khoá phần nhập vi phạm.
 *  Bước 2: Lưu thông tin vi phạm    -> đóng modal, tải lại danh sách.
 *
 *  Dùng type="button" + $.ajax thủ công thay vì Ajax.BeginForm, vì hàm
 *  _initButtonSubmit của khung TSFramework sẽ submit MỌI form trong modal
 *  khi bấm một nút submit bất kỳ - không phù hợp với luồng nhiều bước.
 * ========================================================================= */

function $inModal(selector) {
    var $el = $("#ModalContent " + selector);
    return $el.length > 0 ? $el : $(selector);
}

/* Khoá phần nhập vi phạm: vô hiệu hoá toàn bộ input và hiện lớp phủ xám */
function lockViolationPanel() {
    var $panel = $inModal("#violationPanel");
    if ($panel.length === 0) return;

    $panel.attr("data-locked", "1");
    $inModal("#violationFieldset").prop("disabled", true);
    $inModal("#violationPanelOverlay").removeClass("d-none");
    $inModal("#btnSaveViolation").prop("disabled", true);
}

/* Mở khoá phần nhập vi phạm sau khi đối tượng đã được lưu */
function unlockViolationPanel() {
    var $panel = $inModal("#violationPanel");
    if ($panel.length === 0) return;

    $panel.attr("data-locked", "0");
    $inModal("#violationFieldset").prop("disabled", false);
    $inModal("#violationPanelOverlay").addClass("d-none");
    $inModal("#btnSaveViolation").prop("disabled", false);
}

/* BƯỚC 1 - Lưu thông tin đối tượng */
function saveSubjectStep1() {
    var $form = $inModal("#AddSubject");
    if ($form.length === 0) return;

    var cardNumber = $.trim($inModal("#IdentityCardNumber").val() || "");
    var fullName = $.trim($inModal("#FullName").val() || "");

    if (cardNumber === "") {
        showLookupSubjectMessage("warning", "fa-exclamation-circle", "Vui lòng nhập số CCCD/CMND.");
        $inModal("#IdentityCardNumber").focus();
        return;
    }
    if (fullName === "") {
        showLookupSubjectMessage("warning", "fa-exclamation-circle", "Vui lòng nhập họ và tên.");
        $inModal("#FullName").focus();
        return;
    }

    var $btn = $inModal("#btnSaveSubject");
    var originalHtml = $btn.html();
    $btn.prop("disabled", true).html('<i class="fas fa-spinner fa-spin"></i>&nbsp;Đang lưu...');

    $.ajax({
        url: _SubjectActionURLs.Subject_SaveSubject,
        type: "POST",
        dataType: "json",
        data: $form.serialize(),
        success: function (res) {
            if (res && res.status) {
                $inModal("#SubjectId").val(res.subjectId);
                unlockViolationPanel();

                if (res.unchanged) {
                    showLookupSubjectMessage("info", "fa-info-circle",
                        "Thông tin đối tượng không có thay đổi. Mời nhập thông tin vi phạm.");
                } else {
                    if (res.message) { eval(res.message); }
                    showLookupSubjectMessage("success", "fa-check-circle",
                        res.isUpdate ? "Đã cập nhật thông tin đối tượng. Mời nhập thông tin vi phạm."
                                     : "Đã lưu đối tượng mới. Mời nhập thông tin vi phạm.");
                }

                if (typeof _tableSubject !== "undefined") {
                    _tableSubject.ajax.reload(null, false);
                }
                $inModal("#InitialViolationDate").focus();
            } else {
                if (res && res.message) { eval(res.message); }
            }
        },
        error: function () {
            alert("Lỗi kết nối máy chủ khi lưu thông tin đối tượng.");
        },
        complete: function () {
            $btn.prop("disabled", false).html(originalHtml);
        }
    });
}

/* BƯỚC 2 - Lưu thông tin vi phạm */
function saveViolationStep2() {
    var $form = $inModal("#AddSubject");
    if ($form.length === 0) return;

    var subjectId = $.trim($inModal("#SubjectId").val() || "");
    if (subjectId === "" || subjectId === "00000000-0000-0000-0000-000000000000") {
        alert("Vui lòng lưu thông tin đối tượng trước khi ghi nhận vi phạm.");
        return;
    }

    var behaviorIds = $.trim($inModal("#InitialBehaviorIds").val() || "");
    if (behaviorIds === "") {
        alert("Vui lòng chọn ít nhất một hành vi vi phạm.");
        return;
    }

    var $btn = $inModal("#btnSaveViolation");
    var originalHtml = $btn.html();
    $btn.prop("disabled", true).html('<i class="fas fa-spinner fa-spin"></i>&nbsp;Đang lưu...');

    $.ajax({
        url: _SubjectActionURLs.Subject_SaveViolation,
        type: "POST",
        dataType: "json",
        data: $form.serialize(),
        success: function (res) {
            if (res && res.message) { eval(res.message); }
            if (res && res.status) {
                if (typeof _tableSubject !== "undefined") {
                    _tableSubject.ajax.reload(null, false);
                }
                $("#ModalContent #modal_AddSubject").modal("hide");
            }
        },
        error: function () {
            alert("Lỗi kết nối máy chủ khi lưu thông tin vi phạm.");
        },
        complete: function () {
            $btn.prop("disabled", false).html(originalHtml);
        }
    });
}

$(document).on("click", "#btnSaveSubject", function (e) {
    e.preventDefault();
    saveSubjectStep1();
});

$(document).on("click", "#btnSaveViolation", function (e) {
    e.preventDefault();
    saveViolationStep2();
});

/* Nếu người dùng sửa lại số CCCD sau khi đã lưu, phải khoá lại phần vi phạm.
   Nếu không, lần vi phạm sẽ bị gán nhầm sang đối tượng vừa lưu trước đó. */
$(document).on("input", "#IdentityCardNumber", function () {
    var $panel = $inModal("#violationPanel");
    if ($panel.length === 0 || $panel.attr("data-locked") === "1") return;
    // Chỉ áp dụng cho form thêm mới
    if ($inModal("#AddSubject").length === 0) return;

    $inModal("#SubjectId").val("");
    lockViolationPanel();
    showLookupSubjectMessage("warning", "fa-exclamation-circle",
        "Số CCCD đã thay đổi. Vui lòng bấm \"Lưu đối tượng\" lại.");
});


/* =========================================================================
 *  LỊCH SỬ VI PHẠM
 * ========================================================================= */

/* Người không phải người báo cáo: thu gọn / mở rộng phần chi tiết */
function toggleViolationDetail(button) {
    var $body = $(button).closest(".violation-history-card").find(".violation-detail-body");
    $body.slideToggle(150);
}

/* Nut Sua / Xoa lan vi pham dung thuoc tinh data-modal, khung TSFramework tu xu ly.
   Khong con can ham trung gian tao the <a> roi trigger click. */


/* =========================================================================
 *  LOG CẬP NHẬT
 * ========================================================================= */

var _tableSubjectChangeLog;

function initTableSubjectChangeLog(subjectId) {
    if ($.fn.DataTable.isDataTable("#DSSubjectChangeLog")) {
        $("#DSSubjectChangeLog").DataTable().destroy();
    }

    _tableSubjectChangeLog = $("#DSSubjectChangeLog").DataTable({
        "responsive": true,
        "searching": false,
        "lengthChange": false,
        "pageLength": 10,
        "processing": true,
        "serverSide": true,
        "ordering": false,
        "language": {
            "processing": "<div class='overlay'><i class='fas fa-cog fa-spin'></i></div>",
            "emptyTable": "Chưa có lịch sử thay đổi nào",
            "info": "Hiển thị _START_ đến _END_ của _TOTAL_ dòng",
            "infoEmpty": "Hiển thị 0 đến 0 của 0 dòng",
            "paginate": { "first": "Đầu", "last": "Cuối", "next": "Sau", "previous": "Trước" }
        },
        "ajax": {
            "url": _SubjectActionURLs.Subject_GetChangeLog,
            "type": "POST",
            "dataType": "JSON",
            "data": function (d) { d.subjectId = subjectId; }
        },
        "columns": [
            {
                "data": "CreatedDateStr",
                "className": "align-middle text-85",
                "render": function (data) {
                    return '<i class="far fa-clock mr-1 text-secondary"></i>' + (data || "");
                }
            },
            {
                "data": "ActionType",
                "className": "text-center align-middle",
                "render": function (data, type, row) {
                    var label = { "ADD": "Thêm mới", "UPDATE": "Cập nhật", "DELETE": "Xoá" }[data] || data;
                    var css = { "ADD": "badge-success", "UPDATE": "badge-warning", "DELETE": "badge-danger" }[data] || "badge-secondary";
                    var scope = (row.EntityType === "VIOLATION") ? "Vi phạm" : "Đối tượng";
                    return '<span class="badge ' + css + '">' + label + "</span>" +
                        '<br><small class="text-secondary">' + scope + "</small>";
                }
            },
            {
                "data": "ChangedFields",
                "className": "align-middle text-85",
                "render": function (data, type, row) {
                    if (data) {
                        var html = "";
                        try {
                            JSON.parse(data).forEach(function (change) {
                                html += '<div class="mb-1">' +
                                    '<span class="font-bold text-dark-m2">' + escapeLookupHtml(change.Label) + ":</span> " +
                                    '<span class="text-danger text-decoration-line-through">' + (escapeLookupHtml(change.OldValue) || "(trống)") + "</span>" +
                                    ' <i class="fas fa-long-arrow-alt-right mx-1 text-secondary"></i> ' +
                                    '<span class="text-success font-bold">' + (escapeLookupHtml(change.NewValue) || "(trống)") + "</span>" +
                                    "</div>";
                            });
                        } catch (e) {
                            html = escapeLookupHtml(row.ChangedFieldNames || "");
                        }
                        return html;
                    }
                    return '<span class="text-secondary">' + escapeLookupHtml(row.Description || "") + "</span>";
                }
            },
            {
                "data": "ActorName",
                "className": "align-middle text-85",
                "render": function (data, type, row) {
                    var html = '<span class="font-bold text-dark-m1">' + escapeLookupHtml(data || row.ActorUserName || "") + "</span>";
                    if (row.ActorPosition) {
                        html += '<br><small class="text-secondary">' + escapeLookupHtml(row.ActorPosition) + "</small>";
                    }
                    if (row.ActorUnit) {
                        html += '<br><small class="text-secondary">' + escapeLookupHtml(row.ActorUnit) + "</small>";
                    }
                    return html;
                }
            }
        ]
    });
}

/* Khi modal Log cập nhật được mở xong thì khởi tạo bảng */
$(document).on("shown.bs.modal", "#modal_SubjectChangeLog", function () {
    var subjectId = $(this).find("#LogSubjectId").val();
    if (subjectId) { initTableSubjectChangeLog(subjectId); }
});
