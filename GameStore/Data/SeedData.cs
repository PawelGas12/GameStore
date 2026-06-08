using GameStore.Models.DbModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Data
{
    public static class SeedData
    {
        public const string AdminEmail = "admin@gamestore.local";
        public const string AdminPassword = "Admin123!";
        public const string AdminRole = "Admin";

        public static async Task InitializeAsync(IServiceProvider services)
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.Migrate();

            foreach (var role in new[] { AdminRole, "User" })
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var admin = await userManager.FindByEmailAsync(AdminEmail);
            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = AdminEmail,
                    Email = AdminEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, AdminPassword);
                await userManager.AddToRoleAsync(admin, AdminRole);
            }

            if (!context.Games.Any())
            {
                var genre_rpg = new Genre { Name = "RPG" };
                var genre_fps = new Genre { Name = "FPS" };
                var genre_strategia = new Genre { Name = "Strategia" };
                var genre_indie = new Genre { Name = "Indie" };
                var genre_wycigi = new Genre { Name = "Wyścigi" };
                var genre_horror = new Genre { Name = "Horror" };
                var genre_przygodowa = new Genre { Name = "Przygodowa" };
                var genre_sportowa = new Genre { Name = "Sportowa" };
                context.Genres.AddRange(genre_rpg, genre_fps, genre_strategia, genre_indie, genre_wycigi, genre_horror, genre_przygodowa, genre_sportowa);

                var pub_cdprojektred = new Publisher { Name = "CD Projekt RED" };
                var pub_valve = new Publisher { Name = "Valve" };
                var pub_paradoxinteractive = new Publisher { Name = "Paradox Interactive" };
                var pub_devolverdigital = new Publisher { Name = "Devolver Digital" };
                var pub_ubisoft = new Publisher { Name = "Ubisoft" };
                var pub_fromsoftware = new Publisher { Name = "FromSoftware" };
                var pub_electronicarts = new Publisher { Name = "Electronic Arts" };
                context.Publishers.AddRange(pub_cdprojektred, pub_valve, pub_paradoxinteractive, pub_devolverdigital, pub_ubisoft, pub_fromsoftware, pub_electronicarts);
                context.SaveChanges();

                context.Games.AddRange(
                    new Game
                    {
                        Title = "Neon Drifter",
                        Description = "Cyberpunkowe RPG akcji osadzone w neonowym mieście przyszłości. Rozwijaj postać, wybieraj frakcje i decyduj o losach metropolii.",
                        Price = 129.99m,
                        ReleaseDate = new DateTime(2024, 11, 20),
                        CoverImagePath = "/images/covers/neon-drifter.svg",
                        GenreId = genre_rpg.GenreId,
                        PublisherId = pub_cdprojektred.PublisherId
                    },
                    new Game
                    {
                        Title = "Hollow Front",
                        Description = "Dynamiczna strzelanka sieciowa z naciskiem na pracę zespołową i taktyczne starcia na zniszczalnych mapach.",
                        Price = 89.00m,
                        ReleaseDate = new DateTime(2023, 6, 1),
                        CoverImagePath = "/images/covers/hollow-front.svg",
                        GenreId = genre_fps.GenreId,
                        PublisherId = pub_valve.PublisherId
                    },
                    new Game
                    {
                        Title = "Imperia Eternum",
                        Description = "Wielka strategia 4X - buduj imperium, prowadź dyplomację i podbijaj świat na mapie pełnej historycznych wyzwań.",
                        Price = 159.99m,
                        ReleaseDate = new DateTime(2025, 2, 14),
                        CoverImagePath = "/images/covers/imperia-eternum.svg",
                        GenreId = genre_strategia.GenreId,
                        PublisherId = pub_paradoxinteractive.PublisherId
                    },
                    new Game
                    {
                        Title = "Tiny Lantern",
                        Description = "Klimatyczna gra o małej latarni szukającej drogi do domu. Ręcznie malowane plansze i nastrojowa ścieżka dźwiękowa.",
                        Price = 39.99m,
                        ReleaseDate = new DateTime(2024, 3, 9),
                        CoverImagePath = "/images/covers/tiny-lantern.svg",
                        GenreId = genre_indie.GenreId,
                        PublisherId = pub_devolverdigital.PublisherId
                    },
                    new Game
                    {
                        Title = "Crimson Vanguard",
                        Description = "Militarna kampania FPS z rozbudowanym trybem kooperacji i realistyczną balistyką.",
                        Price = 119.00m,
                        ReleaseDate = new DateTime(2025, 4, 18),
                        CoverImagePath = "/images/covers/crimson-vanguard.svg",
                        GenreId = genre_fps.GenreId,
                        PublisherId = pub_ubisoft.PublisherId
                    },
                    new Game
                    {
                        Title = "Starbound Saga",
                        Description = "Rozległe kosmiczne RPG z dziesiątkami planet, frakcji i zakończeń zależnych od decyzji gracza.",
                        Price = 149.99m,
                        ReleaseDate = new DateTime(2025, 5, 30),
                        CoverImagePath = "/images/covers/starbound-saga.svg",
                        GenreId = genre_rpg.GenreId,
                        PublisherId = pub_fromsoftware.PublisherId
                    },
                    new Game
                    {
                        Title = "Velocity X",
                        Description = "Zręcznościowe wyścigi z nitro, ligami online i edytorem własnych aut.",
                        Price = 99.99m,
                        ReleaseDate = new DateTime(2024, 9, 5),
                        CoverImagePath = "/images/covers/velocity-x.svg",
                        GenreId = genre_wycigi.GenreId,
                        PublisherId = pub_electronicarts.PublisherId
                    },
                    new Game
                    {
                        Title = "Whispering Woods",
                        Description = "Survival horror w nawiedzonym lesie. Ograniczone zasoby, gęsta atmosfera i nieprzewidywalny przeciwnik.",
                        Price = 69.99m,
                        ReleaseDate = new DateTime(2023, 10, 28),
                        CoverImagePath = "/images/covers/whispering-woods.svg",
                        GenreId = genre_horror.GenreId,
                        PublisherId = pub_devolverdigital.PublisherId
                    },
                    new Game
                    {
                        Title = "Pixel Kingdoms",
                        Description = "Urocza strategia czasu rzeczywistego w pikselowej oprawie. Zarządzaj królestwem i broń go przed najazdami.",
                        Price = 49.99m,
                        ReleaseDate = new DateTime(2022, 7, 12),
                        CoverImagePath = "/images/covers/pixel-kingdoms.svg",
                        GenreId = genre_strategia.GenreId,
                        PublisherId = pub_paradoxinteractive.PublisherId
                    },
                    new Game
                    {
                        Title = "Aurora Lines",
                        Description = "Relaksacyjna gra logiczna o łączeniu świetlnych szlaków. Setki poziomów i tryb nieskończony.",
                        Price = 24.99m,
                        ReleaseDate = new DateTime(2024, 1, 22),
                        CoverImagePath = "/images/covers/aurora-lines.svg",
                        GenreId = genre_indie.GenreId,
                        PublisherId = pub_devolverdigital.PublisherId
                    },
                    new Game
                    {
                        Title = "Iron Pacto",
                        Description = "Fabularna gra przygodowa o drużynie poszukiwaczy w zapomnianej krainie pełnej zagadek.",
                        Price = 109.99m,
                        ReleaseDate = new DateTime(2025, 3, 3),
                        CoverImagePath = "/images/covers/iron-pacto.svg",
                        GenreId = genre_przygodowa.GenreId,
                        PublisherId = pub_cdprojektred.PublisherId
                    },
                    new Game
                    {
                        Title = "Court Legends",
                        Description = "Arkadowa koszykówka 3v3 z rozbudowaną karierą i meczami rankingowymi online.",
                        Price = 79.99m,
                        ReleaseDate = new DateTime(2024, 12, 10),
                        CoverImagePath = "/images/covers/court-legends.svg",
                        GenreId = genre_sportowa.GenreId,
                        PublisherId = pub_electronicarts.PublisherId
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
