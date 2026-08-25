var _UserFieldActionURLs = {
    UserField_GetData: "/Cate/UserField/Get"
};
var _tableUserField;

$(document).ready(function () {
    initTableUserField();
});

function initTableUserField() {
    _tableUserField = $("#DSUserField").DataTable({
        "responsive": true,
        "language": {
            "processing": "<div class='overlay'><i class='fas fa-cog fa-spin'></i></div>",
            "emptyTable": "Không có dữ liệu người dùng",
            "info": "Hiển thị _START_ đến _END_ của _TOTAL_ người dùng",
            "infoEmpty": "Hiển thị 0 đến 0 của 0 người dùng",
            "lengthMenu": "Hiển thị _MENU_ người dùng",
            "paginate": {
                "first": "Đầu",
                "last": "Cuối",
                "next": "Sau",
                "previous": "Trước"
            }
        },
        "lengthChange": true,
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": _UserFieldActionURLs.UserField_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": function (d) {
                d.Key = $("#Search #Key").val();
            }
        },
        "columns": [
            {
                "data": null,
                "orderable": false,
                "className": "text-center",
                "render": function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { "data": "UserName" },
            { "data": "FullName" },
            {
                "data": "FieldNames",
                "orderable": false,
                "render": function (data) {
                    if (!data) return '<span class="text-secondary font-italic">Chưa phân quyền lĩnh vực</span>';
                    var html = "";
                    data.split(",").forEach(function (name) {
                        if ($.trim(name) !== "") {
                            html += '<span class="badge badge-info mr-1 mb-1">' + $("<div/>").text($.trim(name)).html() + "</span>";
                        }
                    });
                    return html;
                }
            },
            {
                "data": "TotalField",
                "orderable": false,
                "className": "text-center",
                "render": function (data) {
                    var css = (data > 0) ? "badge-primary" : "badge-secondary";
                    return '<span class="badge ' + css + '">' + (data || 0) + "</span>";
                }
            },
            {
                "data": "UserName",
                "orderable": false,
                "className": "text-center",
                "render": function (data) {
                    return _renderButton(true,
                        "EditUserField",
                        "btn px-2 btn-lighter-primary mr-1 v-hover",
                        "/Cate/UserField/Edit/" + encodeURIComponent(data),
                        '<i class="far fa-edit text-primary text-120"></i>',
                        "Phân quyền lĩnh vực", { "data-width": "800px" });
                }
            }
        ],
        "order": [[1, "asc"]]
    });
}

function UserField_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        $("#ModalContent #modal_" + formId).modal("hide");
        $("#ModalContent #modal_" + formId).on("hidden.bs.modal", function () {
            if (response.status != undefined) {
                eval(response.message);
                if (typeof _tableUserField !== "undefined") {
                    _tableUserField.ajax.reload(null, false);
                }
                response.status = undefined;
            }
        });
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}

/* Cập nhật ô ẩn FieldIds mỗi khi người dùng tích / bỏ tích một lĩnh vực */
function onUserFieldChange(checkbox) {
    var $chk = $(checkbox);
    $("#card-field-" + $chk.val()).toggleClass("is-selected", $chk.is(":checked"));
    syncSelectedUserFields();
}

function toggleAllUserFields(isSelectAll) {
    $(".js-user-field").each(function () {
        $(this).prop("checked", isSelectAll);
        $("#card-field-" + $(this).val()).toggleClass("is-selected", isSelectAll);
    });
    syncSelectedUserFields();
}

function syncSelectedUserFields() {
    var selected = [];
    $(".js-user-field:checked").each(function () {
        selected.push($(this).val());
    });
    $("#FieldIds").val(selected.join(","));
    $("#countSelectedFields").text(selected.length);
}
