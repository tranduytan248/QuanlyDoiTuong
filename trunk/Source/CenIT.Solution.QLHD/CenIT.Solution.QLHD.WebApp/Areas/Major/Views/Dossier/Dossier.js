var _DossierActionURLs = {
    Dossier_GetData: "/Major/Dossier/Get"
};
var _tableDossier;
$(document).ready(function () {
    initTableDossier();
});

function onSearchDossier() {
    $("#DSDossier").parents('.table-responsive-md').removeClass('d-none');
    _tableDossier.ajax.reload(null, false);
}

function initTableDossier() {
    var hasQueryValue = $("#Search #SearchValue").val();
    var isDeferLoading = hasQueryValue == undefined || hasQueryValue.length <= 0 ? 0 : null;

    _tableDossier = $("#DSDossier").DataTable({
        "Responsive": true,
        "language": {
            "processing": '<div id="tableLoading" class="pageload-overlay show pageload-loading" data-opening="M 0,0 80,-10 80,60 0,70 0,0" data-closing="M 0,-10 80,-20 80,-10 0,0 0,-10" style=""><svg xmlns="http://www.w3.org/2000/svg" width="100%" height="100%" viewBox="0 0 80 60" preserveAspectRatio="none"><path d="M 0,70 80,60 80,80 0,80 0,70"></path></svg></div>'
                //"<div class='overlay'><i class='fas fa-cog fa-spin'></i></div>"
        },
        "order": [[0, 'asc']],
        "aaSorting": [[0, "asc"]],
        "lengthChange": true,
        "processing": true,
        "serverSide": true,
        "searching": false,
        "deferLoading": isDeferLoading,
        "ajax":
        {
            "url": _DossierActionURLs.Dossier_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": {
                "SearchValue": function () {
                    return $("#Search #SearchValue").val() != null &&
                        $("#Search #SearchValue").val().length > 0
                        ? $("#Search #SearchValue").val()
                        : "";
                },
                "FromDate": function () {
                    return $("#Search #FromDate").val() != null &&
                        $("#Search #FromDate").val().length > 0
                        ? $("#Search #FromDate").val()
                        : "";
                },
                "ToDate": function () {
                    return $("#Search #ToDate").val() != null &&
                        $("#Search #ToDate").val().length > 0
                        ? $("#Search #ToDate").val()
                        : "";
                },
                "GiveResultFromDate": function () {
                    return $("#Search #GiveResultFromDate").val() != null &&
                        $("#Search #GiveResultFromDate").val().length > 0
                        ? $("#Search #GiveResultFromDate").val()
                        : "";
                },
                "GiveResultToDate": function () {
                    return $("#Search #GiveResultToDate").val() != null &&
                        $("#Search #GiveResultToDate").val().length > 0
                        ? $("#Search #GiveResultToDate").val()
                        : "";
                },
                "DossierStatus": function () {
                    return $("#Search select#ListDossierStatusIds").val() != null &&
                        $("#Search select#ListDossierStatusIds").val().length > 0
                        ? $("#Search select#ListDossierStatusIds").val()
                        : "";
                },
                "HandleTypes": function () {
                    return $("#Search select#ListHandleTypeIds").val() != null &&
                        $("#Search select#ListHandleTypeIds").val().length > 0
                        ? $("#Search select#ListHandleTypeIds").val()
                        : "";
                },
                "TypeContractIds": function () {
                    return $("#Search select#ListTypeContractIds").val() != null &&
                        $("#Search select#ListTypeContractIds").val().length > 0
                        ? $("#Search select#ListTypeContractIds").val()
                        : "";
                },
                "TypeCusIds": function () {
                    return $("#Search select#ListTypeCusIds").val() != null &&
                        $("#Search select#ListTypeCusIds").val().length > 0
                        ? $("#Search select#ListTypeCusIds").val()
                        : "";
                },
                "UnionIds": function () {
                    return $("#Search select#ListUnionIds").val() != null &&
                        $("#Search select#ListUnionIds").val().length > 0
                        ? $("#Search select#ListUnionIds").val()
                        : "";
                }
            },
            "dataSrc": function (json) {
                //debugger;
                // You can also modify `json.data` if required
                dataPermissions = json.permissions;
                return json.data;
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
                "data": "DossierName",
                "width": "50%",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    var html = "";
                    if (type === "display") {
                        var arrTypeCus = { "CONSUMER": "info", "BUSINESS": "success" };
                        var arrTypeContracts = { "1": "primary", "2": "success", "3": "info", "4": "warning", "5": "purple" };
                        html = '<div class="text-100"><div class="alert bgc-transparent radius-0 text-dark-tp2 border-none border-l-2 brc-{4}-m1 p-2 mb-0" role="alert"><span class="d-inline-block radius-round bgc-primary-l2 text-primary-d3 text-90 px-25 py-3px mx-2px my-2px text-500">{0}</span><div class="text-600 text-primary-d1"><span class="badge radius-2 bgc-white border-1 brc-{1}-m2 btn-text-{1} text-95 px-2 py-1 m-2px">{2}</span><span class="text-105 text-danger-d1">&nbsp; {3}</span></div><span class="m-1 badge badge-lg bgc-primary-l4 border-1 brc-primary-m3 text-danger-d2 px-3">{6}</span><span class="badge bgc-{4} brc-{4} badge-lg text-white arrowed arrowed-in-right mb-1">{5}</span><span class="d-inline-block radius-round bgc-purple-l2 text-dark-tp3 text-90 px-25 py-3px mx-2px my-2px">{7}</span><hr class="brc-secondary-l3 my-1"><span class="d-inline-block radius-round bgc-purple-l2 text-dark-tp3 px-25 py-3px mx-2px my-2px"><i class="far fa-address-card text-blue mx-1 w-2"></i>&nbsp;<span class="text-95 text-500 text-blue-d3">{8}</span></span></div></div>'.format(row["UnionName"], arrTypeCus[row["TypeCus"]], row["TypeCusName"], row["CusName"], arrTypeContracts[row["ContractTypeId"]], row["ContractTypeName"], row["ContractNoInfo"], data, row["LandParcelNo"] != null || row["MapNo"] != null ? "Thửa đất số " + row["LandParcelNo"] + " thuộc bản đồ số " + row["MapNo"] + " - " + row["Address"] : row["Address"]);

                        return html;
                    }
                    return data;
                }
            },
            {
                "data": "DelayDay",
                //"width": "45%",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    if (type == "display") {
                        var html = "";
                        var templateUl = '<ul>{0}</ul>';
                        var liTemplate = '<li class="{2}">{0} {1}</li>';

                        var receivedOn = row["ApprovedOn"]; //Ngày tiếp nhận
                        var giveResultOn = row["GiveResultOn"]; //Ngày hẹn trả
                        var delayDay = row["DelayDay"]; //Số ngày hẹn trả còn lại
                        var checkContractLate = row["CheckContractLate"]; //Kiểm tra trễ hạn  1:Chưa trễ, -1:Đã trễ, 0:Sắp đến hạn

                        if (row.Status > 0 && giveResultOn != null) {
                            if (checkContractLate == 0) {
                                var lateText = "Sắp đến hạn, còn";
                                var lateColor = "warning";
                            } else if (checkContractLate == -1) {
                                var lateText = "Trễ hạn";
                                var lateColor = "danger";
                                delayDay *= -1; // Đổi dấu để hiển thị số ngày trễ dương
                            } else {
                                var lateText = "Còn";
                            }

                            var late = liTemplate.format(lateText + ":", delayDay + " ngày", "text-600 text-120 text-" + lateColor);
                            if (row.Status < 3) {
                                html += late + '<li class="dropdown-divider brc-primary-l2"></li>';
                            }
                        }

                        var handleOnHtml = liTemplate.format("Thời gian tiếp nhận:", moment(receivedOn).format("DD/MM/YYYY HH:mm:ss"), "text-600 text-success-d1 text-105");
                        html += handleOnHtml;

                        if (row.Status > 0 && giveResultOn != null) {
                            var expiredOnHtml = liTemplate.format("Hạn hoàn thành:", moment(giveResultOn).format("DD/MM/YYYY"), "text-600 text-primary-d1");
                            html += expiredOnHtml;
                        }

                        return templateUl.format(html);

                    }
                }
            },
            {
                "data": "InStepName",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    var arrStatusColors = { "-1": "secondary", "1": "warning", "2": "primary", "3": "success" };
                    var html = "";
                    if (type === "display") {
                        html = '<span class="d-inline-block radius-round bgc-danger-l2 text-danger-d3 text-500 text-95 px-25 py-3px mx-2px my-2px">{0}</span><span class="badge bgc-{2} brc-{2} text-white badge-lg arrowed arrowed-in-right mb-1"><span class="px-3"> {1}</span></span>'.format(data, row["StatusName"], arrStatusColors[row["Status"]]);
                        return html;
                    }
                    return data;
                }
            },
            {
                "data": "DossierId",
                //"width": "100px",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';
                    html += '<div class="dropdown d-inline-block"><button class="btn px-4 btn-lighter-primary mr-1 v-hover dropdown-toggle" type="button" data-toggle="dropdown" title="Thao tác" data-rel="tooltip" aria-haspopup="true" aria-expanded="false"><i class="fa fa-ellipsis-h text-120"></i></button><div class="dropdown-menu dropdown-menu-right dropdown-caret px-1px py-2px border-1 brc-default-l1 shadow radius-1 dropdown-animated animated-2">';

                    if (type === "display") {
                        ///Vừa khởi tạo chờ xác nhận
                        if (row.Handle) {
                            if (row["Status"] == -1) {
                                html += _renderButton(true,
                                    "ContinueTask",
                                    "dropdown-item btn px-4 btn-outline-success mr-1",
                                    "/Major/Dossier/ContinueTask/" + row["TaskId"],
                                    '<i class="fas fa-play text-120"></i> Tiếp tục xử lý',
                                    "Tiếp tục xử lý");
                            }
                            else if (row["Status"] == 0) {
                                html += _renderButton(true,
                                    "ApproveDossier",
                                    "dropdown-item btn px-4 btn-outline-success mr-1",
                                    "/Major/Dossier/Approve/" + data,
                                    '<i class="fas fa-check-double text-120"></i> Xác nhận',
                                    "Xác nhận");

                                if (row["CanEdit"]) {
                                    html += _renderButton(true,
                                        "EditDossier",
                                        "dropdown-item btn px-4 btn-outline-primary mr-1",
                                        "/Major/Dossier/Edit/" + data,
                                        '<i class="far fa-edit text-120"></i> Cập nhật',
                                        "Cập nhật");
                                }

                                html += _renderButton(true,
                                    "DeleteDossier",
                                    "dropdown-item btn px-4 btn-outline-danger mr-1",
                                    "/Major/Dossier/Delete/" + data,
                                    '<i class="far fa-trash-alt text-120"></i> Xoá',
                                    "Xoá");
                            }
                            ///Đã xác nhận chờ xử lý
                            else if (row["Status"] == 1) {
                                html += _renderButton(true,
                                    "HandleDossier",
                                    "dropdown-item btn px-4 btn-outline-primary mr-1",
                                    "/Major/Dossier/Handle/" + row["TaskId"],
                                    '<i class="fas fa-user-check text-120"></i> Tiếp nhận xử lý',
                                    "Tiếp nhận xử lý", "1024px");
                            }
                            ///Hoàn thành xử lý
                            else if (row["Status"] == 2) {
                                html += _renderButton(true,
                                    "CompleteDossier",
                                    "dropdown-item btn px-4 btn-outline-success mr-1",
                                    "/Major/Dossier/Complete/" + row["TaskId"],
                                    '<i class="fas fa-clipboard-check text-120"></i> Hoàn thành xử lý',
                                    "Hoàn thành xử lý", "1024px");

                                if (row.AllowSwitchHandler && !row.SwitchedHandler) {
                                    html += _renderButton(true,
                                        "SwitchHandlers",
                                        "dropdown-item btn px-4 btn-outline-purple mr-1",
                                        "/Major/Dossier/SwitchHandlers/" + row["TaskId"],
                                        '<i class="fas fa-people-arrows text-120"></i> Chuyển người xử lý',
                                        "Chuyển người xử lý", "860", "data-scrollbars-inside=true'");
                                }
                                
                                html += _renderButton(true,
                                    "PauseTask",
                                    "dropdown-item btn px-4 btn-outline-secondary mr-1",
                                    "/Major/Dossier/PauseTask/" + row["TaskId"],
                                    '<i class="fas fa-pause text-120"></i> Tạm dừng xử lý',
                                    "Tạm dừng xử lý");
                            }
                        }
                        else if (row.Supervisor && row.TaskId != null) {
                            //html += _renderButton(true,
                            //    "SwitchHandlers",
                            //    "dropdown-item btn px-4 btn-outline-purple mr-1",
                            //    "/Major/Dossier/SwitchHandlers/" + row.TaskId,
                            //    '<i class="fas fa-exchange-alt text-120"></i> Chuyển người xử lý',
                            //    "Chuyển người xử lý");

                            html += _renderButton(true,
                                "ChangeHandler",
                                "dropdown-item btn px-4 btn-outline-danger mr-1",
                                "/Major/Dossier/ChangeHandler/" + row["TaskId"],
                                '<i class="fas fa-people-arrows text-120"></i> Chuyển người xử lý',
                                "Chuyển người xử lý");

                            html += _renderButton(true,
                                "ChangeNextStep",
                                "dropdown-item btn px-4 btn-outline-purple mr-1",
                                "/Major/Dossier/ChangeNextStep/" + row["TaskId"],
                                '<i class="fas fa-exchange-alt text-120"></i> Chuyển bước xử lý',
                                "Chuyển bước xử lý");
                        }

                        if (row.Status > 0 || row.Status == -1) {
                            html += _renderButton(true,
                                "Activity",
                                "dropdown-item btn px-4 btn-outline-info mr-1",
                                "/Major/Dossier/Activity/" + data,
                                '<i class="fas fa-clipboard-list text-120"></i> Thông tin xử lý',
                                "Thông tin xử lý", "1024", "data-aside-placement='right' data-aside-dismiss=true");
                        }

                        html += _renderButton(true,
                            "ViewContract",
                            "dropdown-item btn px-4 btn-outline-pink mr-1",
                            "/Major/Dossier/ViewContract?contractId=" + data,
                            '<i class="fas fa-eye text-120"></i> Thông tin hợp đồng',
                            "Thông tin hợp đồng", "1024");
                    }
                    html += "</div></span>";

                    return html;
                }
            }
        ],
        "createdRow": function (row, data, dataIndex) {
            if (data.Handle || (data.Supervisor && data.TaskId != null)) {
                $(row).addClass('bgc-danger-l4');
            }
        },
        "drawCallback": function (settings) {
            //var api = this.api();
            if (isDeferLoading != 0) {
                $("#DSDossier").parents('.table-responsive-md').removeClass('d-none');
            }
        }
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
                    _initElements(this);
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
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response).promise().done(function () {_initElements(this);});
    }
}
