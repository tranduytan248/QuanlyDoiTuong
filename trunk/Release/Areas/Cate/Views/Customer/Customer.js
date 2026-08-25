var _CustomerActionURLs = {
    Customer_GetData: "/Cate/Customer/Get",
    Customer_Export: "/Cate/Customer/ExportCustomer",
};
var _tableCustomer;
$(document).ready(function () {
    initTableCustomer();
});

function Export() {
    var FullName = $("#Search #FullName").val() || "";
    var UserType = getUserType();

    window.open(_CustomerActionURLs.Customer_Export + "?fullName=" + FullName + "&userType=" + UserType);
}

function initTableCustomer() {
    _tableCustomer = $("#DSCustomer").DataTable({
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
            "url": _CustomerActionURLs.Customer_GetData,
            "type": "POST",
            "dataType": "JSON",
            //"data": {
                //"FullName": function () {
                //    return $("#Search #FullName").val() != null &&
                //        $("#Search #FullName").val().length > 0
                //        ? $("#Search #FullName").val()
                //        : "";
                //},
            //}
            "data": function (d) {
                d.FullName = $("#Search #FullName").val() || "";
                // Gọi hàm để lấy giá trị userType và thêm vào dữ liệu Ajax
                d.UserType = getUserType();
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
                "data": "UnionName",
                "defaultContent": "",
            }, 
            {
                "data": "FullName",
                "defaultContent": "",
            },   
            {
                "data": "CitizenIdentification",
                "defaultContent": "",
            },  
            {
                "data": "Address",
                "defaultContent": "",
                "width":"200px"
            }, 
            {
                "data": "TaxCode",
                "defaultContent": "",
            }, 
            //{
            //    "data": "PositionName",
            //    "defaultContent": "",
            //}, 
            {
                "data": "PhoneNumber",
                "defaultContent": "",
            }, 
            {
                "data": "Email",
                "defaultContent": "",
            }, 
            //{
            //    "data": "Zalo",
            //    "defaultContent": "",
            //    "render": function (data, type, row, meta) {
            //        if (data) {
            //            return `<a href="https://zalo.me/${data}">${data}</a>`
            //        }
            //        return ""
            //    }
            //},         
            {
                "data": "CustomerId",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';
                    if (type === "display") {
                        html += '<div class="dropdown d-inline-block"><button class="btn px-4 btn-lighter-primary mr-1 v-hover dropdown-toggle" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="fa fa-ellipsis-h text-120"></i></button><div class="dropdown-menu dropdown-menu-right">';
                        html += _renderButton(true,
                            "EditCustomer",
                            "btn text-primary mr-1 dropdown-item",
                            "/Cate/Customer/Edit/" + data,
                            '<i class="fa fa-edit text-primary text-120 mr-1"></i> Cập nhật',
                            "Cập nhật", 1024);
                        html += _renderButton(true,
                            "DeleteCustomer",
                            "btn text-danger mr-1 dropdown-item",
                            "/Cate/Customer/Delete/" + data,
                            '<i class="far fa-trash-alt text-danger text-120 mr-1"></i> Xóa',
                            "Xóa");
                    }
                    html += "</span>";

                    return html;
                }
            }
        ]
    });
}
function getUserType() {
    // Lấy giá trị của userType từ các phần tử radio button
    var userType = $("input[name='UserType']:checked").val();
    console.log(userType)
    return userType;
}
function Customer_OnProcessSuccess(response, formId) {
    console.log(response)
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableCustomer.ajax.reload(null, false);
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
                        _tableCustomer.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}

function getAddress(data) {
    var addressContent = ""

    if (data.ApartmentNumber) {
        addressContent += `${data.ApartmentNumber}, `
    }

    if (data.Alley) {
        addressContent += `${data.Alley}, `
    }

    if (data.StreetName) {
        addressContent += `đường ${data.StreetName}, `
    }

    if (data.WardName) {
        addressContent += `${data.WardName}, `
    }

    if (data.DistrictName) {
        addressContent += `${data.DistrictName}, `
    }

    if (data.ProvinceName) {
        addressContent += `${data.ProvinceName}`
    }

    $("#Address").val(addressContent)
}

function updateAddessHref(data) {

    $("#addressButton").attr("href",
        `/Cate/Customer/EditAddress?appN=${data.ApartmentNumber ? data.ApartmentNumber : ""}&alley=${data.Alley ? data.Alley : ""}&pId=${data.ProvinceId}&dId=${data.DistrictId}&wId=${data.WardId}&sId=${data.StreetId}&pName=${data.ProvinceName ? data.ProvinceName : ""}&dName=${data.DistrictName ? data.DistrictName : ""}&wName=${data.WardName ? data.WardName : ""}&sName=${data.StreetName ? data.StreetName : ""}`
    )
}

function CustomerAddress_OnProcessSuccess(response, formId) {
    console.log(response)
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
        } else {
            $("#ModalContent #modal_" + formId).modal("hide");
            $("#ModalContent #modal_" + formId).on("hidden.bs.modal",
                function () {
                    if (response.status != undefined) {
                        const { data } = response
                        console.log(data)
                        $('input#ApartmentNumber').val(data.ApartmentNumber);
                        $('input#Alley').val(data.Alley);
                        $('input#ProvinceId').val(data.ProvinceId);
                        $('input#DistrictId').val(data.DistrictId);
                        $('input#WardId').val(data.WardId);
                        $('input#StreetId').val(data.StreetId);

                        updateAddessHref(data)
                        getAddress(data)
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}

$(document).ready(function () {
    // Sự kiện click cho nút tìm kiếm
    $("#searchButton").click(function () {
        // Reload DataTables với dữ liệu mới
        _tableCustomer.ajax.reload(null, false);
    });
})