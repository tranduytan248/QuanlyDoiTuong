var _InvActionURLs = {
    Inv_GetData: "/Invoice/EInvoiceAdjust/Get"
};
var _tableInv;
function Search() {
    _tableInv.ajax.reload(null, false);
}
$(document).ready(function () {
    initTableInv();
});

function initTableInv() {
    _tableInv = $("#DSEInvoiceAdjust").DataTable({
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
                "Pattern": function () { return $("#SearchEInvoiceAdjust #Pattern").val() },
                "Serial": function () { return $("#SearchEInvoiceAdjust #Serial").val() },
                "InvNo": function () { return $("#SearchEInvoiceAdjust #InvNo").val() },
                "InvType": function () { return $("#SearchEInvoiceAdjust #InvType").val() },
                "CreateBy": function () { return $("#SearchEInvoiceAdjust #CreateBy").val() },
                "CreateOn": function () { return $("#SearchEInvoiceAdjust #CreateOn").val() },
                "CreateTo": function () { return $("#SearchEInvoiceAdjust #CreateTo").val() },
                "CusName": function () { return $("#SearchEInvoiceAdjust #CusName").val() },
                "CusTaxCode": function () { return $("#SearchEInvoiceAdjust #CusTaxCode").val() },
                "CusAddress": function () { return $("#SearchEInvoiceAdjust #CusAddress").val() }

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
                "data": "ElnvAjustedInvNo",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span>';
                        html += _renderButton(true,
                            "Detail",
                            "btn px-4 btn-lighter-primary  mr-1",
                            "/Invoice/Inv/Detail/" + row.ElnvAjustedInvId,
                            '<span>'+data+'</span>',
                            "Xem chi tiết");
                    html += "</span>";
                    return html;
                }
            },
            {
                "data": "ElnvAjustedPattern",
                "defaultContent": ""
            },
            {
                "data": "ElnvAjustedSerial",
                "defaultContent": ""
            },
            {
                "data": "ElnvAjustInvNo",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span>';
                    html += _renderButton(true,
                        "Detail",
                        "btn px-4 btn-lighter-primary  mr-1",
                        "/Invoice/Inv/Detail/" + row.ElnvAjustInvId,
                        '<span>' + data + '</span>',
                        "Xem chi tiết");
                    html += "</span>";
                    return html;
                }
            },
            {
                "data": "ElnvAjustPattern",
                "defaultContent": ""
            },
            {
                "data": "ElnvAjustSerial",
                "defaultContent": ""
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

