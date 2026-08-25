var _UnionActionURLs = {
    Union_GetData: "/Cate/Union/Get"
};
var _tableUnion;
$(document).ready(function () {
    initTableUnion();
});

var arrColors = { 1: "primary", 2: "danger" };

function initTableUnion() {
    _tableUnion = $("#DSUnion").DataTable({
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
            "url": _UnionActionURLs.Union_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": {
                "BelongUnions": function () {
                    return $("#Search select#BelongUnions").val() != null &&
                        $("#Search select#BelongUnions").val().length > 0
                        ? $("#Search select#BelongUnions").val()
                        : "";
                },
                "TypeUnions": function () {
                    return $("#Search select#TypeUnions").val() != null &&
                        $("#Search select#TypeUnions").val().length > 0
                        ? $("#Search select#TypeUnions").val()
                        : "";
                },
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
                "data": "UnionCode",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    if (type == "display") {
                        return ("{0}".format(data)).toUpperCase();
                    }
                    return data;
                }
            },
            {
                "data": "UnionName",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    if (type == "display") {
                        if (!row.IsActive) {
                            return '<s data-rel="tooltip" data-placement="auto" title="Ngưng hoạt động" >{0}</s>'.format(data);
                        }
                        else {
                            if (row["TypeUnionName"] != null) {
                                return '{0}<div><span class="badge badge-{2} text-85">{1}</span></div>'.format(data, row["TypeUnionName"], arrColors[row["TypeUnion"]]);
                            }
                                /*return '{3} - {0}<div><span class="badge badge-{2} text-85">{1}</span></div>'.format(data, row["TypeUnionName"], arrColors[row["TypeUnion"]], row["UnionCode"]);*/
                        }
                    }
                    return data;
                }
            },
            {
                "data": "BelongUnionName",
                "defaultContent": ""
            },
            {
                "data": "Note",
                "defaultContent": "",
                "visible": false
            },
            {
                "data": "UnionId",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';

                    if (type === "display") {
                        html += _renderButton(true,
                            "MembersBelong",
                            "btn px-4 btn-outline-green mr-1 v-hover",
                            "/Cate/Union/Members/" + data,
                            '<i class="fas fa-users text-120"></i>',
                            "Nhân sự", "1024px");

                        if (row["TypeUnion"] == 1) {
                            html += _renderButton(true,
                                "EditInfo",
                                "btn px-4 btn-lighter-primary mr-1 v-hover",
                                "/Cate/Union/Info/" + data,
                                '<i class="fas fa-user-edit text-purple text-120"></i>',
                                "Thông tin đơn vị");
                        }                        

                        html += _renderButton(true,
                            "EditUnion",
                            "btn px-4 btn-lighter-primary mr-1 v-hover",
                            "/Cate/Union/Edit/" + data,
                            '<i class="far fa-edit text-primary text-120"></i>',
                            "Cập nhật");

                        if (row.IsActive) {
                            html += _renderButton(true,
                                "ToggleStatus",
                                "btn px-4 btn-lighter-warning mr-1 v-hover",
                                "/Cate/Union/ToggleStatus/" + data,
                                '<i class="fas fa-lock text-warning text-120"></i>',
                                "Ngưng hoạt động");
                        } else {
                            html += _renderButton(true,
                                "ToggleStatus",
                                "btn px-4 btn-lighter-success mr-1 v-hover",
                                "/Cate/Union/ToggleStatus/" + data,
                                '<i class="fas fa-lock-open text-success text-120"></i>',
                                "Kích hoạt lại");
                        }

                        html += _renderButton(true,
                            "DeleteUnion",
                            "btn px-4 btn-lighter-danger mr-1 v-hover",
                            "/Cate/Union/Delete/" + data,
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

function Union_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableUnion.ajax.reload(null, false);
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
                        _tableUnion.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}
