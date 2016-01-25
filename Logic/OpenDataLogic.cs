using PetAdopt.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetAdopt.Logic
{
    public class OpenDataLogic : _BaseLogic
    {
        public OpenDataLogic(Operation operation) : base(operation) { }

        public void AddOpenDataInfoToDB(int id)
        {
            Client client = new Client();
            var animalList = client.GetAnimalInfo();
            var _animalLogic = new AnimalLogic(new Operation { Display = "system" });

            foreach (var item in animalList)
            {
                _animalLogic.AddAnimal(new PetAdopt.DTO.Animal.CreateAnimal
                {
                    Photo = item.album_file,
                    Title = item.animal_title,
                    Shelters = item.shelter_name,
                    ClassId = 0,
                    StatusId = 0,
                    AreaId = 0,
                    Address = item.animal_place,
                    StartDate = new DateTime()
                });
            }
        }
    }
}