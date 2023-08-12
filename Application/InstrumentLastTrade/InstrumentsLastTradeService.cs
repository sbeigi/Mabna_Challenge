using Domain.Abstractions;
using Infrastructure.DataBaseRelationBase;
using Infrastructure.DataBaseWithNoRelation;
using Domain.DTO;
using Microsoft.EntityFrameworkCore;
using Application.Common.Models;

namespace Application.InstrumentLastTrade;

public class InstrumentsLastTradeService : IInstrumentsTradeService
{
    private readonly NoRelationDBContext _DefinedDataDBContext;
    private readonly RelationBaseDBContext _RegularWayDBContext;

    public InstrumentsLastTradeService(
        NoRelationDBContext definedDataDBContext,
        RelationBaseDBContext regularWayDBContext)
    {
        _DefinedDataDBContext = definedDataDBContext;
        _RegularWayDBContext = regularWayDBContext;
    }

    public async Task<HashSet<InstrumentTradeDTO>> GetLastTrades_RegularWay()
    {
        var result = await Task.Run(() => _RegularWayDBContext.Trades
                                        .AsNoTracking()
                                        .Include(x => x.Instrument)
                                        .GroupBy(x => x.Instrument.Name)
                                        .Select(x => new RegularLastTrades(x.Key, x.Where(T => T.DateEn == x.Max(d => d.DateEn)).SingleOrDefault()))
                                        .ToHashSet());

        if (result is null)
            return new HashSet<InstrumentTradeDTO>();

        return result.Select(x => new InstrumentTradeDTO(x.Name, x.Trades?.DateEn, x.Trades?.Open, x.Trades?.High, x.Trades?.Low, x.Trades?.Close))
                     .ToHashSet();
    }
    public async Task<HashSet<InstrumentTradeDTO>> GetLastTrades_RegularWay_Tunned()
    {
        var maxValues = _RegularWayDBContext.Trades
                                             .AsNoTracking()
                                             .GroupBy(x => x.InstrumentId)
                                             .Select(x => new { Id = x.Key, Trades = x.OrderByDescending(o => o.DateEn).FirstOrDefault() })
                                             .ToHashSet();

        // we Also Can Cache the Instrument Table instead of fetching data per request Using Idistributed cache
        var inst = _RegularWayDBContext.Instruments.AsNoTracking()
                                                   .Where(x => maxValues.Select(s => s.Id).ToHashSet().Contains(x.Id))
                                                   .ToHashSet();

        var result = await Task.Run(() => maxValues.Join(inst,
                                                         x => x.Id,
                                                         y => y.Id,
                                                         (x, y) => new InstrumentTradeDTO(y.Name, x.Trades!.DateEn, x.Trades.Open, x.Trades.High, x.Trades.Low, x.Trades.Close))
                                                   .ToHashSet()); ;

        return result;
    }
    public async Task<HashSet<InstrumentTradeDTO>> GetLastTrades_RawSQL()
    {
        var result = await Task.Run(() => _DefinedDataDBContext.LastTrades.FromSqlRaw(@"select Name
                                                                         ,[DateEn]
                                                                         ,[Open]
                                                                         ,[High]
                                                                         ,[Low]
                                                                         ,[Close]
                                                                   FROM (
                                                                   		SELECT b.Name
                                                                   		      ,[DateEn]
                                                                   		      ,[Open]
                                                                   		      ,[High]
                                                                   		      ,[Low]
                                                                   		      ,[Close]
                                                                   			  ,ROW_NUMBER()over(partition by a.[InstrumentId] order by DateEn desc) AS Row
                                                                   		  FROM [MabnaChallenge].[dbo].[Trade] a
                                                                   		  inner join [dbo].[Instrument] b
                                                                   		  on a.InstrumentId = b.Id ) InstrumentTrade
                                                                   where InstrumentTrade.Row = 1 ")
                                                     .ToHashSet());

        if (result is null)
            return new HashSet<InstrumentTradeDTO>();

        return result.Select(x => new InstrumentTradeDTO(x.Name, x.DateEn, x.Open, x.High, x.Low, x.Close))
                     .ToHashSet();
    }
}

