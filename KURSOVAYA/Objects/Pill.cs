using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KURSOVAYA.Objects
{
    public class Pill
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public int Count { get; set; }
        public int StorageCount { get; set; }
        public int Code { get; set; }

        public Pill(string name, string description, string unit, int count, int storageCount, int code) {
            Name = name;
            Description = description;
            Unit = unit;
            Count = count;
            StorageCount = storageCount;
            Code = code;
        }
    }
}
