using System;
using System.Collections.Generic;
using System.Web;
using TSFramework.App.Attributes;
using TSFramework.App.Models;

namespace Cores.Sys.Models.Sys
{
    public class SysUserModel : BaseModel
    {
        public int? UserId { get; set; }

        [CustomDisplayName("User_Label_OfficeName")]
        public string OfficeName { get; set; }

        [CustomDisplayName("User_Label_FullName")]
        public string FullName { get; set; }

        [CustomRequired]
        [CustomDisplayName("User_Label_UserName")]
        public string UserName { get; set; }

        [CustomDisplayName("User_Label_Email")]
        [CustomRequired]
        public string Email { get; set; }

        public string DetailUrl { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsActive { get; set; }

        public bool IsOnline { get; set; }

        //public int Processing { get; set; }
        public string RoleIDs { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string HostUrl { get; set; }

        [CustomDisplayName("User_Label_Avatar")]
        public string Avatar { get; set; }

        [CustomDisplayName("User_Label_Avatar")]
        public string AvatarPath { get; set; }

        [CustomDisplayName("User_Label_Avatar")]
        public HttpPostedFileBase AvatarFileBase { get; set; }

        [CustomDisplayName("User_Label_Phone")]
        public string Phone { get; set; }

        /// <summary>Don vi cong tac - lay tu Cate_Unions_Members.</summary>
        public string UnionName { get; set; }

        /// <summary>Chuc vu trong don vi.</summary>
        public string PositionName { get; set; }

        /// <summary>Danh sach vai tro he thong, phan tach bang dau phay.</summary>
        public string RoleNames { get; set; }

        /// <summary>
        /// So linh vuc duoc phan cong. Bang 0 nghia la nguoi dung chua thao tac
        /// duoc gi voi du lieu doi tuong - can canh bao tren giao dien.
        /// </summary>
        public int FieldCount { get; set; }

        /// <summary>Tai khoan co dang bi khoa hay khong.</summary>
        public bool IsLocked { get; set; }

        [RequiredIfNot("UserId", null)]
        [CustomDisplayName("Reason_Title")]
        public override string Reason { get; set; }

        public new int? TotalRow { get; set; } = 0;

        public List<SysRoleModel> ListRoles { get; set; }

        public List<SysPermissionModel> ListPermissions { get; set; } = new List<SysPermissionModel>();
    }
}