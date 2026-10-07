using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation_task_2
{
    internal class SumResult
    {
        public IEnumerable SumResults(List<IngredientUsage> result)
        {
            IEnumerable<IGrouping<DateTime, IngredientUsage>> groupedResults = result.GroupBy(item => 
            new DateTime(item.TIMESTAMP.Year, item.TIMESTAMP.Month, item.TIMESTAMP.Day, item.TIMESTAMP.Hour, 0, 0));
            List<IngredientUsage> summedResults = groupedResults.Select(group => new IngredientUsage
            {
                TIMESTAMP = group.Key,
                FLOUR = group.Sum(item => item.FLOUR),
                GROAT = group.Sum(item => item.GROAT),
                MILK = group.Sum(item => item.MILK),
                EGG = group.Sum(item => item.EGG)
            }).ToList();
            IOrderedEnumerable<IngredientUsage> sortedResults = summedResults.OrderBy(item => item.TIMESTAMP);
            return sortedResults;
        }
    }
}