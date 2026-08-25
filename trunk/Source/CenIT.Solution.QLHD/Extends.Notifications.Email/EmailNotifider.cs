using Cores.Base.Interfaces;
using System;
using System.Collections.Generic;
using Cores.Base.Models;
using TSFramework.App.Attributes;
using TSFramework.App.Processors;
using TSFramework.Core.Providers;
using System.Web.Hosting;
using TSFramework.Core.Members.Mail;
using System.Configuration;
using Cores.Base.Enums;
using Cores.Sys.Caches.Sys;

namespace Extends.Notifications.Email
{
    [ClassInfo("Extends.EmailNotifider", "Thông báo qua email")]
    public class EmailNotifider : INotify
    {
        public string Name { get; } = "Extends.EmailNotifider";

        public string Description { get; } = "Thông báo qua email";

        public void Push(string senderName, string typeObjName, string title, List<NotifyReceiverModel> lstReceivers, string detailUrl, string hostName, Dictionary<string, object> extParrams)
        {
            throw new NotImplementedException();
        }

        private static readonly Dictionary<EnumTypeEmail, (string EmailTemplatePath, string TitleEmail)> mappingEmailTemplates = new Dictionary<EnumTypeEmail, (string EmailTemplatePath, string TitleEmail)>
        {
            { EnumTypeEmail.ContractConfirmation, ("~/Contents/Modules/Sys/EmailTemplates/_TemplateContractConfirmation.cshtml", "ContractConfirmation_Message") },
            { EnumTypeEmail.ContractRejection, ("~/Contents/Modules/Sys/EmailTemplates/_TemplateContractRejection.cshtml", "ContractRejection_Message") },
            { EnumTypeEmail.ContractResult, ("~/Contents/Modules/Sys/EmailTemplates/_TemplateContractResult.cshtml", "ContractResult_Message") },
            { EnumTypeEmail.ContractPending, ("~/Contents/Modules/Sys/EmailTemplates/_TemplateContractPending.cshtml", "ContractPending_Message") },
            { EnumTypeEmail.ProfileOverdue, ("~/Contents/Modules/Sys/EmailTemplates/_TemplateProfileOverdue.cshtml", "ProfileOverdue_Message") },
            { EnumTypeEmail.ProfileApproachingDeadline, ("~/Contents/Modules/Sys/EmailTemplates/_TemplateProfileApproachingDeadline.cshtml", "ProfileApproachingDeadline_Message") }
        };

        public void Push(ContentNotifyModel model)
        {
            SysConfigCache configCache = new SysConfigCache();

            var needSendEmail = configCache.GetViaKey("CONFIG_KEY_ENABLE_SEND_EMAIL_STAFF")?.ConfigValue == "1";
            if (model == null || !needSendEmail) return;

            string emailTemplatePath;
            string titleEmail;

            if (mappingEmailTemplates.TryGetValue(model.TypeEmail, out var emailTemplate))
            {
                emailTemplatePath = emailTemplate.EmailTemplatePath;
                titleEmail = AppProcessor.Messagor.GetMessage(emailTemplate.TitleEmail);
            }
            else
            {
                emailTemplatePath = "";
                titleEmail = "";
            }
            if (!string.IsNullOrEmpty(emailTemplatePath))
            {
                var mailBodyHtml = RenderTemplateHtmlProvider.RenderPartialToHtml(HostingEnvironment.MapPath(emailTemplatePath), new ContentEmailModel { ContractInfo = model.ContractInfo, UserInfo = model.UserInfo });

                AppProcessor.Mailer.PushEmail(new List<MailModel>
                {
                    new MailModel
                    {
                        Subject =
                            $"[{AppProcessor.Messagor.GetMessage("App_Title")}] {titleEmail}",
                        To = new List<string> { model.CusInfo.Email },
                        Body = mailBodyHtml,
                        IsBodyHtml = true,
                        DisplayNameFrom = AppProcessor.Messagor.GetMessage("App_Owner_DisplayName"),
                        DicImgs = new Dictionary<string, byte[]>
                        {
                            {
                                "LogoVPDKDD",
                                System.IO.File.ReadAllBytes(
                                    $"{HostingEnvironment.MapPath(ConfigurationManager.AppSettings["Logo_VPDKDD_Path"])}")
                            }
                        }
                    }
                });

            }

        }

    }
}
