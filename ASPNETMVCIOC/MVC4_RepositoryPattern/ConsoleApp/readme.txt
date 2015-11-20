IoC example
// simple example, can of course be simplified using a ioc wrapper.
public void Button1_Clicked(object source, EventArgs empty)
{
	using (var scope = Program.Autofac.BeginLifetimeScope())
	{
		var uow = scope.Resolve<IUnitOfWork>();
		var repos = scope.Resolve<IUserRepository>();
		var user = repos.GetUser(1);
		user.LastName = txtLastName.Text;
		repos.Save(user);
		uow.Commit();
	}
}

Manual unit of work
public void Button1_Clicked(object source, EventArgs empty)
{
	using (var uow = UnitOfWorkFactory.Create())
	{
		var repos = new UserRepository(uow);
		var user = repos.GetUser(1);
		user.LastName = txtLastName.Text;
		repos.Save(user);
		uow.Commit();
	}
}
