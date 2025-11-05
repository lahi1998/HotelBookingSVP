namespace Domain.Entities
{
    public class RoomTypeImage
    {
        public int Id { get; set; }
        public int RoomTypeId { get; set; }
        public RoomType? RoomType { get; set; }

        public required string FilePath { get; set; }
        public required string FileType { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
