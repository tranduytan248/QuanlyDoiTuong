using System.Linq;
using System.Web.Mvc;
using Cores.Base.Apps;
using Cores.Sys.Caches.Sys;
using TSFramework.App.Attributes;
using TSFramework.Core.Enums;

namespace Modules.Sys.Areas.Sys.Controllers
{
    /// <summary>
    /// Giam sat truc tuyen: hien nhung tai khoan dang dang nhap va man hinh ho
    /// dang xem. Chi hien phien con hoat dong trong khoang thoi gian cho.
    ///
    /// Quyen truy cap do man hinh Phan quyen chuc nang quyet dinh - dang ky
    /// trong script 25 va mac dinh chi cap cho vai tro Quan tri he thong.
    /// </summary>
    public class UserActivityController : AppController
    {
        /// <summary>So phut khong hoat dong thi coi la da thoat.</summary>
        private const int TIMEOUT_MINUTES = 5;

        private readonly SysUserActivityCache _activityCache = new SysUserActivityCache();

        // GET: Sys/UserActivity
        [ActionType(Type = EnumActionType.View)]
        [HttpGet]
        public ActionResult Index()
        {
            // Bien cuc bo, khong dung static: moi request co nguoi dung khac nhau,
            // dung static se lan quyen giua cac phien.
            var permissions = GetPermissionViaUser(User.UserName);
            ViewBag.TimeoutMinutes = TIMEOUT_MINUTES;
            return View(permissions);
        }

        /// <summary>
        /// Danh sach phien dang hoat dong. Giao dien tu goi lai dinh ky de lam moi.
        /// </summary>
        [AjaxOnly]
        [HttpPost]
        [ActionType(Type = EnumActionType.View)]
        public ActionResult Get()
        {
            var data = _activityCache.Get(TIMEOUT_MINUTES);

            return Json(new
            {
                data = data.Select(item => new
                {
                    item.SessionId,
                    item.UserName,
                    item.FullName,
                    item.Email,
                    item.Phone,
                    item.Avatar,
                    item.CurrentUrl,
                    item.ScreenName,
                    item.IpAddress,
                    item.SecondsAgo,
                    item.MinutesOnline,
                    item.UnionName,
                    item.PositionName,
                    LoginTime = item.LoginTime.ToString("HH:mm dd/MM/yyyy"),
                    LastActivity = item.LastActivity.ToString("HH:mm:ss")
                }).ToList(),
                total = data.Count
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
