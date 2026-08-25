var _ContractPaymentActionURLs = {
    ContractPayment_GetData: "/Major/ContractPayment/GetPayments"
};

var _tableContractPayment;

$(document).ready(function () {
    initTableContractPayment()
});

function Search() {
    _tableContractPayment.ajax.reload(null, false);
}

function initTableContractPayment() {
    _tableContractPayment = $("#DSContractPayment").DataTable({
        "Responsive": true,
        "language": {
            "processing": '<div id="tableLoading" class="pageload-overlay show pageload-loading" data-opening="M 0,0 80,-10 80,60 0,70 0,0" data-closing="M 0,-10 80,-20 80,-10 0,0 0,-10" style=""><svg xmlns="http://www.w3.org/2000/svg" width="100%" height="100%" viewBox="0 0 80 60" preserveAspectRatio="none"><path d="M 0,70 80,60 80,80 0,80 0,70"></path></svg></div>'
                //"<div class='overlay'><i class='fas fa-cog fa-spin'></i></div>"
        },
        "lengthChange": true,
        "processing": true,
        "serverSide": true,
        "ajax":
        {
            "url": _ContractPaymentActionURLs.ContractPayment_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": {
                "ContractId": function () { return $("#bodyForm #ContractId").val() ? $("#bodyForm #ContractId").val() : 0; },
            }
        },
        "columns": [
            {
                "data": "",
                "className": "text-center",
                "defaultContent": "1",
                "render": function (data, type, row, meta) {
                    if (type == "display") { return "Lần {0}".format(meta.row + 1); }
                    return meta.row+1;
                }
            },
            {
                "data": "Status",
                "render": function (data, type, row, meta) {
                    if (data == 1) {
                        return '<span class="badge bgc-info brc-info text-white badge-lg arrowed arrowed-in-right mb-1"><span class="px-3">' + row.StatusName + '</span></span>';
                    } else {
                        return '<span class="badge bgc-success brc-success text-white badge-lg arrowed arrowed-in-right mb-1"><span class="px-3">' + row.StatusName + '</span></span>';
                    }
                }
            },
            {
                "data": "PaidAmount",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    if (type === "display") {
                        return VND.format(data);
                    }
                    return data;
                }
            },
            {
                "data": "PaidOn",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    if (type == "display") {
                        if (data == null) return "";
                        return moment(data).format("DD/MM/YYYY");
                    }
                    return data;
                }
            },
            {
                "data": "RefDocNo",
                "sortable": false,
                "defaultContent": ""
            },
            {
                "data": "PaymentInfo",
                "sortable": false,
                "defaultContent": ""
            },
            {
                "data": "PaymentId",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';

                    if (type === "display") {
                        html += _renderButton(true,
                            "EditPayment",
                            "btn px-4 btn-outline-primary mr-1 v-hover",
                            "/Major/ContractPayment/EditPayment?paymentId=" + data + "&contractId=" + row.ContractId,
                            '<i class="far fa-edit text-120"></i>',
                            "Cập nhật");

                        //html += _renderButton(true,
                        //    "DeletePayment",
                        //    "btn px-4 btn-outline-danger mr-1 v-hover",
                        //    "/Major/ContractPayment/DeletePayment?paymentId=" + data + "&contractId=" + row.ContractId,
                        //    '<i class="far fa-trash-alt text-120"></i>',
                        //    "Xoá");
                    }
                    html += "</span>";

                    return html;
                }
            }
        ],
        "createdRow": function (row, data, dataIndex) {
            $(row).addClass("d-style bgc-h-default-l4");
        },
        //"footerCallback": function (row, data, start, end, display) {
        //    var api = this.api();

        //    api.columns('#Amount', {
        //        page: 'current'
        //    }).every(function () {
        //        var totalPaidAmount = this
        //            .data()
        //            .reduce(function (a, b) {
        //                var x = parseFloat(a) || 0;
        //                var y = parseFloat(b) || 0;
        //                return x + y;
        //            }, 0);

        //        var totalAmount = @Model.Total ?? 0;

        //        if (totalAmount == totalPaidAmount) {
        //            $('a[data-modal-id="AddPayment"]').addClass("d-none");
        //        }
        //        else {
        //            $('a[data-modal-id="AddPayment"]').removeClass("d-none");
        //        }
        //    });
        //}
    });
}

function ContractPayment_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableContractPayment.ajax.reload(null, false);
                response.status = undefined;
                //$("#ModalContent #modal_" + formId + " form")[0].reset();
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
                        _tableContractPayment.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response).promise().done(function () {_initElements(this);});
    }
}