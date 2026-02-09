namespace Weather.Models.DTOs.Forcast
{
	public record ForecastResponseDto(IReadOnlyList<ForecastDayDto> Days);
}
