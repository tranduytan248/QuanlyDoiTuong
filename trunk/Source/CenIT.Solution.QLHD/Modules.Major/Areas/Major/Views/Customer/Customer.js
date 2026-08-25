
var _CustomerActionURLs = {
    Customer_GetData: "/Major/Customer/Get"
};
var _tableCustomer;

$(document).ready(function () {
    initTableCustomer()
});

function onSearchCus() {
    $("#DSCustomer").parents('.table-responsive-md').removeClass('d-none');
    _tableCustomer.ajax.reload(null, false);
}

function initTableCustomer() {
    _tableCustomer = $("#DSCustomer").DataTable({
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
            "url": _CustomerActionURLs.Customer_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": function(d){
                d.Keyword = $("#Search #Keyword").val();
                d.TypeCus = getCusType();
            }
        },
        "columns": [
            {
                "data": "",
                "className": "text-center",
                "defaultContent": "1",
                "render": function (data, type, row, meta) {
                    return meta.settings._iDisplayStart + meta.row + 1;
                }
            },
            //{
            //    "data": "TypeCus",
            //    "render": function (data, type, row, meta) {
            //        if (data == 'CONSUMER') {
            //            return '<span class="badge bgc-info brc-info text-white badge-lg arrowed arrowed-in-right mb-1"><span class="px-2">' + 'Cá nhân' + '</span></span>';
            //        } else {
            //            return '<span class="badge bgc-success brc-success text-white badge-lg arrowed arrowed-in-right mb-1"><span class="px-2">' + 'Doanh nghiệp' + '</span></span>';
            //        }
            //    }
            //},
            {
                "data": "CusName",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    if (type == 'display') {
                        var arrTypeCus = { "CONSUMER": "info", "BUSINESS": "success" };
                        var arrTypeCusName = { "CONSUMER": "Cá nhân", "BUSINESS": "Doanh nghiệp" };

                        return `<span class="d-table badge bgc-{1} brc-{1} text-white badge-lg arrowed arrowed-in-right mb-1"><span class="px-2">{2}</span></span><span class="text-105 text-600 text-primary-d3">{0}</span> <div class="mt-2"><span class="d-table badge radius-2 bgc-white border-1 brc-default-m2 btn-text-default text-95 px-2 py-1 m-2px"><span class="pos-rel">{3}</span></span><span class="d-inline-block badge radius-2 bgc-white border-1 brc-red-m2 btn-text-red text-95 px-2 py-1 m-2px text-500"><i class="fa fa-phone text-danger"></i> {4}</span>${(row.TaxCode != null ? `<span class="badge bgc-white border-1 brc-purple-m2 btn-text-purple radius-2 text-95" data-rel="tooltip" title="Mã số thuế"><i class="fas fa-code text-purple"></i>&nbsp;${row.TaxCode}</span>`:"")}{5}</div>`.format(data, arrTypeCus[row.TypeCus], arrTypeCusName[row.TypeCus], row.Address, row.Phone.replace(/(\d{3})(\d{3})(\d{4})/, "$1 $2 $3"), (row.IdentifierNo != null && row.IdentifierNo.length > 0 ? `<span class="badge badge-info badge-lg arrowed-in-right mb-1">{0}</span><span class="badge btn-pink badge-lg arrowed ml-n1 mb-1">{1}</span>`.format(row.TypeIdentifierName, row.IdentifierNo) : ""));
                    }

                    return data;
                }
            },
            //{
            //    "data": "Address",
            //    "defaultContent": ""
            //},
            //{
            //    "data": "Phone",
            //    "defaultContent": ""
            //},
            {
                "data": "CusId",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';
                    if (type === "display") {
                        /*html += '<div class="dropdown d-inline-block"><button class="btn px-4 btn-lighter-primary mr-1 v-hover dropdown-toggle" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="fa fa-ellipsis-h text-120"></i></button><div class="dropdown-menu dropdown-menu-right">';*/

                        html += _renderButton(true,
                            "EditCustomer",
                            "btn px-4 btn-outline-primary mr-1 v-hover",
                            "/Major/Customer/Edit/" + data,
                            '<i class="fa fa-edit text-120"></i>',
                            "Cập nhật", 860);

                        html += _renderButton(true,
                            "DeleteCustomer",
                            "btn btn px-4 btn-outline-danger mr-1 v-hover",
                            "/Major/Customer/Delete/" + data,
                            '<i class="far fa-trash-alt text-120"></i>',
                            "Xóa");
                    }
                    html += "</span>";

                    return html;
                }
            }
        ],
        "createdRow": function (row, data, dataIndex) {
            $(row).addClass("d-style bgc-h-default-l4");
        }
    });
}

function Customer_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableCustomer.ajax.reload(null, false);
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
                        _tableCustomer.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #CusInfo").html(response).promise().done(function () {_initElements(this);});
    }
}

function getCusType() {
    // Lấy giá trị của userType từ các phần tử radio button
    var CusType = $("input[name='TypeCus']:checked").val();
    //console.log(CusType)
    return CusType;
}