
var _HolidayActionURLs = {
    Holiday_GetData: "/Cate/Holiday/Get"
};
var _tableHoliday;

$(document).ready(function () {
    initTableHoliday()
});

function Search() {
    _tableHoliday.ajax.reload(null, false);
}

function initTableHoliday() {
    _tableHoliday = $("#DSHoliday").DataTable({
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
            "url": _HolidayActionURLs.Holiday_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": function (d) {
                d.LunarCalendar = getLunarCalendar();
            }
        },
        "columns": [
            {
                "data": "",
                "className": "text-center",
                "defaultContent": "1",
                "render": function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                "data": "IsLunarCalendar",
                "render": function (data, type, row, meta) {
                    if (data) {
                        return '<span class="badge bgc-info brc-info text-white badge-lg arrowed arrowed-in-right"><span class="px-3">' + 'Âm lịch' + '</span></span>';
                    } else {
                        return '<span class="badge bgc-success brc-success text-white badge-lg arrowed arrowed-in-right"><span class="px-3">' + 'Dương lịch' + '</span></span>';
                    }
                }
            },
            {
                "data": "Date",
                "defaultContent": "",
                //"render": function (data, type, row, meta) {
                //    var lunarBadge = row.IsLunarCalendar ? '<span class="label label-info">Âm lịch</span>' : '<span class="label label-success">Dương lịch</span>';
                //    return '<div class="clearfix">' +
                //        '<div class="pull-left">' + data + '</div>' +
                //        '<div class="pull-right">' + lunarBadge + '</div>' +
                //        '</div>';
                //}
            },
            {
                "data": "HolidayName",
                "defaultContent": ""
            },
            {
                "data": "IsPermanent",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    return '<input type="checkbox" ' + (data ? 'checked' : '') + ' disabled>';
                }
            },
            {
                "data": "HolidayId",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';
                    if (type === "display") {
                        html += _renderButton(true,
                            "EditHoliday",
                            "btn px-4 btn-lighter-primary mr-1 v-hover",
                            "/Cate/Holiday/Edit/" + data,
                            '<i class="fa fa-edit text-primary text-120"></i>',
                            "Cập nhật");
                        html += _renderButton(true,
                            "DeleteHoliday",
                            "btn px-4 btn-lighter-danger mr-1 v-hover",
                            "/Cate/Holiday/Delete/" + data,
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

function Holiday_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableHoliday.ajax.reload(null, false);
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
                        _tableHoliday.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}


function getLunarCalendar() {
    // Lấy giá trị của userType từ các phần tử radio button
    var lunarCalendar = $("input[name='LunarCalendar']:checked").val();
    return lunarCalendar;
}