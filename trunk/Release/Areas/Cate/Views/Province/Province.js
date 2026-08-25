var _ProvinceActionURLs = {
    Province_GetData: "/Cate/Province/Get"
};
var _tableProvince;
$(document).ready(function () {
    initTableProvince();
});

function initTableProvince() {
    _tableProvince = $("#DSProvince").DataTable({
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
            "url": _ProvinceActionURLs.Province_GetData,
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
                "data": "ProvinceCode",
                "defaultContent": ""
            },
            {
                "data": "ProvinceName",
                "defaultContent": ""
            },
            {
                "data": "ProvinceId",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';

                    if (type === "display") {
                        html += _renderButton(true,
                            "EditProvince",
                            "btn px-4 btn-lighter-primary mr-1 v-hover",
                            "/Cate/Province/Edit/" + data,
                            '<i class="far fa-edit text-primary text-120"></i>',
                            "Cập nhật");

                        html += _renderButton(true,
                            "ListDistricts",
                            "btn px-4 btn-lighter-purple mr-1 v-hover",
                            "/Cate/District/DistrictsViaProvince/" + data,
                            '<i class="fas fa-list-ul text-purple text-120"></i>',
                            "Danh sách Quận/Huyện",
                            1024);

                        html += _renderButton(true,
                            "DeleteProvince",
                            "btn px-4 btn-lighter-danger mr-1 v-hover",
                            "/Cate/Province/Delete/" + data,
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

function Province_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableProvince.ajax.reload(null, false);
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
                        _tableProvince.ajax.reload(null, false);
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