using System;

namespace TSFramework.App.Principals
{
    public class AppPrincipalSerializeModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Token { get; set; }
        public string UnionName { get; set; }
    }
}