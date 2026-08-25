var _InvActionURLs = {
    Inv_GetData: "/Invoice/InvAdjust/Get"
};

var _tableAdjustInvs;

$(document).ready(function () {
    initTableInv();
});

function onSearchAdjustInvs() {
    $("#DSAdjustInvs").parents('.table-responsive-md').removeClass('d-none');
    _tableAdjustInvs.ajax.reload(null, false);
}

function initTableInv() {
    _tableAdjustInvs = $("#DSAdjustInvs").DataTable({
        "Responsive": true,
        "language": {
            "processing": '<div id="tableLoading" class="pageload-overlay show pageload-loading" data-opening="M 0,0 80,-10 80,60 0,70 0,0" data-closing="M 0,-10 80,-20 80,-10 0,0 0,-10" style=""><svg xmlns="http://www.w3.org/2000/svg" width="100%" height="100%" viewBox="0 0 80 60" preserveAspectRatio="none"><path d="M 0,70 80,60 80,80 0,80 0,70"></path></svg></div>'
                //"<div class='overlay'><i class='fas fa-cog fa-spin'></i></div>"
        },
        "lengthChange": true,
        "processing": true,
        "serverSide": true,
        "deferLoading": 0,
        "ajax":
        {
            "url": _InvActionURLs.Inv_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": {
                "Pattern": function () { return $("#SearchInv #Pattern").val() },
                "Serials": function () { return $("#SearchInv #ListSerials").val() },
                "InvNo": function () { return $("#SearchInv #InvNo").val() },
                "InvStatus": function () { return $("#SearchInv #ListStatusInvs").val() },
                "InvTypes": function () { return $("#SearchInv #ListTypeInvs").val() },
                "Creators": function () { return $("#SearchInv #ListCreators").val() },
                "CreatedFrom": function () { return $("#SearchInv #CreatedFrom").val() },
                "CreatedTo": function () { return $("#SearchInv #CreatedTo").val() },
                "CusName": function () { return $("#SearchInv #CusName").val() },
                "CusCode": function () { return $("#SearchInv #CusCode").val() },
                "CusTaxCode": function () { return $("#SearchInv #CusTaxCode").val() },
                "UnionIds": function () {
                    return $("#SearchInv select#ListUnionIds").val() != null &&
                        $("#SearchInv select#ListUnionIds").val().length > 0
                        ? $("#SearchInv select#ListUnionIds").val()
                        : "";
                }
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
                "data": "AdjustedInvNo",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span>';
                        html += _renderButton(true,
                            "Detail",
                            "btn px-4 btn-lighter-primary  mr-1",
                            "/Invoice/Inv/Detail/" + row.AdjustedInvId,
                            '<span>'+data+'</span>',
                            "Xem chi tiết");
                    html += "</span>";
                    return html;
                }
            },
            {
                "data": "AdjustedInvPattern",
                "defaultContent": ""
            },
            {
                "data": "AdjustedInvSerial",
                "defaultContent": ""
            },
            {
                "data": "AdjustInvNo",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span>';
                    html += _renderButton(true,
                        "Detail",
                        "btn px-4 btn-lighter-primary  mr-1",
                        "/Invoice/Inv/Detail/" + row.AdjustInvId,
                        '<span>' + data + '</span>',
                        "Xem chi tiết");
                    html += "</span>";
                    return html;
                }
            },
            {
                "data": "AdjustInvPattern",
                "defaultContent": ""
            },
            {
                "data": "AdjustInvSerial",
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
                _tableAdjustInvs.ajax.reload(null, false);
                response.status = undefined;
                var urlAction = $("#ModalContent #modal_" + formId + " form").attr("action");
                $("#ModalContent #modal_" + formId + " #modal-content").load(urlAction, function (data, textStatus, xhr) {
                    _initElements(this);
                });
            }
        } else {
            $("#ModalContent #modal_" + formId).modal("hide");
            $("#ModalContent #modal_" + formId).on("hidden.bs.modal",
                function () {
                    if (response.status != undefined) {
                        eval(response.message);
                        _tableAdjustInvs.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response).promise().done(function () {_initElements(this);});
    }
}

