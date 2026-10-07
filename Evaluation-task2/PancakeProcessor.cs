using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Evaluation_task_2
{
    class PancakeProcessor
    {
        public List<IngredientUsage> ProcessData(IEnumerable<IGrouping<DateTime, PancakeData>> groupedData)
        {
            List<IngredientUsage> result = new();
            foreach (var item in groupedData)
            {
                double flourInKg = item.Sum(r => r.FLOUR / 100.0); // dkg to kg
                double groatInKg = Math.Round(item.Sum(r => r.GROAT / 1000.0), 2); // g to kg
                double milkInL = Math.Round(item.Sum(r => r.MILK / 1000.0), 2); // ml to l
                int eggsCount = item.Sum(r => r.EGG);
                result.Add(new IngredientUsage
                {
                    TIMESTAMP = item.Key,
                    FLOUR = flourInKg,
                    GROAT = groatInKg,
                    MILK = milkInL,
                    EGG = eggsCount
                });
            }
            return result;
        }
    }
}