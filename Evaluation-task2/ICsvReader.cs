using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation_task_2
{
    internal interface ICsvReader
    {
        List<PancakeData> ReadCsvFile(string filePath);
    }
}