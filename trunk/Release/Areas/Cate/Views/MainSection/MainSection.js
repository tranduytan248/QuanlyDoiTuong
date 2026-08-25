
var _mainSectionActionURLs = {
    MainSection_GetData: "/Cate/MainSection/Get"
};
var _tableMainSection;

$(document).ready(function () {
    initTableMainSection()
});

function Search() {
    _tableMainSection.ajax.reload(null, false);
}

function initTableMainSection() {
    _tableMainSection = $("#DSMainSection").DataTable({
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
            "url": _mainSectionActionURLs.MainSection_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": {
                "loaiHopDong": function () { return $("#SearchMainSection #ContractTypeId").val() ? $("#SearchMainSection #ContractTypeId").val() : 0; },
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
            {
                "data": "MainSectionName",
                "defaultContent": ""
            },
            {
                "data": "ContractTypeName",
                "defaultContent": ""
            },
            {
                "data": "MainSectionId",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';
                    if (type === "display") {
                        html += '<div class="dropdown d-inline-block"><button class="btn px-4 btn-lighter-primary mr-1 v-hover dropdown-toggle" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="fa fa-ellipsis-h text-120"></i></button><div class="dropdown-menu dropdown-menu-right">';
                        //html += '<a href="/Cate/SubSection/Index/' + data + '" class="btn text-warning mr-1 dropdown-item" data-rel="tooltip" title="" data-original-title="Đơn giá"><i class="fas fa-money-bill-alt text-warning text-120 mr-1"></i> Đơn giá</a>';
                        html += _renderButton(true,
                            "GetSubSection",
                            "btn text-warning mr-1 dropdown-item",
                            "/Cate/SubSection/Index/" + data,
                            '<i class="fas fa-money-bill-alt text-warning text-120 mr-1"></i> Đơn giá',
                            "Đơn giá", "1024px");
                        html += _renderButton(true,
                            "EditMainSection",
                            "btn text-primary mr-1 dropdown-item",
                            "/Cate/MainSection/Edit/" + data,
                            '<i class="fa fa-edit text-primary text-120"></i> Cập nhật',
                            "Cập nhật");
                        html += _renderButton(true,
                            "DeleteMainSection",
                            "btn text-danger mr-1 dropdown-item",
                            "/Cate/MainSection/Delete/" + data,
                            '<i class="far fa-trash-alt text-danger text-120"></i> Xóa',
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

function MainSection_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableMainSection.ajax.reload(null, false);
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
                        _tableMainSection.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}