
var _CustomerActionURLs = {
    Customer_GetData: "/Major/Customer/Get"
};
var _tableCustomer;

$(document).ready(function () {
    initTableCustomer()
});

function Search() {
    _tableCustomer.ajax.reload(null, false);
}

function initTableCustomer() {
    _tableCustomer = $("#DSCustomer").DataTable({
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
            {
                "data": "TypeCus",
                "render": function (data, type, row, meta) {
                    if (data == 'CONSUMER') {
                        return '<span class="badge bgc-info brc-info text-white badge-lg arrowed arrowed-in-right mb-1"><span class="px-3">' + 'Cá nhân' + '</span></span>';
                    } else {
                        return '<span class="badge bgc-success brc-success text-white badge-lg arrowed arrowed-in-right mb-1"><span class="px-3">' + 'Doanh nghiệp' + '</span></span>';
                    }
                }
            },
            {
                "data": "CusName",
                "defaultContent": ""
            },
            {
                "data": "Address",
                "defaultContent": ""
            },
            {
                "data": "Phone",
                "defaultContent": ""
            },
            {
                "data": "CusId",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';
                    if (type === "display") {
                        html += '<div class="dropdown d-inline-block"><button class="btn px-4 btn-lighter-primary mr-1 v-hover dropdown-toggle" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="fa fa-ellipsis-h text-120"></i></button><div class="dropdown-menu dropdown-menu-right">';
                        html += _renderButton(true,
                            "EditCustomer",
                            "btn text-primary mr-1 dropdown-item",
                            "/Major/Customer/Edit/" + data,
                            '<i class="fa fa-edit text-primary text-120 mr-1"></i> Cập nhật',
                            "Cập nhật", 1024);
                        html += _renderButton(true,
                            "DeleteCustomer",
                            "btn text-danger mr-1 dropdown-item",
                            "/Major/Customer/Delete/" + data,
                            '<i class="far fa-trash-alt text-danger text-120 mr-1"></i> Xóa',
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
                    _initElement();
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
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}


function getCusType() {
    // Lấy giá trị của userType từ các phần tử radio button
    var CusType = $("input[name='TypeCus']:checked").val();
    //console.log(CusType)
    return CusType;
}