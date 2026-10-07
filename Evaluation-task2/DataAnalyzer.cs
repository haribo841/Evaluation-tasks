using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Evaluation_task_2
{
    internal class DataAnalyzer
    {
        public void AnalyzeData(List<IngredientUsage> result)
        {
            SumResult sumResult = new SumResult();
            IEnumerable<IngredientUsage> summedResults = (IEnumerable<IngredientUsage>)sumResult.SumResults(result);
            Console.WriteLine("TIMESTAMP\tFlour [kg]\tGroat [kg]\tMilk [l]\tEgg");
            foreach (var item in summedResults)
            {
                Console.WriteLine(FormattableString.Invariant(
                    $"{item.TIMESTAMP:yyyy-MM-dd HH:mm:ss}\t{item.FLOUR:G}\t{item.GROAT:G}\t{item.MILK:G}\t{item.EGG}"));
            }
        }
    }
}
