namespace Veruthian.Library.Processing
{
    public interface ISpeculator
    {
        bool IsSpeculating { get; }


        void Mark();


        void Commit();


        void Rollback();
    }
}