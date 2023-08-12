using Domain.DTO;

namespace Domain.Abstractions;

public interface IInstrumentsTradeService
{
    Task<HashSet<InstrumentTradeDTO>> GetLastTrades_RegularWay();
    Task<HashSet<InstrumentTradeDTO>> GetLastTrades_RegularWay_Tunned();
    Task<HashSet<InstrumentTradeDTO>> GetLastTrades_RawSQL();
}
