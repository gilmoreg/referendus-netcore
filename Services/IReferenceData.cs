namespace referendus_netcore
{
	using System.Collections.Generic;
	
	public interface IReferenceData
    {
		IEnumerable<Reference> GetAll(string userId);
		Reference Get(int id);
		Reference Add(Reference reference);
		Reference Update(Reference reference);
	}
}
