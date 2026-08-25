var _CategoryActionURLs = {
    Category_GetData: "/Cate/Category/Get"
};
var _tableCategory;
$(document).ready(function () {
    initTableCategory();
});

function initTableCategory() {
    _tableCategory = $("#DSCategory").DataTable({
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
            "url": _CategoryActionURLs.Category_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": {
                "CateTypes": function () {
                    return $("#Search select#ListCates").val() != null &&
                        $("#Search select#ListCates").val().length > 0
                        ? $("#Search select#ListCates").val()
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
                "data": "CateCode",
                "defaultContent": ""
            },
            {
                "data": "CateName",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    var html = "";
                    if (type === "display") {
                        return '<div class="mx-2 text-grey-d1 my-auto"><div class="text-600 text-secondary-d1"><span class="text-110 btn-h-text-primary">{0}</span></div><span class="text-100 text-blue-d1"><i class="fas fa-bookmark"></i>&nbsp;{1}</span></div>'.format(data, row["CateTypeName"]);
                    }
                    return data;
                }
            },
            {
                "data": "CateParentName",
                "defaultContent": ""
            },
            {
                "data": "CateId",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';

                    if (type === "display") {
                        html += _renderButton(true,
                            "EditCategory",
                            "btn px-4 btn-lighter-primary mr-1 v-hover",
                            "/Cate/Category/Edit/" + data,
                            '<i class="far fa-edit text-primary text-120"></i>',
                            "Cập nhật");

                        html += _renderButton(true,
                            "DeleteCategory",
                            "btn px-4 btn-lighter-danger mr-1 v-hover",
                            "/Cate/Category/Delete/" + data,
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

function Category_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableCategory.ajax.reload(null, false);
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
                        _tableCategory.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}
