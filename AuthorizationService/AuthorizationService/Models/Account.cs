using EntitiesLibrary;

namespace AuthorizationService.Models
{
    public class Account : AccountBase
    {
        public string NickName { get; set; }
        public int RoleId { get; set; }

        public Login Login { get; set; }
        public Role Role { get; set; }

        public Account() : base() { }

    }
}
