using System;
using System.IO;
using System.Collections.Generic;

namespace System.IO.Csv
{
    public class CsvReader : IDisposable
    {
        private TextReader reader;

        private string[] ParseLine(string line)
        {
            return line.Split(new string[] { this.Delimiter },
                StringSplitOptions.None);
        }

        public string Delimiter { get; private set; }
        public string[] Captions { get; private set; }
        public string[] ReadLine()
        {
            string line = this.reader.ReadLine();
            if (line == null) return null;
            return this.ParseLine(line);
        }

        public CsvReader(string fileName, string delimiter, bool hasCaptions)
        {
            this.Delimiter = delimiter;
            reader = File.OpenText(fileName);
            if (hasCaptions)
            {
                string line = this.reader.ReadLine();
                if (line == null) return;
                this.Captions = this.ParseLine(line);
            }
        }

        public void Dispose()
        {
            this.reader.Close();
        }
    }

    public class CsvWriter : IDisposable
    {
        private TextWriter writer;

        public string Delimiter { get; private set; }
        public string[] Captions { get; private set; }

        public void WriteLine(params string[] values)
        {
            if (this.Captions != null && this.Captions.Length != values.Length)
                throw new ArgumentException("Captions and Values length must be equals");
            this.writer.WriteLine(string.Join(this.Delimiter, values));
        }

        public CsvWriter(string fileName, string delimiter)
        {
            this.writer = File.CreateText(fileName);
            this.Delimiter = delimiter;
        }

        public CsvWriter(string fileName, string delimiter, params string[] captions)
        {
            this.writer = File.CreateText(fileName);
            this.Delimiter = delimiter;
            this.Captions = captions;
            this.writer.WriteLine(string.Join(this.Delimiter, this.Captions));
        }

        public void Dispose()
        {
            this.writer.Close();
        }
    }
}
