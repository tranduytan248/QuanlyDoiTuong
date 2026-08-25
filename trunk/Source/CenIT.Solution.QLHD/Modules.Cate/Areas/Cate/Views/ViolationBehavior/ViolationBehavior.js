var _ViolationBehaviorActionURLs = {
    ViolationBehavior_GetData: "/Cate/ViolationBehavior/Get"
};
var _tableViolationBehavior;
$(document).ready(function () {
    initTableViolationBehavior();
});

function initTableViolationBehavior() {
    _tableViolationBehavior = $("#DSViolationBehavior").DataTable({
        "responsive": true,
        "language": {
            "processing": "<div class='overlay'><i class='fas fa-cog fa-spin'></i></div>",
            "emptyTable": "Không có dữ liệu",
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
            "url": _ViolationBehaviorActionURLs.ViolationBehavior_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": function (d) {
                d.Key = $("#Search #Key").val();
                d.FieldId = $("#Search #Search_FieldId").val();
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
                "data": "BehaviorCode",
                "defaultContent": "",
                "className": "font-bold text-primary"
            },
            {
                "data": "BehaviorName",
                "defaultContent": ""
            },
            {
                "data": "FieldName",
                "defaultContent": "",
                "render": function (data, type, row) {
                    return row.FieldCode ? '<b>[' + row.FieldCode + ']</b> ' + data : data;
                }
            },
            {
                "data": "Description",
                "defaultContent": ""
            },
            {
                "data": "IsActive",
                "className": "text-center",
                "render": function (data) {
                    return data ? '<span class="badge badge-success">Hoạt động</span>' : '<span class="badge badge-secondary">Tạm dừng</span>';
                }
            },
            {
                "data": "BehaviorId",
                "className": "text-center",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';
                    if (type === "display") {
                        html += _renderButton(true,
                            "EditViolationBehavior",
                            "btn px-2 btn-lighter-primary mr-1 v-hover",
                            "/Cate/ViolationBehavior/Edit/" + data,
                            '<i class="far fa-edit text-primary text-120"></i>',
                            "Cập nhật");
                        html += _renderButton(true,
                            "DeleteViolationBehavior",
                            "btn px-2 btn-lighter-danger mr-1 v-hover",
                            "/Cate/ViolationBehavior/Delete/" + data,
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

function ViolationBehavior_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableViolationBehavior.ajax.reload(null, false);
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
                    _tableViolationBehavior.ajax.reload(null, false);
                    response.status = undefined;
                }
            });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}
