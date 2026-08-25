
var _FlowContractActionURLs = {
    FlowContract_GetData: "/Cate/FlowContract/Get"
};
var _tableFlowContract;

$(document).ready(function () {
    initTableFlowContract()
});

function Search() {
    _tableFlowContract.ajax.reload(null, false);
}

function initTableFlowContract() {
    _tableFlowContract = $("#DSFlowContract").DataTable({
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
            "url": _FlowContractActionURLs.FlowContract_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": {

            }
        },
        "columns": [
            {
                "data": "",
                "className": "text-center",
                "defaultContent": "1",
                "render": function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            //{
            //    "data": "ContractFlowTemplateId",
            //    "defaultContent": ""
            //},
            {
                "data": "Name",
                "defaultContent": ""
            },
            {
                "data": "Description",
                "defaultContent": ""
            },
            {
                "data": "Disable",
                "render": function (data, type, row, meta) {
                    if (data == 'false') {
                        return '<span class="badge badge-pill badge-success">Đang hoạt động</span>';
                    } else {
                        return '<span class="badge badge-pill badge-danger">Ngưng hoạt động</span>';
                    }
                }
            },
            {
                "data": "ContractFlowTemplateId",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';
                    if (type === "display") {
                        html += _renderButton(true,
                            "DetailsFlowContract",
                            "btn px-4 btn-lighter-info mr-1 v-hover",
                            "/Cate/FlowContract/Details/" + data,
                            '<i class="fa fa-eye text-info text-120"></i>',
                            "Xem chi tiết", "1024px");
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

function FlowContract_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableFlowContract.ajax.reload(null, false);
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
                        _tableFlowContract.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}