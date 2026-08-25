var _WardActionURLs = {
    Ward_GetData: "/Cate/Ward/Get"
};
var _tableWard;
$(document).ready(function() {
    initTableWard();
});

function initTableWard() {
    _tableWard = $("#DSWard").DataTable({
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
            "url": _WardActionURLs.Ward_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": {
                "ProvinceIds": function() {
                    return $("#Search select#ListProvinceId").val() != null &&
                        $("#Search select#ListProvinceId").val().length > 0
                        ? $("#Search select#ListProvinceId").val()
                        : "";
                },
                "DistrictIds": function() {
                    return $("#Search select#ListDistrictId").val() != null &&
                        $("#Search select#ListDistrictId").val().length > 0
                        ? $("#Search select#ListDistrictId").val()
                        : "";
                }
            }
        },
        "columns": [
            {
                "data": "",
                "defaultContent": "1",
                "render": function(data, type, row, meta) {
                    return meta.settings._iDisplayStart + meta.row + 1;
                }
            },
            {
                "data": "DistrictName",
                "defaultContent": ""
            },
            {
                "data": "WardCode",
                "defaultContent": ""
            },
            {
                "data": "WardName",
                "defaultContent": ""
            },
            {
                "data": "WardId",
                "style": "width:100px;",
                "orderable": false,
                "render": function(data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';

                    if (type === "display") {
                        html += _renderButton(true,
                            "EditWard",
                            "btn px-4 btn-lighter-primary mr-1 v-hover",
                            "/Cate/Ward/Edit/" + data,
                            '<i class="far fa-edit text-primary text-120"></i>',
                            "Cập nhật");

                        html += _renderButton(true,
                            "ListTeams",
                            "btn px-4 btn-lighter-purple mr-1 v-hover",
                            "/Cate/Team/TeamsViaWard/" + data,
                            '<i class="fas fa-list-ul text-purple text-120"></i>',
                            "Danh sách Khu vực",
                            1024);

                        html += _renderButton(true,
                            "ListStreets",
                            "btn px-4 btn-lighter-default mr-1 v-hover",
                            "/Cate/Street/StreetsViaWard/" + data,
                            '<i class="fas fa-map text-default text-120"></i>',
                            "Danh sách Tuyến đường",
                            1024);

                        html += _renderButton(true,
                            "DeleteWard",
                            "btn px-4 btn-lighter-danger mr-1 v-hover",
                            "/Cate/Ward/Delete/" + data,
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

function Ward_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableWard.ajax.reload(null, false);
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
                        _tableWard.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}

function OnChangeCombo(cbb, eleName) {
    $(eleName).val($(cbb).children("option:selected").text());

}