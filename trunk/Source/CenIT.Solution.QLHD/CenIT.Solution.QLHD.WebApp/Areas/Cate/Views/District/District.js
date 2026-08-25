var _DistrictActionURLs = {
    District_GetData: "/Cate/District/Get"
};
var _tableDistrict;
$(document).ready(function () {
    initTableDistrict();
});

function initTableDistrict() {
    _tableDistrict = $("#DSDistrict").DataTable({
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
            "url": _DistrictActionURLs.District_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": {
                "ProvinceIds": function () {
                    return $("#Search select#ListProvinceId").val() != null &&
                        $("#Search select#ListProvinceId").val().length > 0
                        ? $("#Search select#ListProvinceId").val()
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
                "data": "ProvinceName",
                "defaultContent": ""
            },
            {
                "data": "DistrictCode",
                "defaultContent": ""
            },
            {
                "data": "DistrictName",
                "defaultContent": ""
            },
            {
                "data": "DistrictId",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';
                    if (type === "display") {
                        html += _renderButton(true,
                            "EditDistrict",
                            "btn px-4 btn-lighter-primary mr-1 v-hover",
                            "/Cate/District/Edit/" + data,
                            '<i class="far fa-edit text-primary text-120"></i>',
                            "Cập nhật");

                        html += _renderButton(true,
                            "ListWards",
                            "btn px-4 btn-lighter-purple mr-1 v-hover",
                            "/Cate/Ward/WardsViaDistrict/" + data,
                            '<i class="fas fa-list-ul text-purple text-120"></i>',
                            "Danh sách Phường/Xã",
                            1024);

                        html += _renderButton(true,
                            "DeleteDistrict",
                            "btn px-4 btn-lighter-danger mr-1 v-hover",
                            "/Cate/District/Delete/" + data,
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

function District_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableDistrict.ajax.reload(null, false);
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
                        _tableDistrict.ajax.reload(null, false);
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