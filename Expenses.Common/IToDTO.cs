namespace Expenses.Common
{
    public interface IToDTO<T> where T : IDTO
    {
        T ToDTO();
    }
}
