﻿using Microsoft.AspNetCore.Identity;

namespace AUCTIONHIVE.Data
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}
