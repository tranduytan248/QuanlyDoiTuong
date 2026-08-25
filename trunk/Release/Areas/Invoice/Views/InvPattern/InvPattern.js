var _InvPatternActionURLs = {
    InvPattern_GetData: "/Invoice/InvPattern/Get"
};
var _tableInvPattern;

$(document).ready(function () {
    initTableInvPattern();
});

function initTableInvPattern() {
    _tableInvPattern = $("#DSPattern").DataTable({
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
            "url": _InvPatternActionURLs.InvPattern_GetData,
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
                "data": "Pattern",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    if (type == "display") {
                        return "{0}-{1}".format(data, row["Serial"]);
                    }
                    return data;
                }
            },
            {
                "data": "TotalRemainingInv",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    if (type == "display") {
                        return data.toLocaleString();
                    }
                    return data;
                }
            },
            {
                "data": "IsActive",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    if (type == "display") {
                        var color = data == 1 ? "success" : "warning"
                        return '<span class="badge badge-{1} arrowed arrowed-in-right">{0}</span>'.format(row["StatusName"], color);
                    }
                    return data;
                }
            },
            {
                "data": "PatternId",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';
                    if (type === "display") {
                        html += _renderButton(true,
                            "EditPattern",
                            "btn px-4 btn-lighter-primary mr-1 v-hover",
                            "/Invoice/InvPattern/Edit/" + data,
                            '<i class="far fa-edit text-primary text-120"></i>',
                            "Cập nhật");

                        html += _renderButton(true,
                            "DeletePattern",
                            "btn px-4 btn-lighter-danger mr-1 v-hover",
                            "/Invoice/InvPattern/Delete/" + data,
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

function InvPattern_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableInvPattern.ajax.reload(null, false);
                response.status = undefined;
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
                        _tableInvPattern.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}