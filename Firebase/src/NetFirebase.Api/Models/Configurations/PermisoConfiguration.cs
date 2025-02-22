using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetFirebase.Api.Models.Domain;
using NetFirebase.Api.Models.Enums;

namespace NetFirebase.Api.Models.Configurations;

public class PermisoConfiguration : IEntityTypeConfiguration<Permiso>
{
    public void Configure(EntityTypeBuilder<Permiso> builder)
    {
        builder.ToTable("Permisos");
        builder.HasKey(x => x.Id);

        IEnumerable<Permiso> permisos = Enum.GetValues<PermisoEnum>()
            .Select(x => new Permiso { 
                Id = (int)x, 
                Nombre = x.ToString() });

        builder.HasData(permisos);
    }
}
