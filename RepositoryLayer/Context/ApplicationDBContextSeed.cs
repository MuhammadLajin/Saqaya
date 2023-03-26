using DomainLayer.Models;
using SharedDTO;
using Microsoft.EntityFrameworkCore;
using System;

namespace RepositoryLayer.Context
{
    public static class ApplicationDBContextSeed
    {
        public static void BuildEnums(this ModelBuilder modelBuilder)
        {
            modelBuilder.SeedUsers();
        }

        private static void SeedUsers(this ModelBuilder modelBuilder)
        {
            #region Users
            modelBuilder.Entity<User>().HasData(new User() {Id = "testKeyONE", firstName = "User", lastName="One",email="testUserOne", CreatedAt=DateTime.Now, IsDeleted = false });
            modelBuilder.Entity<User>().HasData(new User() {Id = "testKeyTWO", firstName = "User", lastName="TWO",email= "testUserTWO", CreatedAt=DateTime.Now, IsDeleted = false });
            
            #endregion
        }



    }
}
