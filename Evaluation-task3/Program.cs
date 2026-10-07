using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Emit;

class Program
{
    static void Main()
    {
        string intervalCsvPath = "C:\\Users\\Adam\\Desktop\\interval.csv";
        string contractCsvPath = "C:\\Users\\Adam\\Desktop\\test.csv";
        string outputCsvPath = "C:\\Users\\Adam\\Desktop\\output.csv";

        var intervals = ReadIntervalsFromCsv(intervalCsvPath);
        var contracts = ReadContractsFromCsv(contractCsvPath);

        DateTime? startReportDate = intervals.Any() ? intervals.Min() : DateTime.UtcNow;
        DateTime? endReportDate = intervals.Any() ? intervals.Max() : DateTime.UtcNow;

        if (startReportDate == null || endReportDate == null)
        {
            throw new Exception("Błąd");
        }

        List<ReportRecord> reportIntervals = GenerateReportIntervals(startReportDate.Value, endReportDate.Value, contracts);

        Console.WriteLine($"POCZĄTEK:\t\t\tKONIEC:");

        foreach (var interval in reportIntervals)
        {
            Console.WriteLine($"{interval.StartDate}\t{interval.EndDate}");
        }

        SaveReportToCsv(reportIntervals, outputCsvPath);
        Console.WriteLine("Raport został zapisany do pliku report.csv");
    }

    static List<DateTime> ReadIntervalsFromCsv(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            return csv.GetRecords<DateTime>().ToList();
        }
    }
    static List<Contract> ReadContractsFromCsv(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            return csv.GetRecords<Contract>().ToList();
        }
    }
    static List<ReportRecord> GenerateReportIntervals(DateTime start, DateTime end, List<Contract> contracts)
    {
        List<ReportRecord> reportIntervals = new List<ReportRecord>();
        DateTime currentIntervalStart = start;

        foreach (var contract in contracts)
        {
            if (contract.StartDate.HasValue && contract.StartDate.Value > currentIntervalStart)
            {
                reportIntervals.Add(new ReportRecord
                {
                    StartDate = currentIntervalStart.ToString("yyyy-MM-ddTHH:mm:ss zzz"),
                    EndDate = contract.StartDate.Value.ToString("yyyy-MM-ddTHH:mm:ss zzz")
                });
            }

            if (contract.EndDate.HasValue)
            {
                currentIntervalStart = contract.EndDate.Value;
            }
        }

        if (currentIntervalStart < end)
        {
            reportIntervals.Add(new ReportRecord
            {
                StartDate = currentIntervalStart.ToString("yyyy-MM-ddTHH:mm:ss zzz"),
                EndDate = end.ToString("yyyy-MM-ddTHH:mm:ss zzz")
            });
        }

        return reportIntervals;
    }
    public static void SaveReportToCsv(List<ReportRecord> reportData, string filePath)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        };

        using (var writer = new StreamWriter(filePath))
        using (var csv = new CsvWriter(writer, config))
        {
            csv.WriteRecords(reportData);
        }
    }
}
class Contract
{
    [TypeConverter(typeof(CustomDateTimeConverter))]
    public DateTime? StartDate { get; set; }

    [TypeConverter(typeof(CustomDateTimeConverter))]
    public DateTime? EndDate { get; set; }
}
public class NullableDateTimeConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (text == "-")
        {
            return null;
        }
        else
        {
            return base.ConvertFromString(text, row, memberMapData);
        }
    }
}
public class CustomDateTimeConverter : ITypeConverter
{
    public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (DateTime.TryParse(text, out DateTime result))
        {
            return result;
        }
        return null;
    }
    public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
        if (value is DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ss zzz");
        }
        return null;
    }
}

class ReportRecord
{
    public string StartDate { get; set; }
    public string EndDate { get; set; }
}
