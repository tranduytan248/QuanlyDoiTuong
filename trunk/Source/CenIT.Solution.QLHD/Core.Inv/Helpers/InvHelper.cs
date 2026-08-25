using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Core.Inv.Enums;
using TSFramework.Core.Helpers;

namespace Core.Inv.Helpers
{
    public class InvHelper
    {
        public static string GenFKey()
        {
            long i = 1;
            foreach (var b in Guid.NewGuid().ToByteArray()) i *= b + 1;
            return $"{i - DateTime.Now.Ticks:x}".ToUpper();
        }

        #region Function

        public static string InvStatusDescription(EnumInvStatus invStatus)
        {
            switch (invStatus)
            {
                case EnumInvStatus.InvoiceJustCreated:
                {
                    return "Vừa khởi tạo";
                }

                case EnumInvStatus.InvoiceHasSignature:
                {
                    return "Đã phát hành";
                }

                case EnumInvStatus.InvoiceTaxDeclaration:
                {
                    return "Đã khai báo thuế";
                }

                case EnumInvStatus.InvoiceAreReplaced:
                {
                    return "Sai sót bị thay thế";
                }

                case EnumInvStatus.InvoiceAreAdjusted:
                {
                    return "Sai sót bị điều chỉnh";
                }

                case EnumInvStatus.InvoiceAreCancled:
                {
                    return "Xóa bỏ";
                }

                default:
                    return "";
            }
        }

        public static string InvPaymentStatusDescription(EnumInvPaymentStatus paymentStatus)
        {
            switch (paymentStatus)
            {
                case EnumInvPaymentStatus.NotYet:
                {
                    return "Chưa thanh toán";
                }

                case EnumInvPaymentStatus.Paid:
                {
                    return "Đã thanh toán";
                }

                default:
                    return "";
            }
        }

        public static string InvTypeDescription(EnumInvType invType)
        {
            switch (invType)
            {
                case EnumInvType.InvoiceTypeNormal:
                {
                    return "Thông thường";
                }

                case EnumInvType.InvoiceTypeReplace:
                {
                    return "Thay thế";
                }

                case EnumInvType.InvoiceAdjustIncrease:
                {
                    return "Điều chỉnh tăng";
                }

                case EnumInvType.InvoiceAdjustDecrease:
                {
                    return "Điều chỉnh giảm";
                }

                case EnumInvType.InvoiceAdjustInfo:
                {
                    return "Điều chỉnh thông tin";
                }

                default:
                    return "";
            }
        }

        public static string InvTransferStatusDescription(EnumInvTransferStatus invTransferStatus)
        {
            switch (invTransferStatus)
            {
                case EnumInvTransferStatus.NotYet:
                {
                    return "Chưa chuyển đổi";
                }

                case EnumInvTransferStatus.Transferred:
                {
                    return "Đã chuyển đổi";
                }

                default:
                    return "";
            }
        }

        public static string InvCancelStatusDescription(EnumInvCancelStatus invCancelStatus)
        {
            switch (invCancelStatus)
            {
                case EnumInvCancelStatus.Duplicate:
                {
                    return "Trùng hóa đơn / Duplicate";
                }

                case EnumInvCancelStatus.WrongInfo:
                {
                    return "Sai thông tin / Wrong information";
                }

                case EnumInvCancelStatus.Other:
                {
                    return "Lý do khác / Other";
                }

                default:
                    return "";
            }
        }

        public static string InvPaymentMethobDescription(EnumInvPaymentMethob paymentMethob)
        {
            switch (paymentMethob)
            {
                case EnumInvPaymentMethob.Cash:
                {
                    return "Thanh toán tiền mặt";
                }

                case EnumInvPaymentMethob.Transfer:
                {
                    return "Thanh toán chuyển khoản";
                }

                case EnumInvPaymentMethob.Credit:
                {
                    return "Thanh toán thẻ tín dụng";
                }

                case EnumInvPaymentMethob.TransferOrCash:
                {
                    return "Thanh toán chuyển khoản hoặc tiền mặt";
                }

                case EnumInvPaymentMethob.Clearing:
                {
                    return "Thanh toán bù trừ";
                }

                default:
                    return "";
            }
        }

        public static string InvPaymentMethobValue(EnumInvPaymentMethob paymentMethob)
        {
            return EnumHelper.GetDescription(paymentMethob);
        }

        #endregion

        #region Init List Items

        public static List<ListItem> GetListStatus()
        {
            var data = new[]
            {
                new ListItem
                {
                    Value = ((int)EnumInvStatus.InvoiceJustCreated).ToString(),
                    Text = "Vừa khởi tạo"
                },
                new ListItem
                {
                    Value = ((int)EnumInvStatus.InvoiceHasSignature).ToString(),
                    Text = "Đã phát hành"
                },
                new ListItem
                {
                    Value = ((int)EnumInvStatus.InvoiceTaxDeclaration).ToString(),
                    Text = "Đã khai báo thuế"
                },
                new ListItem
                {
                    Value = ((int)EnumInvStatus.InvoiceAreReplaced).ToString(),
                    Text = "Sai sót bị thay thế"
                },
                new ListItem
                {
                    Value = ((int)EnumInvStatus.InvoiceAreAdjusted).ToString(),
                    Text = "Sai sót bị điều chỉnh"
                },
                new ListItem
                {
                    Value = ((int)EnumInvStatus.InvoiceAreCancled).ToString(),
                    Text = "Xóa bỏ"
                }
            };
            var statusList = data.ToList();
            return statusList;
        }

        public static List<ListItem> GetListPaymentMethob()
        {
            var data = new[]
            {
                new ListItem
                {
                    Value = InvPaymentMethobValue(EnumInvPaymentMethob.Cash),
                    Text = InvPaymentMethobDescription(EnumInvPaymentMethob.Cash)
                },
                new ListItem
                {
                    Value = InvPaymentMethobValue(EnumInvPaymentMethob.Transfer),
                    Text = InvPaymentMethobDescription(EnumInvPaymentMethob.Transfer)
                },
                new ListItem
                {
                    Value = InvPaymentMethobValue(EnumInvPaymentMethob.TransferOrCash),
                    Text = InvPaymentMethobDescription(EnumInvPaymentMethob.TransferOrCash)
                },
                new ListItem
                {
                    Value = InvPaymentMethobValue(EnumInvPaymentMethob.Credit),
                    Text = InvPaymentMethobDescription(EnumInvPaymentMethob.Credit)
                },
                new ListItem
                {
                    Value = InvPaymentMethobValue(EnumInvPaymentMethob.Clearing),
                    Text = InvPaymentMethobDescription(EnumInvPaymentMethob.Clearing)
                }
            };
            var statusList = data.ToList();
            return statusList;
        }

        public static List<ListItem> GetListStatusPublish()
        {
            var data = new[]
            {
                new ListItem
                {
                    Value = ((int)EnumInvStatus.InvoiceHasSignature).ToString(),
                    Text = "Đã phát hành"
                },
                new ListItem
                {
                    Value = ((int)EnumInvStatus.InvoiceAreReplaced).ToString(),
                    Text = "Sai sót bị thay thế"
                },
                new ListItem
                {
                    Value = ((int)EnumInvStatus.InvoiceAreAdjusted).ToString(),
                    Text = "Sai sót bị điều chỉnh"
                },
                new ListItem
                {
                    Value = ((int)EnumInvStatus.InvoiceAreCancled).ToString(),
                    Text = "Xóa bỏ"
                }
            };
            var statusList = data.ToList();
            return statusList;
        }

        public static List<ListItem> GetListInvType()
        {
            var data = new[]
            {
                new ListItem
                {
                    Value = ((int)EnumInvType.InvoiceTypeNormal).ToString(),
                    Text = "Thông thường"
                },
                new ListItem
                {
                    Value = ((int)EnumInvType.InvoiceTypeReplace).ToString(),
                    Text = "Thay thế"
                },
                new ListItem
                {
                    Value = ((int)EnumInvType.InvoiceAdjustIncrease).ToString(),
                    Text = "Điều chỉnh tăng"
                },
                new ListItem
                {
                    Value = ((int)EnumInvType.InvoiceAdjustDecrease).ToString(),
                    Text = "Điều chỉnh giảm"
                },
                new ListItem
                {
                    Value = ((int)EnumInvType.InvoiceAdjustInfo).ToString(),
                    Text = "Điều chỉnh thông tin"
                }
            };
            var invTypeList = data.ToList();
            return invTypeList;
        }

        public static List<ListItem> GetListAdjustType()
        {
            var data = new[]
            {
                new ListItem
                {
                    Value = ((int)EnumInvType.InvoiceAdjustIncrease).ToString(),
                    Text = "Điều chỉnh tăng"
                },
                new ListItem
                {
                    Value = ((int)EnumInvType.InvoiceAdjustDecrease).ToString(),
                    Text = "Điều chỉnh giảm"
                },
                new ListItem
                {
                    Value = ((int)EnumInvType.InvoiceAdjustInfo).ToString(),
                    Text = "Điều chỉnh thông tin"
                }
            };
            var adjustTypeList = data.ToList();
            return adjustTypeList;
        }

        public static List<ListItem> GetListPaymentStatus()
        {
            var data = new[]
            {
                new ListItem
                {
                    Value = ((int)EnumInvPaymentStatus.NotYet).ToString(),
                    Text = "Chưa thanh toán"
                },
                new ListItem
                {
                    Value = ((int)EnumInvPaymentStatus.Paid).ToString(),
                    Text = "Đã thanh toán"
                }
            };
            var paymentStatusList = data.ToList();
            return paymentStatusList;
        }

        public static List<ListItem> GetListTransferStatus()
        {
            var data = new[]
            {
                new ListItem
                {
                    Value = ((int)EnumInvTransferStatus.NotYet).ToString(),
                    //Text = BaseAppContext.Current.GetMessage("InvTransferStatus_NotTransferYet")
                    Text = "Chưa chuyển đổi"
                },
                new ListItem
                {
                    Value = ((int)EnumInvTransferStatus.Transferred).ToString(),
                    //Text = BaseAppContext.Current.GetMessage("InvTransferStatus_HasTransferred")
                    Text = "Đã chuyển đổi"
                }
            };
            var transferStatusList = data.ToList();
            return transferStatusList;
        }

        public static List<ListItem> GetListCancelStatus()
        {
            var data = new[]
            {
                new ListItem { Value = "Trùng hóa đơn / Duplicate", Text = "Trùng hóa đơn / Duplicate" },
                new ListItem
                    { Value = "Sai thông tin / Wrong information", Text = "Sai thông tin / Wrong information" },
                new ListItem { Value = "Lý do khác / Other", Text = "Lý do khác / Other" }
            };
            var transferStatusList = data.ToList();
            return transferStatusList;
        }

        public static List<ListItem> GetListPublishStatus()
        {
            var data = new[]
            {
                new ListItem { Value = "Tách hóa đơn/ Breakdown invoice", Text = "Tách hóa đơn/ Breakdown invoice" },
                new ListItem { Value = "Xuất lại/ Re-issue invoice", Text = "Xuất lại/ Re-issue invoice" },
                new ListItem { Value = "Lý do khác / Other", Text = "Lý do khác / Other" }
            };
            var transferStatusList = data.ToList();
            return transferStatusList;
        }

        #endregion
    }
}