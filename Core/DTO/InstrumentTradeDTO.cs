namespace Domain.DTO;

public record InstrumentTradeDTO(
    string Name,
    DateTime? DateEn,
    decimal? Open,
    decimal? High,
    decimal? Low,
    decimal? Close
);
