if ($.fn.datepicker != undefined) {
    $.fn.datepicker.dates["vi"] = {
        days: ["Chủ nhật", "Thứ hai", "Thứ ba", "Thứ tư", "Thứ năm", "Thứ sáu", "Thứ bảy", "Chủ nhật"],
        daysShort: ["CN", "T2", "T3", "T4", "T5", "T6", "T7", "CN"],
        daysMin: ["CN", "T2", "T3", "T4", "T5", "T6", "T7", "CN"],
        months: [
            "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10",
            "Tháng 11", "Tháng 12"
        ],
        monthsShort: ["Th1", "Th2", "Th3", "Th4", "Th5", "Th6", "Th7", "Th8", "Th9", "Th10", "Th11", "Th12"],
        today: "Hôm nay"
    };
    $.fn.datepicker.defaults.language = "vi";
    $.fn.datepicker.defaults.weekStart = 1;
}

$.fn.selectpicker.Constructor.DEFAULTS.liveSearch = true;
$.fn.selectpicker.Constructor.DEFAULTS.actionsBox = true;
$.fn.selectpicker.Constructor.DEFAULTS.selectedTextFormat = "count > 5";

$.extend(true,
    $.fn.dataTable.defaults,
    {
        dom:
            "<'row'<'col-12 col-sm-6'l><'col-12 col-sm-6 text-right table-tools-col'f>>" +
            "<'row'<'col-12'tr>>" +
            "<'row'<'col-12 col-md-5'i><'col-12 col-md-7'p>>",
        renderer: "bootstrap",
        language: {
            "lengthMenu": "<span>Hiển thị</span> <b>_MENU_</b> <span >dòng</span>",
            "zeroRecords": "<span>Không tìm thấy</span>",
            "emptyTable": "<span>Không có dữ liệu</span>",
            //"info": "<span>Đang hiển thị trang</span> _PAGE_ <span >trên</span> _PAGES_",
            "info":
                "<span>Hiển thị từ </span> <b>_START_</b> <span> đến </span> <b>_END_</b> <span >trên tổng số </span> <b>_TOTAL_</b> dữ liệu",
            "infoEmpty": "<span>Không có dữ liệu</span>",
            "infoFiltered": "(<span >Lọc từ</span> <b>_MAX_</b> <span >tổng dòng</span>)",
            "search": "<span>Tìm kiếm:</span>",
            "paginate": {
                "first": "<span>Trang đầu</span>",
                "last": "<span>Trang cuối</span>",
                "next": "<span>Tiếp</span>",
                "previous": "<span>Trước</span>"
            },
            "aria": {
                "sortAscending": ": <span >Sắp xếp tăng dần</span>",
                "sortDescending": ": <span >Sắp xếp giảm dần</span>"
            }
        },
        classes: {
            sLength: "dataTables_length text-left w-auto",
        },
        buttons: {
            dom: {
                button: {
                    className: "btn" //remove the default 'btn-secondary'
                },
                container: {
                    className: "dt-buttons btn-group bgc-white-tp2 text-left w-auto"
                }
            },
            buttons: [
                {
                    "extend": "colvis",
                    "text": "<i class='far fa-eye text-110'></i> <span class='d-none'>Show/hide columns</span>",
                    "className": "btn btn-outline-info",
                    columns: ":not(:first)"
                }
            ]
        },
        createdRow: function (row) {
            $(row).addClass("d-style bgc-h-default-l4");
        }
    });

$.extend($.validator.messages,
    {
        required: "Trường này bắt buộc nhập.",
        remote: "Hãy sửa cho đúng.",
        email: "Hãy nhập email.",
        url: "Hãy nhập URL.",
        date: "Hãy nhập ngày.",
        dateISO: "Hãy nhập ngày (ISO).",
        number: "Hãy nhập số.",
        digits: "Hãy nhập chữ số.",
        creditcard: "Hãy nhập số thẻ tín dụng.",
        equalTo: "Hãy nhập thêm lần nữa.",
        extension: "Phần mở rộng không đúng.",
        maxlength: $.validator.format("Hãy nhập từ {0} kí tự trở xuống."),
        minlength: $.validator.format("Hãy nhập từ {0} kí tự trở lên."),
        rangelength: $.validator.format("Hãy nhập từ {0} đến {1} kí tự."),
        range: $.validator.format("Hãy nhập từ {0} đến {1}."),
        max: $.validator.format("Hãy nhập từ {0} trở xuống."),
        min: $.validator.format("Hãy nhập từ {0} trở lên.")
    });

$.validator.addMethod("date",
    function (value, element) {
        var bits = value.match(/([0-9]+)/gi), str;
        if (bits === null) return true;
        if (bits.length === 2) {
            str = bits[0] + "/" + "01" + "/" + bits[1];
            return this.optional(element) || !/Invalid|NaN/.test(new Date(str));
        } else if (bits.length === 3) {
            if (!bits) {
                return this.optional(element) || false;
            }
            str = bits[1] + "/" + bits[0] + "/" + bits[2];
            return this.optional(element) || !/Invalid|NaN/.test(new Date(str));
        }
        return this.optional(element) || !/Invalid|NaN/.test(new Date(str));
    },
    "Nhập ngày theo định dạng [dd/mm/yyyy]");

$.fn.select2.defaults.set("amdBase", "select2/");
$.fn.select2.defaults.set("amdLanguageBase", "select2/i18n/");
$.fn.select2.defaults.set("width", "resolve");
$.fn.select2.defaults.set("language", "vi");

if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g,
            function (match, number) {
                return typeof args[number] != "undefined"
                    ? args[number]
                    : match;
            });
    };
}

moment.locale("vi");

$(document)
    .bind("ajaxStart", function (event, jqxhr, settings, thrownError) {
        _onWaiting();
    })
    .bind("ajaxSend", function (event, jqxhr, settings, thrownError) {
    })
    .bind("ajaxComplete", function (event, jqxhr, settings, thrownError) {
        //if (settings.dataType == "html") {
        //    _initElements(jqxhr.responseText);
        //}
        _endWaiting();
    })
    .bind("ajaxError", function (event, jqxhr, settings, thrownError) {
        if (jqxhr.status === 401) {
            window.location.href = window.location.origin + "/Account/Login";
        }
        else if (jqxhr.status === 500) {
            window.location.href = window.location.origin + "/Error/Error";
        }
        //window.location = "/Error/Error";
    });

var htmlModalTemplate =
    '<div class="modal fade {0}" id="{1}" parent-id="{2}" data-keyboard="false" data-backdrop-bg="bgc-grey-tp4" data-backdrop="static" tabindex="-1" role="dialog" aria-labelledby="modalTitle" aria-hidden="true"><div class="modal-dialog" {3} role="document"><div id="modal-content" class="modal-content border-0 shadow radius-1"></div></div></div>';

$(function () {

    var htmlLoader =
        '<div id="loader" class="pageload-overlay" data-opening="M 0,0 80,-10 80,60 0,70 0,0" data-closing="M 0,-10 80,-20 80,-10 0,0 0,-10"><svg xmlns="http://www.w3.org/2000/svg" width="100%" height="100%" viewBox="0 0 80 60" preserveAspectRatio="none"><path d="M 0,70 80,60 80,80 0,80 0,70"/></svg></div>';
    $("body").append(htmlLoader);

    $("body").append('<div id="ModalContent"></div>');

    $('<audio id="soundSuccess"><source src="/Contents/Base/sound/notify/success.mp3" type="audio/mpeg"></audio>').appendTo('body');
    $('<audio id="soundError"><source src="/Contents/Base/sound/notify/error.mp3" type="audio/mpeg"></audio>').appendTo('body');
    $('<audio id="soundNotify"><source src="/Contents/Base/sound/notify/notify.wav" type="audio/wav"></audio>').appendTo('body');
    $('<audio id="soundModal"><source src="/Contents/Base/sound/notify/modal.wav" type="audio/wav"></audio>').appendTo('body');
    /* 
        Parrams
        - 0: Modal width
        - 1: Modal id
        - 2: Parent id
        modal size 
        - modal-fs: fullscreen
        - modal-lg: large
        - modal-xl: x-large
    */

    $(document, "table", "form").on("click",
        "[data-modal]",
        function () {
            _onWaiting();
            var noneTemplate = $(this).attr("data-none-template") || $(this).attr("data_none_template");
            var usingScrollbarsInside = $(this).attr("data-scrollbars-inside") || $(this).attr("data_scrollbars_inside");

            if (noneTemplate == "1") {
                var idModal = $(this).attr("data-modal-id");
                $("<div></div>").load(this.href, function (data, textStatus, xhr) {
                    $("#ModalContent").append(data);
                    if (typeof usingScrollbarsInside != "undefined") {
                        $("#" + idModal + " .modal-dialog").addClass('modal-dialog-scrollable');
                        $("#" + idModal + " .modal-dialog .modal-body").addClass('ace-scrollbar');
                    }
                    $("#" + idModal).modal("show");
                });
                return false;
            }
            var idModal = "modal_" + $(this).attr("data-modal-id");
            var idParentModal = "modal_" + ($(this).attr("data-modal-id-parent") || $(this).attr("data_modal_id_parent"));
            var closeParentModal = $(this).attr("data-close-modal-parent") || $(this).attr("data_close_modal_parent");
            if (closeParentModal == null || closeParentModal == undefined) {
                closeParentModal = false;
            }

            if ($("#" + idModal).length > 0) {
                $("#" + idModal).remove();
            }

            var asidePlacement = $(this).attr("data-aside-placement");
            var asideDismiss = $(this).attr("data-aside-dismiss");

            var modalWidthClass = "modal-lg";
            var styleWithModal = '';
            var width = $(this).attr("data-width");
            if (typeof width == "undefined") width = $(this).attr("data_width");
            if (width != undefined) {
                width = width.replace("px", "");
                if (width == "fullscreen") {
                    modalWidthClass = "modal-fs";
                } else if (parseInt(width) == 1024) {
                    modalWidthClass = "modal-xl";
                } else if (parseInt(width) > 1024) {
                    modalWidthClass = "modal-xl";
                    styleWithModal = 'style="max-width: calc(100vw - 5%);"';
                } else if (parseInt(width) < 1024 && parseInt(width) >= 800) {
                    modalWidthClass = "modal-lg";
                }
            }

            var htmlModal = htmlModalTemplate.format(modalWidthClass, idModal, idParentModal, styleWithModal);

            $("#ModalContent").append(htmlModal);

            $("#" + idModal + " #modal-content").load(this.href,
                function (data, textStatus, xhr) {
                    if (xhr.status === 401) {
                        window.location.href = window.location.origin + "/Account/Login";
                    } else if (xhr.status === 404) {
                        window.location.href = window.location.origin + "/Error/NotFound";
                    } else if (xhr.status === 500) {
                        window.location.href = window.location.origin + "/Error/Error";
                    } else if (xhr.status === 405) {
                        window.location.href = window.location.origin + "/Error/AccessDenied";
                    } else {
                        if (_isJson(data)) {
                            var response = JSON.parse(data);
                            if (response.status != undefined) {
                                eval(response.message);
                                $(this).remove();
                            }
                            _endWaiting();
                        } else {
                            $("#" + idModal).modal("show");

                            if (typeof asidePlacement != "undefined") {
                                $("#" + idModal).aceAside({
                                    placement: asidePlacement,
                                    dismiss: asideDismiss,
                                    belowNav: true,
                                    extrwNav: true,
                                    extraClass: 'my-0'
                                });
                                $("#" + idModal + " .modal-dialog").css("width", "680px");
                                //.addClass('w-auto');
                            }
                        }
                        if (typeof usingScrollbarsInside != "undefined") {
                            $("#" + idModal + " .modal-dialog").addClass('modal-dialog-scrollable');
                            $("#" + idModal + " .modal-dialog .modal-body").addClass('ace-scrollbar');
                        }

                        if (closeParentModal) {
                            $("#" + idModal).on('hidden.bs.modal', function () {
                                $("#" + idParentModal).modal("hide");
                            });
                        }

                    }
                });
            return false;
        });

    $(document, "table", "form").on("click",
        "[data-in-page]",
        function () {
            var formId = $(this).attr("data-form-id");
            if (formId != undefined) {
                _onWaiting();
                var usingScrollbarsInside = $(this).attr("data-scrollbars-inside") || $(this).attr("data_scrollbars_inside");

                $("#" + formId + " #bodyForm").load(this.href,
                    function (data, textStatus, xhr) {
                        if (xhr.status === 401) {
                            window.location.href = window.location.origin + "/Account/Login";
                        } else if (xhr.status === 404) {
                            window.location.href = window.location.origin + "/Error/NotFound";
                        } else if (xhr.status === 500) {
                            window.location.href = window.location.origin + "/Error/Error";
                        } else if (xhr.status === 405) {
                            window.location.href = window.location.origin + "/Error/AccessDenied";
                        } else {
                            if (_isJson(data)) {
                                var response = JSON.parse(data);
                                if (response.status != undefined) {
                                    eval(response.message);
                                    $(this).remove();
                                }
                            } else {
                                $("#modal_" + idModal).modal("show");
                            }
                            if (typeof usingScrollbarsInside != "undefined") {
                                $("#" + idModal + " .modal-dialog").addClass('modal-dialog-scrollable');
                                $("#" + idModal + " .modal-dialog .modal-body").addClass('ace-scrollbar');
                            }
                            _endWaiting();
                        }
                    });
            }
            return false;
        });

    $(document).on({
        'show.bs.modal': function () {
            var zIndex = 1040 + (10 * $(".modal:visible").length);
            $(this).css("z-index", zIndex);
            setTimeout(function () {
                $(".modal-backdrop").not(".modal-stack").css("z-index", zIndex - 1).addClass("modal-stack");
            }, 0);
            //$('#soundModal')[0].play();
        },
        'shown.bs.modal': function () {
            _initElements(this);

            $(document).off('focusin.modal');
            if ($(this).find('#modal-content .modal-header').attr('rule') != "Confirm") {
                $('#soundModal')[0].play();
            }

            //_endWaiting();
        },
        'hidden.bs.modal': function () {
            if ($(".modal:visible").length > 0) {
                setTimeout(function () {
                    $(document.body).addClass("modal-open");
                },
                    0);
            }
            if ($(this).parent().is("#ModalContent")) {
                $(this).remove();
            }
        }
    }, ".modal");
});

$(document).ready(function () {
    _initMenuView();
    _initElements(this);
    _initValidateForm(this);
});

window.onerror = function (message, source, lineno, colno, error) {
    toastr.error(message);
    $('#soundError')[0].play();
};

function showNotify(title, icon, message, url, target, type, placement) {
    var clssName = type;

    if (typeof placement === "undefined") placement = "tc";

    $.aceToaster.add({
        placement: placement,
        width: "40rem",
        title: title,
        body: message,
        icon: '<i class="text-' +
            clssName +
            ' mr-2 text-130"><i class="fas ' +
            icon +
            " mt-25 fa-2x text-" +
            clssName +
            '"></i></i>',
        iconClass: "mt-3",
        delay: 5000,
        animation: true,
        //showHideTransition: 'slide',
        showHideTransition: "plain",
        closeClass: "btn btn-light-danger border-0 btn-bgc-tp btn-xs px-2 py-0 text-150 position-tr mt-n25",
        className: "bgc-" + clssName + "-l4 border-none border-t-4 brc-" + clssName + "-tp1 rounded-sm pl-3 pr-1",
        headerClass: "bg-transparent border-0 text-120 text-" + clssName + "-d3 font-bolder mt-3",
        bodyClass: "pt-0 pb-3 text-105 text-" + clssName,
        progress: "position-bl bgc-" + clssName + "-tp4 py-2px m-1px",
        progressReverse: true
    });

    if (type === "success" || type === "info") {
        $('#soundSuccess')[0].play();
    } else if (type === "danger" || type === "warning") {
        $('#soundError')[0].play();
    }
}

function _isJson(str) {
    try {
        return (JSON.parse(str) && !!str);
    } catch (e) {
        return false;
    }
}

$(document).on("init.dt",
    function (e, settings) {
        _initColTable(settings);
    });

$(document).on('draw.dt', function (e, settings, data) {
    $('[data-rel="tooltip"], [data_rel="tooltip"]').tooltip({ trigger: 'hover' });
    $("#" + settings.sTableId + ' tbody td').addClass("align-middle");
});

$(document).on('')

function _initColTable(settings) {
    if (typeof (settings) == "undefined")
        return;
    var dataTable = $("#" + settings.sTableId);
    $(".table-tools-col > .dataTables_filter").find("input").removeClass("form-control-sm");
    $(".table-tools-col > .dataTables_filter").find("div.dt-buttons").remove();
    $(".table-tools-col > .dataTables_filter")
        .prepend($(dataTable).DataTable().buttons().container().css("margin-right", "5px"));
    $("#" + settings.sTableId + ' tbody td').addClass("align-middle");
}

function _initElements(htmlContents) {
    _initDatePicker(htmlContents);
    _initDateTimePicker(htmlContents);
    _initButtonSubmit(htmlContents);
    _initSelectElement(htmlContents);
    _initTooltip(htmlContents)
}

function _initDatePicker(htmlContents) {
    if (htmlContents == null || htmlContents == undefined || htmlContents.length <= 0) return;

    var eleDatepickers = $(htmlContents).find("input.datepicker")
    if (eleDatepickers == null || eleDatepickers == undefined) {
        eleDatepickers = $(".datepicker");
    }

    if (eleDatepickers.length <= 0) return;

    $.each(eleDatepickers, function (idx, ele) {
        if (!$(ele).hasClass("datepicker-dropdown")) {
            $(ele).datepicker("destroy");
            $(ele).datepicker({
                autoclose: true,
                format: "dd/mm/yyyy",
                todayhighlight: true,
                orientation: "auto",
                todaybtn: true,
                minDate: null,
                maxDate: null,
                weekStart: 1,
                language: "vi",
                calendarWeeks: true
            });
        }
        $(ele).next('.input-group-addon').on("click",
            function () {
                var datePickerElement = $(this).next();
                if ($(datePickerElement).hasClass("datepicker")) {
                    $(datePickerElement).datepicker("show");
                } else {
                    $(this).next().trigger("click");
                }
            });
    });
}

function _initDateTimePicker(htmlContents) {
    if (htmlContents == null || htmlContents == undefined || htmlContents.length <= 0) return;
    var eleDateTimePickers = $(htmlContents).find('input.datetimepicker');

    if (eleDateTimePickers == null || eleDateTimePickers == undefined) {
        eleDateTimePickers = $(".datetimepicker");
    }
    if (eleDateTimePickers.length <= 0) return;

    $.each(eleDateTimePickers, function (idx, ele) {
        $(ele).datetimepicker({
            locale: 'vi',
            format: 'DD/MM/YYYY HH:mm',
            dayViewHeaderFormat: 'MMMM YYYY',
            calendarWeeks: true,
            showTodayButton: true,
            icons: {
                time: 'fas fa-clock',
                date: 'fas fa-calendar-alt',
                up: 'fas fa-chevron-up',
                down: 'fas fa-chevron-down',
                previous: 'fas fa-chevron-left',
                next: 'fas fa-chevron-right',
                today: 'fas fa-calendar-day',
                clear: 'fas fa-trash',
                close: 'far fa-window-close'
            }
        });
    });
}

function _initButtonSubmit(htmlContents) {
    var btnSubmits = $(htmlContents).find("button[type='submit']");
    if (btnSubmits == null || btnSubmits == undefined) {
        btnSubmits = $("button[type='submit']");
    }
    if (btnSubmits.length <= 0) return;

    //$.each($("button[type='submit']"),
    $.each(btnSubmits, function (idx, btnSubmit) {
        if ($(btnSubmit).parents("form").length == 0) {
            $(btnSubmit).unbind("click");
            //$(btnSubmit).removeAttr('disabled');

            $(btnSubmit).on("click", function () {
                if ($(this).parents("div.modal").find("form").length > 0) {
                    $(this).parents("div.modal").find("form").submit();
                    //$(btnSubmit).prop('disabled', true);
                }
            });
        }
    });
}

function _initSelectElement(htmlContents) {
    if (htmlContents == null || htmlContents == undefined || htmlContents.length <= 0) return;
    var selectEles = $(htmlContents).find('select');

    if (selectEles == null || selectEles == undefined) {
        selectEles = $("select");
    }
    if (selectEles.length <= 0) return;

    $.each(selectEles,
        function (idx, ele) {
            var isComboboxLength = !ele.name.includes("_length");
            if (!$(ele).hasClass("none-select2")) {
                //if ($(ele).data("select2")) {
                //    $(ele).select2("destroy");
                //}
                $(ele).select2({
                    allowClear: isComboboxLength,
                    placeholder: "Chọn 1 giá trị",
                    width: "element",
                    dropdownParent: $(this).parent(),
                    dropdownAutoWidth: true,
                    language: "vi"
                });
            }
        });
}

function _initTooltip(htmlContents) {
    if (htmlContents == null || htmlContents == undefined || htmlContents.length <= 0) return;
    var toolTipEles = $(htmlContents).find('[data-rel="tooltip"], [data_rel="tooltip"]');

    if (toolTipEles.length <= 0) return;
    $.each(toolTipEles, function (idx, ele) {
        $(ele).tooltip({ trigger: 'hover' });
    });
}

function _initSelect(cbbEle) {
    var ele = $(cbbEle)[0];
    var isComboboxLength = !ele.name.includes("_length");
    if ($(ele).data("select2")) {
        $(ele).select2("destroy");
    }
    $(ele).select2({
        allowClear: isComboboxLength,
        placeholder: "Chọn 1 giá trị",
        width: "element",
        dropdownParent: $(cbbEle).parent(),
        dropdownAutoWidth: true,
        language: "vi"
    });
}

function _initSelectFromHtml(htmlContents) {
    if (htmlContents == null || htmlContents == undefined || htmlContents.length <= 0) return;

    //var eleSelects = $(htmlContent).find('select');
    $.each($(htmlContent).find('select'), function (idx, ele) {
        var isComboboxLength = !ele.name.includes("_length");
        if (!$(ele).hasClass("none-select2")) {
            if ($(ele).data("select2")) {
                $(ele).select2("destroy");
            }
            $(ele).select2({
                allowClear: isComboboxLength,
                placeholder: "Chọn 1 giá trị",
                width: "element",
                dropdownParent: $(this).parent(),
                dropdownAutoWidth: true,
                language: "vi"
            });
        }
    });
}

function _initMenuView() {
    var url = window.location.href;

    $("#sidebar ul.nav li.nav-item a").each(function () {
        if (this.href.toString().replace("#", "").toLowerCase() === (url.toLowerCase().toString())) {
            var name = $(this).attr("name");
            if (name != undefined) {
                var ids = name.split(",");
                if (ids.length > 1) {
                    var i;
                    for (i = 0; i < ids.length; i++) {
                        $("#" + ids[i]).addClass("active");
                        if ($(this).parent('[id="' + ids[i] + '"]').length <= 0) {
                            $("#" + ids[i]).addClass("open");
                            $("#" + ids[i] + ' > a').removeClass('collapsed');
                            $("#" + ids[i] + '.active.open > .collapse').addClass('show');
                        }
                    }
                } else {
                    var i;
                    for (i = 0; i < ids.length; i++) {
                        $("#" + ids[i]).addClass("active");
                    }
                }
            }
        }
    });
}

function _initValidateForm(htmlContents) {

    if (htmlContents == null || htmlContents == undefined || htmlContents.length <= 0) return;
    var formEles = $(htmlContents).find('form');

    if (formEles.length <= 0) return;
    $.each(formEles, function (idx, ele) {
        $(ele).validate({
            errorClass: "help-block animation-slideDown",
            errorElement: "div",
            errorPlacement: function (error, e) {
                e.parents(".form-group > div").append(error);
            },
            highlight: function (e) {
                $(e).closest(".form-group").removeClass("has-success has-error").addClass("has-error");
                $(e).closest(".help-block").remove();
            },
            success: function (e) {
                e.closest(".form-group").removeClass("has-success has-error");
                e.closest(".help-block").remove();
            }
        });
    });

    var dataValidate = window.mvcClientValidationMetadata == undefined
        ? undefined
        : window.mvcClientValidationMetadata[0];
    if (typeof dataValidate != "undefined") {
        var dataFields = dataValidate.Fields;
        dataFields.forEach(function (item, idx) {
            var validateRules = {};
            var validateMessages = {};

            item.ValidationRules.forEach(function (itemRule) {
                switch (itemRule.ValidationType) {
                    case "required":
                        {
                            validateRules[itemRule.ValidationType] = true;
                            validateMessages[itemRule.ValidationType] = itemRule.ErrorMessage;
                        }
                        break;
                    case "url":
                        {
                            validateRules[itemRule.ValidationType] = true;
                            validateMessages[itemRule.ValidationType] = itemRule.ErrorMessage;
                        }
                        break;
                    case "date":
                        {
                            validateRules[itemRule.ValidationType] = true;
                            validateMessages[itemRule.ValidationType] = itemRule.ErrorMessage;
                        }
                        break;
                    case "equalto":
                        {
                            validateRules["equalTo"] = itemRule.ValidationParameters.other.replace("*.", "#");
                            validateMessages["equalTo"] = itemRule.ErrorMessage;
                        }
                        break;
                    case "length":
                        {
                            var paramsLength = itemRule.ValidationParameters;
                            if (typeof paramsLength.min !== "undefined") {
                                validateRules["minlength"] = paramsLength.min;
                                validateMessages["minlength"] = itemRule.ErrorMessage;
                            }
                            if (typeof paramsLength.max !== "undefined") {
                                validateRules["maxlength"] = paramsLength.max;
                                validateMessages["maxlength"] = itemRule.ErrorMessage;
                            }
                        }
                        break;
                    case "range":
                        {
                            var paramsRange = itemRule.ValidationParameters;
                            validateRules[itemRule.ValidationType] = [paramsRange.min, paramsRange.max];
                            validateMessages[itemRule.ValidationType] = itemRule.ErrorMessage;
                        }
                        break;
                    default:
                        {
                            validateRules[itemRule.ValidationType] = true;
                            validateMessages[itemRule.ValidationType] = itemRule.ErrorMessage;
                        }
                        break;
                }
            });

            $("form").validate().settings.rules[item.FieldName] = validateRules;
            $("form").validate().settings.messages[item.FieldName] = validateMessages;
        });
    }
}

function _initValidForm(formId, arrIgnore) {
    var $invalidClass = 'brc-danger-tp2'
    var $validClass = 'brc-info-tp2'
    var $errorClass = 'form-text form-error text-danger-m2'

    var $formValid = $(formId).validate({
        errorElement: 'span',
        errorClass: $errorClass,
        focusInvalid: false,
        ignore: arrIgnore != undefined ? arrIgnore.join(',') : "",
        rules: {
            //    email: {
            //        required: true,
            //        email: true
            //    },
            //    password: {
            //        required: true,
            //        minlength: 5
            //    },
            //    password2: {
            //        required: true,
            //        minlength: 5,
            //        equalTo: "#password"
            //    },
            //    name: {
            //        required: true
            //    },
            //    phone: {
            //        required: true,
            //        phone: 'required'
            //    },
            //    url: {
            //        required: true,
            //        url: true
            //    },
            //    comment: {
            //        //required: true
            //    },
            //    state: {
            //        //required: true
            //    },
            //    platform: {
            //        required: true
            //    },
            //    subscription: {
            //        required: true
            //    },
            //    gender: {
            //        required: true,
            //    },
            //    agree: {
            //        required: true,
            //    }
        },

        errorPlacement: function (error, element) {
            // prepend 'fa-exclamation-circle' icon
            error.prepend('<i class="form-text fa fa-exclamation-circle text-danger-m1 text-100 mr-1 ml-2"></i>')

            if (element.is('input[type=checkbox]') || element.is('input[type=radio]')) {
                element.closest('div[class*="col-"]').append(error)
            }
            //else if (element.is('.select2')) {
            else if (element.data('select2')) {
                var container = element.siblings('[class*="select2-container"]')
                error.insertAfter(container)
                container.find('.select2-selection').addClass($invalidClass)
            }
            //else if (element.is('.chosen')) {
            else if (element.data('chosen')) {
                var container = element.siblings('[class*="chosen-container"]')
                error.insertAfter(container)
                container.find('.chosen-choices, .chosen-single').addClass($invalidClass)
            }
            else {
                error.addClass('d-inline-block').insertAfter(element)
            }

            element.focus(function () {
                element.removeClass($errorClass);
            });
        },

        submitHandler: function (form) {
        }
    });

    return $formValid;
}

function _renderButton(isModal, modalId, eleClass, urlAction, icon, title, modalWidth, attrs) {
    var attrWidth = modalWidth != null && modalWidth !== undefined ? 'data-width="{0}"'.format(modalWidth) : "";

    var tmpUrlAction = urlAction;
    var idxParram = tmpUrlAction.indexOf("?");
    if (idxParram > -1) {
        tmpUrlAction = tmpUrlAction.substring(0, idxParram);
    }

    var pActions = tmpUrlAction != null ? tmpUrlAction.split("/") : [];
    pActions = pActions.filter(function (v) { return v !== "" });
    var id = pActions.join("");

    var dataParrams = {
        controllerName: pActions.length > 2 ? pActions[1] : pActions[0],
        actionName: pActions.length > 2 ? pActions[2] : pActions[1],
        areaName: pActions.length > 2 ? pActions[0] : null
    };

    var dataPermissions = window['Permissions'];

    if (dataPermissions != null && dataPermissions.length > 0) {
        var permitHashValue = MD5("{0}_{1}_{2}".format(dataParrams.areaName, dataParrams.controllerName, dataParrams.actionName))
        if (jQuery.inArray(permitHashValue.toUpperCase(), dataPermissions) === -1) {
            $("a#" + id).remove();
        }
    } else {
        $.ajax({
            type: "GET",
            async: true,
            url: "/App/ActionIsAllow",
            dataType: "JSON",
            data: dataParrams,
            success: function (response) {
                if (!response.status) {
                    $("a#" + id).remove();
                }
            }
        });
    }

    var template =
        '<a {7} id={6} {8} {0} data-modal-id="{1}" class="{2}" data-rel="tooltip" title="{5}" href="{3}">{4}</a>';
    var sModal = isModal ? 'data-modal=""' : "";
    return template.format(sModal, modalId, eleClass, urlAction, icon, title, id, attrWidth, attrs);
}

function _showPassword(inpt) {
    if ("password" == $(inpt).attr("type")) {
        $(inpt).prop("type", "text");
    } else {
        $(inpt).prop("type", "password");
    }
}

function _onWaiting() {
    $("#loader").addClass("show pageload-loading").css("display", "inherit");
}

function _endWaiting() {
    $("#loader").removeClass("show pageload-loading").css("display", "none");
}

function _onChangeCombo(cbb, eleName, isOptgroup) {
    if (isOptgroup) {
        $(eleName).val($(cbb).children("optgroup").children("option:selected").text());
    } else {
        $(eleName).val($(cbb).children("option:selected").text());
    }
}

function _onDownloadDoc(btnDownloadDoc) {
    if (typeof btnDownloadDoc != "undefined") {
        var url = $(btnDownloadDoc).data("href");
        $.ajax({
            type: "GET",
            url: url,
            success: function (response) {
                if (typeof response != "undefined" && response != null && response.status) {
                    window.location = response.downloadPath;
                } else if (typeof response != "undefined" && response != null && !response.status) {
                    eval(response.message);
                    return false;
                }
            }
        });
    }
}

function _showModal(modalHtml, modalId, modalWidth, parentModalId) {
    _onWaiting();
    if ($("#" + modalId).length > 0) {
        $("#" + modalId).remove();
    }
    var modalWidthClass = "modal-lg";
    if (typeof modalWidth == "undefined") modalWidth = "860";
    if (modalWidth != undefined) {
        modalWidth = modalWidth.replace("px", "");
        if (modalWidth == "fullscreen") {
            modalWidthClass = "modal-fs";
        } else if (parseInt(modalWidth) >= 1024) {
            modalWidthClass = "modal-xl";
        } else if (parseInt(modalWidth) < 1024 && parseInt(width) >= 800) {
            modalWidthClass = "modal-lg";
        }
    }

    var htmlModal = htmlModalTemplate.format(modalWidthClass, modalId, parentModalId);
    $("#ModalContent").append(htmlModal);
    $("#" + modalId + " #modal-content").html(modalHtml);
    $("#" + modalId).modal("show");
    _endWaiting();
}

function XuLy_VietHoa_VietThuong(NameInput) {
    // Lấy giá trị của input
    var Input_Value = $("[name='" + NameInput + "']:not(span)");
    var currentVal = Input_Value.val();

    // Tạo input cho viết hoa
    var inputVietHoa = document.createElement("input");
    inputVietHoa.type = "hidden";
    inputVietHoa.name = NameInput + "_Hoa";
    inputVietHoa.value = currentVal.charAt(0).toUpperCase() + currentVal.slice(1);

    // Tạo input cho viết thường
    var inputVietThuong = document.createElement("input");
    inputVietThuong.type = "hidden";
    inputVietThuong.name = NameInput + "_Thuong";
    inputVietThuong.value = currentVal.charAt(0).toLowerCase() + currentVal.slice(1);

    // Lấy thẻ form
    var formElement = document.getElementById("VietHoa_VietThuong");

    // Thêm inputVietHoa và inputVietThuong vào form
    formElement.appendChild(inputVietHoa);
    formElement.appendChild(inputVietThuong);
}

const VND = new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND',
});

const FormatCurrency = new Intl.NumberFormat('en', {
    maximumFractionDigits: 0
});

$.fn.inputFilter = function (callback, errMsg) {
    return this.on("input keydown keyup mousedown mouseup select contextmenu drop focusout", function (e) {
        if (callback(this.value)) {
            // Accepted value
            if (["keydown", "mousedown", "focusout"].indexOf(e.type) >= 0) {
                $(this).removeClass("input-error");
                this.setCustomValidity("");
            }
            this.oldValue = this.value;
            this.oldSelectionStart = this.selectionStart;
            this.oldSelectionEnd = this.selectionEnd;
        } else if (this.hasOwnProperty("oldValue")) {
            // Rejected value - restore the previous one
            $(this).addClass("input-error");
            this.setCustomValidity(errMsg);
            this.reportValidity();
            this.value = this.oldValue;
            this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
        } else {
            // Rejected value - nothing to restore
            this.value = "";
        }
    });
};

//====================================================================
// MD5 Function
//====================================================================

var MD5 = function (d) {
    var r = M(V(Y(X(d), 8 * d.length)));
    return r.toLowerCase()
};

function M(d) {
    for (var _, m = "0123456789ABCDEF", f = "", r = 0; r < d.length; r++) _ = d.charCodeAt(r), f += m.charAt(_ >>> 4 & 15) + m.charAt(15 & _);
    return f
}

function X(d) {
    for (var _ = Array(d.length >> 2), m = 0; m < _.length; m++) _[m] = 0;
    for (m = 0; m < 8 * d.length; m += 8) _[m >> 5] |= (255 & d.charCodeAt(m / 8)) << m % 32;
    return _
}

function V(d) {
    for (var _ = "", m = 0; m < 32 * d.length; m += 8) _ += String.fromCharCode(d[m >> 5] >>> m % 32 & 255);
    return _
}

function Y(d, _) {
    d[_ >> 5] |= 128 << _ % 32, d[14 + (_ + 64 >>> 9 << 4)] = _;
    for (var m = 1732584193, f = -271733879, r = -1732584194, i = 271733878, n = 0; n < d.length; n += 16) {
        var h = m,
            t = f,
            g = r,
            e = i;
        f = md5_ii(f = md5_ii(f = md5_ii(f = md5_ii(f = md5_hh(f = md5_hh(f = md5_hh(f = md5_hh(f = md5_gg(f = md5_gg(f = md5_gg(f = md5_gg(f = md5_ff(f = md5_ff(f = md5_ff(f = md5_ff(f, r = md5_ff(r, i = md5_ff(i, m = md5_ff(m, f, r, i, d[n + 0], 7, -680876936), f, r, d[n + 1], 12, -389564586), m, f, d[n + 2], 17, 606105819), i, m, d[n + 3], 22, -1044525330), r = md5_ff(r, i = md5_ff(i, m = md5_ff(m, f, r, i, d[n + 4], 7, -176418897), f, r, d[n + 5], 12, 1200080426), m, f, d[n + 6], 17, -1473231341), i, m, d[n + 7], 22, -45705983), r = md5_ff(r, i = md5_ff(i, m = md5_ff(m, f, r, i, d[n + 8], 7, 1770035416), f, r, d[n + 9], 12, -1958414417), m, f, d[n + 10], 17, -42063), i, m, d[n + 11], 22, -1990404162), r = md5_ff(r, i = md5_ff(i, m = md5_ff(m, f, r, i, d[n + 12], 7, 1804603682), f, r, d[n + 13], 12, -40341101), m, f, d[n + 14], 17, -1502002290), i, m, d[n + 15], 22, 1236535329), r = md5_gg(r, i = md5_gg(i, m = md5_gg(m, f, r, i, d[n + 1], 5, -165796510), f, r, d[n + 6], 9, -1069501632), m, f, d[n + 11], 14, 643717713), i, m, d[n + 0], 20, -373897302), r = md5_gg(r, i = md5_gg(i, m = md5_gg(m, f, r, i, d[n + 5], 5, -701558691), f, r, d[n + 10], 9, 38016083), m, f, d[n + 15], 14, -660478335), i, m, d[n + 4], 20, -405537848), r = md5_gg(r, i = md5_gg(i, m = md5_gg(m, f, r, i, d[n + 9], 5, 568446438), f, r, d[n + 14], 9, -1019803690), m, f, d[n + 3], 14, -187363961), i, m, d[n + 8], 20, 1163531501), r = md5_gg(r, i = md5_gg(i, m = md5_gg(m, f, r, i, d[n + 13], 5, -1444681467), f, r, d[n + 2], 9, -51403784), m, f, d[n + 7], 14, 1735328473), i, m, d[n + 12], 20, -1926607734), r = md5_hh(r, i = md5_hh(i, m = md5_hh(m, f, r, i, d[n + 5], 4, -378558), f, r, d[n + 8], 11, -2022574463), m, f, d[n + 11], 16, 1839030562), i, m, d[n + 14], 23, -35309556), r = md5_hh(r, i = md5_hh(i, m = md5_hh(m, f, r, i, d[n + 1], 4, -1530992060), f, r, d[n + 4], 11, 1272893353), m, f, d[n + 7], 16, -155497632), i, m, d[n + 10], 23, -1094730640), r = md5_hh(r, i = md5_hh(i, m = md5_hh(m, f, r, i, d[n + 13], 4, 681279174), f, r, d[n + 0], 11, -358537222), m, f, d[n + 3], 16, -722521979), i, m, d[n + 6], 23, 76029189), r = md5_hh(r, i = md5_hh(i, m = md5_hh(m, f, r, i, d[n + 9], 4, -640364487), f, r, d[n + 12], 11, -421815835), m, f, d[n + 15], 16, 530742520), i, m, d[n + 2], 23, -995338651), r = md5_ii(r, i = md5_ii(i, m = md5_ii(m, f, r, i, d[n + 0], 6, -198630844), f, r, d[n + 7], 10, 1126891415), m, f, d[n + 14], 15, -1416354905), i, m, d[n + 5], 21, -57434055), r = md5_ii(r, i = md5_ii(i, m = md5_ii(m, f, r, i, d[n + 12], 6, 1700485571), f, r, d[n + 3], 10, -1894986606), m, f, d[n + 10], 15, -1051523), i, m, d[n + 1], 21, -2054922799), r = md5_ii(r, i = md5_ii(i, m = md5_ii(m, f, r, i, d[n + 8], 6, 1873313359), f, r, d[n + 15], 10, -30611744), m, f, d[n + 6], 15, -1560198380), i, m, d[n + 13], 21, 1309151649), r = md5_ii(r, i = md5_ii(i, m = md5_ii(m, f, r, i, d[n + 4], 6, -145523070), f, r, d[n + 11], 10, -1120210379), m, f, d[n + 2], 15, 718787259), i, m, d[n + 9], 21, -343485551), m = safe_add(m, h), f = safe_add(f, t), r = safe_add(r, g), i = safe_add(i, e)
    }
    return Array(m, f, r, i)
}

function md5_cmn(d, _, m, f, r, i) {
    return safe_add(bit_rol(safe_add(safe_add(_, d), safe_add(f, i)), r), m)
}

function md5_ff(d, _, m, f, r, i, n) {
    return md5_cmn(_ & m | ~_ & f, d, _, r, i, n)
}

function md5_gg(d, _, m, f, r, i, n) {
    return md5_cmn(_ & f | m & ~f, d, _, r, i, n)
}

function md5_hh(d, _, m, f, r, i, n) {
    return md5_cmn(_ ^ m ^ f, d, _, r, i, n)
}

function md5_ii(d, _, m, f, r, i, n) {
    return md5_cmn(m ^ (_ | ~f), d, _, r, i, n)
}

function safe_add(d, _) {
    var m = (65535 & d) + (65535 & _);
    return (d >> 16) + (_ >> 16) + (m >> 16) << 16 | 65535 & m
}

function bit_rol(d, _) {
    return d << _ | d >>> 32 - _
}