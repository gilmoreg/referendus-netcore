namespace referendus_netcore
{
	using System.Collections.Generic;
	
	interface IReferenceData
    {
		IEnumerable<Reference> GetAll();
		Reference Get(string id);
		Reference Add(Reference reference);
		Reference Update(Reference reference);
	}
}
