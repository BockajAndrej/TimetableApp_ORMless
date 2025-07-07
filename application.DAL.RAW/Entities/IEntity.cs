namespace application.DAL.RAW.Entities
{
    public interface IEntity<TDataType>
    {
        public TDataType Id { get; set; }
    }
}