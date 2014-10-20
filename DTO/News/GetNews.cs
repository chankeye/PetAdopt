namespace PetAdopt.DTO.News
{
    public class GetNews
    {
        public string Photo { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public short? AreaId { get; set; }

        public string Area { get; set; }

        public string Url { get; set; }
    }
}