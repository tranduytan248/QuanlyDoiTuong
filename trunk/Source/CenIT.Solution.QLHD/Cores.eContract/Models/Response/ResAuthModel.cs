using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Cores.eContract.Models.Response
{
    public class ResAuthModel : BaseResponseModel<object>
    {
        ///// <summary>
        ///// Token dùng truy cập các API của hệ  thống HĐĐT 
        ///// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        ///// <summary>
        ///// Loại token: bearer 
        ///// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        ///// <summary>
        ///// Thời gian hết hạn token.
        ///// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        ///// <summary>
        ///// Phạm vi sử dụng
        ///// </summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }

        ///// <summary>
        ///// Tài khoản
        ///// </summary>
        [JsonProperty("user_name")]
        public string UserName { get; set; }

        ///// <summary>
        ///// Danh sách mã quyền 
        ///// </summary>
        [JsonProperty("roles")]
        public List<string> Roles { get; set; }

        /// <summary>
        /// Domain
        /// </summary>
        [JsonProperty("domain")]
        public string Domain { get; set; }

        /// <summary>
        /// Id tổ chức 
        /// </summary>
        [JsonProperty("partyId")]
        public string PartyId { get; set; }

        ///// <summary>
        ///// Id JTI
        ///// </summary>
        [JsonProperty("jti")]
        public string Jti { get; set; }

    }

    public class ResAuthUserModel : ResAuthModel
    {
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("idUser")]
        public string IdUser { get; set; }

        [JsonProperty("packageExpired")]
        public object PackageExpired { get; set; }

        [JsonProperty("packageName")]
        public object PackageName { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("orgId")]
        public string OrgId { get; set; }

        [JsonProperty("createDate")]
        public DateTime CreateDate { get; set; }
    }
}