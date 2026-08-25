
var _SubSectionActionURLs = {
    SubSection_GetData: "/Cate/SubSection/Get"
};
var _tableSubSection;

$(document).ready(function () {
    initTableSubSection()
});

function Search() {
    _tableSubSection.ajax.reload(null, false);
}

function initTableSubSection() {
    _tableSubSection = $("#DSSubSection").DataTable({
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
            "url": _SubSectionActionURLs.SubSection_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": {
                "mainSection": function () { return $("#bodyForm #Cate_MainSectionId").val() ? $("#bodyForm #Cate_MainSectionId").val() : 0; },
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
                "data": "SubSectionName",
                "defaultContent": ""
            },
            {
                "data": "MainSectionName",
                "defaultContent": ""
            },
            {
                "data": "Unit",
                "defaultContent": ""
            },
            {
                "data": "Price",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    if (data != null) {
                        return row.Price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".") + "đ";
                    }
                }
            },
            {
                "data": "SubSectionId",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';
                    if (type === "display") {
                        html += _renderButton(true,
                            "EditSubSection",
                            "btn px-4 btn-lighter-primary mr-1 v-hover",
                            "/Cate/SubSection/Edit/" + data,
                            '<i class="fa fa-edit text-primary text-120"></i>',
                            "Cập nhật");
                        html += _renderButton(true,
                            "DeleteSubSection",
                            "btn px-4 btn-lighter-danger mr-1 v-hover",
                            "/Cate/SubSection/Delete/" + data,
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

function SubSection_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableSubSection.ajax.reload(null, false);
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
                        _tableSubSection.ajax.reload(null, false);
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