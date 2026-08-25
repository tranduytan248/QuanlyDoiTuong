
var _LandAreaActionURLs = {
    LandArea_GetData: "/Cate/LandArea/Get"
};
var _tableLandArea;

$(document).ready(function () {
    initTableLandArea()
});

function Search() {
    _tableLandArea.ajax.reload(null, false);
}

function initTableLandArea() {
    _tableLandArea = $("#DSLandArea").DataTable({
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
            "url": _LandAreaActionURLs.LandArea_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": {
                "LandType_ID": function () { return $("#SearchLandArea #LandTypeDropdown").val() ? $("#SearchLandArea #LandTypeDropdown").val() : 0; },
                "TuKhoa": function () { return $("#SearchLandArea #TuKhoa").val(); },
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
                "data": "LandSize",
                "defaultContent": ""
            },
            {
                "data": "LandTypeName",
                "defaultContent": ""
            },
            {
                "data": "Unit",
                "defaultContent": ""
            },
            {
                "data": "UnitPrice",
                "defaultContent": "0",
                "render": function (data, type, row, meta) {
                    if (data != null) {
                        return row.UnitPrice.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".") + "đ";
                    }
                }
            },
            {
                "data": "LandArea_ID",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';
                    if (type === "display") {
                        html += _renderButton(true,
                            "EditLandArea",
                            "btn px-4 btn-lighter-primary mr-1 v-hover",
                            "/Cate/LandArea/Edit/" + data,
                            '<i class="fa fa-edit text-primary text-120"></i>',
                            "Cập nhật");
                        html += _renderButton(true,
                            "DeleteLandArea",
                            "btn px-4 btn-lighter-danger mr-1 v-hover",
                            "/Cate/LandArea/Delete/" + data,
                            '<i class="far fa-trash-alt text-danger text-120"></i>',
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

function LandArea_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableLandArea.ajax.reload(null, false);
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
                        _tableLandArea.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}