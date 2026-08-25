var _ContractActionURLs = {
    Contract_GetData: "/Major/Contract/Get"
};
var _tableContracts;
$(document).ready(function () {
    initTableContract();
});

function onSearchContract() {
    $("#ListContracts").parents('.table-responsive-md').removeClass('d-none');
    _tableContracts.ajax.reload(null, false);
}

function initTableContract() {
    _tableContracts = $("#ListContracts").DataTable({
        "Responsive": true,
        "language": {
            "processing": '<div id="tableLoading" class="pageload-overlay show pageload-loading" data-opening="M 0,0 80,-10 80,60 0,70 0,0" data-closing="M 0,-10 80,-20 80,-10 0,0 0,-10" style=""><svg xmlns="http://www.w3.org/2000/svg" width="100%" height="100%" viewBox="0 0 80 60" preserveAspectRatio="none"><path d="M 0,70 80,60 80,80 0,80 0,70"></path></svg></div>'
            /*"<div class='overlay'><i class='fas fa-cog fa-spin'></i></div>"*/
        },
        "order": [[0, 'asc']],
        "aaSorting": [[0, "asc"]],
        "lengthChange": true,
        "processing": true,
        "serverSide": true,
        "searching": false,
        "deferLoading": 0,
        "ajax":
        {
            "url": _ContractActionURLs.Contract_GetData,
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
                "ContractStatus": function () {
                    return $("#Search select#ListContractStatusIds").val() != null &&
                        $("#Search select#ListContractStatusIds").val().length > 0
                        ? $("#Search select#ListContractStatusIds").val()
                        : "";
                    //return $("#Search [name='ListContractStatusIds']:checked").val() != null &&
                    //    $("#Search [name='ListContractStatusIds']:checked").val().length > 0
                    //    ? $.map($("#Search [name='ListContractStatusIds']:checked"), function (n, i) { return n.value }).join(",")
                    //    : "";
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
                "data": "ContractNo",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    var html = "";
                    if (type === "display") {
                        var arrTypeCus = { "CONSUMER": "primary", "BUSINESS": "success" };
                        var arrTypeContracts = { "1": "primary", "2": "success", "3": "info", "4": "warning", "5": "purple" };
                        if (row["ContractNoInfo"] != null && row["ContractNoInfo"].length > 0) {
                            html = '<div class="text-100"><div class="alert bgc-transparent radius-0 text-dark-tp2 border-none border-l-2 brc-{4}-m1 p-2 mb-0" role="alert"><span class="d-inline-block radius-round bgc-primary-l2 text-primary-d3 text-90 px-25 py-3px mx-2px my-2px text-500">{0}</span><div class="text-600 text-primary-d1"><span class="badge radius-2 bgc-white border-1 brc-{1}-m2 btn-text-{1} text-95 px-2 py-1 m-2px">{2}</span><span class="text-105 text-danger-d1">&nbsp; {3}</span></div><span class="m-1 badge badge-lg bgc-primary-l4 border-1 brc-primary-m3 text-danger-d2 px-3">{6}</span><span class="badge bgc-{4} brc-{4} badge-lg text-white arrowed arrowed-in-right mb-1">{5}</span><span class="badge bgc-purple brc-purple badge-lg text-white mr-1 arrowed arrowed-in-right">{9}</span><span class="d-inline-block radius-round bgc-purple-l2 text-dark-tp3 text-90 px-25 py-3px mx-2px my-2px">{7}</span><hr class="brc-secondary-l3 my-1"><span class="d-inline-block radius-round bgc-purple-l2 text-dark-tp3 px-25 py-3px mx-2px my-2px"><i class="far fa-address-card text-blue mx-1 w-2"></i>&nbsp;<span class="text-95 text-500 text-blue-d3">{8}</span></span></div></div>'.format(row["UnionName"], arrTypeCus[row["TypeCus"]], row["TypeCusName"], row["CusName"], arrTypeContracts[row["ContractTypeId"]], row["ContractTypeName"], row["ContractNoInfo"], row["PurposeName"], row["LandParcelNo"] != null || row["MapNo"] != null ? "Thửa đất số " + row["LandParcelNo"] + " thuộc bản đồ số " + row["MapNo"] + " - " + row["Address"] : row["Address"], row.HasInv ? "Đã phát hành HĐ" : "");
                        }
                        else {
                            html = '<div class="text-100"><div class="alert bgc-transparent radius-0 text-dark-tp2 border-none border-l-2 brc-{4}-m1 p-2 mb-0" role="alert"><span class="d-inline-block radius-round bgc-primary-l2 text-primary-d3 text-90 px-25 py-3px mx-2px my-2px text-500">{0}</span><div class="text-600 text-primary-d1"><span class="badge radius-2 bgc-white border-1 brc-{1}-m2 btn-text-{1} text-95 px-2 py-1 m-2px">{2}</span><span class="text-105 text-danger-d1">&nbsp; {3}</span></div><span class="badge bgc-{4} brc-{4} badge-lg text-white arrowed arrowed-in-right mb-1">{5}</span><span class="badge bgc-success brc-success text-white badge-lg mr-1 arrowed arrowed-in-right">{8}</span><span class="d-inline-block radius-round bgc-purple-l2 text-dark-tp3 text-90 px-25 py-3px mx-2px my-2px">{6}</span><hr class="brc-secondary-l3 my-1"><span class="d-inline-block radius-round bgc-purple-l2 text-dark-tp3 px-25 py-3px mx-2px my-2px"><i class="far fa-address-card text-blue mx-1 w-2"></i>&nbsp;<span class="text-95 text-500 text-blue-d3">{7}</span></span></div></div>'.format(row["UnionName"], arrTypeCus[row["TypeCus"]], row["TypeCusName"], row["CusName"], arrTypeContracts[row["ContractTypeId"]], row["ContractTypeName"], row["PurposeName"], row["LandParcelNo"] != null || row["MapNo"] != null ? "Thửa đất số " + row["LandParcelNo"] + " thuộc bản đồ số " + row["MapNo"] + " - " + row["Address"] : row["Address"], row.HasInv ? "Đã phát hành HĐ" : "");
                        }
                        return html;
                    }
                    return data;
                }
            },
            {
                "data": "DelayDay",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    if (type == "display") {
                        var html = "";
                        var templateUl = '<ul>{0}</ul>';
                        var liTemplate = '<li class="{2}">{0} {1}</li>';

                        //var receivedOn = row["ReceivedOn"]; //Ngày tạo hợp đồng nháp
                        var confirmOn = row["ConfirmOn"]; //Ngày tiếp nhận
                        var giveResultOn = row["GiveResultOn"]; //Ngày hẹn trả
                        var rejectOn = row["RejectOn"]; //Ngày hủy
                        var delayDay = row["DelayDay"]; //Số ngày hẹn trả còn lại
                        var checkContractLate = row["CheckContractLate"]; //Kiểm tra trễ hạn  1:Chưa trễ, -1:Đã trễ, 0:Sắp đến hạn

                        if (row.Status != 2 && giveResultOn != null) {
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
                        if (confirmOn != null && confirmOn != undefined) {
                            var handleOnHtml = liTemplate.format("Ngày tiếp nhận:", moment(confirmOn).format("DD/MM/YYYY HH:mm:ss"), "text-600 text-success-d1 text-105");
                            html += handleOnHtml;
                        }

                        if (giveResultOn != null) {
                            var expiredOnHtml = liTemplate.format("Ngày hẹn trả:", moment(giveResultOn).format("DD/MM/YYYY"), "text-600 text-primary-d1");
                            html += expiredOnHtml;
                        }
                        else if (rejectOn != null) {
                            var rejectOnHtml = liTemplate.format("Ngày hủy:", moment(rejectOn).format("DD/MM/YYYY"), "text-600 text-danger-d1");
                            html += rejectOnHtml;
                        }

                        return templateUl.format(html);

                    }
                }
            },
            {
                "data": "Status",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    if (type == "display") {
                        var arrStatusContracts = { "-1": "info", "0": "warning", "1": "primary", "2": "secondary", "3": "success", "4": "purple", "99": "danger" };
                        return '<div class="mx-2 my-auto"><span class="badge bgc-{0} brc-{0} text-white badge-lg arrowed arrowed-in-right mb-1">{1}</span></div>'.format(arrStatusContracts[data], row["StatusName"]);
                    }
                    return data;
                }
            },
            {
                "data": "ContractId",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';
                    html += '<div class="dropdown d-inline-block"><button class="btn px-4 btn-lighter-primary mr-1 v-hover dropdown-toggle" type="button" data-toggle="dropdown" title="Thao tác" data-rel="tooltip" aria-haspopup="true" aria-expanded="false"><i class="fa fa-ellipsis-h text-120"></i></button><div class="dropdown-menu dropdown-menu-right dropdown-caret px-1px py-2px border-1 brc-default-l1 shadow radius-1 dropdown-animated animated-2">';

                    if (type === "display") {
                        ///Vừa khởi tạo chờ xác nhận
                        if (row.CanEdit) {
                            if (row.Status == -1) {
                                html += _renderButton(true,
                                    "ValidContract",
                                    "dropdown-item btn px-4 btn-outline-info mr-1",
                                    "/Major/Contract/Valid?contractId=" + data,
                                    '<i class="fas fa-user-check text-120"></i> Tiếp nhận',
                                    "Tiếp nhận");

                                html += _renderButton(true,
                                    "EditContract",
                                    "dropdown-item btn px-4 btn-outline-primary mr-1",
                                    "/Major/Contract/Edit?contractId=" + data,
                                    '<i class="far fa-edit text-120"></i> Cập nhật',
                                    "Cập nhật", "1024px");

                                html += _renderButton(true,
                                    "DeleteContract",
                                    "dropdown-item btn px-4 btn-outline-danger mr-1",
                                    "/Major/Contract/Delete?contractId=" + data,
                                    '<i class="far fa-trash-alt text-120"></i> Xoá',
                                    "Xoá");
                            }
                            else if (row.Status == 0) {
                                html += _renderButton(true,
                                    "ApproveContract",
                                    "dropdown-item btn px-4 btn-outline-success mr-1",
                                    "/Major/Contract/Approve?contractId=" + data,
                                    '<i class="fas fa-check-double text-120"></i> Xác nhận',
                                    "Xác nhận");

                                html += _renderButton(true,
                                    "RejectContract",
                                    "dropdown-item btn px-4 btn-outline-danger mr-1",
                                    "/Major/Contract/Reject?contractId=" + data,
                                    '<i class="fas fa-times-circle text-120"></i> Từ chối',
                                    "Từ chối");
                            }
                            else if (row.Status > 2 && row.Status != 99) {
                                //if (row.Status == 3) {
                                //    html += _renderButton(true,
                                //        "Acceptant",
                                //        "dropdown-item btn px-4 btn-outline-danger mr-1",
                                //        "/Major/Contract/Acceptant?contractId=" + data,
                                //        '<i class="fas fa-spell-check text-120"></i> Nghiệm thu và thanh lý',
                                //        "Nghiệm thu và thanh lý");
                                //}
                                if (row.IsPaid) {
                                    if (!row.HasInv) {
                                        html += _renderButton(true,
                                            "PublishInv",
                                            "dropdown-item btn px-4 btn-outline-success mr-1",
                                            "/Major/Contract/PublishInv?contractId=" + data,
                                            '<i class="fas fa-file-invoice text-120"></i> Phát hành hoá đơn',
                                            "Phát hành hoá đơn", "1024px");
                                    }
                                    if (row.Status != 4) {
                                        html += _renderButton(true,
                                            "Acceptant",
                                            "dropdown-item btn px-4 btn-outline-danger mr-1",
                                            "/Major/Contract/Acceptant?contractId=" + data,
                                            '<i class="fas fa-spell-check text-120"></i> Nghiệm thu và thanh lý',
                                            "Nghiệm thu và thanh lý");
                                    }
                                    else if (row.Status == 4) {
                                        //html += _renderButton(true,
                                        //    "ShowAcceptant",
                                        //    "dropdown-item btn px-4 btn-outline-danger mr-1",
                                        //    "/Major/Contract/ShowAcceptant?contractId=" + data + "&fileType=.pdf",
                                        //    '<i class="fas fa-eye text-120"></i> Biên bản Nghiệm thu và thanh lý',
                                        //    "Biên bản Nghiệm thu và thanh lý", "fullscreen");
                                    }
                                }
                            }
                            //else if (row.Status == 1) { }

                        }

                        if (row.Status == 4) {
                            html += _renderButton(true,
                                "ShowAcceptant",
                                "dropdown-item btn px-4 btn-outline-danger mr-1",
                                "/Major/Contract/ShowAcceptant?contractId=" + data + "&fileType=.pdf",
                                '<i class="fas fa-eye text-120"></i> Biên bản Nghiệm thu và thanh lý',
                                "Biên bản Nghiệm thu và thanh lý", "fullscreen", "data-scrollbars-inside=true");
                        }
                        if (row.Status > -1) {
                            html += _renderButton(true,
                                "Payments",
                                "dropdown-item btn px-4 btn-outline-purple mr-1",
                                "/Major/Contract/Payments?contractId=" + data,
                                '<i class="fas fa-file-invoice-dollar text-120"></i> Thông tin thanh toán',
                                "Thông tin thanh toán", "1024px");
                        }
                        if (row.HasInv) {
                            html += _renderButton(true,
                                "ViewInv",
                                "dropdown-item btn px-4 btn-outline-green mr-1",
                                "/Major/Contract/ViewInv?contractId=" + data,
                                '<i class="fas fa-file-invoice-dollar text-120"></i> Xem hoá đơn',
                                "Xem hoá đơn", "1024px", "data-scrollbars-inside=true");
                        }
                        html += _renderButton(true,
                            "ViewContract",
                            "dropdown-item btn px-4 btn-outline-info mr-1",
                            "/Major/Contract/ViewContract?contractId=" + data,
                            '<i class="fas fa-eye text-120"></i> Xem hợp đồng',
                            "Xem hợp đồng", "fullscreen", "data-scrollbars-inside=true");

                        if (row.Status == 99) {
                            html += _renderButton(true,
                                "UploadFile",
                                "dropdown-item btn px-4 btn-outline-success mr-1",
                                "/Major/Contract/UploadFile?contractId=" + data,
                                '<i class="fas fa-cloud-upload-alt text-120"></i> Tải tệp lên',
                                "Tải tệp lên", "860");
                        }
                        //html += _renderButton(true,
                        //    "RenderContract",
                        //    "dropdown-item btn px-4 btn-outline-warning mr-1",
                        //    "/Major/Contract/ShowRender?contractId=" + data,
                        //    '<i class="fas fa-file-pdf text-120"></i> Xem hợp đồng',
                        //    "Xem hợp đồng", "1024px");
                    }
                    html += "</div></span>";

                    return html;
                }
            }
        ],
        "createdRow": function (row, data, dataIndex) {
            if (data.CanEdit && data.Status != 99) {
                $(row).addClass('bgc-danger-l4');
            }
        }
    });
}

function Contract_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableContracts.ajax.reload(null, false);
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
                        _tableContracts.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response).promise().done(function () { _initElements(this); });
    }
}
