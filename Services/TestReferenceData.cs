namespace referendus_netcore
{
	using System.Collections.Generic;
	using System.Linq;

	public class TestReferenceData : IReferenceData
	{
		List<Reference> _references;

		public TestReferenceData()
		{
			_references = new List<Reference>();
		}

		public Reference Add(Reference reference)
		{
			reference.Id = _references.Max(r => r.Id) + 1;
			_references.Add(reference);
			return reference;
		}

		public Reference Get(int id)
		{
			return _references.FirstOrDefault(r => r.Id == id);
		}

		public IEnumerable<Reference> GetAll()
		{
			return _references;
		}

		public Reference Update(Reference reference)
		{
			var index = _references.FindIndex(r => r.Id == reference.Id);
			if (index >= 0)
			{
				_references[index] = reference;
				return reference;
			}
			return null;
		}
	}
}
