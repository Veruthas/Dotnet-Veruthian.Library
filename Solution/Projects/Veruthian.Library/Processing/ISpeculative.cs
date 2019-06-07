namespace Veruthian.Library.Processing
{
    public interface ISpeculative
    {
        bool IsSpeculating { get; }


        void Mark();


        void Commit();


        void Rollback();
    }
}