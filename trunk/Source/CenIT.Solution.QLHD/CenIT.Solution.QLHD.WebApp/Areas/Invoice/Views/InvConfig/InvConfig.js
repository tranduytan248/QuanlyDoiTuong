var _InvConfigActionURLs = {
    InvConfig_GetData: "/Invoice/InvConfig/Get"
};
var _tableInvConfig;

$(document).ready(function () {
    initTableInvConfig();
});

function initTableInvConfig() {
    _tableInvConfig = $("#DSConfig").DataTable({
        "Responsive": true,
        "language": {
            "processing": '<div id="tableLoading" class="pageload-overlay show pageload-loading" data-opening="M 0,0 80,-10 80,60 0,70 0,0" data-closing="M 0,-10 80,-20 80,-10 0,0 0,-10" style=""><svg xmlns="http://www.w3.org/2000/svg" width="100%" height="100%" viewBox="0 0 80 60" preserveAspectRatio="none"><path d="M 0,70 80,60 80,80 0,80 0,70"></path></svg></div>'
                //"<div class='overlay'><i class='fas fa-cog fa-spin'></i></div>"
        },
        "lengthChange": true,
        "processing": true,
        "serverSide": true,
        "ajax":
        {
            "url": _InvConfigActionURLs.InvConfig_GetData,
            "type": "POST",
            "dataType": "JSON"
        },
        "columns": [
            {
                "data": "",
                "defaultContent": "1",
                "render": function (data, type, row, meta) {
                    return meta.settings._iDisplayStart + meta.row + 1;
                }
            },
            {
                "data": "ConfigKey",
                "defaultContent": ""
            },
            {
                "data": "ConfigValue",
                "defaultContent": "",
                "class": "max-width-280",
                "render": function (data, type, row, meta) {
                    if (data.length > 500) {
                        data = data.substring(0, 498) + '...';
                    }
                    return data;
                }
            },
            {
                "data": "ConfigDesc",
                "visible": false,
                "defaultContent": ""
            },
            {
                "data": "ConfigId",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';
                    if (type === "display") {
                        html += _renderButton(true,
                            "EditConfig",
                            "btn px-4 btn-lighter-primary mr-1 v-hover",
                            "/Invoice/InvConfig/Edit/" + data,
                            '<i class="far fa-edit text-primary text-120"></i>',
                            "Cập nhật");

                        html += _renderButton(true,
                            "DeleteConfig",
                            "btn px-4 btn-lighter-danger mr-1 v-hover",
                            "/Invoice/InvConfig/Delete/" + data,
                            '<i class="far fa-trash-alt text-danger text-120"></i>',
                            "Xoá");
                    }
                    html += "</span>";
                    return html;
                }
            }
        ]
    });
}

function InvConfig_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableInvConfig.ajax.reload(null, false);
                response.status = undefined;
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
                        _tableInvConfig.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response).promise().done(function () {_initElements(this);});
    }
}