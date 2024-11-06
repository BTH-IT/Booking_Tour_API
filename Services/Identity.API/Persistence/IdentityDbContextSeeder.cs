using Identity.API.Entites;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using ILogger = Serilog.ILogger;

namespace Identity.API.Persistence
{
	public class IdentityDbContextSeeder
	{
		private readonly IdentityDbContext _context;
		private readonly ILogger _logger;

		public IdentityDbContextSeeder(IdentityDbContext context, ILogger logger)
		{
			this._context = context;
			this._logger = logger;
		}

		public async Task InitialiseAsync()
		{
			try
			{
				_logger.Information("Initializing Db");
				if (_context.Database.IsMySql())
				{
					await _context.Database.MigrateAsync();
					_logger.Information("Initialized Db");
				}
			}
			catch (Exception ex)
			{
				_logger.Error($"Failed to Initialise database: {ex.Message}");
			}
		}

		public async Task IdentityDbSeedAsync()
		{
			_logger.Information("Begin : IdentityDbSeedAsync");

			// Seed Permissions
			if (!await _context.Permissions.AnyAsync())
			{
				await _context.AddRangeAsync(GetPermissions());
				await _context.SaveChangesAsync();
				_logger.Information("Permissions seeded");
			}

			// Seed Roles
			if (!await _context.Roles.AnyAsync())
			{
				await _context.AddRangeAsync(GetRoles());
				await _context.SaveChangesAsync();
				_logger.Information("Roles seeded");
			}

			// Seed Role Details
			if (!await _context.RoleDetails.AnyAsync())
			{
				await _context.AddRangeAsync(GetRoleDetails());
				await _context.SaveChangesAsync();
				_logger.Information("Role Details seeded");
			}

			// Seed Accounts
			if (!await _context.Accounts.AnyAsync())
			{
				var adminRole = await _context.Roles.FirstOrDefaultAsync(c => c.RoleName.Equals("Admin"));
				if (adminRole != null)
				{
					for (int i = 1; i <= 5; i++)
					{
						await _context.Accounts.AddAsync(new Account()
						{
							Email = $"admin{i}@example.com",
							Password = "admin@1234",
							RoleId = adminRole.Id
						});
					}
					await _context.SaveChangesAsync();
					_logger.Information("Admin accounts seeded");
				}
			}

			// Seed Users
			if (!await _context.Users.AnyAsync())
			{
				var accounts = await _context.Accounts.Take(5).ToListAsync();
				foreach (var account in accounts)
				{
					await _context.Users.AddAsync(new User()
					{
						Fullname = $"User {account.Email}",
						BirthDate = GetRandomBirthDate(),
						Country = GetRandomCountry(),
						Phone = GetRandomPhoneNumber(),
						Gender = GetRandomGender(),
						AccountId = account.Id,
					});
				}
				await _context.SaveChangesAsync();
				_logger.Information("Users seeded");
			}

			_logger.Information("End : IdentityDbSeedAsync");
		}

		private List<Role> GetRoles()
		{
			return new List<Role>
			{
				new Role { RoleName = "Admin", Status = true },
				new Role { RoleName = "User", Status = true },
				new Role { RoleName = "Manager", Status = true },
				new Role { RoleName = "Editor", Status = true },
				new Role { RoleName = "Viewer", Status = true },
			};
		}

		private List<Permission> GetPermissions()
		{
			return new List<Permission>
			{
				new Permission { Name = "View",Status = true },
				new Permission { Name = "Create",Status = true },
				new Permission { Name = "Edit" , Status = true },
				new Permission { Name = "Delete" , Status = true },
				new Permission { Name = "Manage" , Status = true },
			};
		}

		private List<RoleDetail> GetRoleDetails()
		{
			var roles = _context.Roles.ToListAsync().Result;
			var permissions = _context.Permissions.ToListAsync().Result;
			var roleDetails = new List<RoleDetail>();
			var random = new Random();

			foreach (var role in roles)
			{
				foreach (var permission in permissions)
				{
					var actionType = random.Next(1, 5) == 1 ? ActionType.Create : (ActionType)random.Next(1, 5);
					roleDetails.Add(new RoleDetail
					{
						RoleId = role.Id,
						PermissionId = permission.Id,
						ActionName = actionType,
						Status = true
					});
				}
			}
			return roleDetails;
		}


		private DateTime GetRandomBirthDate()
		{
			// Generate a random birth date between 1970 and 2005
			Random rand = new Random();
			int year = rand.Next(1970, 2006);
			int month = rand.Next(1, 13);
			int day = rand.Next(1, 29); // To avoid issues with February
			return new DateTime(year, month, day);
		}

		private string GetRandomCountry()
		{
			// Sample countries
			var countries = new List<string>
			{
				"United States",
				"United Kingdom",
				"Canada",
				"Australia",
				"Germany",
				"France",
				"Spain",
				"Italy",
				"Japan",
				"India"
			};
			Random rand = new Random();
			return countries[rand.Next(countries.Count)];
		}

		private string GetRandomPhoneNumber()
		{
			// Generate a random phone number
			Random rand = new Random();
			return $"+1-{rand.Next(100, 1000)}-{rand.Next(1000000, 9999999)}";
		}

		private string GetRandomGender()
		{
			// Randomly choose a gender
			var genders = new List<string> { "Male", "Female", "Other" };
			Random rand = new Random();
			return genders[rand.Next(genders.Count)];
		}
	}
}
