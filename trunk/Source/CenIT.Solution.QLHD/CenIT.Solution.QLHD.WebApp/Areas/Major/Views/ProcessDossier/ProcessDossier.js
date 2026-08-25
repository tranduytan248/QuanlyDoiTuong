var _ProcessDossierActionURLs = {
    ProcessDossier_GetData: "/Major/ProcessDossier/Get"
};
var _tableProcessDossier;
$(document).ready(function () {
    initTableProcessDossiers();
});

function initTableProcessDossiers() {
    _tableProcessDossier = $("#ProcessDossiers").DataTable({
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
            "url": _ProcessDossierActionURLs.ProcessDossier_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": {
                "Sources": function () {
                    return $("#Search select#Sources").val() != null &&
                        $("#Search select#Sources").val().length > 0
                        ? $("#Search select#Sources").val()
                        : "";
                },
                "Procedures": function () {
                    return $("#Search select#Procedures").val() != null &&
                        $("#Search select#Procedures").val().length > 0
                        ? $("#Search select#Procedures").val()
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
            {
                "data": "Title",
                //"width": "35%",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    var html = "";
                    if (type === "display") {
                        return '<div class="mx-2 my-auto"><div class="text-600 text-primary-d1"><span class="text-110">{0}</span></div><span class="text-100 text-danger-d1"><i class="fas fa-file-word"></i>&nbsp;{1}</span></div>'.format(data, row["ProcedureName"]);
                    }
                    return data;
                }
            },
            {
                "data": "ProcessedTime",
                //"width": "45%",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    if (type == "display") {
                        var html = "";
                        var durationTemplate = "{0} ngày {1} giờ {2} phút {3} giây";
                        var warningTemplate = "<span class='text-600 text-danger'>{0}<span>"
                        var templateUl = '<ul>{0}</ul>';
                        var liTemplate = '<li class="{2}">{0}: {1}</li>';
                        var receiverOn = liTemplate.format("Ngày tiếp nhận", moment(row["CreatedOn"]).format("DD/MM/YYYY HH:mm:ss"), "text-600");
                        var expiredOn = liTemplate.format("Hạn xử lý", moment(row["CreatedOn"]).add(data, "d").format("DD/MM/YYYY HH:mm:ss"), "text-600 text-blue");

                        var diffDays = moment(row["CreatedOn"]).add(data, "d").diff(new Date(), "s");
                        if (diffDays < 0) {
                            const duration = moment.duration(Math.abs(diffDays), 'seconds');
                            html += warningTemplate.format(durationTemplate.format(duration.days(), duration.hours(), duration.minutes(), duration.seconds()));
                        }
                        html += templateUl.format(receiverOn + expiredOn);
                        return html;
                    }
                }
            },
            {
                "data": "StatusName",
                //"width": "45%",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    if (type == "display") {
                        var html = '<span class="badge badge-success badge-lg arrowed arrowed-in-right">{0}</span>'.format(data);
                        return html;
                    }
                    return data;
                }
            },
            //{
            //    "data": "RefDocs",
            //    "width": "45%",
            //    "defaultContent": ""
            //},
            {
                "data": "DossierId",
                //"class":"w-25",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';

                    if (type === "display") {
                        //if (row["Status"] == 0) {
                            html += _renderButton(true,
                                "HandleDossier",
                                "btn px-4 btn-outline-success mr-1 v-hover",
                                "/Major/ProcessDossier/Handle/" + data,
                                '<i class="fas fa-microchip text-120"></i>',
                                "Xử lý phản ánh", "fullscreen");

                        //    html += _renderButton(true,
                        //        "EditDossier",
                        //        "btn px-4 btn-outline-primary mr-1 v-hover",
                        //        "/Major/Dossier/Edit/" + data,
                        //        '<i class="far fa-edit text-120"></i>',
                        //        "Cập nhật", "fullscreen");

                        //    html += _renderButton(true,
                        //        "DeleteDossier",
                        //        "btn px-4 btn-outline-danger mr-1 v-hover",
                        //        "/Major/Dossier/Delete/" + data,
                        //        '<i class="far fa-trash-alt text-120"></i>',
                        //        "Xoá");
                        //}
                    }
                    html += "</span>";

                    return html;
                }
            }
        ]
    });
}

function ProcessDossiers_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableDossier.ajax.reload(null, false);
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
                        _tableDossier.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyDossier").html(response);
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
        $("#ModalContent #modal_" + formId + " #bodyDossier").html(response);
    }
}
