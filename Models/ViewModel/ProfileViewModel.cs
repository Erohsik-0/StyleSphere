namespace StyleSphere.Models.ViewModel
{
    public class ProfileViewModel
    {

        public int Id { get; set; }
        public string? UserName { get; set; }
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string? PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public int AccessFailedCount { get; set; }

        public string? AvatarUrl { get; set; }

        public IFormFile? AvatarFile { get; set; }

        public List<ActivityLog> Activities { get; set; }

        public class ActivityLog
        {
            public string Time { get; set; } 

            public string Action { get; set; }

        }

    }
}
