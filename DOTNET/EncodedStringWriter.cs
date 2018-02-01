using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DOTNET
{
    public class EncodedStringWriter : System.IO.StringWriter
    {
        Encoding encoding;

        public EncodedStringWriter(StringBuilder builder, Encoding encoding)
            : base(builder)
        {
            this.encoding = encoding;
        }

        public override Encoding Encoding
        {
            get { return encoding; }
        }
    }
}
