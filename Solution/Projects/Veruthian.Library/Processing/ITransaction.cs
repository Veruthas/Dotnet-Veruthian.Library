namespace Veruthian.Library.Processing
{
    public interface ITransaction
    {
        void Mark();


        void Commit();


        void Rollback();
    }
}