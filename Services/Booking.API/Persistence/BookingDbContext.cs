﻿using Booking.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Booking.API.Persistence
{
    public class BookingDbContext : DbContext
    {
        #region db_set
        public DbSet<BookingTour> BookingTours { get; set; }
        public DbSet<BookingRoom> BookingRooms { get; set; }
        public DbSet<TourBookingRoom> TourBookingRooms { get; set; }
        public DbSet<DetailBookingRoom> DetailBookingRooms { get; set; }

        #endregion
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) 
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetailBookingRoom>()
                .HasOne(c => c.BookingRoom)
                .WithMany(e => e.DetailBookingRooms)
                .HasForeignKey(c => c.BookingId);

            modelBuilder.Entity<TourBookingRoom>()
                .HasOne(c => c.BookingTour)
                .WithMany(e => e.TourBookingRooms)
                .HasForeignKey(c=> c.BookingTourId);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}