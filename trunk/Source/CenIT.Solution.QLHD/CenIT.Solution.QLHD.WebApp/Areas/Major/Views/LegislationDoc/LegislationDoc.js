var _LegislationDocActionURLs = {
    LegislationDoc_GetData: "/Major/LegislationDoc/Get"
};
var _tableLegislationDoc;
$(document).ready(function () {
    initTableLegislationDoc();
});

function initTableLegislationDoc() {
    _tableLegislationDoc = $("#DSLegislationDoc").DataTable({
        "Responsive": true,
        "language": {
            "processing":
                "<div class='overlay'><i class='fas fa-cog fa-spin'></i></div>"
        },
        "lengthChange": true,
        "processing": true,
        "serverSide": true,
        "autoWidth": false,
        "ajax":
        {
            "url": _LegislationDocActionURLs.LegislationDoc_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": {}
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
            //    "data": "DocName",
            //    "width": "30%",
            //    "defaultContent": "",
            //    "render": function (data, type, row, meta) {
            //        var html = "";
            //        if (type === "display") {
            //            return '<div class="mx-2 my-auto"><div class="text-600 text-primary-d1"><span class="text-110">{2} - {0}</span></div><span class="text-100 text-danger-d1"><i class="fas fa-file-word"></i>&nbsp;{1}</span></div>'.format(data, row["LegislationDocTypeName"], row["LegislationDocCode"]);
            //        }
            //        return data;
            //    }
            //},
            {
                "data": "RefDocs",
                "width": "50%",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    if (data == null || data.length <= 0) {
                        return row["DocName"];
                    }
                    return data;
                }
            },
            {
                "data": "DocId",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';

                    if (type === "display") {
                        html += _renderButton(true,
                            "EditLegislationDoc",
                            "btn px-4 btn-outline-primary mr-1 v-hover",
                            "/Major/LegislationDoc/Edit/" + data,
                            '<i class="far fa-edit text-120"></i>',
                            "Cập nhật");

                        html += _renderButton(true,
                            "DeleteLegislationDoc",
                            "btn px-4 btn-outline-danger mr-1 v-hover",
                            "/Major/LegislationDoc/Delete/" + data,
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

function LegislationDoc_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableLegislationDoc.ajax.reload(null, false);
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
                        _tableLegislationDoc.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyLegislationDoc").html(response);
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
        $("#ModalContent #modal_" + formId + " #bodyLegislationDoc").html(response);
    }
}
