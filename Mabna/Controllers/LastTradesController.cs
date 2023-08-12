using Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Mabna.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LastTradesController : ControllerBase
{
    private readonly IInstrumentsTradeService _InstrumentsTradeService;

    public LastTradesController(IInstrumentsTradeService instrumentsTradeService)
    {
        _InstrumentsTradeService = instrumentsTradeService;
    }

    /// <summary>
    /// due to violation of table relations constrain in challenge's data i tried to implement two strategy :
    /// first is based on existance of relationship (foreign key) => GetLastTrades_RegularWay
    /// and the second one is based on no relationship existance => GetLastTrades_RawSQL
    /// </summary>
    /// <returns> Time with No Index Usage</returns>
    [HttpGet]
    public async Task<ActionResult<string>> Get()
    {
        // takes 21.74s for 30,000,000 records and for the first request (do not considering saved execution plan)
        //var result = await _InstrumentsTradeService.GetLastTrades_RegularWay();

        // take 17s for 30,000,000 records and for the first request (do not considering saved execution plan)
        //var result = await _InstrumentsTradeService.GetLastTrades_RegularWay_Tunned();

        // takes 11s for 30,000,000 and for the first request (do not considering saved execution plan)
        var result = await _InstrumentsTradeService.GetLastTrades_RawSQL();

        Log.Information("Requested result => {0}", result);

        return Ok(result);
    }
}