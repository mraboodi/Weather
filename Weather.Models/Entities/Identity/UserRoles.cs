namespace Weather.Models.Entities.Identity
{
	public static class UserRoles
	{
		public const string SimpleUser = "SimpleUser";				// lowest level, basic access
		public const string SuperUser = "SuperUser";				// mid-level, elevated privileges
		public const string Administrator = "Administrator";        // highest level, owner privileges
		// ... you can add more roles here

		/// <summary>
		/// Returns all role names defined as public constants in this class.
		/// </summary>
		public static IReadOnlyCollection<string> All => 
			typeof(UserRoles)
				.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
				.Where(f => f.IsLiteral && !f.IsInitOnly)
				.Select(f => (string)f.GetRawConstantValue()!)
				.ToArray();
	}
}
