
var _ImplementContentActionURLs = {
    ImplementContent_GetData: "/Cate/ImplementContent/Get"
};
var _tableImplementContent;




$(document).ready(function () {
    initTableImplementContent()
});

function Search() {
    _tableImplementContent.ajax.reload(null, false);
}

function initTableImplementContent() {
    _tableImplementContent = $("#DSGPCN").DataTable({
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
            "url": _ImplementContentActionURLs.ImplementContent_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": {
                "TuKhoa": function () { return $("#Search #TuKhoa").val() },
                },
                
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
                "data": "WorkContent",
                "defaultContent": ""
            },
            {
                "data": "WorkPurpose",
                "defaultContent": ""
            },
            {
                "data": "FilePath",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    if (type === "display") {
                        console.log(data);
                        if (data !== null) {
                            return '<a href="' + data + '" download style="text-align: center; display: block;"><span class="fa fa-download"></a>';
                        } else {
                            return "";
                        }
                    }
                    return data;
                }
            },
            {
                "data": "ImplementContentId",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';
                    if (type === "display") {
                        html += _renderButton(true,
                            "EditImplementContent",
                            "btn px-4 btn-lighter-primary mr-1 v-hover",
                            "/Cate/ImplementContent/Edit/" + data,
                            '<i class="fa fa-edit text-primary text-120"></i>',
                            "Cập nhật");
                        html += _renderButton(true,
                            "DeleteImplementContent",
                            "btn px-4 btn-lighter-danger mr-1 v-hover",
                            "/Cate/ImplementContent/Delete/" + data,
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


function ImplementContent_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableImplementContent.ajax.reload(null, false);
                response.status = undefined;
                $("#ModalContent #modal_" + formId + " form")[0].reset();
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
                        _tableImplementContent.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}