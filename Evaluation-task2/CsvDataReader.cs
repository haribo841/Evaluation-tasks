using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace Evaluation_task_2
{
    class CsvDataReader : ICsvReader
    {
        public List<PancakeData> ReadCsvFile(string filePath)
        {
            List<PancakeData> records = new();
            using (StreamReader reader = new(filePath))
            using (CsvReader csv = new(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.Context.RegisterClassMap<PancakeDataMap>();
                records = csv.GetRecords<PancakeData>().ToList();
            }
            List<PancakeData> groupedData = GroupAndSumData(records);
            return groupedData;
        }
        private List<PancakeData> GroupAndSumData(List<PancakeData> records)
        {
            List<PancakeData> groupedData = new();

            foreach (PancakeData record in records)
            {
                DateTime roundedTime = RoundDownToHour(record.TIMESTAMP);

                PancakeData? existingRecord = groupedData.FirstOrDefault(item => item.TIMESTAMP == roundedTime);
                switch (existingRecord)
                {
                    case null:
                        groupedData.Add(new PancakeData
                        {
                            TIMESTAMP = roundedTime,
                            FLOUR = record.FLOUR,
                            GROAT = record.GROAT,
                            MILK = record.MILK,
                            EGG = record.EGG
                        });
                        break;
                    default:
                        existingRecord.FLOUR += record.FLOUR;
                        existingRecord.GROAT += record.GROAT;
                        existingRecord.MILK += record.MILK;
                        existingRecord.EGG += record.EGG;
                        break;
                }
            }
            return groupedData;
        }
        static DateTime RoundDownToHour(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
        }
    }
}