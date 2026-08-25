var _SubjectActionURLs = {
    Subject_GetData: "/Major/Subject/Get",
    Subject_UploadFile: "/Major/Subject/UploadFile"
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
                d.Key = $("#Search #Key").val();
                d.Gender = $("#Search #Gender").val();
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
                "data": "AvatarUrl",
                "defaultContent": "",
                "className": "text-center align-middle",
                "orderable": false,
                "render": function (data) {
                    var src = data ? data : "/Contents/Base/imgs/avatar-default.png";
                    return '<img src="' + src + '" class="radius-round border-1 brc-primary-m3" style="width:42px;height:42px;object-fit:cover;" />';
                }
            },
            {
                "data": "IdentityCardNumber",
                "defaultContent": "",
                "className": "font-bold text-primary align-middle",
                "render": function (data, type, row) {
                    return '<a href="/Major/Subject/Detail/' + row.SubjectId + '" class="text-primary font-bold"><i class="far fa-id-card mr-1"></i>' + data + '</a>';
                }
            },
            {
                "data": "FullName",
                "defaultContent": "",
                "className": "align-middle",
                "render": function (data, type, row) {
                    var html = '<a href="/Major/Subject/Detail/' + row.SubjectId + '" class="font-bold text-dark-m1">' + data + '</a>';
                    if (row.OtherName) {
                        html += '<br><small class="text-secondary">(Tên gọi khác: ' + row.OtherName + ')</small>';
                    }
                    return html;
                }
            },
            {
                "data": "DateOfBirthStr",
                "defaultContent": "",
                "className": "text-center align-middle"
            },
            {
                "data": "Gender",
                "defaultContent": "",
                "className": "text-center align-middle"
            },
            {
                "data": "PhoneNumber",
                "className": "align-middle",
                "defaultContent": ""
            },
            {
                "data": "CurrentResidence",
                "className": "align-middle",
                "defaultContent": ""
            },
            {
                "data": "ViolationCount",
                "defaultContent": 0,
                "className": "text-center align-middle",
                "render": function (data) {
                    if (data > 0) {
                        return '<span class="badge badge-danger text-110 px-2 py-1"><i class="fas fa-exclamation-triangle mr-1"></i>' + data + ' lần</span>';
                    }
                    return '<span class="badge badge-secondary px-2 py-1">Chưa có</span>';
                }
            },
            {
                "data": "SubjectId",
                "className": "text-center align-middle",
                "orderable": false,
                "render": function (data, type, row) {
                    var html = '<span class="d-none d-lg-inline">';
                    if (type === "display") {
                        html += '<a href="/Major/Subject/Detail/' + data + '" class="btn px-2 btn-lighter-info mr-1 v-hover" title="Xem hồ sơ chi tiết"><i class="far fa-eye text-info text-120"></i></a>';
                        html += _renderButton(true,
                            "EditSubject",
                            "btn px-2 btn-lighter-primary mr-1 v-hover",
                            "/Major/Subject/Edit/" + data,
                            '<i class="far fa-edit text-primary text-120"></i>',
                            "Cập nhật", { "data-width": "1350px" });
                        html += _renderButton(true,
                            "DeleteSubject",
                            "btn px-2 btn-lighter-danger mr-1 v-hover",
                            "/Major/Subject/Delete/" + data,
                            '<i class="far fa-trash-alt text-danger text-120"></i>',
                            "Xoá");
                    }
                    html += "</span>";
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
