﻿namespace referendus_netcore
{
	using Microsoft.EntityFrameworkCore;
	using System.Collections.Generic;
	using System.Linq;

	public class SqlReferenceData : IReferenceData
    {
		PsqlContext _context;

		public SqlReferenceData(PsqlContext context)
		{
			_context = context;
		}

		public Reference Add(Reference reference)
		{
			_context.References.Add(reference);
			_context.SaveChanges();
			// Entity Framework will autopopulate Id with what was generated by the SQL server
			return reference;
		}

		public Reference Get(int id)
		{
			return _context.References.FirstOrDefault(r => r.Id == id);
		}

		public IEnumerable<Reference> GetAll(string userId)
		{
			return _context.References.Where(r => r.UserId == userId);
		}

		public Reference Update(Reference reference)
		{
			_context.Attach(reference).State = EntityState.Modified;
			_context.SaveChanges();
			return reference;
		}
	}
}
