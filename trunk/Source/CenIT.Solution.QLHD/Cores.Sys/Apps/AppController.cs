using Cores.Sys.Caches.Sys;
using Cores.Sys.Models.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using TSFramework.App.Attributes;
using TSFramework.App.BaseApps;
using TSFramework.Core.Enums;
using TSFramework.Core.Helpers;
using TSFramework.Core.Utils;

namespace Cores.Sys.Apps
{
    /// <remarks>
    /// Khai bao abstract de ASP.NET MVC KHONG coi day la mot controller co the
    /// dinh tuyen. Truoc day ca Cores.Base.Apps.AppController va
    /// Cores.Sys.Apps.AppController deu la lop public thuong, khien MVC bao loi
    /// "Multiple types were found that match the controller named App".
    /// Lop nay chi dung lam lop cha, khong bao gio duoc goi truc tiep.
    /// </remarks>
    public abstract class AppController : BaseController
    {
        //private const string SESSION_KEY_MODULE_WIDGET = "ModuleWidgetForUser{0}";
        private readonly SysMenuCache _sysMenuCache = new SysMenuCache();
        //private readonly SysModuleCache _sysModuleCache = new SysModuleCache();
        //private Dictionary<string, List<SysPanelModuleModel>> _mappingModuleWidget;

        //protected Dictionary<string, List<SysPanelModuleModel>> MappingModuleWidget
        //{
        //    get
        //    {
        //        _mappingModuleWidget =
        //            Session[string.Format(SESSION_KEY_MODULE_WIDGET, User.UserName)] as
        //                Dictionary<string, List<SysPanelModuleModel>> ??
        //            new Dictionary<string, List<SysPanelModuleModel>>();
        //        return _mappingModuleWidget;
        //    }
        //    set
        //    {
        //        Session[string.Format(SESSION_KEY_MODULE_WIDGET, User.UserName)] = value;
        //        _mappingModuleWidget = value;
        //    }
        //}

        [ChildActionOnly]
        [ActionType(Type = EnumActionType.View)]
        [AllowAnonymous]
        public ActionResult Menu()
        {
            var appMenus = _sysMenuCache.GetByUserName(User?.UserName);
            var viewMenus = GetViewMenus(appMenus);
            var htmlMenu = CreateViewMenu(viewMenus);
            return PartialView("_Menu", htmlMenu);
        }

        [NonAction]
        public List<SysMenuViewModel> GetViewMenus(List<SysMenuModel> data, int menuParentId = 0, string sDepth = null)
        {
            var viewMenus = new List<SysMenuViewModel>();

            if (data == null) return viewMenus;
            var appMenus = menuParentId == 0
                ? data.Where(mn => !mn.ParentId.HasValue).ToList()
                : data.Where(mn => mn.ParentId == menuParentId).ToList();

            foreach (var item in appMenus)
            {
                var arrParentDepth = sDepth != null ? sDepth.Split(',').ToList() : new List<string>();
                var arrDepth = item.Depth != null ? item.Depth.Split(',').ToList() : new List<string>();
                string sChildDepth;
                if (sDepth != null)
                {
                    arrDepth.AddRange(arrParentDepth.ToArray());
                    arrDepth = arrDepth.Distinct().ToList();
                    sChildDepth = string.Join(",", arrDepth);
                }
                else
                {
                    sChildDepth = item.Depth;
                }

                viewMenus.Add(new SysMenuViewModel
                {
                    Depth = sChildDepth,
                    ModuleName = item.ModuleName,
                    FunctionActionId = item.FunctionActionId.GetValueOrDefault(),
                    Icon = item.Icon,
                    Id = item.MenuId,
                    LevelMenu = item.LevelMenu.GetValueOrDefault(),
                    Link = item.Link,
                    Name = item.Name,
                    UseModal = item.UseModal,
                    ModalId = item.ModalId,
                    Position = item.Position.GetValueOrDefault(),
                    Childs = GetViewMenus(data, item.MenuId, sChildDepth)
                });
            }

            return viewMenus;
        }

        [NonAction]
        public string CreateViewMenu(List<SysMenuViewModel> data, int iLevel = 0)
        {
            var dataHtml = new StringBuilder();
            if (data == null) return dataHtml.ToString();
            foreach (var menu in data)
                dataHtml.Append(menu.Childs.Count > 0
                    ? $"<li class=\"nav-item\" id=\"{menu.Id}\"><a href=\"#\" class=\"nav-link dropdown-toggle collapsed\"><i class=\"nav-icon {menu.Icon}\"></i><span class=\"nav-text {(iLevel == 0 ? "fadeable" : "")}\"><span>{menu.Name}</span></span><b class=\"caret fa fa-angle-left rt-n90\"></b></a><div class=\"hideable submenu collapse\"><ul class=\"submenu-inner\">{CreateViewMenu(menu.Childs, iLevel + 1)}</ul></div><b class=\"sub-arrow\"></b></li>"
                    : $"<li class=\"nav-item\" id=\"{menu.Id}\"><a {(menu.UseModal ? "data-modal='true'" : "")} {(menu.UseModal ? $"data-modal-id='{menu.ModalId}'" : "")} href=\"{menu.Link}\" class=\"nav-link\" name=\"{menu.Depth}\"><i class=\"nav-icon {menu.Icon}\"></i><span class=\"nav-text\"><span>{menu.Name}</span></span></a></li>");

            //dataHtml.Append(menu.Childs.Count > 0
            //    ? $"<li class=\"nav-item\" id=\"{menu.Id}\"><a href=\"#\" class=\"nav-link dropdown-toggle collapsed\"><i class=\"nav-icon {menu.Icon}\"></i><span class=\"nav-text fadeable\"><span>{menu.Name}</span></span><b class=\"caret fa fa-angle-left rt-n90\"></b></a><div class=\"hideable submenu collapse\"><ul class=\"submenu-inner\">{CreateViewMenu(menu.Childs)}</ul></div><b class=\"sub-arrow\"></b></li>"
            //    : $"<li class=\"nav-item\" id=\"{menu.Id}\"><a href=\"{menu.Link}\" class=\"nav-link\" name=\"{menu.Depth}\"><i class=\"nav-icon {menu.Icon}\"></i><span class=\"nav-text\"><span>{menu.Name}</span></span></a></li>");


            return dataHtml.ToString();
        }

        [NonAction]
        protected string[] GetPermissionViaUser(string userName)
        {
            var lstPermissionViaUser = new List<string>();

            SysPermissionCache permissionCache = new SysPermissionCache();
            var lstPermissions = permissionCache.GetViaUser(userName);

            lstPermissions.ForEach(p =>
            {
                var areaName = p.Area;
                var controllerName = p.FunctionName;
                var actionName = p.ActionName;

                var namespaces = RouteTable.Routes.OfType<Route>()
                    .Where(d => d.DataTokens != null && d.DataTokens.ContainsKey("area") &&
                                (areaName == null || d.DataTokens["area"].Equals(areaName)))
                    .Select(r => r.DataTokens).ToArray()[0]["Namespaces"];

                var lstNamespaces = new List<string>((string[])namespaces);

                var lstControllers = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => a.FullName.StartsWith("Modules.")).ToList()
                    .SelectMany(a => a.GetTypes())
                    .Where(t => t.IsClass && typeof(Controller).IsAssignableFrom(t) &&
                                lstNamespaces.Exists(n => n == t.Namespace))
                    .OrderBy(c => c.FullName)
                    .ToList();
                lstControllers.ForEach(c =>
                {
                    ReflectedControllerDescriptor controllerDescriptor = new ReflectedControllerDescriptor(c);
                    controllerDescriptor.GetCanonicalActions().ToList().ForEach(actionDescriptor =>
                    {
                        var action = actionDescriptor.ActionName;

                        {
                            var actionAllowAnyPermission =
                                actionDescriptor.GetCustomAttributes(typeof(AllowAnyPermissionAttribute), false);
                            var controllerAllowAnyPermission =
                                controllerDescriptor.GetCustomAttributes(
                                    typeof(AllowAnyPermissionAttribute), false);

                            if (actionAllowAnyPermission.Any() || controllerAllowAnyPermission.Any())
                            {
                                lstPermissionViaUser.Add(EHashMD5.CalculateMD5Hash($"{areaName}_{controllerName}_{action}"));
                            }

                            var actionTypes = actionDescriptor
                                .GetCustomAttributes(typeof(ActionTypeAttribute), false);
                            var currentActionType =
                                actionTypes.Length > 0 ? actionTypes[0] as ActionTypeAttribute : null;
                            if (currentActionType != null)
                            {
                                var actionTypeName = EnumHelper.GetDescription(currentActionType.Type);
                                var permitHashValue =
                                    EHashMD5.CalculateMD5Hash($"{areaName}_{controllerName}_{action}");

                                if (actionTypeName == actionName && !lstPermissionViaUser.Exists(permit =>
                                        permit == permitHashValue))
                                {
                                    lstPermissionViaUser.Add(permitHashValue);
                                }
                            }
                        }
                    });
                });
            });

            return lstPermissionViaUser.ToArray();
        }

        //[NonAction]
        //protected void InitModulePanel()
        //{
        //    var listModuleContentPanels = _sysModuleCache.GetByUser(User.UserName);
        //    var mappingModuleWidget = new Dictionary<string, List<SysPanelModuleModel>>();
        //    listModuleContentPanels?.ForEach(m =>
        //    {
        //        var listModules = new List<SysPanelModuleModel>
        //        {
        //            new SysPanelModuleModel
        //            {
        //                ModuleName = m.ModuleName,
        //                ModuleView = m.ModuleView,
        //                AssemblyName = m.AssemblyName,
        //                MainController = m.MainController,
        //                ModuleId = m.ModuleId,
        //                OrderBy = m.OrderBy
        //            }
        //        };
        //        if (mappingModuleWidget.ContainsKey(m.ContentPanelName))
        //            mappingModuleWidget[m.ContentPanelName].AddRange(listModules);
        //        else
        //            mappingModuleWidget.Add(m.ContentPanelName, listModules);
        //    });
        //    MappingModuleWidget = mappingModuleWidget;
        //}
    }
}