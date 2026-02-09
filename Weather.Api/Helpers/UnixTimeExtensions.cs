namespace Weather.Api.Helpers
{
	// UNIX → DateTime (returns UTC)
	public static class UnixTimeExtensions
	{
		public static DateTime ToUtcDateTime(this long unixSeconds)
			=> DateTimeOffset
				.FromUnixTimeSeconds(unixSeconds)
				.UtcDateTime;
	}
}


