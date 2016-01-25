using System.Collections.Generic;

namespace PetAdopt.DTO
{
    public class serviceEndpoint
    {
        public List<AnimalInfo> Contents { get; set; }
    }

    public class AnimalInfo
    {
        public string animal_id { get; set; }

        public string animal_subid { get; set; }

        public string animal_area_pkid { get; set; }

        public string animal_kind { get; set; }

        public string animal_bodytype { get; set; }

        public string animal_shelter_pkid { get; set; }

        public string animal_place { get; set; }

        public string animal_age { get; set; }

        public string animal_sterilization { get; set; }

        public string animal_bacterin { get; set; }

        public string animal_foundplace { get; set; }

        public string animal_title { get; set; }

        public string animal_status { get; set; }

        public string animal_remark { get; set; }

        public string animal_caption { get; set; }

        public string animal_sex { get; set; }

        public string animal_opendate { get; set; }

        public string animal_closeddate { get; set; }

        public string animal_colour { get; set; }

        public string animal_update { get; set; }

        public string animal_createtime { get; set; }

        public string shelter_name { get; set; }

        public string album_name { get; set; }

        public string album_file { get; set; }

        public string album_base64 { get; set; }

        public string album_update { get; set; }

        public string cDate { get; set; }
    }
}