using Bogus;
using ImmoDAL.DAOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ImmoDAL
{
    public static class DbInitializer
    {
        public static void Initialize(ImmoContext context)
        {
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }

            context.Admins.Add(new SuperAdminDAO()
            {
                UserId = Guid.NewGuid(),
                FirstName = "Super",
                LastName = "Admin",
                Mail = "superadmin@grosdev.com",
                Password = "cacatoes",
                PhoneNumber = "000"
            });
            
            context.SaveChanges();

            var pm1 = new PropertyManagerDAO()
            {
                UserId = Guid.NewGuid(),
                LastName = "Duran",
                FirstName = "Samir",
                Mail = "samirduran@big.fr",
                Password = "1234",
                PhoneNumber = "0699887766",
                CompanyName = "Duran Immobilier",
                Siret = "01247895423651",
                IsActive = true,
            };

            var pm2 = new PropertyManagerDAO()
            {
                UserId = Guid.NewGuid(),
                LastName = "Castellanos",
                FirstName = "Ethan",
                Mail = "ethcast@big.fr",
                Password = "1234",
                PhoneNumber = "0699887765",
                CompanyName = "Cast Solutions",
                Siret = "01247895423751",
                IsActive = true,
            };

            var sm1 = new SalesManagerDAO()
            {
                UserId = Guid.NewGuid(),
                LastName = "Abitbol",
                FirstName = "George",
                Mail = "LaClasse@big.fr",
                Password = "1234",
                PhoneNumber = "0699877765",
                CompanyName = "Immo Class",
                Siret = "01247895423451",
                IsActive = true,
            };

            var sm2 = new SalesManagerDAO()
            {
                UserId = Guid.NewGuid(),
                LastName = "Gaïbeul",
                FirstName = "Clark",
                Mail = "ClGb@big.fr",
                Password = "1234",
                PhoneNumber = "0699787765",
                CompanyName = "Gone with the sale",
                Siret = "01397895423751",
                IsActive = true,
            };

            var sm3 = new SalesManagerDAO()
            {
                UserId = Guid.NewGuid(),
                LastName = "Aidan",
                FirstName = "Alenko",
                Mail = "AA@big.fr",
                Password = "1234",
                PhoneNumber = "0645186309",
                CompanyName = "Masse Immobillière",
                Siret = "01247315723751",
                IsActive = true,
            };

            var cl1 = new ClientDAO()
            {
                UserId = Guid.NewGuid(),
                LastName = "Pierre",
                FirstName = "Pierre",
                Mail = "PierrePierre@big.fr",
                Password ="1234",
                PhoneNumber = "0642157893",
                PostalCode = "33170",
            };

            var cl2 = new ClientDAO()
            {
                UserId = Guid.NewGuid(),
                LastName = "Valjean",
                FirstName = "Jean",
                Mail = "Depardieu@big.fr",
                Password = "1234",
                PhoneNumber = "0642158893",
                PostalCode = "75000",
            };

            var cl3 = new ClientDAO()
            {
                UserId = Guid.NewGuid(),
                LastName = "Seagull",
                FirstName = "Steven",
                Mail = "Mouette@big.fr",
                Password = "1234",
                PhoneNumber = "0642157883",
                PostalCode = "29000",
            };

            var cl4 = new ClientDAO()
            {
                UserId = Guid.NewGuid(),
                LastName = "Kane",
                FirstName = "Charles Foster",
                Mail = "Rosebud@big.fr",
                Password = "1234",
                PhoneNumber = "0642157143",
                PostalCode = "92300",
            };

            var cl5 = new ClientDAO()
            {
                UserId = Guid.NewGuid(),
                LastName = "Gorthaur",
                FirstName = "Sauron",
                Mail = "LotR@big.md",
                Password = "1234",
                PhoneNumber = "0666066600",
                PostalCode = "66600",
            };

            context.Users.Add(pm1);
            context.Users.Add(pm2);
            context.Users.Add(sm1);
            context.Users.Add(sm2);
            context.Users.Add(sm3);
            context.Users.Add(cl1);
            context.Users.Add(cl2);
            context.Users.Add(cl3);
            context.Users.Add(cl4);
            context.Users.Add(cl5);

            context.SaveChanges();

            var ppt1 = new PropertyDAO()
            {
                PropertyId = Guid.NewGuid(),
                Title = "Maison en pierre",
                Description = "Belle maison en pierre dans la périphérie de Gradignan avec un jardin.",
                PostalCode = "33170",
                Surface = 150,
                NumberOfRooms = 4,
                DesiredPrice = 600000,
                MinimumPrice =500000,
                Status = PropertyStatus.Not_Reserved,
                ReferenceCode = "PPP-12345",
                ClientSeller = cl1,
                ClientSellerId =cl1.UserId,
                PropertyManager = pm1,
                PropertyManagerId = pm1.UserId,
            };

            var ppt2 = new PropertyDAO()
            {
                PropertyId = Guid.NewGuid(),
                Title = "Maison communale au coeur de Paris",
                Description = "Vieil immeuble en plein centre de Paris. Quartier animé. Levées de barricades à prendre en compte pour le trafic routier",
                PostalCode = "75000",
                Surface = 25,
                NumberOfRooms = 1,
                DesiredPrice = 450000,
                MinimumPrice = 400000,
                Status = PropertyStatus.Not_Reserved,
                ReferenceCode = "RVL-12345",
                ClientSeller = cl2,
                ClientSellerId = cl2.UserId,
                PropertyManager = pm1,
                PropertyManagerId = pm1.UserId,
            };

            var ppt3 = new PropertyDAO()
            {
                PropertyId = Guid.NewGuid(),
                Title = "Phare Breton",
                Description = "Très beau phare breton dans la périphérie de Quimper, idéal pour l'étude des mouettes et chardons",
                PostalCode = "29023",
                Surface = 30,
                NumberOfRooms = 2,
                DesiredPrice = 650000,
                MinimumPrice = 500000,
                Status = PropertyStatus.Not_Reserved,
                ReferenceCode = "PBT-12345",
                ClientSeller = cl3,
                ClientSellerId = cl3.UserId,
                PropertyManager = pm2,
                PropertyManagerId = pm2.UserId,
            };

            var ppt4 = new PropertyDAO()
            {
                PropertyId = Guid.NewGuid(),
                Title = "Xanadu",
                Description = "Vaste domaine palatial rattaché à Levallois-Perret",
                PostalCode = "92300",
                Surface = 300,
                NumberOfRooms = 30,
                DesiredPrice = 2500000,
                MinimumPrice = 125000,
                Status = PropertyStatus.Not_Reserved,
                ReferenceCode = "RBD-12345",
                ClientSeller = cl4,
                ClientSellerId = cl4.UserId,
                PropertyManager = pm2,
                PropertyManagerId = pm2.UserId,
            };

            var ppt5 = new PropertyDAO()
            {
                PropertyId = Guid.NewGuid(),
                Title = "Tour du mal",
                Description = "Très belle tour en obsidienne avec éclairage exterieur automatisé. Pour les amateurs de plaines cendrées, le volcan mitoyen assurera aux futurs acquéreurs un chauffage centralisé des plus pérformants.",
                PostalCode = "66600",
                Surface = 500,
                NumberOfRooms = 50,
                DesiredPrice = 1000000000,
                MinimumPrice = 900000000,
                Status = PropertyStatus.Sold,
                ReferenceCode = "MOR-12345",
                ClientSeller = cl5,
                ClientSellerId = cl5.UserId,
                PropertyManager = pm2,
                PropertyManagerId = pm2.UserId,
            };

            context.Properties.Add(ppt1);
            context.Properties.Add(ppt2);
            context.Properties.Add(ppt3);
            context.Properties.Add(ppt4);
            context.Properties.Add(ppt5);

            context.SaveChanges();


            //var propertyManagerFaker = new Faker<PropertyManagerDAO>()
            //    .RuleFor(t => t.UserId, f => f.Random.Guid())
            //    .RuleFor(t => t.LastName, f => f.Name.LastName())
            //    .RuleFor(t => t.FirstName, f => f.Name.FirstName())
            //    .RuleFor(t => t.Mail, f => f.Internet.Email())
            //    .RuleFor(t => t.Password, f => f.Internet.Password())
            //    .RuleFor(t => t.PhoneNumber, f => f.Phone.PhoneNumber())
            //    .RuleFor(t => t.CompanyName, f => f.Company.CompanyName())
            //    .RuleFor(t => t.Siret, f => string.Join("", f.Random.Digits(14)));

            //var salesManagerFaker = new Faker<SalesManagerDAO>()
            //    .RuleFor(t => t.UserId, f => f.Random.Guid())
            //    .RuleFor(t => t.LastName, f => f.Name.LastName())
            //    .RuleFor(t => t.FirstName, f => f.Name.FirstName())
            //    .RuleFor(t => t.Mail, f => f.Internet.Email())
            //    .RuleFor(t => t.Password, f => f.Internet.Password())
            //    .RuleFor(t => t.PhoneNumber, f => f.Phone.PhoneNumber())
            //    .RuleFor(t => t.CompanyName, f => f.Company.CompanyName())
            //    .RuleFor(t => t.Siret, f => string.Join("", f.Random.Digits(14)));

            //var userFaker = new Faker<ClientDAO>()
            //    .RuleFor(t => t.UserId, f => f.Random.Guid())
            //    .RuleFor(t => t.LastName, f => f.Name.LastName())
            //    .RuleFor(t => t.FirstName, f => f.Name.FirstName())
            //    .RuleFor(t => t.Mail, f => f.Internet.Email())
            //    .RuleFor(t => t.Password, f => f.Internet.Password())
            //    .RuleFor(t => t.PhoneNumber, f => f.Phone.PhoneNumber())
            //    .RuleFor(t => t.PostalCode, f => f.Address.ZipCode());

            //List<PropertyManagerDAO> propertyManagers = propertyManagerFaker.Generate(5);
            //foreach (PropertyManagerDAO proppertyManager in propertyManagers)
            //{
            //    context.Users.Add(proppertyManager);
            //}

            //List<SalesManagerDAO> salesManagers = salesManagerFaker.Generate(5);
            //foreach (SalesManagerDAO salesManager in salesManagers)
            //{
            //    context.Users.Add(salesManager);
            //}

            //List<ClientDAO> clients = userFaker.Generate(5);
            //foreach (ClientDAO client in clients)
            //{
            //    context.Users.Add(client);
            //}


            //var propertyFaker = new Faker<PropertyDAO>()
            //    .RuleFor(t => t.PropertyId, f => f.Random.Guid())
            //    .RuleFor(t => t.Title, f => f.Commerce.ProductName())
            //    .RuleFor(t => t.Description, f => f.Commerce.ProductDescription())
            //    .RuleFor(t => t.PostalCode, f => f.Address.ZipCode())
            //    .RuleFor(t => t.Surface, f => f.Random.Int(0, 20000))
            //    .RuleFor(t => t.NumberOfRooms, f => f.Random.Int(0, 500))
            //    .RuleFor(a => a.DesiredPrice, f => decimal.Parse(f.Commerce.Price()))
            //    .RuleFor(a => a.MinimumPrice, (f, ann) => decimal.Parse(f.Commerce.Price(0, ann.DesiredPrice)))
            //    .RuleFor(a => a.Status, PropertyStatus.Not_Reserved)
            //    .RuleFor(a => a.ReferenceCode,
            //    f => new string(f.Random.Chars('\u0041', '\u005A', 3)) + "-" + new string(f.Random.Chars('\u0030', '\u0039', 5)));

            //List<PropertyDAO> properties = propertyFaker.Generate(5);
            //for (int i = 0; i < properties.Count; i++)
            //{
            //    properties[i].PropertyManagerId = propertyManagers[i].UserId;
            //    properties[i].ClientSellerId = clients[i].UserId;
            //    context.Properties.Add(properties[i]);
            //}


        }
    }
}
