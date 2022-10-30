namespace Expenses.API.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        private static readonly string _format = "{0} with ID {1} not be found";

        public EntityNotFoundException(Type entityType, int entityId)
            : base(string.Format(_format, entityType.Name, entityId))
        { }
    }
}
