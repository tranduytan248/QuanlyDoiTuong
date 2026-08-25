var _ComplainActionURLs = {
    Complain_GetData: "/Major/Complain/Get"
};
var _tableComplain;
$(document).ready(function () {
    initTableComplain();
});

var arrColorStatus = { 0: "warning", 1: "danger", 2: "success" };

function initTableComplain() {
    _tableComplain = $("#DSComplain").DataTable({
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
            "url": _ComplainActionURLs.Complain_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": {
                "Sources": function () {
                    return $("#Search select#ListSourceIds").val() != null &&
                        $("#Search select#ListSourceIds").val().length > 0
                        ? $("#Search select#ListSourceIds").val()
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
                    return $("#Search select#ListHandleStatusIds").val() != null &&
                        $("#Search select#ListHandleStatusIds").val().length > 0
                        ? $("#Search select#ListHandleStatusIds").val()
                        : "";
                },
                "HandleOnFrom": function () {
                    return $("#Search input#HandleOnFrom").val() != null &&
                        $("#Search input#HandleOnFrom").val().length > 0
                        ? $("#Search input#HandleOnFrom").val()
                        : "";
                },
                "HandleOnTo": function () {
                    return $("#Search input#HandleOnTo").val() != null &&
                        $("#Search input#HandleOnTo").val().length > 0
                        ? $("#Search input#HandleOnTo").val()
                        : "";
                },
                "ComplainCode": function () {
                    return $("#Search input#ComplainCode").val() != null &&
                        $("#Search input#ComplainCode").val().length > 0
                        ? $("#Search input#ComplainCode").val()
                        : "";
                },
                "ComplainStatus": function () {
                    return $("#Search select#ListComplainStatusIds").val() != null &&
                        $("#Search select#ListComplainStatusIds").val().length > 0
                        ? $("#Search select#ListComplainStatusIds").val()
                        : "";
                }
            }
        },
        "columns": [
            {
                "data": "",
                "class": "align-middle",
                "defaultContent": "1",
                "render": function (data, type, row, meta) {
                    return meta.settings._iDisplayStart + meta.row + 1;
                }
            },
            {
                "data": "Title",
                "width": "35%",
                "class": "align-middle",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    var html = "";
                    if (type === "display") {

                        var ulExtendInfo = '<ul class="list-unstyled text-dark-tp3">{0}</ul>';
                        var extendInfoItem = '<li class="mb-2"><i class="w-3 text-center {1} text-95"></i>&nbsp;{0}</li>';

                        var extendInfo = '<div class="text-95 text-secondary-d1"><i class="{1}"></i>&nbsp;{0}</div>';

                        var html = extendInfoItem.format("Nguồn: <b>" + row["SourceName"] + "</b>", "fas fa-code text-danger");
                        //if (row["SourceId"] == 1) {
                        //    html += extendInfoItem.format("Đối tượng PAKN: <b>" + row["TypeSenderObjName"] + "</b>", "fas fa-users-cog text-purple");
                        //    html += extendInfoItem.format("PAKN bởi: " + "<b class='text-blue-d3'>" + row["SenderName"] + "</b>", "fas fa-user-edit text-blue");
                        //    html += extendInfoItem.format("Địa điểm: <b>" + row["WardName"] + ", " + row["DistrictName"] + ", " + row["ProvinceName"] + "</b>", "fas fa-map-marker-alt text-green");
                        //}
                        html = ulExtendInfo.format(html);
                        //return '<p class="text-110 text-600 text-primary-d1 mb-0">{0}</p><p class="text-100 text-500 text-danger-d2 mb-0"><i class="fas fa-qrcode"></i>&nbsp;{2}</p>{1}'.format(data, html, row["ComplainCode"]);
                        return '<p class="text-110 text-600 text-primary-d1 mb-0">{0}</p>{1}'.format(data, html, row["ComplainCode"]);
                    }
                    return data;
                }
            },
            {
                "data": "CreatedOn",
                //"width": "45%",
                "class": "align-middle",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    if (type == "display") {
                        var estimateTime = parseInt(row["EstimateTime"]);
                        var html = "";
                        var durationTemplate = "{0} ngày {1} giờ {2} phút {3} giây";
                        var warningTemplate = "<span class='text-600 text-danger'>{0}<span>"
                        var templateUl = '<ul>{0}</ul>';
                        var liTemplate = '<li class="{2}">{0} {1}</li>';
                        var receiverOn = liTemplate.format("Ngày phản ánh:", moment(data).format("DD/MM/YYYY HH:mm:ss"), "text-600");
                        if (row["IsHandled"]) {
                            //var handledBy = row["LastModifiedBy"];
                            //var handledOn = liTemplate.format("<span class='text-primary-m1 font-bolder text-120'>{0}</span> đã thực hiện kiểm tra lúc".format(handledBy), moment(row["LastModifiedOn"]).format("HH:mm:ss ngày DD/MM/YYYY"), "text-600 text-danger");
                            //var typeHandleStatus = row["HandleStatus"] == 0 ? "warning" : "danger";
                            //var handledStatus = '<span class="badge badge-sm badge-{1} arrowed-in arrowed-in-right">{0}</span>'.format(row["HandleStatusName"], typeHandleStatus);

                            var handledBy = row["UnitCreatedName"];
                            var handledOn = liTemplate.format("<span class='text-primary-m1 font-bolder text-120'>{0}</span> đã thực hiện kiểm tra lúc".format(handledBy), moment(row["ProcessingDate"]).format("HH:mm:ss ngày DD/MM/YYYY"), "text-600 text-danger");

                            var now = moment(data).add(estimateTime, "d"); //todays date
                            var end = moment(row["ProcessingDate"]); // another date
                            var duration = moment.duration(now.diff(end));
                            if (duration.milliseconds() > 0) {
                                var yearLate = duration.years() > 0 ? "{0} năm ".format(duration.years()) : "";
                                var monthLate = duration.months() > 0 ? "{0} tháng ".format(duration.months()) : "";
                                var dayLate = duration.days() > 0 ? "{0} ngày ".format(duration.days()) : "";
                                var hourLate = duration.hours() > 0 ? "{0} giờ ".format(duration.hours()) : "";
                                var minuteLate = duration.minutes() > 0 ? "{0} phút ".format(duration.minutes()) : "";

                                var infoLate = yearLate + monthLate + dayLate + hourLate + minuteLate;

                                var late = liTemplate.format("Trễ:", "{0} {1} giây".format(infoLate, duration.seconds()), "text-600 text-120 text-danger");
                                html += templateUl.format(late + '<li class="dropdown-divider brc-primary-l2"></li>' + receiverOn + handledOn);
                                //html += templateUl.format(late + '<li class="dropdown-divider brc-primary-l2"></li>' + receiverOn + handledOn + handledStatus);
                            }
                            else {
                                html += templateUl.format(receiverOn + handledOn);
                                //html += templateUl.format(receiverOn + handledOn + handledStatus);
                            }
                        }
                        else {
                            var now = moment(new Date()); //todays date
                            var end = moment(data).add(estimateTime, "d"); // another date
                            var duration = moment.duration(now.diff(end));
                            var expiredOn = liTemplate.format("Hạn xử lý:", moment(data).add(estimateTime, "d").format("HH:mm:ss ngày DD/MM/YYYY"), "text-600 text-blue");

                            if (duration.milliseconds() > 0) {
                                var yearLate = duration.years() > 0 ? "{0} năm ".format(duration.years()) : "";
                                var monthLate = duration.months() > 0 ? "{0} tháng ".format(duration.months()) : "";
                                var dayLate = duration.days() > 0 ? "{0} ngày ".format(duration.days()) : "";
                                var hourLate = duration.hours() > 0 ? "{0} giờ ".format(duration.hours()) : "";
                                var minuteLate = duration.minutes() > 0 ? "{0} phút ".format(duration.minutes()) : "";

                                var infoLate = yearLate + monthLate + dayLate + hourLate + minuteLate;

                                var late = liTemplate.format("Trễ:", "{0} {1} giây".format(infoLate, duration.seconds()), "text-600 text-120 text-danger");
                                html += templateUl.format(late + '<li class="dropdown-divider brc-primary-l2"></li>' + receiverOn + expiredOn);
                            } else {
                                html += templateUl.format(receiverOn + expiredOn);
                            }
                        }

                        return html;
                    }
                    return data;
                }
            },
            {
                "data": "StatusName",
                //"width": "45%",
                "class": "align-middle",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    if (type == "display") {
                        var className = arrColorStatus[row["Status"]];
                        //if (!row["IsHandled"]) {
                        //    className = "warning"
                        //}

                        var html = '<span class="badge badge-{1} badge-lg arrowed arrowed-in-right">{0}</span>'.format(data, className);
                        return html;
                    }
                    return data;
                }
            },
            {
                "data": "ComplainId",
                //"class":"w-25",
                "orderable": false,
                "class": "align-middle",
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';

                    if (type === "display") {
                        if (row["IsHandled"] == 0) {
                            html += _renderButton(true,
                                "ResultChecking",
                                "btn px-4 btn-outline-success mr-1 v-hover",
                                "/Major/Complain/ResultChecking/" + data,
                                '<i class="fas fa-tasks text-120"></i>',
                                "Kết quả kiểm tra", "fullscreen");

                            if (currentUser == row["CreatedBy"]) {
                                html += _renderButton(true,
                                    "EditComplain",
                                    "btn px-4 btn-outline-primary mr-1 v-hover",
                                    "/Major/Complain/Edit/" + data,
                                    '<i class="far fa-edit text-120"></i>',
                                    "Cập nhật", "1024");
                            }
                            html += _renderButton(true,
                                "DeleteComplain",
                                "btn px-4 btn-outline-danger mr-1 v-hover",
                                "/Major/Complain/Delete/" + data,
                                '<i class="far fa-trash-alt text-120"></i>',
                                "Xoá");
                        }
                        else {
                            html += _renderButton(true,
                                "ReviewComplain",
                                "btn px-4 btn-outline-info mr-1 v-hover",
                                "/Major/Complain/Review/" + data,
                                '<i class="fas fa-eye text-120"></i>',
                                "Thông tin", row["SourceId"] == 1 ? "fullscreen" : "1024");

                            if (row["HandleStatus"] == 1 && !row["HasDossier"]) {
                                html += _renderButton(true,
                                    "CreateDossier",
                                    "btn px-4 btn-outline-danger mr-1 v-hover",
                                    "/Major/Dossier/Add?complainId=" + data,
                                    '<i class="fas fa-file-signature text-120"></i>',
                                    "Quyết định xử phạt", "fullscreen");
                            }
                            if (row["HasDossier"]) {
                                html += _renderButton(true,
                                    "Activity",
                                    "btn px-4 btn-outline-info mr-1 v-hover",
                                    "/Major/Dossier/Activity/" + row["DossierId"],
                                    '<i class="fas fa-clipboard-list text-120"></i>',
                                    "Thông tin Xử lý Vi phạm", "1024");
                            }
                        }
                    }
                    html += "</span>";

                    return html;
                }
            }
        ]
    });
}

function Complain_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableComplain.ajax.reload(null, false);
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
                        _tableComplain.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyComplain").html(response);
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
        $("#ModalContent #modal_" + formId + " #bodyComplain").html(response);
    }
}
