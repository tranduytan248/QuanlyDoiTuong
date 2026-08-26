var _SubjectViolationActionURLs = {
    SubjectViolation_GetData: "/Major/SubjectViolation/Get",
    SubjectViolation_UploadImages: "/Major/SubjectViolation/UploadViolationImages"
};
var _tableSubjectViolation;
$(document).ready(function () {
    initTableSubjectViolation();
});

function initTableSubjectViolation() {
    if ($("#DSSubjectViolation").length > 0) {
        _tableSubjectViolation = $("#DSSubjectViolation").DataTable({
            "responsive": true,
            "language": {
                "processing": "<div class='overlay'><i class='fas fa-cog fa-spin'></i></div>",
                "emptyTable": "Không có dữ liệu vi phạm",
                "info": "Hiển thị _START_ đến _END_ của _TOTAL_ lần vi phạm",
                "infoEmpty": "Hiển thị 0 đến 0 của 0 lần vi phạm",
                "lengthMenu": "Hiển thị _MENU_ dòng",
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
                "url": _SubjectViolationActionURLs.SubjectViolation_GetData,
                "type": "POST",
                "dataType": "JSON",
                "data": function (d) {
                    d.Key = $("#Search #Key").val();
                    d.SubjectId = $("#Search #Search_SubjectId").val();
                    d.FieldId = $("#Search #Search_FieldId").val();
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
                    "data": "ViolationDateStr",
                    "defaultContent": "",
                    "className": "font-bold text-danger align-middle"
                },
                {
                    "data": "IdentityCardNumber",
                    "className": "align-middle font-bold text-primary",
                    "defaultContent": ""
                },
                {
                    "data": "SubjectName",
                    "defaultContent": "",
                    "className": "align-middle",
                    "render": function (data, type, row) {
                        return '<a href="/Major/Subject/Detail/' + row.SubjectId + '" class="font-bold text-primary">' + data + '</a>';
                    }
                },
                {
                    "data": "BehaviorNames",
                    "className": "align-middle text-danger font-bold",
                    "defaultContent": ""
                },
                {
                    "data": "TreatmentMeasures",
                    "className": "align-middle",
                    "defaultContent": "",
                    "render": function (data) {
                        if (!data) return "";
                        var text = data.replace(/<[^>]*>?/gm, '');
                        return text.length > 80 ? text.substring(0, 80) + '...' : text;
                    }
                },
                {
                    "data": "Images",
                    "defaultContent": "",
                    "className": "text-center align-middle",
                    "render": function (data) {
                        if (!data) return '<span class="text-secondary">-</span>';
                        var imgs = data.split(/[,;]/);
                        return '<span class="badge badge-info"><i class="far fa-images mr-1"></i>' + imgs.length + ' ảnh</span>';
                    }
                },
                {
                    "data": "ViolationId",
                    "defaultContent": "",
                    "className": "text-center align-middle",
                    "orderable": false,
                    "render": function (data, type, row) {
                        var html = '<span class="d-none d-lg-inline">';
                        if (type === "display") {
                            html += _renderButton(true,
                                "EditSubjectViolation",
                                "btn px-2 btn-lighter-primary mr-1 v-hover",
                                "/Major/SubjectViolation/Edit/" + data,
                                '<i class="far fa-edit text-primary text-120"></i>',
                                "Cập nhật", { "data-width": "960px" });
                            html += _renderButton(true,
                                "DeleteSubjectViolation",
                                "btn px-2 btn-lighter-danger mr-1 v-hover",
                                "/Major/SubjectViolation/Delete/" + data,
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
}

function SubjectViolation_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                if (typeof _tableSubjectViolation !== "undefined") {
                    _tableSubjectViolation.ajax.reload(null, false);
                }
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
                    if (typeof _tableSubjectViolation !== "undefined") {
                        _tableSubjectViolation.ajax.reload(null, false);
                    } else {
                        location.reload();
                    }
                    response.status = undefined;
                }
            });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}

function handleViolationImagesUpload(input) {
    if (input.files && input.files.length > 0) {
        var formData = new FormData();
        for (var i = 0; i < input.files.length; i++) {
            formData.append("files[" + i + "]", input.files[i]);
        }
        $.ajax({
            url: _SubjectViolationActionURLs.SubjectViolation_UploadImages,
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (res) {
                if (res.status && res.urls) {
                    var current = $("#Violation_Images").val();
                    var arr = current ? current.split(",") : [];
                    for (var j = 0; j < res.urls.length; j++) {
                        arr.push(res.urls[j]);
                        $("#containerViolationImages").append(
                            '<div class="position-relative d-inline-block mr-2 mb-2">' +
                            '<img src="' + res.urls[j] + '" class="radius-1 border-1 brc-default-m2" style="width:90px;height:70px;object-fit:cover;" />' +
                            '<button type="button" class="btn btn-xs btn-danger position-absolute" style="top:2px;right:2px;" onclick="removeViolationImage(this, \'' + res.urls[j] + '\')"><i class="fas fa-times"></i></button>' +
                            '</div>'
                        );
                    }
                    $("#Violation_Images").val(arr.join(","));
                } else {
                    alert(res.message || "Tải ảnh thất bại.");
                }
            },
            error: function () {
                alert("Lỗi máy chủ khi tải ảnh.");
            }
        });
    }
}

function removeViolationImage(btn, url) {
    $(btn).closest(".position-relative").remove();
    var current = $("#Violation_Images").val();
    var arr = current ? current.split(",") : [];
    arr = arr.filter(function (item) { return item !== url; });
    $("#Violation_Images").val(arr.join(","));
}

/* Bo chon hanh vi vi pham nay da chuyen sang giao dien tabs + checkbox,
   dat truc tiep trong _Violation.cshtml (onViolationBehaviorChange /
   updateViolationBehaviorsUI). Ham dong bo select2 cu khong con dung nua. */
