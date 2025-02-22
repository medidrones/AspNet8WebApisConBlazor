using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetFirebase.Api.Models.Domain;

namespace NetFirebase.Api.Models.Configurations;

public class UsuarioRoleConfiguration : IEntityTypeConfiguration<UsuarioRole>
{
    public void Configure(EntityTypeBuilder<UsuarioRole> builder)
    {       
        builder.HasKey(x => new { x.RoleId, x.UsuarioId });       
    }
}
