namespace referendus_netcore
{
	using System.Collections.Generic;
	using System.Linq;

	public class TestUserData : IUserData
	{
		List<User> _users;

		public TestUserData()
		{
			_users = new List<User>();
		}

		public User Add(User user)
		{
			user.Id = _users.Max(u => u.Id) + 1;
			_users.Add(user);
			return user;
		}

		public User Get(string id)
		{
			return _users.Find(u => u.OAuthId == id);
		}

		public IEnumerable<User> GetAll()
		{
			return _users;
		}

		public User Update(User user)
		{
			var index = _users.FindIndex(u => u.Id == user.Id);
			if (index >= 0)
			{
				_users[index] = user;
				return user;
			}
			return null;
		}
	}
}
