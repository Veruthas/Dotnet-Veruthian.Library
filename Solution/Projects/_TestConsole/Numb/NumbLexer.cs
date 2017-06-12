using System;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Compilers.Lexers;
using Soedeum.Dotnet.Library.Text;

namespace _TestConsole.Numb
{
    public class NumbLexer : Lexer<NumbToken, NumbType, SimpleReader<char>>
    {
        string source;

        public NumbLexer(string source, SimpleReader<char> reader)
            : base(reader)
        {
            this.source = source;
        }

        protected override NumbToken CreateEofToken(TextLocation location)
        {
            return new NumbToken(source, new TextSpan(location, "\0"), NumbType.EofToken);
        }

        protected override NumbToken CreateToken(NumbType type, TextLocation location, string value)
        {
            return new NumbToken(source, new TextSpan(location, value), type);
        }

        protected override string GetDefaultString(NumbType type)
        {
            switch (type)
            {
                case NumbType.PlusToken:
                    return "+";
                case NumbType.MinusToken:
                    return "-";
                case NumbType.TimesToken:
                    return "*";
                case NumbType.DivideByToken:
                    return "/";
                case NumbType.PowerOfToken:
                    return "^";
                case NumbType.OpenParenthesesToken:
                    return "(";
                case NumbType.CloseParenthesesToken:
                    return ")";
                case NumbType.AssignToken:
                    return ":=";
                case NumbType.SemicolonToken:
                    return ";";
                default:
                    return null;
            }
        }

        public void Skip()
        {
            while (true)
            {
                char c = Peek();
                if (c.IsIn(CharSet.Whitespace))
                {
                    reader.Read();
                    SkipWhitespace();
                }
                else if (c == '#')
                {
                    reader.Read();
                    SkipComment();
                }
                else
                    break;
            }
        }

        public void SkipWhitespace()
        {
            while (PeekIsIn(CharSet.Whitespace))
                reader.Read();
        }


        public void SkipComment()
        {
            while (!IsEnd && !PeekIsIn(CharSet.NewLine))
                reader.Read();
        }

        protected override NumbToken GetNextToken()
        {
            Skip();

            CaptureRead();

            char c = reader.Read();

            switch (c)
            {
                case '+':
                    if (PeekIsIn(CharSet.Digit))
                    {
                        return GetNumberToken();
                    }
                    else
                        return CreateTokenFromBuffer(NumbType.PlusToken);
                
                case '-':
                    if (PeekIsIn(CharSet.Digit))
                    {
                        return GetNumberToken(0, -1);
                    }
                    else
                        return CreateTokenFromBuffer(NumbType.MinusToken);
                
                case '*':
                    return CreateTokenFromBuffer(NumbType.TimesToken);
                
                case '/':
                    return CreateTokenFromBuffer(NumbType.DivideByToken);
                
                case '^':
                    return CreateTokenFromBuffer(NumbType.PowerOfToken);
                
                case ':':
                    if (PeekIs('='))
                    {
                        Read();
                        return CreateTokenFromBuffer(NumbType.AssignToken);
                    }
                    else
                        return CreateTokenFromBuffer(NumbType.ErrorToken);
                
                case '(':
                    return CreateTokenFromBuffer(NumbType.OpenParenthesesToken);
                
                case ')':
                    return CreateTokenFromBuffer(NumbType.CloseParenthesesToken);
                
                case ';':
                    return CreateTokenFromBuffer(NumbType.SemicolonToken);

                default:
                    if (c.IsIn(CharSet.CStyleIndentifierStart))
                        return GetVariableToken();

                    else if (c.IsIn(CharSet.Digit))
                        return GetNumberToken(c - '0');

                    else
                        return CreateTokenFromBuffer(NumbType.ErrorToken);
            }
        }

        private NumbToken GetVariableToken()
        {
            while (PeekIsIn(CharSet.CStyleIndentifier))
                Read();

            return CreateTokenFromBuffer(NumbType.VariableToken);
        }

        private NumbToken GetNumberToken(double baseInteger = 0, double signOfValue = 1)
        {
            double powerOfTen = 0;

            char c;

            while (PeekIsIn(CharSet.Digit))
            {
                c = reader.Read();

                baseInteger *= 10;

                baseInteger += (c - '0');
            }

            if (PeekIs('.'))
            {
                Read();

                if (!PeekIsIn(CharSet.Digit))
                    return CreateTokenFromBuffer(NumbType.ErrorToken);

                while (PeekIsIn(CharSet.Digit))
                {
                    c = reader.Read();

                    baseInteger *= 10;

                    baseInteger += (c - '0');

                    powerOfTen--;
                }
            }

            c = Peek();

            if (c == 'e' || c == 'E')
            {
                Read();

                c = Peek();

                double signOfExponent = 1;
                double exponent = 0;

                if (c == '+')
                    Read();
                else if (c == '-')
                {
                    signOfExponent = -1;
                    Read();
                }
                else if (!c.IsIn(CharSet.Digit))
                    return CreateTokenFromBuffer(NumbType.ErrorToken);

                while (PeekIsIn(CharSet.Digit))
                {
                    c = Read();

                    exponent *= 10;

                    exponent += (c - '0');
                }

                powerOfTen += (signOfExponent * exponent);
            }

            double result = signOfValue * baseInteger * Math.Pow(10, powerOfTen);

            var token = CreateTokenFromBuffer(NumbType.NumberToken);

            token.Value = result;

            return token;
        }
    }
}