using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetFirebase.Api.Models.Domain;

namespace NetFirebase.Api.Models.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.Permisos)
            .WithMany()
            .UsingEntity<RolePermiso>();
       
        builder.HasData(Role.GetValues());
    }
}
