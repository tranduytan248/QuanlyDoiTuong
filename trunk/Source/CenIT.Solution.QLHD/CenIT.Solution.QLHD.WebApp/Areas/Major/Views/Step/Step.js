var _StepActionURLs = {
    Step_GetData: "/Major/Step/Get"
};
var _tableStep;
$(document).ready(function () {
    initTableStep();
});

function initTableStep() {
    _tableStep = $("#Steps").DataTable({
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
            "url": _StepActionURLs.Step_GetData,
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
            //{
            //    "data": "",
            //    "defaultContent": "1",
            //    "render": function (data, type, row, meta) {
            //        return meta.settings._iDisplayStart + meta.row + 1;
            //    }
            //},
            //{
            //    "data": "ProcedureName",
            //    "defaultContent": ""
            //},
            {
                "data": "StepName",
                "width": "55%",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    var html = "";
                    if (type === "display") {
                        return '<div class="mx-2 my-auto"><div class="text-600 text-primary-d1"><span class="text-110">{0}</span></div><span class="text-100 text-danger-d1"><i class="fas fa-random"></i>&nbsp;{1}</span></div>'.format(data, row["ProcedureName"]);

                        //html = '<span class="badge badge-info badge-lg ' + (row["RejectStep"] == null ? "" : "arrowed-in") + ' arrowed-right mb-1">' + (row["RejectStepName"] == null ? "" : row["RejectStepName"]) + '</span><span class="badge btn-danger badge-lg arrowed-in arrowed-right mb-1">' + data + '</span><span class="badge btn-info badge-lg arrowed-in ' + (row["ApproveStep"] == null ? "" : "arrowed-right") + ' mb-1">' + (row["ApproveStepName"] == null ? "" : row["ApproveStepName"]) + '</span>';
                        //return html;
                        //return '<span class="badge btn-info badge-lg arrowed-in arrowed-right mb-1">' + data + '</span>';
                    }
                    return data;
                }
            },
            {
                "data": "Handler",
                "width": "55%",
                "defaultContent": ""
            },
            {
                "data": "ProcessedTime",
                "defaultContent": "",
                //"visible": false,
                "render": function (data, type, row, meta) {
                    if (type === "display")
                    {
                        var html = '<span class="text-danger-d1 text-md">{0}&nbsp;ngày</span>'.format(data);
                        return html;
                    }
                    return data;
                }
            },
            {
                "data": "StepId",
                //"style": "width:100px;",
                "width": "15%",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';

                    if (type === "display") {
                        html += _renderButton(true,
                            "EditStep",
                            "btn px-4 btn-outline-primary mr-1 v-hover",
                            "/Major/Step/Edit/" + data,
                            '<i class="far fa-edit text-120"></i>',
                            "Cập nhật");

                        html += _renderButton(true,
                          "StepSituation",
                          "btn px-4 btn-outline-purple mr-1 v-hover",
                          "/Major/Step/Situations/" + data,
                          '<i class="fab fa-playstation text-120"></i>',
                          "Tình huống", "fullscreen");

                        //html += _renderButton(true,
                        //   "StepPermit",
                        //   "btn px-4 btn-outline-purple mr-1 v-hover",
                        //   "/Major/Step/Permits/" + data,
                        //   '<i class="fa fa-shield-alt text-120"></i>',
                        //   "Quyền xử lý", 1024);

                        if (row["TotalSituations"] > 0) {
                            html += _renderButton(true,
                          "Flow",
                          "btn px-4 btn-outline-success mr-1 v-hover",
                          "/Major/Step/Flow/" + data,
                          '<i class="fas fa-project-diagram text-120"></i>',
                          "Sơ đồ", "fullscreen");
                        }

                        html += _renderButton(true,
                            "DeleteStep",
                            "btn px-4 btn-outline-danger mr-1 v-hover",
                            "/Major/Step/Delete/" + data,
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

function Step_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableStep.ajax.reload(null, false);
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
                        _tableStep.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}
