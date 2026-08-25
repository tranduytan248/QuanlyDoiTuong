using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Hosting;
using Cores.Base.Enums;
using Cores.Base.Models;
using TSFramework.App.Processors;
using TSFramework.Core.Members.Mail;
using TSFramework.Core.Providers;

namespace Cores.Base.Helpers
{
    public class SendNotificationHelper
    {
        private static readonly Dictionary<EnumTypeEmail, (string viewUrl, string titleContent)> notificationMappings =
            new Dictionary<EnumTypeEmail, (string viewUrl, string titleContent)>
            {
                {
                    EnumTypeEmail.ContractConfirmation,
                    ("~/Contents/Modules/Sys/EmailTemplates/_TemplateContractConfirmation.cshtml",
                        "ContractConfirmation_Message")
                },
                {
                    EnumTypeEmail.ContractRejection,
                    ("~/Contents/Modules/Sys/EmailTemplates/_TemplateContractRejection.cshtml",
                        "ContractRejection_Message")
                },
                {
                    EnumTypeEmail.ContractResult,
                    ("~/Contents/Modules/Sys/EmailTemplates/_TemplateContractResult.cshtml", "ContractResult_Message")
                },
                {
                    EnumTypeEmail.ContractPending,
                    ("~/Contents/Modules/Sys/EmailTemplates/_TemplateContractPending.cshtml", "ContractPending_Message")
                },
                {
                    EnumTypeEmail.ProfileOverdue,
                    ("~/Contents/Modules/Sys/EmailTemplates/_TemplateProfileOverdue.cshtml", "ProfileOverdue_Message")
                },
                {
                    EnumTypeEmail.ProfileApproachingDeadline,
                    ("~/Contents/Modules/Sys/EmailTemplates/_TemplateProfileApproachingDeadline.cshtml",
                        "ProfileApproachingDeadline_Message")
                }
            };

        public static void Send(ContentNotificationModel model)
        {
            string viewUrl;
            string titleContent;

            // Kiểm tra xem EnumSituationNotification có tồn tại trong dictionary không
            if (notificationMappings.TryGetValue(model.TypeEmail, out var notification))
            {
                viewUrl = notification.viewUrl;
                titleContent = AppProcessor.Messagor.GetMessage(notification.titleContent);
            }
            else
            {
                viewUrl = "";
                titleContent = "";
            }

            var mailBodyHtml =
                RenderTemplateHtmlProvider.RenderPartialToHtml(HostingEnvironment.MapPath(viewUrl), model);

            AppProcessor.Mailer.PushEmail(new List<MailModel>
            {
                new MailModel
                {
                    Subject =
                        $"[{AppProcessor.Messagor.GetMessage("App_Title")}] {titleContent}",
                    To = new List<string> { model.CusInfo.Email },
                    Body = mailBodyHtml,
                    IsBodyHtml = true,
                    DisplayNameFrom = AppProcessor.Messagor.GetMessage("App_Owner_DisplayName"),
                    DicImgs = new Dictionary<string, byte[]>
                    {
                        {
                            "LogoVPDKDD",
                            File.ReadAllBytes(
                                $"{HostingEnvironment.MapPath(ConfigurationManager.AppSettings["Logo_VPDKDD_Path"])}")
                        }
                    }
                }
            });
        }
    }
}