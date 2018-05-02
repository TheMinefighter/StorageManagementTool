namespace StorageManagementCore.Configuration {
	/// <summary>
	///  Represents all available actions for the event, when a new file or folder has been created
	/// </summary>
	public enum MonitoringAction
	{
		/// <summary>
		///  Just does nothing
		/// </summary>
		Ignore,

		/// <summary>
		///  Asks the user what to do
		/// </summary>
		Ask,

		/// <summary>
		///  Moves the object to the configured location
		/// </summary>
		Move
	}
}