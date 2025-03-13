using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyShop.Entities.Models;

namespace MyShop.DataAccess.Data
{
	public class AppsDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
	{
		public AppsDbContext(DbContextOptions<AppsDbContext> options) : base(options)
		{

		}
		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ShoppingCardVM> ShoppingCards { get; set; }
		public DbSet<OrderDetail> orderDetails { get; set; }
		public DbSet<OrderHeader> orderHeaders { get; set; }



	}
}
