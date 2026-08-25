var _ProcedureActionURLs = {
    Procedure_GetData: "/Major/Procedure/Get"
};
var _tableProcedure;
$(document).ready(function () {
    initTableProcedure();
});

$("#ModalContent #modal_ConfigProcedure").on("hidden.bs.modal", function () {
    if (response.status != undefined) {
        _tableProcedure.ajax.reload(null, false);
    }
});

function initTableProcedure() {
    _tableProcedure = $("#DSProcedure").DataTable({
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
            "url": _ProcedureActionURLs.Procedure_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": {
                "UnionIds": function () {
                    return $("#Search select#ListUnionIds").val() != null &&
                        $("#Search select#ListUnionIds").val().length > 0
                        ? $("#Search select#ListUnionIds").val()
                        : "";
                },
                "TypeContractIds": function () {
                    return $("#Search select#ListTypeContractIds").val() != null &&
                        $("#Search select#ListTypeContractIds").val().length > 0
                        ? $("#Search select#ListTypeContractIds").val()
                        : "";
                },
            }
        },
        "columns": [
            {
                "data": "",
                "defaultContent": "1",
                "render": function (data, type, row, meta) {
                    return meta.settings._iDisplayStart + meta.row + 1;
                }
            },
            //{
            //    "data": "ProcedureCode",
            //    "defaultContent": ""
            //},
            {
                "data": "ProcedureName",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    var html = "";
                    if (type === "display") {
                        html =
                            ' <div class="mx-2 my-auto"><div class="text-600 text-primary-d1"><span class="text-110">{0}</span></div><span class="badge badge-danger">{1}</span><span class="d-inline-block radius-round bgc-blue-l2 text-dark-tp3 text-90 px-25 py-3px mx-2px my-2px">{2}</span><hr>{3}</div>'.format(data, row["ProcedureCode"], row.ContractTypeName, row.Unions);
                        return html;
                    }
                    return data;
                }
            },
            {
                "data": "ProcedureId",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';

                    if (type === "display") {
                        if (row["CanEdit"]) {
                            html += _renderButton(true,
                                "EditProcedure",
                                "btn px-4 btn-outline-primary mr-1 v-hover",
                                "/Major/Procedure/Edit/" + data,
                                '<i class="far fa-edit text-120"></i>',
                                "Cập nhật");
                        }

                        html += _renderButton(true,
                          "View",
                          "btn px-4 btn-outline-warning mr-1 v-hover",
                          "/Major/Procedure/View/" + data,
                          '<i class="fas fa-project-diagram text-120"></i>',
                          "Bước quy trình", "fullscreen");

                        if (row["CanDelete"]) {
                            html += _renderButton(true,
                                "DeleteProcedure",
                                "btn px-4 btn-outline-danger mr-1 v-hover",
                                "/Major/Procedure/Delete/" + data,
                                '<i class="far fa-trash-alt text-120"></i>',
                                "Xoá");
                        }
                    }
                    html += "</span>";

                    return html;
                }
            }
        ]
    });
}

function Procedure_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableProcedure.ajax.reload(null, false);
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
                        _tableProcedure.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}
