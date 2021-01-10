using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Database
{
    public class TraceLogger
    {
        public bool Log(string Message)
        {
          if (!String.IsNullOrEmpty(Message))
          {
                SQLCommands sQLCommands = new SQLCommands("Database");
                bool sekmingai = sQLCommands.Insert(new List<Tuple<string, string>>() { new Tuple<string, string>("Zinute", Message) }, "logs");
                return sekmingai;
          }
            return false;
        }

    }
}
