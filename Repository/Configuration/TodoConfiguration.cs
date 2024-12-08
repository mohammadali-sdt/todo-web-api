using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Models;

namespace Repository.Configuration
{
    internal class TodoConfiguration : IEntityTypeConfiguration<Todo>
    {

        public void Configure(EntityTypeBuilder<Todo> builder)
        {
            builder.HasData(

                new Todo()
                {
                    Id = new Guid("6a241878-2e19-4115-982a-984983ad6b96"),
                    Title = "Title1",
                    Description = "Some Description",
                    IsDone = false,
                    UserId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870")
                },
                new Todo()
                {
                    Id = new Guid("a82bdd43-ab9c-48b6-a433-eedc515215ab"),
                    Title = "Title2",
                    Description = "Some Description 2",
                    IsDone = false,
                    UserId = new Guid("134103d7-6c6a-4c1f-93e9-eb2f15367ca5")
                },
                new Todo()
                {
                    Id = new Guid("6ad8dc78-2f22-477d-9665-cedad4d9880a"),
                    Title = "Title3",
                    Description = "Some Description 3",
                    IsDone = true,
                    UserId = new Guid("134103d7-6c6a-4c1f-93e9-eb2f15367ca5")
                }
            );
        }
    }
}
