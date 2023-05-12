namespace IS.ScaleModelsShop.API.HealthChecks
{
	/// <summary>
	/// IndividualHealthCheckResponse.
	/// </summary>
	public class IndividualHealthCheckResponse
    {
		/// <summary>
		/// Gets the status.
		/// </summary>
		/// <value>
		/// The status.
		/// </value>
		public string Status { get; init; }

		/// <summary>
		/// Gets the component.
		/// </summary>
		/// <value>
		/// The component.
		/// </value>
		public string Component { get; init; }

		/// <summary>
		/// Gets the description.
		/// </summary>
		/// <value>
		/// The description.
		/// </value>
		public string Description { get; init; }
    }
}