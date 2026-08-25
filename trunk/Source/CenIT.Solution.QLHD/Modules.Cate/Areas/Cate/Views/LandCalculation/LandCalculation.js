
var _LandCalculationActionURLs = {
    LandCalculation_GetData: "/Cate/LandCalculation/Get"
};
var _tableLandCalculation;

$(document).ready(function () {
    initTableLandCalculation()
});

function Search() {
    _tableLandCalculation.ajax.reload(null, false);
}

function initTableLandCalculation() {
    _tableLandCalculation = $("#DSLandCalculation").DataTable({
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
            "url": _LandCalculationActionURLs.LandCalculation_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": {
                "ContentLandIds": function () {
                    return $("#Search select#ListContentLandIds").val() != null &&
                        $("#Search select#ListContentLandIds").val().length > 0
                        ? $("#Search select#ListContentLandIds").val()
                        : "";
                }
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
                "data": "ContentLandName",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    var html = '{0}<div> <span class="badge text-white badge-lg arrowed arrowed-in-right mb-1 bgc-{1} brc-{1}">{2}</span></div>';
                    if (type === "display") {

                        var arrTypeContracts = { "1": "primary", "2": "success", "3": "info", "4": "warning", "5": "purple" };

                        return html.format(data, arrTypeContracts[row.TypeContract], row.TypeContractName);
                    }
                    return data;
                }
            },
            {
                "data": "Condition",
                "defaultContent": ""
            },
            {
                "data": "Recipe",
                "defaultContent": ""
            },
            {
                "data": "LandCalculationId",
                "style": "width:250px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';
                    if (type === "display") {
                        html += _renderButton(true,
                            "EditLandCalculation",
                            "btn px-4 btn-lighter-primary mr-1 v-hover",
                            "/Cate/LandCalculation/Edit/" + data,
                            '<i class="fa fa-edit text-primary text-120"></i>',
                            "Cập nhật");
                        html += _renderButton(true,
                            "DeleteLandCalculation",
                            "btn px-4 btn-lighter-danger mr-1 v-hover",
                            "/Cate/LandCalculation/Delete/" + data,
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

function LandCalculation_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableLandCalculation.ajax.reload(null, false);
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
                        _tableLandCalculation.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}