namespace referendus_netcore
{
    public class ErrorMessages
    {
		public const string InvalidFormat = "Invalid format - must be apa, chicago, or mla";
		public const string NoNameIdentifier = "Invalid authorization - no name identifier claim present";
		public const string InternalServerError = "Something went wrong";
		public const string InvalidPropertyType = "Invalid property type";
	}

	public class ReferenceTypes
	{
		public const string Article = "article";
		public const string Book = "book";
		public const string Website = "website";
	}

	public class Formats
	{
		public const string APA = "apa";
		public const string Chicago = "chicago";
		public const string MLA = "mla";
	}
}
