namespace PetAdopt.DTO.Activity
{
    public class GetActivity
    {
        public string Photo { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public short? AreaId { get; set; }

        public string Area { get; set; }

        public string Address { get; set; }
    }
}