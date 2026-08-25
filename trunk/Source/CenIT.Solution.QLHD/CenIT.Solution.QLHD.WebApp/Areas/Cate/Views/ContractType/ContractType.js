
var _ContractTypeActionURLs = {
    ContractType_GetData: "/Cate/ContractType/Get"
};
var _tableContractType;
$(document).ready(function () {
    initTableContractType()
});

function Search() {
    _tableContractType.ajax.reload(null, false);
}

function initTableContractType() {
    _tableContractType = $("#ListContractTypes").DataTable({
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
            "url": _ContractTypeActionURLs.ContractType_GetData,
            "type": "POST",
            "dataType": "JSON",
            //"data": {
            //    "TuKhoa": function () { return $("#Search #TuKhoa").val() },
            //},

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
                "data": "ContractTypeCode",
                "defaultContent": ""
            },
            {
                "data": "ContractTypeName",
                "defaultContent": ""
            },
            {
                "data": "FileName",
                "defaultContent": "",

            },
            {
                "data": "FormattedPercentAdvance",
                "defaultContent": "",

            },
            {
                "data": "ContractSignal",
                "defaultContent": "",

            },
            {
                "data": "ContractTypeId",
                "style": "width:100px;",
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var html = '<span class="d-none d-lg-inline">';
                    if (type === "display") {
                        html += _renderButton(true,
                            "EditContractType",
                            "btn px-4 btn-lighter-primary mr-1 v-hover",
                            "/Cate/ContractType/Edit/" + data,
                            '<i class="fa fa-edit text-primary text-120"></i>',
                            "Cập nhật");

                        html += '<a id="CopyId-{0}" class="btn px-4 btn-lighter-primary mr-1 v-hover" data-rel="tooltip" title="Copy Id" href="javascript:void(0);" onclick="copyToClipboard(\'{0}\'.toUpperCase());"><i class="fas fa-copy text-purple text-120"></i></a>'.format(row.FileId);

                        //html += _renderButton(true,
                        //    "DeleteContractType",
                        //    "btn px-4 btn-lighter-danger mr-1 v-hover",
                        //    "/Cate/ContractType/Delete/" + data,
                        //    '<i class="far fa-trash-alt text-danger text-120"></i>',
                        //    "Xóa");
                    }
                    html += "</span>";

                    return html;
                }
            }
        ],
        //"createdRow": function (row, data, dataIndex) {
        //    $(row).addClass("d-style bgc-h-default-l4");
        //}
    });
}

function copyToClipboard(val) {
    var $txt = $('<textarea />');
    $txt.val(val).css({ width: "1px", height: "1px" }).appendTo('body');
    $txt.select();
    if (document.execCommand('copy')) {
        $txt.remove();
    }
}

function ContractType_OnProcessSuccess(response, formId) {
    if (response.status != undefined) {
        if ($("#ModalContent #modal_" + formId + " #chkNotDismissModal").is(":checked")) {
            if (response.status != undefined) {
                eval(response.message);
                _tableContractType.ajax.reload(null, false);
                response.status = undefined;
                $("#ModalContent #modal_" + formId + " form")[0].reset();
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
                        _tableContractType.ajax.reload(null, false);
                        response.status = undefined;
                    }
                });
        }
    } else {
        $("#ModalContent #modal_" + formId + " #bodyForm").html(response);
    }
}