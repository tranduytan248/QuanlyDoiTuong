var _StepPermitActionURLs = {
    StepPermit_GetData: "/Major/StepPermit/Get"
};
var _tableStepPermit;
$(document).ready(function () {
    initTableStepPermit();
});

function initTableStepPermit() {
    _tableStepPermit = $("#StepPermits").DataTable({
        "Responsive": true,
        "language": {
            "processing": "<div class='overlay'><i class='fas fa-cog fa-spin'></i></div>"
        },
        "lengthChange": true,
        "processing": true,
        "serverSide": true,
        "ajax":
        {
            "url": _StepPermitActionURLs.StepPermit_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": {
                "ProcedureIds": function () {
                    return $("#Search select#ListProcedureIds").val() != null &&
                        $("#Search select#ListProcedureIds").val().length > 0
                        ? $("#Search select#ListProcedureIds").val()
                        : "";
                }
            }
        },
        "columns": [
            {
                "data": "",
                "defaultContent": "1",
                "render": function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                "data": "ProcedureName",
                "defaultContent": ""
            },
            {
                "data": "StepPermitName",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    var html = "";
                    if (type === "display") {

                        //html = '<span class="badge badge-info badge-lg ' + (row["RejectStepPermit"] == null ? "" : "arrowed-in") + ' arrowed-right mb-1">' + (row["RejectStepPermitName"] == null ? "" : row["RejectStepPermitName"]) + '</span><span class="badge btn-danger badge-lg arrowed-in arrowed-right mb-1">' + data + '</span><span class="badge btn-info badge-lg arrowed-in ' + (row["ApproveStepPermit"] == null ? "" : "arrowed-right") + ' mb-1">' + (row["ApproveStepPermitName"] == null ? "" : row["ApproveStepPermitName"]) + '</span>';
                        //return html;
                        return '<span class="badge btn-info badge-lg arrowed-in arrowed-right mb-1">' + data + '</span>';
                    }
                    return data;
                }
            },
            {
                "data": "ProcessedTime",
                "defaultContent": "",
                //"visible": false,
                "render": function (data, type, row, meta) {
                    if (type === "display") {
                        var html = '<span class="text-danger-d1 text-md">{0}&nbsp;ngày</span>'.format(data);
                        return html;
                    }
                    return data;
                }
            },
            {
                "data": "StepPermitId",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';

                    if (type === "display") {
                        html += _renderButton(true,
                            "EditStepPermit",
                            "btn px-4 btn-outline-primary mr-1 v-hover",
                            "/Major/StepPermit/Edit/" + data,
                            '<i class="far fa-edit text-120"></i>',
                            "Cập nhật");

                        html += _renderButton(true,
                            "DeleteStepPermit",
                            "btn px-4 btn-outline-danger mr-1 v-hover",
                            "/Major/StepPermit/Delete/" + data,
                            '<i class="far fa-trash-alt text-120"></i>',
                            "Xoá");
                    }
                    html += "</span>";

                    return html;
                }
            }
        ]
    });
}

function StepPermit_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableStepPermit.ajax.reload(null, false);
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
                        _tableStepPermit.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}
