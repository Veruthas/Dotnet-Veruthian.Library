namespace Veruthian.Library.Text
{
    public enum LineEnding
    {
        None,

        Cr,

        Lf,

        CrLf,

        NewLine = Lf,

        LineFeed = Lf,

        CarriageReturn = Cr,

        CarriageReturnLineFeed = CrLf,

        EndOfFile
    }
}