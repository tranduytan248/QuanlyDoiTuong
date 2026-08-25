
var _PurPoseActionURLs = {
    PurPose_GetData: "/Cate/PurPose/Get"
};
var _tablePurPose;


$(document).ready(function () {
    initTablePurPose()
});

function Search() {
    _tablePurPose.ajax.reload(null, false);
}

function initTablePurPose() {
    _tablePurPose = $("#DSGPCN").DataTable({
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
            "url": _PurPoseActionURLs.PurPose_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": {
                /*"ContractTypeId": function () { return $("#SearchPurPose #ContractTypeId").val() ? $("#SearchPurPose #ContractTypeId").val() : 0; },*/
                /*"TuKhoa": function () { return $("#SearchPurPose #TuKhoa").val() },*/
                "TypeContractIds": function () {
                    return $("#SearchPurPose select#ListTypeContractIds").val() != null &&
                        $("#SearchPurPose select#ListTypeContractIds").val().length > 0
                        ? $("#SearchPurPose select#ListTypeContractIds").val()
                        : "";
                },
                "SearchValue": function () { return $("#SearchPurPose #SearchValue").val() },
            },

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
                "data": "PurPoseName",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    var html = '{0}<div> <span class="badge text-white badge-lg arrowed arrowed-in-right mb-1 bgc-{1} brc-{1}">{2}</span></div>';
                    if (type === "display") {

                        var arrTypeContracts = { "1": "primary", "2": "success", "3": "info", "4": "warning", "5": "purple" };

                        return html.format(data, arrTypeContracts[row.ContractTypeId], row.ContractTypeName);
                    }
                    return data;
                }
            },
            //{
            //    "data": "ContractTypeName",
            //    "defaultContent": ""
            //},
            {
                "data": "PurPoseId",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';
                    if (type === "display") {
                        html += _renderButton(true,
                            "EditPurPose",
                            "btn px-4 btn-lighter-primary mr-1 v-hover",
                            "/Cate/PurPose/Edit/" + data,
                            '<i class="fa fa-edit text-primary text-120"></i>',
                            "Cập nhật");
                        html += _renderButton(true,
                            "DeletePurPose",
                            "btn px-4 btn-lighter-danger mr-1 v-hover",
                            "/Cate/PurPose/Delete/" + data,
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

function PurPose_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tablePurPose.ajax.reload(null, false);
                response.status = undefined;
                $("#ModalContent #modal_" + formId + " form")[0].reset();
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
                        _tablePurPose.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}