namespace Weather.Models.Enums
{
	public record ServiceResult<T>(T? Data, ServiceErrorType Error = ServiceErrorType.None);
}
