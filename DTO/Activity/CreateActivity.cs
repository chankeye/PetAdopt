namespace PetAdopt.DTO.Activity
{
    public class CreateActivity
    {
        public string Photo { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public short? AreaId { get; set; }

        public string Address { get; set; }
    }
}