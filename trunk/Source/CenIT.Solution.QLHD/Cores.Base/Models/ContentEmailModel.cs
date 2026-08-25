namespace Cores.Base.Models
{
    public class ContentEmailModel
    {
        public User UserInfo { get; set; } = new User();
        public Contract ContractInfo { get; set; } = new Contract();
    }
}