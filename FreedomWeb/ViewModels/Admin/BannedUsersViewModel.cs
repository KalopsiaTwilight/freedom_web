using System.Collections.Generic;

namespace FreedomWeb.ViewModels.Admin
{
    public class BannedUsersViewModel
    {
        public List<BannedUserListItem> BanList { get; set; }
    }
    public class BannedUserListItem
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        
        public string BanDate { get; set; }
        public string UnbanDate { get; set; }

        public string BannedBy { get; set; }
        public string BanReason { get; set; }
        public bool Active { get; set; }
    }
}