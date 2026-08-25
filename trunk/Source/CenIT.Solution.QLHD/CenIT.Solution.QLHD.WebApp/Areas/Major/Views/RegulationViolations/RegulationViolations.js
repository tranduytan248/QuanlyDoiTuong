var _RegulationViolationsActionURLs = {
    RegulationViolations_GetData: "/Major/RegulationViolations/Get"
};
var _tableRegulationViolations;
$(document).ready(function () {
    initTableRegulationViolations();
});

function initTableRegulationViolations() {
    _tableRegulationViolations = $("#DSRegulationViolations").DataTable({
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
            "url": _RegulationViolationsActionURLs.RegulationViolations_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": {
                "SearchFieldViolateId": function () {
                    var FieldViolateId = $("#Search [name='SearchFieldViolateId']:checked").val();

                    return FieldViolateId != null && FieldViolateId.length > 0
                        ? FieldViolateId
                        : null;
                },
                "SearchConstructionParticipantId": function () {
                    var constructionParticipantId = $("#Search [name='SearchConstructionParticipantId']").val();

                    return constructionParticipantId != null && constructionParticipantId.length > 0
                        ? constructionParticipantId
                        : null;
                },
                "SearchViolationBehaviorId": function () {
                    var violationBehaviorId = $("#Search [name='SearchViolationBehaviorId']").val();

                    return violationBehaviorId != null && violationBehaviorId.length > 0
                        ? violationBehaviorId
                        : null;
                },
                "SearchViolatedConstructionId": function () {
                    var violatedConstructionId = $("#Search [name='SearchViolatedConstructionId']").val();

                    return violatedConstructionId != null && violatedConstructionId.length > 0
                        ? violatedConstructionId
                        : null;
                },
                "SearchCateId": function () {
                    var cateId = $("#Search [name='SearchCateId']").val();

                    return cateId != null && cateId.length > 0
                        ? cateId
                        : null;
                },
                "SearchRegulationId": function () {
                    var regulationId = $("#Search [name='SearchRegulationId']").val();

                    return regulationId != null && regulationId.length > 0
                        ? regulationId
                        : null;
                },
                "SearchRemedialMeasureId": function () {
                    var remedialMeasureId = $("#Search [name='SearchRemedialMeasureId']").val();

                    return remedialMeasureId != null && remedialMeasureId.length > 0
                        ? remedialMeasureId
                        : null;
                },
                "SearchRemediationRegulationId": function () {
                    var remediationRegulationId = $("#Search [name='SearchRemediationRegulationId']").val();

                    return remediationRegulationId != null && remediationRegulationId.length > 0
                        ? remediationRegulationId
                        : null;
                },
                "SearchViolatedAreaId": function () {
                    var violatedAreaId = $("#Search [name='SearchViolatedAreaId']").val();

                    return violatedAreaId != null && violatedAreaId.length > 0
                        ? violatedAreaId
                        : null;
                },
                "SearchDeductionAccountId": function () {
                    var deductionAccountId = $("#Search [name='SearchDeductionAccountId']").val();

                    return deductionAccountId != null && deductionAccountId.length > 0
                        ? deductionAccountId
                        : null;
                },
                "SearchDeductionRegulationId": function () {
                    var deductionRegulationId = $("#Search [name='SearchDeductionRegulationId']").val();

                    return deductionRegulationId != null && deductionRegulationId.length > 0
                        ? deductionRegulationId
                        : null;
                },
                "SearchFineAmount": function () {
                    var fineAmount = $("#Search [name='SearchFineAmount']").val();

                    return fineAmount != null && fineAmount.length > 0
                        ? fineAmount
                        : null;
                },
            }
        },
        "columns": (function () {
            var fieldViolateId = $("input[name='SearchFieldViolateId']:checked").val();
            if (fieldViolateId == "8d0e3621-5bab-4b1f-85cb-e47f0be4bc86") {
                // Cập nhật cột cho trường hợp đầu tiên
                return [
                    { "data": "", "defaultContent": "1", "render": function (data, type, row, meta) { return meta.settings._iDisplayStart + meta.row + 1; } },
                    { "data": "ViolationBehaviorName", "defaultContent": "" },
                    { "data": "ViolatedConstructionName", "defaultContent": "" },
                    { "data": "AuthorizationLevelName", "defaultContent": "" },
                    { "data": "RegulationName", "defaultContent": "" },
                    { "data": "ContentOfRemedialMeasure", "defaultContent": "" },
                    { "data": "RemediationRegulationName", "defaultContent": "" },
                    {
                        "data": "FineAmountRange",
                        "defaultContent": "",
                        "render": function (data, type, row, meta) {
                            var formattedMin = Number(row.FineAmountMin).toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
                            var formattedMax = Number(row.FineAmountMax).toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
                            return formattedMin + " đến " + formattedMax;
                        }
                    },
                    {
                        "data": "RegulationViolationsId",
                        "style": "width:100px;",
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            var html = '<span class="d-none d-lg-inline">';
                            if (type === "display") {
                                html += _renderButton(true,
                                    "EditRegulationViolations",
                                    "btn px-4 btn-lighter-primary mr-1 v-hover",
                                    "/Major/RegulationViolations/Edit/" + data,
                                    '<i class="far fa-edit text-primary text-120"></i>',
                                    "Cập nhật", 1024);
                                html += _renderButton(true,
                                    "DeleteRegulationViolations",
                                    "btn px-4 btn-lighter-danger mr-1 v-hover",
                                    "/Major/RegulationViolations/Delete/" + data,
                                    '<i class="far fa-trash-alt text-danger text-120"></i>',
                                    "Xoá");
                            }
                            html += "</span>";
                            return html;
                        }
                    }
                ];
            } else if (fieldViolateId == "693e1da5-05ea-447e-8da6-22a962c80a02") {
                // Cập nhật cột cho trường hợp thứ hai
                return [
                    { "data": "", "defaultContent": "1", "render": function (data, type, row, meta) { return meta.settings._iDisplayStart + meta.row + 1; } },
                    { "data": "ViolationBehaviorName", "defaultContent": "" },   
                    { "data": "ViolationAreaName", "defaultContent": "" },   
                    { "data": "AuthorizationLevelName", "defaultContent": "" },
                    { "data": "RegulationName", "defaultContent": "" },
                    { "data": "ContentOfRemedialMeasure", "defaultContent": "" },
                    { "data": "RemediationRegulationName", "defaultContent": "" },
                    { "data": "DeductionAccountName", "defaultContent": "" },
                    { "data": "DeductionRegulationName", "defaultContent": "" },
                    {
                        "data": "FineAmountRange",
                        "defaultContent": "",
                        "render": function (data, type, row, meta) {
                            var formattedMin = Number(row.FineAmountMin).toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
                            var formattedMax = Number(row.FineAmountMax).toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
                            return formattedMin + " đến " + formattedMax;
                        }
                    },
                    {
                        "data": "RegulationViolationsId",
                        "style": "width:100px;",
                        "orderable": false,
                        "render": function (data, type, row, meta) {
                            var html = '<span class="d-none d-lg-inline">';
                            if (type === "display") {
                                html += _renderButton(true,
                                    "EditRegulationViolations",
                                    "btn px-4 btn-lighter-primary mr-1 v-hover",
                                    "/Major/RegulationViolations/Edit/" + data,
                                    '<i class="far fa-edit text-primary text-120"></i>',
                                    "Cập nhật", 1024);
                                html += _renderButton(true,
                                    "DeleteRegulationViolations",
                                    "btn px-4 btn-lighter-danger mr-1 v-hover",
                                    "/Major/RegulationViolations/Delete/" + data,
                                    '<i class="far fa-trash-alt text-danger text-120"></i>',
                                    "Xoá");
                            }
                            html += "</span>";
                            return html;
                        }
                    }
                ];
            }
        })()
    });
}

function RegulationViolations_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableRegulationViolations.ajax.reload(null, false);
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
                        _tableRegulationViolations.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}

function OnChangeCombo(cbb, eleName) {
    $(eleName).val($(cbb).children("option:selected").text());
}
