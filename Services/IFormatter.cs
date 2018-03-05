namespace referendus_netcore
{
	using Newtonsoft.Json.Linq;

	public interface IFormatter
    {
		JObject Format(Reference reference);
    }
}
