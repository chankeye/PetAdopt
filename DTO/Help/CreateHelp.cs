﻿namespace PetAdopt.DTO.Help
{
    public class CreateHelp
    {
        public string Photo { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public short ClassId { get; set; }

        public short AreaId { get; set; }

        public string Address { get; set; }
    }
}