using System;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Compilers.Lexers;
using Soedeum.Dotnet.Library.Text;

namespace _TestConsole.Numb
{
    public class NumbLexer : Lexer<NumbToken, NumbType, SimpleReader<char>>
    {
        public string source;

        public NumbLexer(string source, SimpleReader<char> reader)
            : base(reader)
        {
            this.source = source;
        }

        protected override NumbToken CreateEofToken(TextLocation location)
        {
            return new NumbToken(source, new TextSpan(location, "\0"), NumbType.Eof);
        }

        protected override NumbToken CreateToken(NumbType tokenType, TextLocation location, string value)
        {
            return new NumbToken(source, new TextSpan(location, value), tokenType);
        }

        protected override string GetDefaultString(NumbType tokenType)
        {
            switch (tokenType)
            {
                case NumbType.Plus:
                    return "+";
                case NumbType.Minus:
                    return "-";
                case NumbType.Times:
                    return "*";
                case NumbType.DivideBy:
                    return "/";
                case NumbType.PowerOf:
                    return "^";
                case NumbType.OpenParentheses:
                    return "(";
                case NumbType.CloseParentheses:
                    return ")";
                case NumbType.Assign:
                    return ":=";
                case NumbType.Semicolon:
                    return ";";
                default:
                    return null;
            }
        }

        public void Skip()
        {
            while (true)
            {
                char c = reader.Peek();
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
            while (reader.Peek().IsIn(CharSet.Whitespace))
                reader.Read();
        }


        public void SkipComment()
        {
            while (!reader.IsEnd && !reader.Peek().IsIn(CharSet.NewLine))
                reader.Read();
        }

        protected override NumbToken GetNextToken()
        {
            Skip();

            Capture();

            char c = reader.Read();

            switch (c)
            {
                case '+':
                    if (reader.Peek().IsIn(CharSet.Digit))
                    {
                        return GetNumberToken();
                    }
                    else
                        return CreateTokenFromBuffer(NumbType.Plus);
                case '-':
                    if (reader.Peek().IsIn(CharSet.Digit))
                    {
                        return GetNumberToken(0, -1);
                    }
                    else
                        return CreateTokenFromBuffer(NumbType.Minus);
                case '*':
                    return CreateTokenFromBuffer(NumbType.Times);
                case '/':
                    return CreateTokenFromBuffer(NumbType.DivideBy);
                case '^':
                    return CreateTokenFromBuffer(NumbType.PowerOf);
                case ':':
                    if (reader.Peek() == '=')
                    {
                        reader.Read();
                        return CreateTokenFromBuffer(NumbType.Assign);
                    }
                    else
                        return CreateTokenFromBuffer(NumbType.Error);
                case '(':
                    return CreateTokenFromBuffer(NumbType.OpenParentheses);
                case ')':
                    return CreateTokenFromBuffer(NumbType.CloseParentheses);
                case ';':
                    return CreateTokenFromBuffer(NumbType.Semicolon);
                default:
                    if (c.IsIn(CharSet.CStyleIndentifierStart))
                        return GetVariableToken();
                    else if (c.IsIn(CharSet.Digit))
                        return GetNumberToken(c - '0');
                    else
                        return CreateTokenFromBuffer(NumbType.Error);
            }
        }

        private NumbToken GetVariableToken()
        {
            while (Peek().IsIn(CharSet.CStyleIndentifier))
                reader.Read();

            return CreateTokenFromBuffer(NumbType.Variable);
        }

        private NumbToken GetNumberToken(double baseInteger = 0, double signOfValue = 1)
        {
            double powerOfTen = 0;

            char c;

            while (reader.Peek().IsIn(CharSet.Digit))
            {
                c = reader.Read();

                baseInteger *= 10;

                baseInteger += (c - '0');
            }

            if (reader.Peek() == '.')
            {
                reader.Read();

                if (!reader.Peek().IsIn(CharSet.Digit))
                    return CreateTokenFromBuffer(NumbType.Error);

                while (reader.Peek().IsIn(CharSet.Digit))
                {
                    c = reader.Read();

                    baseInteger *= 10;

                    baseInteger += (c - '0');

                    powerOfTen--;
                }
            }

            c = reader.Peek();

            if (c == 'e' || c == 'E')
            {
                reader.Read();

                c = reader.Peek();

                double signOfExponent = 1;
                double exponent = 0;

                if (c == '+')
                    reader.Read();
                else if (c == '-')
                {
                    signOfExponent = -1;
                    reader.Read();
                }
                else if (!c.IsIn(CharSet.Digit))
                    return CreateTokenFromBuffer(NumbType.Error);

                while (reader.Peek().IsIn(CharSet.Digit))
                {
                    c = reader.Read();

                    exponent *= 10;

                    exponent += (c - '0');
                }

                powerOfTen += (signOfExponent * exponent);
            }

            double result = signOfValue * baseInteger * Math.Pow(10, powerOfTen);

            var token = CreateTokenFromBuffer(NumbType.Number);

            token.Value = result;

            return token;
        }
    }
}