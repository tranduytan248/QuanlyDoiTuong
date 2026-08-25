var _FormActionURLs = {
    Form_GetData: "/Major/Form/Get"
};
var _tableForm;
$(document).ready(function () {
    initTableForm();
});

function initTableForm() {
    _tableForm = $("#DSForm").DataTable({
        "Responsive": true,
        "language": {
            "processing": '<div id="tableLoading" class="pageload-overlay show pageload-loading" data-opening="M 0,0 80,-10 80,60 0,70 0,0" data-closing="M 0,-10 80,-20 80,-10 0,0 0,-10" style=""><svg xmlns="http://www.w3.org/2000/svg" width="100%" height="100%" viewBox="0 0 80 60" preserveAspectRatio="none"><path d="M 0,70 80,60 80,80 0,80 0,70"></path></svg></div>'
                //"<div class='overlay'><i class='fas fa-cog fa-spin'></i></div>"
        },
        "lengthChange": true,
        "processing": true,
        "serverSide": true,
        "autoWidth": false,
        "ajax":
        {
            "url": _FormActionURLs.Form_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": {
                "FormTypes": function () {
                    return $("#Search select#FormTypes").val() != null &&
                        $("#Search select#FormTypes").val().length > 0
                        ? $("#Search select#FormTypes").val()
                        : "";
                },
                "ProcedureTypes": function () {
                    return $("#Search select#ProcedureTypes").val() != null &&
                        $("#Search select#ProcedureTypes").val().length > 0
                        ? $("#Search select#ProcedureTypes").val()
                        : "";
                }
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
            //    "data": "FormTypeName",
            //    "defaultContent": ""
            //},
            {
                "data": "FormName",
                "width": "30%",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    var html = "";
                    if (type === "display") {
                        return '<div class="mx-2 my-auto"><div class="text-600 text-primary-d1"><span class="text-110">{2} - {0}</span></div><span class="text-100 text-danger-d1"><i class="fas fa-file-word"></i>&nbsp;{1}</span></div>'.format(data, row["FormTypeName"], row["FormCode"]);
                    }
                    return data;
                }
            },
            {
                "data": "RefDocs",
                "width": "40%",
                "defaultContent": ""
            },
            {
                "data": "LastModifiedOn",
                "width": "10%",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    if (type === "display") {
                        if (data != null) {
                            return moment(data).format("DD/MM/YYYY HH:mm:ss")
                        }
                    }
                    return data;
                }
            },
            {
                "data": "FormId",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';

                    if (type === "display") {
                        html += _renderButton(true,
                            "ExportForm",
                            "btn px-4 btn-outline-success mr-1 v-hover",
                            "/Major/Form/Check/" + data,
                            '<i class="fas fa-cloud-download-alt text-120"></i>',
                            "Xuất biểu mẫu", "1024");

                        html += _renderButton(false,
                            "DesignForm",
                            "btn px-4 btn-outline-purple mr-1 v-hover",
                            "/Major/Design/Index/" + data,
                            '<i class="far fas fa-tools text-120"></i>',
                            "Chỉnh sửa form", null, 'target = "_blank"');

                        html += _renderButton(true,
                            "EditForm",
                            "btn px-4 btn-outline-primary mr-1 v-hover",
                            "/Major/Form/Edit/" + data,
                            '<i class="far fa-edit text-120"></i>',
                            "Cập nhật");

                        html += _renderButton(true,
                            "DeleteForm",
                            "btn px-4 btn-outline-danger mr-1 v-hover",
                            "/Major/Form/Delete/" + data,
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

function Form_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableForm.ajax.reload(null, false);
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
                        _tableForm.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response).promise().done(function () {_initElements(this);});
    }
}

function Doc_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        $("#ModalContent #modal_" + formId).modal("hide");
        $("#ModalContent #modal_" + formId).on("hidden.bs.modal",
            function () {
                if (response.status != undefined) {
                    eval(response.message);
                    var fileId = response.fileId;
                    $('li[id="' + fileId + '"]').remove();
                    response.status = undefined;
                }
            });
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response).promise().done(function () {_initElements(this);});
    }
}
