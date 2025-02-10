using Cinema.Data;
using Cinema.Mapping;
using Cinema.Models.DataBaseModels;
using Cinema.Repositories;
using Cinema.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// ϳ��������� �� ���� �����
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CinemaContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// ������ �������� �����
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>() // ������ �������� �����
    .AddEntityFrameworkStores<CinemaContext>();

// ������ ��������� �� UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();

builder.Services.AddAutoMapper(typeof(Program));

// ��������� DatabaseInitializer
builder.Services.AddScoped<DatabaseInitializer>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// ����������� ���� ����� �� ��������� �����
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<CinemaContext>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var databaseInitializer = services.GetRequiredService<DatabaseInitializer>();

    await context.Database.MigrateAsync(); // ����������� ������������ �������

    // ��������� ���, ���� ���� �� �� �������
    string adminRole = "Admin";

    if (!await roleManager.RoleExistsAsync(adminRole))
    {
        await roleManager.CreateAsync(new IdentityRole(adminRole));
    }

    // ��������� ������������ (����� �� ��� ���)
    string adminEmail = "admin@example.com";
    string adminPassword = "Admin123!"; // ������ ����� ������

    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new User { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
        var result = await userManager.CreateAsync(adminUser, adminPassword);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, adminRole);
        }
    }
    else if (!await userManager.IsInRoleAsync(adminUser, adminRole))
    {
        await userManager.AddToRoleAsync(adminUser, adminRole);
    }

    // ����������� ���� ����� (������ DatabaseInitializer)
    await databaseInitializer.InitializeAsync();
}

// ������������ ����������
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// ������������ ��������
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
