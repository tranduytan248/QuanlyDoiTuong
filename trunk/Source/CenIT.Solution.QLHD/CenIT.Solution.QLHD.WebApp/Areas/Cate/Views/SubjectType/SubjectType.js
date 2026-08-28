﻿var _SubjectTypeActionURLs = {
    SubjectType_GetData: "/Cate/SubjectType/Get"
};
var _tableSubjectType;

$(document).ready(function () {
    initTableSubjectType();
});

function initTableSubjectType() {
    _tableSubjectType = $("#DSSubjectType").DataTable({
        "responsive": true,
        "language": {
            "processing": "<div class='overlay'><i class='fas fa-cog fa-spin'></i></div>",
            "emptyTable": "Không có dữ liệu loại đối tượng",
            "info": "Hiển thị _START_ đến _END_ của _TOTAL_ dòng",
            "infoEmpty": "Hiển thị 0 đến 0 của 0 dòng",
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
            "url": _SubjectTypeActionURLs.SubjectType_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": function (d) {
                d.Key = $("#Search #Key").val();
            }
        },
        "columns": [
            {
                "data": null,
                "defaultContent": "",
                "className": "text-center",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    return meta.settings._iDisplayStart + meta.row + 1;
                }
            },
            {
                "data": "SubjectTypeCode",
                "defaultContent": "",
                "className": "font-bold text-primary"
            },
            {
                "data": "SubjectTypeName",
                "defaultContent": "",
                "className": "font-600 text-dark-m1"
            },
            {
                "data": "Description",
                "defaultContent": "",
                "className": "text-secondary"
            },
            {
                "data": "SortOrder",
                "defaultContent": "0",
                "className": "text-center"
            },
            {
                "data": "IsActive",
                "className": "text-center",
                "render": function (data) {
                    return data
                        ? '<span class="badge badge-success px-2 py-1"><i class="fa fa-check mr-1"></i>Hoạt động</span>'
                        : '<span class="badge badge-secondary px-2 py-1"><i class="fa fa-pause mr-1"></i>Tạm dừng</span>';
                }
            },
            {
                "data": "SubjectTypeId",
                "className": "text-center",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';
                    if (type === "display") {
                        html += _renderButton(true,
                            "EditSubjectType",
                            "btn px-2 btn-lighter-primary mr-1 v-hover",
                            "/Cate/SubjectType/Edit/" + data,
                            '<i class="far fa-edit text-primary text-120"></i>',
                            "Cập nhật");
                        html += _renderButton(true,
                            "DeleteSubjectType",
                            "btn px-2 btn-lighter-danger mr-1 v-hover",
                            "/Cate/SubjectType/Delete/" + data,
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

function SubjectType_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableSubjectType.ajax.reload(null, false);
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
                    _tableSubjectType.ajax.reload(null, false);
                    response.status = undefined;
                }
            });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}
