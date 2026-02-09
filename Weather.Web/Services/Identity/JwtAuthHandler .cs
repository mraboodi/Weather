using System.Net.Http.Headers;
using Weather.Web.Interfaces.Identity;

namespace Weather.Web.Services.Identity
{
	public class JwtAuthHandler(IAccessTokenProvider tokenProvider) : DelegatingHandler
	{
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var token = await tokenProvider.GetTokenAsync();
			if (!string.IsNullOrEmpty(token))
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

			return await base.SendAsync(request, cancellationToken);
		}
	}
}
