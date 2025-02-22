using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetFirebase.Api.Models.Domain;

namespace NetFirebase.Api.Models.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.Roles)
            .WithMany()
            .UsingEntity<UsuarioRole>();
    }
}

