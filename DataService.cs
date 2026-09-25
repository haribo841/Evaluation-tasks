using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace Evaluation_task_2
{
    internal class DataService
    {
        private readonly ICsvReader csvReader;
        private readonly PancakeProcessor pancakeProcessor;
        private readonly DataAnalyzer dataAnalyzer;
        public DataService(ICsvReader csvReader, PancakeProcessor pancakeProcessor, DataAnalyzer dataAnalyzer)
        {
            this.csvReader = csvReader;
            this.pancakeProcessor = pancakeProcessor;
            this.dataAnalyzer = dataAnalyzer;
        }
        public List<IngredientUsage>? ProcessCsvFile(string filePath, ICsvReader csvReader)
        {
            switch (csvReader)
            {
                case null:
                    return new List<IngredientUsage>();
                default:
                    {
                        {
                            try
                            {
                                List<PancakeData> data = csvReader.ReadCsvFile(filePath);
                                IEnumerable<IGrouping<DateTime, PancakeData>> groupedData = GroupData(data);
                                List<IngredientUsage> result = pancakeProcessor.ProcessData(groupedData);
                                dataAnalyzer.AnalyzeData(result);
                                return result;
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Cannot process the CSV ({ex.GetType().Name}). Check the path, headers, and values.");
                                return null;
                            }
                        }
                    }
            }
        }
        private IEnumerable<IGrouping<DateTime, PancakeData>> GroupData(List<PancakeData> data)
        {
            IEnumerable<IGrouping<DateTime, PancakeData>> groupedData = data.GroupBy(row =>
            new DateTime(row.TIMESTAMP.Year, row.TIMESTAMP.Month, row.TIMESTAMP.Day, row.TIMESTAMP.Hour, 0, 0));
            return groupedData;
        }
    }
}
