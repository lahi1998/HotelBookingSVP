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

			const string demoHash = "$2a$12$WwwRGOLk51pBPPyaTX.xBOOhKretH42r00uSeBtkLW/Pp567nd/UK";

			context.Staff.AddRange(
				new Staff { UserName = "admin", Password = demoHash, Role = StaffRole.Admin, FullName = "System Administrator" },
				new Staff { UserName = "reception1", Password = demoHash, Role = StaffRole.Receptionist, FullName = "Reception Desk" },
				new Staff { UserName = "cleaner1", Password = demoHash, Role = StaffRole.Cleaning, FullName = "Housekeeping Team" }
			);
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