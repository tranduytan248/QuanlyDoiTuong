var _DossierActionURLs = {
    Dossier_GetData: "/Major/Dossier/Get"
};
var _tableDossier;
$(document).ready(function () {
    initTableDossier();
});

var arrStatus = { 0: "info", 1: "primary", 2: "warning", 3: "success" };

function initTableDossier() {
    _tableDossier = $("#DSDossiers").DataTable({
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
            "url": _DossierActionURLs.Dossier_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": {
                "Sources": function () {
                    return $("#Search select#ListSourceIds").val() != null &&
                        $("#Search select#ListSourceIds").val().length > 0
                        ? $("#Search select#ListSourceIds").val()
                        : "";
                },
                "Procedures": function () {
                    return $("#Search select#ListProcedureIds").val() != null &&
                        $("#Search select#ListProcedureIds").val().length > 0
                        ? $("#Search select#ListProcedureIds").val()
                        : "";
                },
                "Unions": function () {
                    return $("#Search select#ListUnionIds").val() != null &&
                        $("#Search select#ListUnionIds").val().length > 0
                        ? $("#Search select#ListUnionIds").val()
                        : "";
                },
                "CreatedFrom": function () {
                    return $("#Search input#CreatedFrom").val() != null &&
                        $("#Search input#CreatedFrom").val().length > 0
                        ? $("#Search input#CreatedFrom").val()
                        : "";
                },
                "CreatedTo": function () {
                    return $("#Search input#CreatedTo").val() != null &&
                        $("#Search input#CreatedTo").val().length > 0
                        ? $("#Search input#CreatedTo").val()
                        : "";
                },
                "HandleStatus": function () {
                    var handleStatus = $("#Search [name='HandleStatus']:checked").map(function (_, el) {
                        return $(el).val();
                    }).get();

                    return handleStatus != null && handleStatus.length > 0
                        ? handleStatus
                        : "";
                },
                //"HandleStatus": function () {
                //    return $("#Search select#ListHandleStatusIds").val() != null &&
                //        $("#Search select#ListHandleStatusIds").val().length > 0
                //        ? $("#Search select#ListHandleStatusIds").val()
                //        : "";
                //},
                "CompletedOnFrom": function () {
                    return $("#Search input#CompletedOnFrom").val() != null &&
                        $("#Search input#CompletedOnFrom").val().length > 0
                        ? $("#Search input#CompletedOnFrom").val()
                        : "";
                },
                "CompletedOnTo": function () {
                    return $("#Search input#CompletedOnTo").val() != null &&
                        $("#Search input#CompletedOnTo").val().length > 0
                        ? $("#Search input#CompletedOnTo").val()
                        : "";
                },
                "DossierCode": function () {
                    return $("#Search input#DossierCode").val() != null &&
                        $("#Search input#DossierCode").val().length > 0
                        ? $("#Search input#DossierCode").val()
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
                    if (type === "display") {
                        var extendInfoTemplate = '<div class="text-95 {2}"><i class="{1}"></i>&nbsp;{0}</div>';
                        var extendInfo = "";

                        extendInfo = extendInfoTemplate.format(row["ProcedureName"], "fas fa-file-word", "text-info-d2 text-500");
                        extendInfo += extendInfoTemplate.format("Nguồn: " + row["SourceName"], "fas fa-code", "text-success-d3 text-500");
                        //extendInfo += extendInfoTemplate.format("Địa điểm: " + row["Location"], "fas fa-map-marker-alt", "text-secondary-d1");

                        //var title = '<p class="text-110 text-600 text-primary-d1 mb-0">{0}</p><p class="text-100 text-500 text-danger-d2 mb-0"><i class="fas fa-qrcode"></i>&nbsp;{1}</p>'.format(data, row["DossierCode"]);
                        var title = '<p class="text-110 text-600 text-primary-d1 mb-0">{0}</p>'.format(data, row["DossierCode"]);

                        return '<div class="mx-2 my-auto">{0}{1}</div>'.format(title, extendInfo);
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
                        var templateUl = '<ul>{0}</ul>';
                        var liTemplate = '<li class="{2}">{0} {1}</li>';
                        var createdOn = liTemplate.format("Ngày nhập:", moment(row["CreatedOn"]).format("HH:mm:ss DD/MM/YYYY"), "text-600");

                        if (row["Status"] > 1) {
                            var handleOn = liTemplate.format("Tiếp nhận xử lý:", moment(row["StartHandleOn"]).format("HH:mm:ss DD/MM/YYYY"), "text-600 text-warning-d1 text-105");

                            var expiredOn = liTemplate.format("Hạn xử lý:", moment(row["CreatedOn"]).add(data, "d").format("HH:mm:ss DD/MM/YYYY"), "text-600 text-primary-d1");
                           
                            var now = moment(new Date()); //todays date
                            var end = moment(row["CreatedOn"]).add(data, "d"); // another date
                            var duration = moment.duration(now.diff(end));

                            if (duration.milliseconds() > 0) {
                                var yearLate = duration.years() > 0 ? "{0} năm ".format(duration.years()) : "";
                                var monthLate = duration.months() > 0 ? "{0} tháng ".format(duration.months()) : "";
                                var dayLate = duration.days() > 0 ? "{0} ngày ".format(duration.days()) : "";
                                var hourLate = duration.hours() > 0 ? "{0} giờ ".format(duration.hours()) : "";
                                var minuteLate = duration.minutes() > 0 ? "{0} phút ".format(duration.minutes()) : "";

                                var infoLate = yearLate + monthLate + dayLate + hourLate + minuteLate;

                                var late = liTemplate.format("Trễ:", "{0} {1} giây".format(infoLate, duration.seconds()), "text-600 text-120 text-danger");
                                html += late + '<li class="dropdown-divider brc-primary-l2"></li>';
                            }

                            if (row["Status"] == 3) {
                                var completedOn = liTemplate.format("Hoàn thành xử lý:", moment(row["LastModifiedOn"]).format("HH:mm:ss DD/MM/YYYY"), "text-600 text-green");
                                var completedOnDate = row["LastModifiedOn"] == null ? new Date() : moment(row["LastModifiedOn"]);
                                                                
                                html += createdOn + expiredOn + handleOn + completedOn;
                            }
                            else {
                                html += createdOn + expiredOn + handleOn;
                            }

                        } else {
                            html += createdOn;
                        }
                        return templateUl.format(html);

                    }
                }
            },
            {
                "data": "StatusName",
                //"width": "45%",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    if (type == "display") {
                        var html = '<span class="badge badge-{1} badge-lg arrowed arrowed-in-right">{0}</span>'.format(data, arrStatus[row["Status"]]);
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
                        if (row["Status"] < 2) {
                            html += _renderButton(true,
                                "HandleDossier",
                                "btn px-4 btn-outline-primary mr-1 v-hover",
                                "/Major/Dossier/Handle/" + data,
                                '<i class="fas fa-user-edit text-120"></i>',
                                "Tiếp nhận Xử lý", "fullscreen");

                            //html += _renderButton(true,
                            //    "DeleteDossier",
                            //    "btn px-4 btn-outline-danger mr-1 v-hover",
                            //    "/Major/Dossier/Delete/" + data,
                            //    '<i class="far fa-trash-alt text-120"></i>',
                            //    "Xoá");
                        }
                        else if (row["Status"] == 2) {
                            html += _renderButton(true,
                                "HandleDossier",
                                "btn px-4 btn-outline-primary mr-1 v-hover",
                                "/Major/Dossier/Handle/" + data,
                                '<i class="fas fa-user-edit text-120"></i>',
                                "Cập nhật Xử lý", "fullscreen");

                            html += _renderButton(true,
                                "ForwardToHandle",
                                "btn px-4 btn-outline-warning mr-1 v-hover",
                                "/Major/Dossier/ForwardToHandle/" + data,
                                '<i class="fas fa-random text-120"></i>',
                                "Chuyển xử lý", "fullscreen");
                        }
                        else if (row["Status"] == 3) {
                            html += _renderButton(true,
                                    "ViewDossier",
                                    "btn px-4 btn-outline-purple mr-1 v-hover",
                                    "/Major/Dossier/View/" + data,
                                    '<i class="fas fa-eye text-120"></i>',
                                    "Thông tin", "fullscreen");
                        }

                        html += _renderButton(true,
                                "Activity",
                                "btn px-4 btn-outline-info mr-1 v-hover",
                                "/Major/Dossier/Activity/" + data,
                                '<i class="fas fa-clipboard-list text-120"></i>',
                                "Thông tin xử lý", "1024");
                    }
                    html += "</span>";

                    return html;
                }
            }
        ]
    });
}

function Dossier_OnProcessSuccess(response, formId) {
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
