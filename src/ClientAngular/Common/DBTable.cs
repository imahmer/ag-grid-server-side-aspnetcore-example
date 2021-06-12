using System;

namespace ClientAngular.Common
{
    public class DBTable : Attribute
    {
        public string Name { get; set; }
        public string PrimaryKeyColumnCSV { get; set; }
    }
}
