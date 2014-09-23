using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using PetAdopt.Models.Mapping;

namespace PetAdopt.Models
{
    public partial class PetContext : DbContext
    {
        static PetContext()
        {
#if DEBUG
            // 自動建立Database
            Database.SetInitializer<PetContext>(new CreateDatabaseIfNotExists<PetContext>());
#else
            Database.SetInitializer<PetContext>(null);
#endif
        }

        public PetContext()
            : base("Name=PetContext")
        {
        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<Activity_Message_Mapping> Activity_Message_Mapping { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Animal_Message_Mapping> Animal_Message_Mapping { get; set; }
        public DbSet<Animal_Picture_Mapping> Animal_Picture_Mapping { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Ask> Asks { get; set; }
        public DbSet<Ask_Message_Mapping> Ask_Message_Mapping { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Blog_Message_Mapping> Blog_Message_Mapping { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Help> Helps { get; set; }
        public DbSet<Help_Message_Mapping> Help_Message_Mapping { get; set; }
        public DbSet<Knowledge> Knowledges { get; set; }
        public DbSet<Knowledge_Message_Mapping> Knowledge_Message_Mapping { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<News_Message_Mapping> News_Message_Mapping { get; set; }
        public DbSet<OperationInfo> OperationInfoes { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Shelter_Message_Mapping> Shelter_Message_Mapping { get; set; }
        public DbSet<Shelter_Picture_Mapping> Shelter_Picture_Mapping { get; set; }
        public DbSet<Shelter> Shelters { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ActivityMap());
            modelBuilder.Configurations.Add(new Activity_Message_MappingMap());
            modelBuilder.Configurations.Add(new AnimalMap());
            modelBuilder.Configurations.Add(new Animal_Message_MappingMap());
            modelBuilder.Configurations.Add(new Animal_Picture_MappingMap());
            modelBuilder.Configurations.Add(new AreaMap());
            modelBuilder.Configurations.Add(new AskMap());
            modelBuilder.Configurations.Add(new Ask_Message_MappingMap());
            modelBuilder.Configurations.Add(new BlogMap());
            modelBuilder.Configurations.Add(new Blog_Message_MappingMap());
            modelBuilder.Configurations.Add(new ClassMap());
            modelBuilder.Configurations.Add(new HelpMap());
            modelBuilder.Configurations.Add(new Help_Message_MappingMap());
            modelBuilder.Configurations.Add(new KnowledgeMap());
            modelBuilder.Configurations.Add(new Knowledge_Message_MappingMap());
            modelBuilder.Configurations.Add(new MessageMap());
            modelBuilder.Configurations.Add(new NewsMap());
            modelBuilder.Configurations.Add(new News_Message_MappingMap());
            modelBuilder.Configurations.Add(new OperationInfoMap());
            modelBuilder.Configurations.Add(new PictureMap());
            modelBuilder.Configurations.Add(new Shelter_Message_MappingMap());
            modelBuilder.Configurations.Add(new Shelter_Picture_MappingMap());
            modelBuilder.Configurations.Add(new ShelterMap());
            modelBuilder.Configurations.Add(new StatusMap());
            modelBuilder.Configurations.Add(new UserMap());
        }
    }
}
