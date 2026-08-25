var _InvActionURLs = {
    Inv_GetData: "/Invoice/Inv/Get"
};
var _tableInv;
function Search() {
    _tableInv.ajax.reload(null, false);
}
$(document).ready(function () {
    initTableInv();
});

function initTableInv() {
    _tableInv = $("#DSInvoice").DataTable({
        "Responsive": true,
        "language": {
            "processing":
                "<div class='overlay'><i class='fas fa-cog fa-spin'></i></div>"
        },
        "lengthChange": true,
        "processing": true,
        "serverSide": true,
        "ajax":
        {
            "url": _InvActionURLs.Inv_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": {
                "Pattern": function () { return $("#SearchInv #Pattern").val() },
                "Serial": function () { return $("#SearchInv #Serial").val() },
                "InvNo": function () { return $("#SearchInv #InvNo").val() },
                "InvStatus": function () { return $("#SearchInv #InvStatus").val() },
                "InvType": function () { return $("#SearchInv #InvType").val() },
                "CreateBy": function () { return $("#SearchInv #CreateBy").val() },
                "CreateOn": function () { return $("#SearchInv #CreateOn").val() },
                "CreateTo": function () { return $("#SearchInv #CreateTo").val() },
                "CusName": function () { return $("#SearchInv #CusName").val() },
                "CusCode": function () { return $("#SearchInv #CusCode").val() },
                "CusTaxCode": function () { return $("#SearchInv #CusTaxCode").val() },
                "CusAddress": function () { return $("#SearchInv #CusAddress").val() }

            }
        },
        "columns": [
            {
                "data": "",
                "defaultContent": "1",
                "render": function (data, type, row, meta) {
                    return meta.settings._iDisplayStart + meta.row + 1;
                }
            },
            {
                "data": "InvNo",
                "defaultContent": ""
            },
            {
                "data": "CusName",
                "defaultContent": ""
            },
            {
                "data": "CusCode",
                "defaultContent": ""
            },
            {
                "data": "InvTypeName",
                "defaultContent": ""
            },
            {
                "data": "InvStatusName",
                "defaultContent": ""
            },
            {
                "data": "InvId",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';
                    if (type === "display") {
                        html += _renderButton(true,
                            "Detail",
                            "btn px-4 btn-lighter-primary mr-1 v-hover",
                            "/Invoice/Inv/Detail/" + data,
                            '<i class="far fa-eye text-primary text-120"></i>',
                            "Xem chi tiết");
                        if (row.InvStatus != 5 && row.InvStatus != 6) {

                                html += _renderButton(true,
                                    "AdjustInv",
                                    "btn px-4 btn-lighter-info mr-1 v-hover",
                                    "/Invoice/Inv/Adjust/" + data,
                                    '<i class="fa fa-edit text-info text-120"></i>',
                                    "Hiệu chỉnh");
                            if (row.InvStatus != 4) {
                                html += _renderButton(true,
                                    "CancelInv",
                                    "btn px-4 btn-lighter-danger mr-1 v-hover",
                                    "/Invoice/Inv/Cancel/" + data,
                                    '<i class="far fa-trash-alt text-danger text-120"></i>',
                                    "Huỷ hoá đơn");
                            }
                        }
                    }
                    html += "</span>";
                    return html;
                }
            }
        ]
    });
}

function Inv_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableInv.ajax.reload(null, false);
                response.status = undefined;
                var urlAction = $("#ModalContent #modal_" + formId + " form").attr("action");
                $("#ModalContent #modal_" + formId + " #modal-content").load(urlAction, function (data, textStatus, xhr) {
                    _initElement();
                });
            }
        } else {
            $("#ModalContent #modal_" + formId).modal("hide");
            $("#ModalContent #modal_" + formId).on("hidden.bs.modal",
                function () {
                    if (response.status != undefined) {
                        eval(response.message);
                        _tableInv.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}

