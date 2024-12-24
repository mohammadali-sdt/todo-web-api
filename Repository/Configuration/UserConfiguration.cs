

using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.HasData(
                new User()
                {
                    Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                    Age = 18,
                    UserName = "foo",
                    Email = "foo@foo.com",
                    Password = "123456789",
                    Name = "MR.Foo",

                },
                new User()
                {
                    Id = new Guid("134103d7-6c6a-4c1f-93e9-eb2f15367ca5"),
                    Age = 28,
                    UserName = "boo",
                    Email = "boo@boo.com",
                    Password = "123456789",
                    Name = "MR.Boo",

                }
            );

        }

    }
}
