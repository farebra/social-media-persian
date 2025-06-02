using social.Models.Entity;
using System.Collections.Generic;

namespace social.Models.ViewModel
{
    public class Dtoinbox
    {
        public Account account{ get; set; }
        public IList<Account> Accounts { get; set; }
        public chatMessage chatMessage { get; set; }
        public IList<chatMessage> ChatMessage { get; set; }
        public AccountJoin AccountJoin { get; set; }
        public IList<AccountJoin> accountJoin { get; set; }
        public Profile profile { get; set; }

    }
}
