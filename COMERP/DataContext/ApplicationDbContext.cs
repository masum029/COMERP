using COMERP.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;


namespace COMERP.DataContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> op) : base(op) { }
        public DbSet<Company> Companys { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<CompanyDetails> CompanyDetailss { get; set; }
        public DbSet<ContactFormSubmission> ContactFormSubmissions { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<FooterLink> FooterLinks { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<News> Newss { get; set; }
        public DbSet<PageContent> PageContents { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<SiteSettings> SiteSettingss { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SocialMediaLink> SocialMediaLinks { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<SubAbout> SubAbouts { get; set; }
        public DbSet<AboutTopic> AboutTopics { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
            // About -> SubAbout (One-to-Many)
            builder.Entity<About>()
                .HasMany(a => a.SubAbouts)
                .WithOne(sa => sa.About)
                .HasForeignKey(sa => sa.AboutId)
                .OnDelete(DeleteBehavior.Cascade); 

            // SubAbout -> AboutTopic (One-to-Many)
            builder.Entity<SubAbout>()
                .HasMany(sa => sa.AboutTopics)
                .WithOne(at => at.SubAbout)
                .HasForeignKey(at => at.SubAboutId)
                .OnDelete(DeleteBehavior.Cascade); 

            // About -> Company (Many-to-One)
            builder.Entity<About>()
                .HasOne(a => a.Company)
                .WithMany()
                .HasForeignKey(a => a.CompanyId)
                .OnDelete(DeleteBehavior.SetNull); 
        }
    }
}
