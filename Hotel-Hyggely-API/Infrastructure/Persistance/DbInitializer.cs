namespace Infrastructure.Persistance
{
	using Domain.Entities;
	using Domain.Enums;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.DependencyInjection;
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public static class DbInitializer
	{
		public static void Seed(IServiceProvider serviceProvider)
		{
			using var scope = serviceProvider.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			SeedStaff(context);
			context.SaveChanges();

			SeedRoomTypes(context);
			context.SaveChanges();

			SeedRooms(context);
			context.SaveChanges();

			SeedCustomers(context);
			context.SaveChanges();

			SeedBookings(context);
			context.SaveChanges();

			SeedCleaningSchedules(context);
			context.SaveChanges();

			SeedRoomTypeImage(context);
			context.SaveChanges();
		}

		private static void SeedRoomTypes(AppDbContext context)
		{
			if (context.RoomTypes.Any())
				return;

			context.RoomTypes.AddRange(
				new RoomType { Name = "Enkelt", Price = 69m },
				new RoomType { Name = "Dobbelt", Price = 99m },
				new RoomType { Name = "Luksus", Price = 149m },
				new RoomType { Name = "Suite", Price = 219m }
			);
		}

		private static void SeedRooms(AppDbContext context)
		{
			if (context.Rooms.Any())
				return;

			// Map room types by name for easy assignment
			var roomTypes = context.RoomTypes.ToDictionary(rt => rt.Name, rt => rt);

			var rooms = new List<Room>
			{
				new Room { Number = 1,  Floor = 1, RoomTypeId = roomTypes["Enkelt"].ID, BedAmount = 1, LastCleanedDate = DateTime.Today.AddDays(-1) },
				new Room { Number = 2,  Floor = 1, RoomTypeId = roomTypes["Enkelt"].ID, BedAmount = 1, LastCleanedDate = DateTime.Today.AddDays(-2) },
				new Room { Number = 3,  Floor = 1, RoomTypeId = roomTypes["Dobbelt"].ID, BedAmount = 2, LastCleanedDate = DateTime.Today.AddDays(-1) },
				new Room { Number = 4,  Floor = 1, RoomTypeId = roomTypes["Luksus"].ID,   BedAmount = 2, LastCleanedDate = DateTime.Today.AddDays(-3) },
				new Room { Number = 5,  Floor = 2, RoomTypeId = roomTypes["Dobbelt"].ID, BedAmount = 2, LastCleanedDate = DateTime.Today.AddDays(-1) },
				new Room { Number = 6,  Floor = 2, RoomTypeId = roomTypes["Luksus"].ID, BedAmount = 2, LastCleanedDate = DateTime.Today.AddDays(-4) },
				new Room { Number = 7,  Floor = 2, RoomTypeId = roomTypes["Dobbelt"].ID, BedAmount = 2, LastCleanedDate = DateTime.Today.AddDays(-2) },
				new Room { Number = 8,  Floor = 2, RoomTypeId = roomTypes["Suite"].ID,  BedAmount = 3, LastCleanedDate = DateTime.Today.AddDays(-5) },
				new Room { Number = 9,  Floor = 3, RoomTypeId = roomTypes["Enkelt"].ID, BedAmount = 1, LastCleanedDate = DateTime.Today.AddDays(-1) },
				new Room { Number = 10, Floor = 3, RoomTypeId = roomTypes["Dobbelt"].ID, BedAmount = 2, LastCleanedDate = DateTime.Today.AddDays(-2) },
				new Room { Number = 11, Floor = 3, RoomTypeId = roomTypes["Luksus"].ID, BedAmount = 2, LastCleanedDate = DateTime.Today.AddDays(-1) },
				new Room { Number = 12, Floor = 3, RoomTypeId = roomTypes["Suite"].ID,  BedAmount = 3, LastCleanedDate = DateTime.Today.AddDays(-6) }
			};

			context.Rooms.AddRange(rooms);
		}

		private static void SeedStaff(AppDbContext context)
		{
			if (context.Staff.Any())
				return;

			// Reuse the same bcrypt hash for demo accounts (e.g., password "admin" or similar)
			const string demoHash = "$2a$12$WwwRGOLk51pBPPyaTX.xBOOhKretH42r00uSeBtkLW/Pp567nd/UK";

			context.Staff.AddRange(
				new Staff { UserName = "admin", Password = demoHash, Role = StaffRole.Admin, FullName = "System Administrator" },
				new Staff { UserName = "reception1", Password = demoHash, Role = StaffRole.Receptionist, FullName = "Reception Desk" },
				new Staff { UserName = "cleaner1", Password = demoHash, Role = StaffRole.Cleaning, FullName = "Housekeeping Team" }
			);
		}

		private static void SeedCustomers(AppDbContext context)
		{
			if (context.Customers.Any())
				return;

			context.Customers.AddRange(
				new Customer { FullName = "Jannick Sørensen", Email = "jannicksørensen@gmail.com", PhoneNumber = "69696969" },
				new Customer { FullName = "Maria Nielsen", Email = "maria.nielsen@example.com", PhoneNumber = "22223333" },
				new Customer { FullName = "Peter Jensen", Email = "peter.jensen@example.com", PhoneNumber = "12345678" },
				new Customer { FullName = "Sara Larsen", Email = "sara.larsen@example.com", PhoneNumber = "87654321" },
				new Customer { FullName = "Ahmed Ali", Email = "ahmed.ali@example.com", PhoneNumber = "55556666" },
				new Customer { FullName = "Emily Smith", Email = "emily.smith@example.com", PhoneNumber = "44445555" }
			);
		}

		private static void SeedBookings(AppDbContext context)
		{
			if (context.Bookings.Any())
				return;

			// Load needed data
			var rooms = context.Rooms.AsNoTracking().ToList();
			var customers = context.Customers.AsNoTracking().ToList();

			Room R(int number) => rooms.First(r => r.Number == number);
			Customer C(string email) => customers.First(c => c.Email == email);

			var today = DateTime.Today;

			var bookings = new List<Booking>
			{
				// Historical booking
				new Booking
				{
					CustomerId = C("jannicksørensen@gmail.com").ID,
					StartDate = new DateTime(2023, 12, 24),
					EndDate = new DateTime(2023, 12, 26),
					CheckInStatus = CheckInStatus.CheckedOut,
					TotalPrice = 2 * context.RoomTypes.First(rt => rt.Name == "Enkelt").Price,
					PersonCount = 2,
					Comment = "Looking forward to my stay!",
					Rooms = new List<Room> { context.Rooms.First(x => x.Number == 1) }
				},
				// Current stay (checked in)
				new Booking
				{
					CustomerId = C("peter.jensen@example.com").ID,
					StartDate = today.AddDays(-1),
					EndDate = today.AddDays(2),
					CheckInStatus = CheckInStatus.CheckedIn,
					TotalPrice = 3 * context.RoomTypes.First(rt => rt.Name == "Luksus").Price,
					PersonCount = 2,
					Comment = "Business trip",
					Rooms = new List<Room> { context.Rooms.First(x => x.Number == 6) }
				},
				// Upcoming booking (not checked in)
				new Booking
				{
					CustomerId = C("maria.nielsen@example.com").ID,
					StartDate = today.AddDays(10),
					EndDate = today.AddDays(13),
					CheckInStatus = CheckInStatus.NotCheckedIn,
					TotalPrice = 3 * context.RoomTypes.First(rt => rt.Name == "Suite").Price,
					PersonCount = 3,
					Comment = "Anniversary trip",
					Rooms = new List<Room> { context.Rooms.First(x => x.Number == 8) }
				},
				// Completed recent booking
				new Booking
				{
					CustomerId = C("sara.larsen@example.com").ID,
					StartDate = today.AddDays(-7),
					EndDate = today.AddDays(-5),
					CheckInStatus = CheckInStatus.CheckedOut,
					TotalPrice = 2 * context.RoomTypes.First(rt => rt.Name == "Enkelt").Price,
					PersonCount = 1,
					Comment = "Short city break",
					Rooms = new List<Room> { context.Rooms.First(x => x.Number == 2) }
				},
				// Group booking with two rooms
				new Booking
				{
					CustomerId = C("ahmed.ali@example.com").ID,
					StartDate = today.AddDays(30),
					EndDate = today.AddDays(34),
					CheckInStatus = CheckInStatus.NotCheckedIn,
					TotalPrice = 4 * context.RoomTypes.First(rt => rt.Name == "Dobbelt").Price + 4 * context.RoomTypes.First(rt => rt.Name == "Luksus").Price,
					PersonCount = 5,
					Comment = "Family vacation",
					Rooms = new List<Room> { context.Rooms.First(x => x.Number == 10), context.Rooms.First(x => x.Number == 11) }
				}
			};

			context.Bookings.AddRange(bookings);
		}

		private static void SeedCleaningSchedules(AppDbContext context)
		{
			if (context.CleaningSchedules.Any())
				return;

			var today = DateTime.Today;

			var rooms = context.Rooms.Where(r => new[] { 1, 2, 3, 4, 5, 6, 8, 10 }.Contains(r.Number)).ToList();
			var byNumber = rooms.ToDictionary(r => r.Number, r => r);

			var schedules = new List<CleaningSchedule>
			{
				new CleaningSchedule { RoomId = byNumber[1].ID, CleaningDate = today.AddDays(-1), Cleaned = true },
				new CleaningSchedule { RoomId = byNumber[2].ID, CleaningDate = today,             Cleaned = false },
				new CleaningSchedule { RoomId = byNumber[3].ID, CleaningDate = today.AddDays(1),  Cleaned = false },
				new CleaningSchedule { RoomId = byNumber[4].ID, CleaningDate = today.AddDays(2),  Cleaned = false },
				new CleaningSchedule { RoomId = byNumber[5].ID, CleaningDate = today.AddDays(1),  Cleaned = false },
				new CleaningSchedule { RoomId = byNumber[6].ID, CleaningDate = today.AddDays(3),  Cleaned = false },
				new CleaningSchedule { RoomId = byNumber[8].ID, CleaningDate = today.AddDays(9),  Cleaned = false }, // before reservation
				new CleaningSchedule { RoomId = byNumber[10].ID,CleaningDate = today.AddDays(29), Cleaned = false }  // before group booking
			};

			context.CleaningSchedules.AddRange(schedules);
		}

		private static void SeedRoomTypeImage(AppDbContext context)
		{
			if (context.RoomTypeImages.Any())
				return;

			var images = new List<RoomTypeImage>
			{
				new RoomTypeImage
				{
					RoomTypeId = context.RoomTypes.First(rt => rt.Name == "Enkelt").ID,
					FilePath = "/images/roomtypes/single_1.jpg",
					FileType = "jpg",
					UploadedAt = DateTime.Now
				},
				new RoomTypeImage
				{
					RoomTypeId = context.RoomTypes.First(rt => rt.Name == "Dobbelt").ID,
					FilePath = "/images/roomtypes/double_1.jpg",
					FileType = "jpg",
					UploadedAt = DateTime.Now
				},
				new RoomTypeImage
				{
					RoomTypeId = context.RoomTypes.First(rt => rt.Name == "Luksus").ID,
					FilePath = "/images/roomtypes/luxury_1.jpg",
					FileType = "jpg",
					UploadedAt = DateTime.Now
				},
				new RoomTypeImage
				{
					RoomTypeId = context.RoomTypes.First(rt => rt.Name == "Suite").ID,
					FilePath = "/images/roomtypes/suite_1.jpg",
					FileType = "jpg",
					UploadedAt = DateTime.Now
				}
			};

			context.RoomTypeImages.AddRange(images);
		}
	}
}