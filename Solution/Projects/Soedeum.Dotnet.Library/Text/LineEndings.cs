namespace Soedeum.Dotnet.Library.Text
{
    public enum LineEnding
    {
        // \0
        Null = 0,


        // \n
        Lf = 0xA,
        LineFeed = Lf,
        UnixNewLine = Lf,


        // \r
        Cr = 0xD,
        CarriageReturn = Cr,
        MacintoshNewLine = Cr,

        // \r\n        
        CrLf = (Cr << 16) + Lf,
        CarriageReturnLineFeed = CrLf,
        WindowsNewLine = CrLf
    }
}