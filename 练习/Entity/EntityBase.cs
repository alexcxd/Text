namespace Entity
{
    public interface IEntityBase
    {
        bool Delstatus { get; set; }
    }

    public abstract class EntityBase : IEntityBase
    {
        public int Id { get; set; }

        public bool Delstatus { get; set; }
    }
}