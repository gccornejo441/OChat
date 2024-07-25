namespace OllamaClient.Services.Interfaces
{
	public interface IAssemblyProvider
	{
		/// <summary>
		/// Gets [assembly: AssemblyVersion("1.0.0.0")]
		/// </summary>
		/// <returns>Version of the assembly</returns>
		string GetVersion();

		/// <summary>
		/// Gets the company name from the assembly metadata
		/// </summary>
		/// <returns>Company name</returns>
		string GetCompanyName();

		/// <summary>
		/// Gets the product name from the assembly metadata
		/// </summary>
		/// <returns>Product name</returns>
		string GetProductName();

		/// <summary>
		/// Gets the description from the assembly metadata
		/// </summary>
		/// <returns>Description</returns>
		string GetDescription();

		/// <summary>
		/// Gets the title from the assembly metadata
		/// </summary>
		/// <returns>Title</returns>
		string GetTitle();

		/// <summary>
		/// Returns the version formatted as a string in the format "Major.Minor.Build.Revision" for the assembly.
		/// </summary>
		/// <returns>Formatted version string</returns>
		string GetFormattedVersion();

	

	}
}
