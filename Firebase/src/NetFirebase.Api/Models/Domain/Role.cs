namespace NetFirebase.Api.Models.Domain;

public sealed class Role : Enumeration<Role>
{
    public static readonly Role Cliente = new(1, "Cliente");

    public Role(int id, string name) : base(id, name)
    {
    }

    public ICollection<Permiso>? Permisos { get; set; }    
}
