using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using Evaluation_task_2;

internal sealed class PancakeDataMap : ClassMap<PancakeData>
{
    public PancakeDataMap()
    {
        Map(m => m.TIMESTAMP).Name("TIMESTAMP");
        Map(m => m.FLOUR).Name("FLOUR");
        Map(m => m.GROAT).Name("GROAT");
        Map(m => m.MILK).Name("MILK");
        Map(m => m.EGG).Name("EGG");
    }
}