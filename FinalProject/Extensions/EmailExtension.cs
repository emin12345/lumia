using FinalProject.DAL;
using System;

namespace FinalProject.Extensions
{
    public static class EmailExtensions
    {
        public static bool IsRegisteredEmail(this LumiaDbContext dbContext, string email)
        {
            var existingEmail = dbContext.Subscribers.FirstOrDefault(e => e.Email == email);
            return existingEmail != null;
        }

        public static void SaveEmail(this LumiaDbContext dbContext, string email)
        {
            var newEmail = new Entities.Subscriber { Email = email };
            dbContext.Subscribers.Add(newEmail);
            dbContext.SaveChanges();
        }
    }
}
