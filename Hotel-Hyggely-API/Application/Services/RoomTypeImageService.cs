using Application.Dtos.RoomType;
using Application.Interfaces.Repositories;
using Application.Requests.RoomTypeImage;
using AutoMapper;
using Domain.Entities;
using System.Dynamic;

namespace Application.Services
{
	public class RoomTypeImageService
	{
		private readonly IRoomTypeImageRepo imageRepo;
		private readonly IMapper mapper;
		private readonly string imagePath = Path.Combine("/var", "www", "hotel-hyggely", "wwwroot", "images", "roomtypes");

		public RoomTypeImageService(IRoomTypeImageRepo imageRepo, IMapper mapper)
		{
			this.imageRepo = imageRepo;
			this.mapper = mapper;
		}

		public async Task<IEnumerable<RoomTypeImageDto>> GetByRoomTypeIdAsync(int id)
		{
			var images = await imageRepo.GetByRoomTypeIdAsync(id);

			return mapper.Map<IEnumerable<RoomTypeImageDto>>(images);
		}

		public async Task<RoomTypeImageDto> CreateAsync(CreateImageRequest request)
		{
			if (!Directory.Exists(imagePath))
			{
				Directory.CreateDirectory(imagePath);
			}

			var fileName = $"roomtype_{request.RoomTypeId}_{Guid.NewGuid()}.{request.FileType}";
			var fullPath = Path.Combine(imagePath, fileName);

			await File.WriteAllBytesAsync(fullPath, request.ImgData);

			var image = mapper.Map<RoomTypeImage>(request);
			image.FilePath = $"/images/roomtypes/{fileName}";
			image.UploadedAt = DateTime.UtcNow;

			var createdImage = await imageRepo.CreateAsync(image);

			return mapper.Map<RoomTypeImageDto>(createdImage);
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var image = await imageRepo.GetByIdAsync(id);

			if(image is null)
			{
				return false;
			}

			var fullPath = Path.Combine(imagePath, Path.GetFileName(image.FilePath));

			if (File.Exists(fullPath))
			{
				File.Delete(fullPath);
			}

			await imageRepo.DeleteAsync(id);

			return true;
		}
	}
}
