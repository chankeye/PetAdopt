namespace PetAdopt.DTO.Blog
{
    public class CreateBlog
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public int? AnimalId { get; set; }

        public short ClassId { get; set; }
    }
}