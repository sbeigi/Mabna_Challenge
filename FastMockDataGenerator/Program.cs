using EFCore.BulkExtensions;
using FastMockDataGenerator.Database;
using System.Diagnostics;

Console.Write("Welcome to fast mock data generator.\nHow many requests Should be inserted : ");

if (!Int32.TryParse(Console.ReadLine(), out Int32 totalRecords))
    Console.WriteLine("Invalid number");

// Initialization
var _db = new MabnaDBContext();
var elapsedTime = new Stopwatch();
var _rnd = new Random();
var data = new Trade[totalRecords];
var _Instruments = new[] { 1, 2, 5 };


// Procedure
elapsedTime.Start();
var maxId = _db.Trades.Max(m => m.Id) + 1;
Parallel.For(0, totalRecords, index =>
{
    data[index] = new Trade
    {
        Id = index + maxId,
        DateEn = DateTime.Now,
        InstrumentId = _Instruments[_rnd.Next(0, 2)],
        Close = _rnd.Next(1000, 9999),
        Open = _rnd.Next(1000, 9999),
        High = _rnd.Next(1000, 9999),
        Low = _rnd.Next(1000, 9999)
    };
});

var listTime = elapsedTime.ElapsedMilliseconds;

await _db.BulkInsertAsync(data);

elapsedTime.Stop();

// Result
Console.WriteLine($"Consumed time for {totalRecords} mock records is about {listTime} MilliSecond for List generation and {elapsedTime.ElapsedMilliseconds} milli seconds at all.");

