namespace PetAdopt.DTO
{
    public class CreateNews
    {
        public string Poto { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public short? AreaId { get; set; }

        public string Url { get; set; }
    }
}