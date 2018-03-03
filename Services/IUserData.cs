namespace referendus_netcore
{
	using System.Collections.Generic;

	interface IUserData
    {
		IEnumerable<User> GetAll();
		User Get(string id);
		User Add(User user);
		User Update(User user);
	}
}
