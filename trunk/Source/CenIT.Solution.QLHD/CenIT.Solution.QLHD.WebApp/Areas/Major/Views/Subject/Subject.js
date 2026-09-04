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
    initSearchFilters();
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
                d.SubjectTypeIds = $("#Search #SubjectTypeIds").val();
                // Danh sách hành vi vi phạm được chọn, ghép thành chuỗi phân tách bởi dấu phẩy
                var behaviors = $("#Search #BehaviorIds").val();
                var behaviorIds = (behaviors && behaviors.length > 0) ? (Array.isArray(behaviors) ? behaviors.join(",") : behaviors) : "";
                var fieldId = $("#Search #SearchFieldId").val();

                // Nếu người dùng KHÔNG chọn hành vi cụ thể nhưng CÓ chọn lĩnh vực:
                // Tự động gom tất cả BehaviorId thuộc lĩnh vực đó để tìm kiếm theo lĩnh vực
                if (!behaviorIds && fieldId && _allSearchBehaviorOptions.length > 0) {
                    var fieldBehaviors = _allSearchBehaviorOptions.filter(function (x) { return x.fieldId === String(fieldId); }).map(function (x) { return x.id; });
                    if (fieldBehaviors.length > 0) {
                        behaviorIds = fieldBehaviors.join(",");
                    }
                }
                d.BehaviorIds = behaviorIds;
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
                    var html = '<div class="d-flex flex-column py-1">';

                    // Dòng 1: CCCD & Họ tên
                    html += '<div class="d-flex align-items-center flex-wrap">';
                    html += '<a href="/Major/Subject/Detail/' + row.SubjectId + '" class="text-primary font-bolder text-95 mr-2" title="Xem chi tiết hồ sơ">';
                    html += '<i class="far fa-id-card mr-1"></i>' + (data || "---") + '</a>';
                    html += '<span class="font-bold text-dark-m1 text-95 mr-2">' + (row.FullName || "") + '</span>';
                    if (row.OtherName) {
                        html += '<span class="text-secondary text-85 font-normal mr-2">(Tên khác: ' + row.OtherName + ')</span>';
                    }
                    html += '</div>';

                    // Dòng 2: Giới tính | Ngày sinh | Quê quán
                    var details = [];
                    if (row.Gender) {
                        var genderIcon = row.Gender === "Nam" ? "fa-mars text-blue" : (row.Gender === "Nữ" ? "fa-venus text-pink" : "fa-genderless text-secondary");
                        details.push('<span class="text-85 text-dark-m3"><i class="fa ' + genderIcon + ' mr-1"></i>' + row.Gender + '</span>');
                    }
                    if (row.DateOfBirthStr) {
                        details.push('<span class="text-85 text-dark-m3"><i class="far fa-calendar-alt text-orange-d1 mr-1"></i>' + row.DateOfBirthStr + '</span>');
                    }
                    if (row.PlaceOfOrigin) {
                        details.push('<span class="text-85 text-dark-m3" title="' + row.PlaceOfOrigin.replace(/"/g, '&quot;') + '"><i class="fas fa-map-marker-alt text-danger-m1 mr-1"></i>' + row.PlaceOfOrigin + '</span>');
                    }

                    if (details.length > 0) {
                        html += '<div class="text-85 text-secondary mt-1 d-flex align-items-center flex-wrap">' +
                            details.join('<span class="mx-2 text-grey-l2">|</span>') +
                            '</div>';
                    }

                    html += '</div>';
                    return html;
                }
            },
            {
                "data": "SubjectTypeNames",
                "defaultContent": "",
                "className": "align-middle",
                "orderable": false,
                "render": function (data, type, row) {
                    if (!data) return '<span class="text-secondary-m2 font-italic text-85">Chưa phân loại</span>';
                    var types = data.split(',');
                    var badges = types.map(function (t) {
                        return '<span class="badge badge-light-blue text-primary-d2 border-1 brc-primary-m3 text-85 font-bold mr-1 mb-1 px-2 py-1 shadow-none"><i class="fas fa-tag mr-1 text-80"></i>' + (t ? t.trim() : "") + '</span>';
                    });
                    return '<div class="d-flex flex-wrap align-items-center">' + badges.join('') + '</div>';
                }
            },
            {
                "data": "TrackingUnitCount",
                "defaultContent": "1",
                "className": "text-center align-middle",
                "orderable": false,
                "render": function (data, type, row) {
                    if (type !== "display") return data;
                    var count = parseInt(data, 10) || 1;
                    var html = "";

                    if (count > 1) {
                        html += _renderButton(true,
                            "MonitoringUnits",
                            "btn btn-xs btn-outline-danger radius-round px-2 font-bolder text-85 shadow-xs d-inline-flex align-items-center",
                            "/Major/Subject/MonitoringUnits/" + row.SubjectId,
                            '<i class="fa fa-exclamation-triangle mr-1"></i> ' + count + ' đơn vị giám sát',
                            "Đơn vị quản lý & Giám sát đối tượng", "1150px");
                    } else {
                        html += _renderButton(true,
                            "MonitoringUnits",
                            "btn btn-xs btn-outline-default radius-round px-2 text-secondary-d2 font-600 text-85 d-inline-flex align-items-center",
                            "/Major/Subject/MonitoringUnits/" + row.SubjectId,
                            '<i class="fa fa-building mr-1"></i> 1 đơn vị',
                            "Đơn vị quản lý & Giám sát đối tượng", "1150px");
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
    setLookupFieldValue("SubjectTypeIds", data.subjectTypeIds);

    // Gán danh sách Loại đối tượng vào select2
    var $selectTypes = $("#ModalContent #selectSubjectTypeIds");
    if ($selectTypes.length === 0) $selectTypes = $("#selectSubjectTypeIds");
    if ($selectTypes.length > 0) {
        if (data.subjectTypeIds) {
            var typeArr = data.subjectTypeIds.split(',').map(function (s) { return s.trim(); });
            $selectTypes.val(typeArr).trigger('change');
        } else {
            $selectTypes.val([]).trigger('change');
        }
    }

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

        var $selectTypes = $("#ModalContent #selectSubjectTypeIds");
        if ($selectTypes.length === 0) $selectTypes = $("#selectSubjectTypeIds");
        if ($selectTypes.length > 0) {
            $selectTypes.val([]).trigger('change');
        }
        setLookupFieldValue("SubjectTypeIds", "");
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
 *  CHUAN HOA VA KIEM TRA SO CCCD
 *  CCCD Viet Nam gom dung 12 chu so. Chan go ky tu khac ngay tu dau va
 *  bao loi ro rang thay vi de nguoi dung luu roi moi bao.
 * ========================================================================= */

var CCCD_LENGTH = 12;

/* Chi giu chu so, cat toi da 12 ky tu */
function normalizeIdentityCard(value) {
    return String(value || "").replace(/[^0-9]/g, "").substring(0, CCCD_LENGTH);
}

/* Kiem tra va hien thong bao ngay duoi o nhap. Tra ve true neu hop le. */
function validateIdentityCardInput($input) {
    var value = normalizeIdentityCard($input.val());
    var $box = $("#lookupSubjectResult");

    if (value.length === 0) {
        $box.addClass("d-none").empty();
        $input.removeClass("is-invalid");
        return false;
    }

    if (value.length < CCCD_LENGTH) {
        $input.addClass("is-invalid");
        $box.removeClass("d-none")
            .html('<span class="text-danger"><i class="fa fa-exclamation-circle mr-1"></i>'
                + 'Số CCCD phải gồm đúng ' + CCCD_LENGTH + ' chữ số (hiện có ' + value.length + ').</span>');
        return false;
    }

    $input.removeClass("is-invalid");
    return true;
}

/* Chi cho go chu so trong o CCCD */
$(document).on("input", "#Subject #IdentityCardNumber", function () {
    var $this = $(this);
    var normalized = normalizeIdentityCard($this.val());
    if ($this.val() !== normalized) $this.val(normalized);

    // Xoa canh bao cu khi nguoi dung dang go lai
    if (normalized.length < CCCD_LENGTH) {
        $("#lookupSubjectResult").addClass("d-none").empty();
    }
});

/* Roi o nhap: kiem tra dinh dang, neu hop le thi goi y neu CCCD da ton tai.
   Dung cho man hinh THEM MOI - man hinh sua khong can goi y. */
$(document).on("blur", "#Subject #IdentityCardNumber", function () {
    var $input = $(this);

    if (!validateIdentityCardInput($input)) return;

    // Da co SubjectId nghia la dang sua ban ghi cu -> khong goi y
    var currentId = $("#Subject #SubjectId").val();
    if (currentId && currentId !== "00000000-0000-0000-0000-000000000000") return;

    suggestExistingSubject($input.val());
});

/* Hoi nguoi dung co muon tai thong tin da co len khong */
function suggestExistingSubject(identityCardNumber) {
    $.ajax({
        type: "GET",
        url: urlActions.Subject_LookupByCard,
        data: { identityCardNumber: identityCardNumber },
        dataType: "json",
        success: function (res) {
            if (!res || res.status !== true || !res.data) return;

            var d = res.data;
            var owner = d.isMine
                ? "Bạn đã từng khai báo đối tượng này."
                : "Đối tượng này đã được " + escapeLookupHtml(d.createdBy || "đơn vị khác") + " khai báo.";

            var html = [
                '<div class="alert alert-info py-2 px-3 mb-0 text-90">',
                '<div class="mb-1"><i class="fa fa-info-circle text-blue-d1 mr-1"></i>',
                '<b>Số CCCD này đã tồn tại trong hệ thống.</b></div>',
                '<div class="mb-1">', escapeLookupHtml(d.fullName || ""),
                d.dateOfBirthStr ? (' &mdash; sinh ngày ' + escapeLookupHtml(d.dateOfBirthStr)) : '',
                '</div>',
                '<div class="text-grey-d1 mb-2">', owner, '</div>',
                '<button type="button" class="btn btn-sm btn-primary py-1 px-2 mr-1" id="btnLoadSuggested">',
                '<i class="fa fa-download mr-1"></i>Tải thông tin lên</button>',
                '<button type="button" class="btn btn-sm btn-light py-1 px-2" id="btnDismissSuggested">',
                'Bỏ qua, tôi tự nhập</button>',
                '</div>'
            ].join("");

            $("#lookupSubjectResult").removeClass("d-none").html(html);
        }
    });
}

/* Tai thong tin da co len form */
$(document).on("click", "#btnLoadSuggested", function (e) {
    e.preventDefault();
    lookupSubjectByIdentityCard();
});

/* Bo qua goi y, tu nhap moi */
$(document).on("click", "#btnDismissSuggested", function (e) {
    e.preventDefault();
    $("#lookupSubjectResult").addClass("d-none").empty();
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
    $("#Search #SubjectTypeIds").val("");
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

/* Đồng bộ giá trị Loại đối tượng cho form Thêm / Sửa đối tượng */
function syncSubjectTypeIds() {
    var $sel = $("#selectSubjectTypeIds");
    if ($sel.length === 0) return;
    var vals = $sel.val();
    $("#SubjectTypeIds").val(vals ? (Array.isArray(vals) ? vals.join(",") : vals) : "");
}

/* Khởi tạo Select2 cho Loại đối tượng khi mở modal Add / Edit */
$(document).on("shown.bs.modal", "#modal_AddSubject, #modal_EditSubject", function () {
    var $sel = $(this).find("#selectSubjectTypeIds");
    if ($sel.length > 0 && $.fn.select2) {
        if (!$sel.hasClass("select2-hidden-accessible")) {
            $sel.select2({
                placeholder: "-- Chọn một hoặc nhiều loại đối tượng --",
                allowClear: false,
                width: "100%"
            });
        }
    }
    syncSubjectTypeIds();
});

/* Cache toàn bộ danh mục hành vi vi phạm ban đầu */
var _allSearchBehaviorOptions = [];

/* Khởi tạo bộ lọc tìm kiếm tại Index */
function initSearchFilters() {
    var $behaviors = $("#Search #BehaviorIds");
    if ($behaviors.length === 0) return;

    // Cache tất cả options lúc ban đầu
    if (_allSearchBehaviorOptions.length === 0) {
        $behaviors.find("option").each(function () {
            var val = $(this).val();
            if (val) {
                _allSearchBehaviorOptions.push({
                    id: String(val),
                    text: $(this).text().trim(),
                    fieldId: String($(this).attr("data-fieldid") || "")
                });
            }
        });
    }

    // Khởi tạo Select2 cho Hành vi vi phạm
    if ($.fn.select2) {
        if ($behaviors.hasClass("select2-hidden-accessible")) {
            $behaviors.select2("destroy");
        }
        $behaviors.select2({
            placeholder: "-- Tất cả hành vi vi phạm --",
            allowClear: true,
            width: "100%"
        });
    }

    // Lắng nghe sự kiện đổi Lĩnh vực (bắt cả native change lẫn Select2 events)
    $(document).off("change", "#Search #SearchFieldId").on("change", "#Search #SearchFieldId", function () {
        filterBehaviorsBySelectedField();
    });

    // Hỗ trợ phím Enter khi đang ở bất kỳ ô nhập tìm kiếm nào
    $("#Search input").off("keypress").on("keypress", function (e) {
        if (e.which === 13) {
            e.preventDefault();
            _tableSubject.ajax.reload(null, false);
        }
    });
}

function filterBehaviorsBySelectedField() {
    var fieldId = String($("#Search #SearchFieldId").val() || "").trim();
    var $behaviors = $("#Search #BehaviorIds");
    if ($behaviors.length === 0) return;

    var selectedVals = $behaviors.val() || [];
    if (!Array.isArray(selectedVals)) {
        selectedVals = selectedVals ? [selectedVals] : [];
    }

    // Xóa sạch options hiện tại trong thẻ select
    $behaviors.empty();

    // Lọc danh sách theo lĩnh vực
    var filtered = _allSearchBehaviorOptions;
    if (fieldId) {
        filtered = _allSearchBehaviorOptions.filter(function (item) {
            return item.fieldId === fieldId;
        });
    }

    // Thêm các option phù hợp vào thẻ select
    filtered.forEach(function (item) {
        var opt = new Option(item.text, item.id, false, false);
        $(opt).attr("data-fieldid", item.fieldId);
        $behaviors.append(opt);
    });

    // Giữ lại các giá trị đã chọn nếu còn hợp lệ
    var validRemaining = selectedVals.filter(function (v) {
        return filtered.some(function (item) { return item.id === String(v); });
    });
    $behaviors.val(validRemaining);

    // Kích hoạt lại Select2 hiển thị danh sách mới
    if ($.fn.select2) {
        if ($behaviors.hasClass("select2-hidden-accessible")) {
            $behaviors.select2("destroy");
        }
        $behaviors.select2({
            placeholder: "-- Tất cả hành vi vi phạm --",
            allowClear: true,
            width: "100%"
        });
    }
}

/* Xoá điều kiện tìm kiếm và reload lại bảng */
function resetSubjectSearch() {
    $("#Search #IdentityCardNumber").val("");
    $("#Search #FullName").val("");
    $("#Search #Gender").val("").trigger("change");
    $("#Search #SubjectTypeIds").val("").trigger("change");
    $("#Search #SearchFieldId").val("").trigger("change");
    filterBehaviorsBySelectedField();
    _tableSubject.ajax.reload(null, false);
}


