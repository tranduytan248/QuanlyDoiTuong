var _UserActionURLs = {
    User_GetData: "/Sys/User/Get"
};
var _tableUser;

$(document).ready(function () {
    initTableUser();
});

function initTableUser() {
    _tableUser = $("#DSUser").DataTable({
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
            "url": _UserActionURLs.User_GetData,
            "type": "POST",
            "dataType": "JSON"
        },
        "columns": [
            {
                "data": "",
                "className": "text-center",
                "defaultContent": "1",
                "render": function (data, type, row, meta) {
                    return meta.settings._iDisplayStart + meta.row + 1;
                }
            },
            {
                "data": "FullName",
                "defaultContent": "",
                "className": "pl-2 pl-lg-4 d-flex",
                "render": function (data, type, row, meta) {
                    var avatarPath = row["AvatarPath"] == null ? "/Contents/Base/imgs/avatar-default.png" : row["AvatarPath"];
                    var html = "";
                    if (type === "display") {
                        if (data == null) data = "";
                        var extendInfo = '<ul class="list-unstyled text-dark-tp3">{0}</ul>';
                        var extendInfoItem = '<li class="mb-1 {2}"><i class="w-3 text-center {1} text-95"></i>&nbsp;{0}</li>';
                        var htmlExtendInfo = extendInfoItem.format(row["Email"], "fas fa-envelope", "text-green-d2");
                        if (row["Phone"] != null) {
                            htmlExtendInfo += extendInfoItem.format(row["Phone"], "fas fa-mobile", "text-primary-d2");
                        }

                        html = ' {2}<div class="mx-2 text-grey-d1 my-auto"><div class="text-600 text-blue-d1"><span class="text-95 btn-text-dark btn-h-text-primary">{0}</span></div>{1}</div>'
                            .format(data, extendInfo.format(htmlExtendInfo), '<img alt="{0}" src="{1}" class="radius-round mr-2 w-10 my-auto">'.format(data, avatarPath));

                        return html;
                    }
                    return data;
                }
            },
            {
                "data": "UserName",
                "defaultContent": ""
            },
            {
                // Don vi cong tac va chuc vu - gop chung mot cot cho gon
                "data": "UnionName",
                "defaultContent": "",
                "orderable": false,
                "render": function (data, type, row) {
                    if (type !== "display") return data || "";
                    var unit = data || "";
                    var pos = row["PositionName"] || "";
                    if (!unit && !pos) {
                        return '<span class="text-grey-m1 text-90"><i class="fa fa-minus"></i> Chưa phân đơn vị</span>';
                    }
                    var html = "";
                    if (unit) {
                        html += '<div class="text-600 text-blue-d1 text-95">'
                              + '<i class="fa fa-sitemap text-grey-m1 mr-1"></i>' + unit + '</div>';
                    }
                    if (pos) {
                        html += '<div class="text-grey-d1 text-90 mt-1">' + pos + '</div>';
                    }
                    return html;
                }
            },
            {
                // Vai tro he thong - moi vai tro mot nhan
                "data": "RoleNames",
                "defaultContent": "",
                "orderable": false,
                "render": function (data, type) {
                    if (type !== "display") return data || "";
                    if (!data) {
                        return '<span class="text-grey-m1 text-90"><i class="fa fa-minus"></i> Chưa phân vai trò</span>';
                    }
                    return data.split(",").map(function (r) {
                        return '<span class="badge badge-sm bgc-blue-l3 text-blue-d2 mr-1 mb-1 px-2 py-1">'
                             + r.trim() + '</span>';
                    }).join("");
                }
            },
            {
                // So linh vuc duoc phan. Bang 0 thi nguoi dung khong thao tac
                // duoc gi voi du lieu doi tuong - canh bao mau do.
                "data": "FieldCount",
                "defaultContent": "0",
                "className": "text-center",
                "orderable": false,
                "render": function (data, type) {
                    if (type !== "display") return data;
                    var n = parseInt(data, 10) || 0;
                    if (n === 0) {
                        return '<span class="badge badge-sm bgc-red-l3 text-red-d2 px-2 py-1" '
                             + 'title="Chưa được phân lĩnh vực nên không thao tác được dữ liệu đối tượng">'
                             + '<i class="fa fa-exclamation-triangle mr-1"></i>Chưa phân</span>';
                    }
                    return '<span class="badge badge-sm bgc-green-l3 text-green-d2 px-2 py-1">'
                         + n + ' lĩnh vực</span>';
                }
            },
            {
                // Trang thai tai khoan: khoa > ngung > dang hoat dong
                "data": "IsActive",
                "defaultContent": "",
                "className": "text-center",
                "orderable": false,
                "render": function (data, type, row) {
                    if (type !== "display") return data;
                    var html = "";
                    if (row["IsLocked"]) {
                        html = '<span class="badge badge-sm bgc-red-l3 text-red-d2 px-2 py-1">'
                             + '<i class="fa fa-lock mr-1"></i>Bị khoá</span>';
                    } else if (data) {
                        html = '<span class="badge badge-sm bgc-green-l3 text-green-d2 px-2 py-1">'
                             + '<i class="fa fa-check-circle mr-1"></i>Hoạt động</span>';
                    } else {
                        html = '<span class="badge badge-sm bgc-grey-l3 text-grey-d2 px-2 py-1">'
                             + '<i class="fa fa-ban mr-1"></i>Ngưng</span>';
                    }
                    // KHONG hien dau hieu truc tuyen: cot IsOnline trong Sys_Users
                    // khong duoc cap nhat o bat ky dau (khong proc nao ghi, dang
                    // nhap / dang xuat deu khong dung toi) nen gia tri la du lieu
                    // chet, hien len se sai lech.
                    return html;
                }
            },
            {
                "data": "UserId",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';
                    if (type === "display") {
                        html += '<div class="dropdown d-inline-block"><button class="btn px-4 btn-lighter-primary mr-1 v-hover dropdown-toggle" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="fa fa-ellipsis-h text-120"></i></button><div class="dropdown-menu dropdown-menu-right">';
                        if (row.IsActive) {
                            html += _renderButton(true,
                                "UnionsBelong",
                                "btn btn-outline-purple mr-1  dropdown-item",
                                "/Cate/Union/UnionsBelong/" + data,
                                '<i class="fas fa-layer-group text-120"></i> Quản lý Đơn vị',
                                "Quản lý Đơn vị", "1024px");

                            html += _renderButton(true,
                                "EditMember",
                                "btn btn-outline-default mr-1 dropdown-item",
                                "/Cate/Union/EditMember?userId=" + data,
                                '<i class="fas fa-university text-120"></i> Đơn vị công tác',
                                "Đơn vị công tác");

                            html += _renderButton(true,
                                "PermitUserField",
                                "btn btn-outline-success mr-1 dropdown-item",
                                "/Cate/UserField/Edit?userName=" + encodeURIComponent(row.UserName),
                                '<i class="fas fa-layer-group text-120"></i> Phân quyền lĩnh vực',
                                "Phân quyền lĩnh vực", "800px");

                            html += _renderButton(true,
                                "EditUser",
                                "btn btn-outline-success mr-1 dropdown-item",
                                "/Sys/User/Edit/" + data,
                                '<i class="fa fa-edit text-120"></i> Cập nhật',
                                "Cập nhật", 860);

                            html += _renderButton(true,
                                "PermitReport",
                                "btn btn-outline-info mr-1 dropdown-item",
                                "/Major/Report/PermitReport?forUser=" + row.UserName,
                                '<i class="fas fa-tasks text-120"></i> Phân quyền báo cáo',
                                "Phân quyền báo cáo");

                            html += _renderButton(true,
                                "PermitUser",
                                "btn btn-outline-warning mr-1 dropdown-item",
                                "/Sys/User/Permit/" + data,
                                '<i class="fas fa-user-shield text-120"></i> Quyền',
                                "Quyền");

                            html += _renderButton(true,
                                "ChangePassword",
                                "btn btn-outline-primary mr-1 dropdown-item",
                                "/Sys/User/ChangePassword/" + data,
                                '<i class="fa fa-key text-120"></i> Đổi mật khẩu',
                                "Đổi mật khẩu");

                            html += _renderButton(true,
                                "DeActiveUser",
                                "btn btn-outline-default mr-1 dropdown-item",
                                "/Sys/User/DeActive/" + data,
                                '<i class="fas fa-user-lock text-120"></i> Ngưng hoạt động',
                                "Ngưng hoạt động");

                            html += _renderButton(true,
                                "ResetPassword",
                                "btn btn-outline-purple mr-1 dropdown-item",
                                "/Sys/User/ResetPassword/" + data,
                                '<i class="fa fa-paper-plane text-120"></i> Reset mật khẩu',
                                "Reset mật khẩu");  

                            html += _renderButton(true,
                                "EditElncAccount",
                                "btn btn-outline-primary mr-1 dropdown-item",
                                "/Sys/User/EditElnvAccount?userId=" + row.UserId + "&userName="+row.UserName,
                                '<i class="fa fa-id-card text-120"></i> Tài khoản hóa đơn điện tử',
                                "Tài khoản hóa đơn điện tử");
                        }
                        else {

                            html += _renderButton(true,
                                "ActiveUser",
                                "btn btn-outline-primary mr-1 dropdown-item",
                                "/Sys/User/Active/" + data,
                                '<i class="fas fa-check-square text-120"></i> Kích hoạt',
                                "Kích hoạt");
                            
                        }
                        if (row.CanDelete == 0) {

                            html += _renderButton(true,
                                "DeleteUser",
                                "btn btn-outline-danger mr-1 dropdown-item",
                                "/Sys/User/Delete/" + data,
                                '<i class="fa fa-trash text-120"></i> Xóa',
                                "Xóa");
                        }
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

function User_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableUser.ajax.reload(null, false);
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
                        _tableUser.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else if ($(response).hasClass("modal-header")) {
        $("#ModalContent #modal_" + formId + " #modal-content").html(response);
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}