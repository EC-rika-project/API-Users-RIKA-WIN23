
using API_Users_RIKA_WIN23.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API_Users_RIKA_WIN23.Infrastructure.Context;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<UserEntity>(options)
{
    public DbSet<UserAddressEntity> Addresses { get; set; }
    public DbSet<UserProfileEntity> Profiles { get; set; }
    public DbSet<UserShoppingCartEntity> ShoppingCarts { get; set;}
    public DbSet<UserWishListEntity> WishLists { get; set; }
    public DbSet<ShoppingCartItemEntity> ShoppingCartItems { get; set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);         

        builder.Entity<UserProfileEntity>()
            .HasOne(x => x.User)
            .WithOne(x => x.Profile);

        builder.Entity<UserAddressEntity>()
            .HasOne(x => x.User)
            .WithOne(x => x.Address);

        builder.Entity<UserProfileEntity>()       
            .HasIndex(x => x.Email)
            .IsUnique();

        builder.Entity<UserShoppingCartEntity>()
            .HasOne(x => x.User)
            .WithMany(x => x.ShoppingCarts);

        builder.Entity<UserShoppingCartEntity>()
            .HasMany(x => x.Products)
            .WithOne(x => x.ShoppingCart);

        builder.Entity<UserWishListEntity>()
            .HasOne(x => x.User)
            .WithOne(x => x.WishList);

        builder.Entity<ShoppingCartItemEntity>()
            .HasKey(x => new {x.UserShoppingCartId, x.ProductId});
    }
}
