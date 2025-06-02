namespace social.Models.ViewModel
{
	public class Profile
	{
		public int following { get; set; }
		public int follower { get; set; }
        public bool followerb { get; set; }
        public bool followingb { get; set; }
        public bool access { get; set; }

        public bool followingt { get; set; } 
		public bool followingt2 { get; set; }
        public bool followert { get; set; }
        public bool followert2 { get; set; }
        public string username { get; set; }
		public string picturename { get; set; }
		public int accountId { get; set; }

    }
}
