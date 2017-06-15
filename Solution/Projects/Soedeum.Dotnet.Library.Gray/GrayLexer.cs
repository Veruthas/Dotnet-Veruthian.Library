using System;
using System.Collections.Generic;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Compilers.Lexers;
using Soedeum.Dotnet.Library.Text;

namespace Soedeum.Dotnet.Library.Gray
{
    public class GrayLexer : Lexer<GrayToken, GrayType, IReader<char>>
    {
        string source;


        public GrayLexer(IEnumerable<char> enumerable, string source)
            : this(enumerable.GetEnumerator(), source) { }

        public GrayLexer(IEnumerator<char> enumerator, string source)
            : base(enumerator.GetSimpleReader())
        {
            this.source = source;
        }


        protected override GrayToken CreateEofToken(TextLocation location)
        {
            return CreateToken(GrayType.Eof, location, "\0");
        }

        protected override GrayToken CreateToken(GrayType type, TextLocation location, string value)
        {
            return new GrayToken(source, new TextSpan(location, value), type);
        }

        protected override string GetDefaultString(GrayType type)
        {
            switch (type)
            {
                case GrayType.IsDefinedAsToken:
                    return ":=";
                case GrayType.ParenthesesStartToken:
                    return "(";
                case GrayType.ParenthesesEndToken:
                    return ")";
                case GrayType.SemicolonToken:
                    return ";";
                case GrayType.CharSetKeyword:
                    return "charset";
                case GrayType.OrKeyword:
                    return "or";
                case GrayType.ToKeyword:
                    return "to";
                case GrayType.NotKeyword:
                    return "not";
                default:
                    return null;
            }
        }

        protected void Skip()
        {

        }


        protected override GrayToken GetNextToken()
        {
            while (true)
            {
                CaptureRead();

                char current = Read();

                switch (current)
                {
                    case '\'':
                        return ScanCharacter();
                    case '(':
                        return CreateTokenFromBuffer(GrayType.ParenthesesStartToken);
                    case ')':
                        return CreateTokenFromBuffer(GrayType.ParenthesesEndToken);
                    case ';':
                        if (PeekIs(';'))
                        {
                            ReleaseRead();
                            return null;
                        }
                        else
                            return CreateTokenFromBuffer(GrayType.SemicolonToken);                        
                    default:
                        return null;
                }
            }
        }

        protected GrayToken ScanCharacter()
        {
            CaptureRead();

            Read();

            return null;
        }
    }
}