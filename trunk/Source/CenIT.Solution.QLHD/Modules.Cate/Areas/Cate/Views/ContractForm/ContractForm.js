
var _ContractFormActionURLs = {
    ContractForm_GetData: "/Cate/ContractForm/Get"
};
var _tableContractForm;

$(document).ready(function () {
    initTableContractForm()
});

function Search() {
    _tableContractForm.ajax.reload(null, false);
}

function initTableContractForm() {
    _tableContractForm = $("#DSContractForm").DataTable({
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
            "url": _ContractFormActionURLs.ContractForm_GetData,
            "type": "POST",
            "dataType": "JSON",
            "data": {

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
            //{
            //    "data": "Id",
            //    "defaultContent": "",
            //    "render": function (data, type, row, meta) {
            //        return data.toUpperCase();
            //    }
            //},
            {
                "data": "FullName",
                "defaultContent": "",
                "render": function (data, type, row, meta) {
                    if (type == "display") {
                        return '{0}<div class="text-95 text-500 text-primary-d1">{1}<span class="d-block"><span class="text-danger">{2}</span> cập nhật lần cuối lúc <span class="text-danger"> {3}</span></span></div>'.format(data, row.Id, row.LastModifiedBy, moment(row.LastModifiedOn).format("HH:mm:ss DD/MM/YYYY"));
                    }
                    return data;
                }
            },
            //{
            //    "data": "TemplateType", <div><span class="badge badge-danger badge-sm">Cập nhật lần cuối: 2011/04/25</span></div>
            //    "visible": false,
            //    "defaultContent": ""
            //},
            //{
            //    "data": "FileName",
            //    "defaultContent": ""
            //},
            //{
            //    "data": "FullName",
            //    "visible": false,
            //    "defaultContent": ""
            //},
            //{
            //    "data": "Status",
            //    "defaultContent": "",
            //    "render": function (data, type, row, meta) {
            //        if (data === "ACTIVE") {
            //            return '<span class="badge badge-pill badge-success">Kích hoạt</span>';
            //        } else {
            //            return '<span class="badge badge-pill badge-danger">Chưa kích hoạt</span>';
            //        }
            //    }
            //},
            {
                "data": "TemplateName",
                "render": function (data, type, row, meta) {
                    if (type === 'display') {
                        if (data) {
                            var fileNames = data.split(',');
                            var templateLinks = "";
                            var prefix = "";
                            fileNames.forEach(function (fileName) {
                                var templatePath = row["TemplatePath"];
                                if (fileName.startsWith("CN-")) {
                                    prefix = "CN-";
                                    templatePath = row["TemplatePathCosumer"];
                                } else if (fileName.startsWith("DN-")) {
                                    prefix = "DN-";
                                    templatePath = row["TemplatePath"];
                                }

                                templateLinks += '<a href="/ContractForm/Download/' + row.Id + '?fileName=' + fileName +'">' + prefix + row.FullName + '</a><br/>';
                                //templateLinks += '<a href="' + templatePath + '" download>' + fileName + '</a><br/>';
                            });

                            return templateLinks;
                        } else {
                            return '';
                        }
                    } else {
                        return data;
                    }
                }
            }
,
            {
                "data": "Id",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';
                    if (type === "display") {
                        //html += _renderButton(true,
                        //    "DetailsContractForm",
                        //    "btn px-4 btn-lighter-info mr-1 v-hover",
                        //    "/Cate/ContractForm/Details/" + data,
                        //    '<i class="fa fa-eye text-info text-120"></i>',
                        //    "Xem chi tiết", "1024px");
                        html += _renderButton(true,
                            "EditContractForm",
                            "btn px-4 btn-lighter-primary mr-1 v-hover",
                            "/Cate/ContractForm/Edit/" + data,
                            '<i class="far fa-edit text-primary text-120"></i>',
                            "Cập nhật");
                        html += _renderButton(true,
                            "DeleteContractForm",
                            "btn px-4 btn-lighter-danger mr-1 v-hover",
                            "/Cate/ContractForm/Delete/" + data,
                            '<i class="far fa-trash-alt text-danger text-120"></i>',
                            "Xoá");
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

function ContractForm_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableContractForm.ajax.reload(null, false);
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
                        _tableContractForm.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}