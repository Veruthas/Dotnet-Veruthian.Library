using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Soedeum.Dotnet.Library.Collections;

namespace Soedeum.Dotnet.Library.Text
{
    public struct TextLine : IEnumerable<TextElement>
    {
        readonly TextPosition position;

        readonly string line;

        readonly LineEnding ending;


        public TextLine(string line, LineEnding ending, bool addEnding = false)
            : this(new TextPosition(), line, ending, addEnding) { }


        public TextLine(TextPosition position, string line, LineEnding ending, bool addEnding = false)
        {
            this.line = addEnding ? line + TextHelper.GetLineEndingAsString(ending) : line;

            this.position = position;

            this.ending = ending;
        }


        public TextLine(string line)
            : this(new TextPosition(), line) { }

        public TextLine(TextPosition position, string line)
        {
            this.line = line;

            this.position = position;

            if (line.Length == 0)
            {
                this.line = "\0";
                this.ending = LineEnding.Null;
            }
            else
            {
                var end = line[line.Length - 1];

                if (end == '\r')
                {
                    this.ending = LineEnding.Cr;
                }
                else if (end == '\n')
                {
                    if (line.Length >= 2)
                        end = line[line.Length - 2];

                    this.ending = (end == '\r') ? LineEnding.CrLf : LineEnding.Lf;
                }
                else
                {
                    this.line += '\0';
                    this.ending = LineEnding.Null;
                }
            }
        }

        public TextLine(IScanner<char> extractFrom, StringBuilder buffer = null)
            : this(extractFrom, new TextPosition(), buffer) { }

        public TextLine(IScanner<char> extractFrom, TextLine previous, StringBuilder buffer = null)
        {
            if (previous.line == "\0")
                this = previous;
            else if (previous.Ending == LineEnding.Null)
                this = new TextLine(previous.position + (previous.LineLength - 1), "\0", LineEnding.Null);
            else
                this = new TextLine(extractFrom, previous.position.IncrementLine(previous.LineLength), buffer);
        }

        public TextLine(IScanner<char> extractFrom, TextPosition start, StringBuilder buffer = null)
        {
            this.position = start;

            if (buffer == null)
                buffer = new StringBuilder();
            else
                buffer.Clear();

            while (true)
            {
                var value = extractFrom.Read();

                buffer.Append(value);

                if (value == '\0')
                {
                    this.ending = LineEnding.Null;
                    break;
                }
                else if (value == '\n')
                {
                    this.ending = LineEnding.Lf;
                    break;
                }
                else if (value == '\r')
                {
                    if (extractFrom.Peek() == '\n')
                    {
                        buffer.Append(extractFrom.Read());
                        this.ending = LineEnding.CrLf;
                    }
                    else
                    {
                        this.ending = LineEnding.Cr;
                    }

                    break;
                }
            }

            this.line = buffer.ToString();
        }


        public TextElement this[int column]
        {
            get
            {
                if (column >= LineLength)
                    throw new ArgumentOutOfRangeException("column");

                return new TextElement(position + column, Line[column]);
            }
        }

        public TextPosition Position { get => position; }

        public string Line { get => line == null ? "\0" : line; }

        public LineEnding Ending { get => ending; }

        public string Text
        {
            get { return Line.Substring(0, Line.Length - TextHelper.GetLineEndingSize(Ending)); }
        }

        public int LineLength
        {
            get { return Line.Length; }
        }

        public int TextLength
        {
            get { return Line.Length - TextHelper.GetLineEndingSize(Ending); }
        }

        public override string ToString()
        {
            return string.Format("{0}; Text: \"{1}\"; LineEnding: {2}", position, Text, TextHelper.GetLineEndingShortName(ending));
        }


        public IEnumerator<TextElement> GetEnumerator()
        {
            string line = Line;

            for (int i = 0; i < line.Length; i++)
            {
                yield return new TextElement(position + i, line[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}