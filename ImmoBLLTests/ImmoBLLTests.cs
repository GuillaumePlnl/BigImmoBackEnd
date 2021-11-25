using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImmoBLL;
using System;
using System.Collections.Generic;
using System.Text;
using ImmoDAL;
using Bogus;
using ImmoBLLTests.MockClasses;
using ImmoBLL.Interfaces;
using ImmoDAL.DAOs;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ImmoBLL.Tests
{
    [TestClass()]
    public class ImmoBLLTests
    {
        private readonly ImmoContext _Db;

        private readonly string _AdminMail = "superadmin@grosdev.com";
        private readonly string _AdminMotDePasse = "cacatoes";

        // Fakers pour générer facilement des objets avec des donnés aléatoires pour les tests
        private readonly Faker<GestionnaireDeBiens> _GestionnairesDeBiensFaker;
        private readonly Faker<GestionnaireDeVentes> _GestionnairesDeVentesFaker;
        private readonly Faker<Client> _ClientsFaker;
        private readonly Faker<Visite> _VisiteFaker;
        private readonly Faker<Annonce> _AnnonceFaker;
        private readonly Faker<Photo> _PhotoFaker;

        /// <summary>
        /// Constructeur pour les tous tests.
        /// Si la BDD existe, elle est effacée. Un compte admin est tousjours ajouté.
        /// </summary>
        public ImmoBLLTests()
        {
            DbContextOptionsBuilder<ImmoContext> optionsBuilder = new DbContextOptionsBuilder<ImmoContext>();
            optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB; Database = ImmoDBTest; Trusted_Connection = True; MultipleActiveResultSets = true");

            this._Db = new ImmoContext(optionsBuilder.Options);
            DbInitializer.Initialize(this._Db);

            //this._Db.Database.EnsureDeleted();
            //this._Db.Database.EnsureCreated();
            //this.AjouterAdminDansBDD();

            _GestionnairesDeBiensFaker = new Faker<GestionnaireDeBiens>()
                .RuleFor(t => t.Id, f => f.Random.Guid())
                .RuleFor(t => t.Nom, f => f.Name.LastName())
                .RuleFor(t => t.Prenom, f => f.Name.FirstName())
                .RuleFor(t => t.Mail, f => f.Internet.Email())
                .RuleFor(t => t.MotDePasse, f => f.Internet.Password())
                .RuleFor(t => t.NumeroDeTelephone, f => f.Phone.PhoneNumber())
                .RuleFor(t => t.RaisonSociale, f => f.Company.CompanyName())
                .RuleFor(t => t.Siret, f => string.Join("", f.Random.Digits(14)));

            _GestionnairesDeVentesFaker = new Faker<GestionnaireDeVentes>()
                .RuleFor(t => t.Id, f => f.Random.Guid())
                .RuleFor(t => t.Nom, f => f.Name.LastName())
                .RuleFor(t => t.Prenom, f => f.Name.FirstName())
                .RuleFor(t => t.Mail, f => f.Internet.Email())
                .RuleFor(t => t.MotDePasse, f => f.Internet.Password())
                .RuleFor(t => t.NumeroDeTelephone, f => f.Phone.PhoneNumber())
                .RuleFor(t => t.RaisonSociale, f => f.Company.CompanyName())
                .RuleFor(t => t.Siret, f => string.Join("", f.Random.Digits(14)));

            _ClientsFaker = new Faker<Client>()
                .RuleFor(t => t.Id, f => f.Random.Guid())
                .RuleFor(t => t.Nom, f => f.Name.LastName())
                .RuleFor(t => t.Prenom, f => f.Name.FirstName())
                .RuleFor(t => t.Mail, f => f.Internet.Email())
                .RuleFor(t => t.MotDePasse, f => f.Internet.Password())
                .RuleFor(t => t.NumeroDeTelephone, f => f.Phone.PhoneNumber())
                .RuleFor(t => t.CodePostal, f => f.Address.ZipCode());

            _VisiteFaker = new Faker<Visite>()
                .RuleFor(t => t.Id, f => f.Random.Guid())
                .RuleFor(t => t.DateVisite, f => f.Date.Future())
                .RuleFor(t => t.Commentaire, f => f.Random.Words());

            _AnnonceFaker = new Faker<Annonce>()
                .RuleFor(t => t.IdAnnonce, f => f.Random.Guid())
                .RuleFor(t => t.Titre, f => f.Commerce.ProductName())
                .RuleFor(t => t.Description, f => f.Commerce.ProductDescription())
                .RuleFor(t => t.CodePostal, f => f.Address.ZipCode())
                .RuleFor(t => t.Surface, f => f.Random.Int(0, 20000))
                .RuleFor(t => t.NombreDePieces, f => f.Random.Int(0, 500))
                .RuleFor(a => a.PrixDesire, f => decimal.Parse(f.Commerce.Price()))
                .RuleFor(a => a.PrixMinimum, (f, ann) => decimal.Parse(f.Commerce.Price(0, ann.PrixDesire)))
                .RuleFor(a => a.Statut, PropertyStatus.Not_Reserved)
                .RuleFor(a => a.ReferenceAnnonce,
                f => new string(f.Random.Chars('\u0041', '\u005A', 3)) + "-" + new string(f.Random.Chars('\u0030', '\u0039', 5)));

            _PhotoFaker = new Faker<Photo>()
                .RuleFor(t => t.IdPhoto, f => f.Random.Guid())
                .RuleFor(t => t.Titre, f => f.Commerce.ProductName())
                .RuleFor(t => t.Description, f => f.Commerce.ProductDescription())
                .RuleFor(t => t.Image, f => f.Random.Bytes(100));

        }

        #region Mèthodes privés
        /// <summary>
        /// Ajoute un utilisateur dans la BDD qui est un superadmin
        /// </summary>
        private void AjouterAdminDansBDD()
        {
            this._Db.Users.Add(new SuperAdminDAO()
            {
                UserId = Guid.NewGuid(),
                FirstName = "Super",
                LastName = "Admin",
                Mail = this._AdminMail,
                Password = this._AdminMotDePasse,
                PhoneNumber = "000"
            });

            this._Db.SaveChanges();
        }

        /// <summary>
        /// Mèthode pour créer une nouveau ImmoBLL et l'utilisateur est un superadmin.
        /// </summary>
        /// <returns>Retourne un ImmoBLL où l'utilisateur connecté est un admin.</returns>
        private IImmoBLL ConnecterCommeAdmin()
        {
            return new ImmoBLL(this._AdminMail, this._AdminMotDePasse, this._Db);
        }

        /// <summary>
        /// Efface tout les changement dans le DbContext. 
        /// A utiliser après un echec de sauvgarde dans un try et catch 
        /// pour éviter que les changements qui étaient impossible sont essayés encore un fois pendant la prochaine sauvgarde.
        /// </summary>
        private void ReculerChangementsNonSauvagardesDansContext()
        {
            this._Db.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);
        }

        #endregion

        #region Utilisateur

        // Irene
        /// <summary>
        /// Test pour verifier qu'un admin peut ajouter des nouveaux utilisateurs.
        /// Test pour verifier qu'il est impossible d'ajouter des comptes avec les mêmes mails ou les mêmes id.
        /// </summary>
        [TestMethod()]
        public async Task AjouterUtilisateurCommeAdminTest()
        {
            IImmoBLL immoAdmin = this.ConnecterCommeAdmin();

            List<Client> clients = this._ClientsFaker.Generate(3);
            List<GestionnaireDeBiens> gestionnairesDeBiens = this._GestionnairesDeBiensFaker.Generate(2);
            List<GestionnaireDeVentes> gestionnairesDeVentes = this._GestionnairesDeVentesFaker.Generate(2);

            // Ajouter des noveaux utilisateurs comme admin
            Exception adminAjouteClient = null;
            try
            {
                await immoAdmin.AjouterUtilisateurAsync(clients[0]);
            }
            catch (Exception e)
            {
                adminAjouteClient = e;
            }

            Exception adminAjoutGestionnaireDeBiens = null;
            try
            {
                await immoAdmin.AjouterUtilisateurAsync(gestionnairesDeBiens[0]);
            }
            catch (Exception e)
            {
                adminAjoutGestionnaireDeBiens = e;
            }

            Exception adminAjoutGestionnaireDeVentes = null;
            try
            {
                await immoAdmin.AjouterUtilisateurAsync(gestionnairesDeVentes[0]);
            }
            catch (Exception e)
            {
                adminAjoutGestionnaireDeVentes = e;
            }

            // Double Id
            clients[1].Id = clients[0].Id;
            Exception doubleId = null;
            try
            {
                await immoAdmin.AjouterUtilisateurAsync(clients[1]);
            }
            catch (Exception e)
            {
                doubleId = e;
            }
            ReculerChangementsNonSauvagardesDansContext();

            // Double Mail
            gestionnairesDeBiens[1].Mail = clients[0].Mail;
            Exception doubleMail = null;
            try
            {
                await immoAdmin.AjouterUtilisateurAsync(gestionnairesDeBiens[1]);
            }
            catch (Exception e)
            {
                doubleMail = e;
            }
            ReculerChangementsNonSauvagardesDansContext();

            // Fausse adresse mail
            //clients[2].Mail = "nomail";
            //Exception fausseMail = null;
            //try
            //{
            //    immoAdmin.AjouterUtilisateur(clients[2]);
            //}
            //catch (Exception e)
            //{
            //    fausseMail = e;
            //}
            //ReculerChangementsNonSauvagardesDansContext();

            Assert.IsNull(adminAjouteClient, "Un client n'est pas ajouté.");
            Assert.IsNull(adminAjoutGestionnaireDeBiens, "Un gestionnaire de biens n'est pas ajouté.");
            Assert.IsNull(adminAjoutGestionnaireDeVentes, "Un gestionnaire de vente n'est pas ajouté.");
            Assert.IsNotNull(doubleId, "Il ne doit pas être possible d'ajouter un utilisateur avec le même id deux fois.");
            Assert.IsNotNull(doubleMail, "Il ne doit pas être possible d'ajouter un nouvel utilisateur avec un mail déjà existant dans la BDD.");
            //Assert.IsNotNull(fausseMail, "Il ne doit pas être possible d'ajouter un nouvel utilisateur avec un mail invalide.");
        }

        // Irene
        /// <summary>
        /// Test pour verifier qu'un gestionnaire de vente peut ajouter un nouveaux clients.
        /// Des gestionnaires ne peuvent pas créer des autres comptes.
        /// </summary>
        [TestMethod()]
        public async Task AjouterUtilisateurCommeGestionnaireTest()
        {
            IImmoBLL immoAdmin = this.ConnecterCommeAdmin();

            List<Client> clients = this._ClientsFaker.Generate(2);
            List<GestionnaireDeBiens> gestionnairesDeBiens = this._GestionnairesDeBiensFaker.Generate(3);
            List<GestionnaireDeVentes> gestionnairesDeVentes = this._GestionnairesDeVentesFaker.Generate(3);

            await immoAdmin.AjouterUtilisateurAsync(gestionnairesDeBiens[0]);
            await immoAdmin.AjouterUtilisateurAsync(gestionnairesDeVentes[0]);

            IImmoBLL immoVente = new ImmoBLL(gestionnairesDeVentes[0].Mail, gestionnairesDeVentes[0].MotDePasse, this._Db);
            IImmoBLL immoBien = new ImmoBLL(gestionnairesDeBiens[0].Mail, gestionnairesDeBiens[0].MotDePasse, this._Db);

            // Gestionnaire de bien ajoute utilisateurs
            Exception gestionnaireBienAjouteClient = null;
            try
            {
                await immoBien.AjouterUtilisateurAsync(clients[0]);
            }
            catch (Exception ex)
            {
                gestionnaireBienAjouteClient = ex;
            }

            List<Exception> gestionnaireBienAjouteGestionnaires = new List<Exception>();
            try
            {
                await immoBien.AjouterUtilisateurAsync(gestionnairesDeBiens[1]);
            }
            catch (Exception ex)
            {
                gestionnaireBienAjouteGestionnaires.Add(ex);
            }
            try
            {
                await immoBien.AjouterUtilisateurAsync(gestionnairesDeVentes[1]);
            }
            catch (Exception ex)
            {
                gestionnaireBienAjouteGestionnaires.Add(ex);
            }

            // Gestionnaire de ventes ajoute utilisateurs
            Exception gestionnaireVenteAjouteClient = null;
            try
            {
                await immoVente.AjouterUtilisateurAsync(clients[1]);
            }
            catch (Exception ex)
            {
                gestionnaireVenteAjouteClient = ex;
            }

            List<Exception> gestionnaireVenteAjouteGestionnaires = new List<Exception>();
            try
            {
                await immoVente.AjouterUtilisateurAsync(gestionnairesDeBiens[2]);
            }
            catch (Exception ex)
            {
                gestionnaireVenteAjouteGestionnaires.Add(ex);
            }
            try
            {
                await immoVente.AjouterUtilisateurAsync(gestionnairesDeVentes[2]);
            }
            catch (Exception ex)
            {
                gestionnaireVenteAjouteGestionnaires.Add(ex);
            }

            Assert.IsNotNull(gestionnaireBienAjouteClient, "Un gestionnaire de biens ne doit pas avoir le droit d'ajouter un nouveau client.");
            Assert.AreEqual(gestionnaireBienAjouteGestionnaires.Count, 2, "Un gestionnaire de biens ne doit pas avoir le droit d'ajouter des nouveaux gestionnaires.");
            Assert.IsNull(gestionnaireVenteAjouteClient, "Un gestionnaire de vente doit avoir le droit d'ajouter un nouveau client.");
            Assert.AreEqual(gestionnaireVenteAjouteGestionnaires.Count, 2, "Un gestionnaire de vente ne doit pas avoir le droit d'ajouter des nouveaux gestionnaires.");
        }

        // Irene
        /// <summary>
        /// Test pour verifier qu'un client ne peut pas ajouter des utilisateurss.
        /// </summary>
        [TestMethod()]
        public async Task AjouterUtilisateurCommeClientTest()
        {
            IImmoBLL immoAdmin = this.ConnecterCommeAdmin();

            List<Client> clients = this._ClientsFaker.Generate(2);
            GestionnaireDeBiens gestionnairesDeBiens = this._GestionnairesDeBiensFaker.Generate();
            GestionnaireDeVentes gestionnairesDeVentes = this._GestionnairesDeVentesFaker.Generate();

            await immoAdmin.AjouterUtilisateurAsync(clients[0]);

            IImmoBLL immoClient = new ImmoBLL(clients[0].Mail, clients[0].MotDePasse, this._Db);

            Exception clientAjoutClient = null;
            try
            {
                await immoClient.AjouterUtilisateurAsync(clients[1]);
            }
            catch (Exception ex)
            {
                clientAjoutClient = ex;
            }

            Exception clientAjoutGestionnaireDeBiens = null;
            try
            {
                await immoClient.AjouterUtilisateurAsync(gestionnairesDeBiens);
            }
            catch (Exception ex)
            {
                clientAjoutGestionnaireDeBiens = ex;
            }

            Exception clientAjoutGestionnaireDeVentes = null;
            try
            {
                await immoClient.AjouterUtilisateurAsync(gestionnairesDeVentes);
            }
            catch (Exception ex)
            {
                clientAjoutGestionnaireDeVentes = ex;
            }

            Assert.IsNotNull(clientAjoutClient, "Un client ne doit pas avoir le droit d'ajouter un nouveau client.");
            Assert.IsNotNull(clientAjoutGestionnaireDeBiens, "Un client ne doit pas avoir le droit d'ajouter un nouveau gestionnaire de biens.");
            Assert.IsNotNull(clientAjoutGestionnaireDeVentes, "Un client ne doit pas avoir le droit d'ajouter un nouveau gestionnaire de vente.");
        }

        // Irene
        // TODO finir le test
        [TestMethod()]
        public async Task ModifierUtilisateur()
        {
            IImmoBLL immoAdmin = ConnecterCommeAdmin();

            List<Client> clients = this._ClientsFaker.Generate(2);
            List<GestionnaireDeBiens> gestionnaireDeBiens = this._GestionnairesDeBiensFaker.Generate(2);
            List<GestionnaireDeVentes> gestionnaireDeVentes = this._GestionnairesDeVentesFaker.Generate(2);

            await immoAdmin.AjouterUtilisateurAsync(clients[0]);
            await immoAdmin.AjouterUtilisateurAsync(gestionnaireDeBiens[0]);
            await immoAdmin.AjouterUtilisateurAsync(gestionnaireDeVentes[0]);

            clients[1].Id = clients[0].Id;
            gestionnaireDeVentes[1].Id = gestionnaireDeVentes[0].Id;
            gestionnaireDeBiens[1].Id = gestionnaireDeBiens[0].Id;

            Exception pasModifie = null;
            try
            {
                await immoAdmin.ModifierUtilisateurAsync(clients[1]);
                await immoAdmin.ModifierUtilisateurAsync(gestionnaireDeBiens[1]);
                await immoAdmin.ModifierUtilisateurAsync(gestionnaireDeVentes[1]);
            }
            catch (Exception ex)
            {
                pasModifie = ex;
            }
            Assert.IsNull(pasModifie, "Un admin doit pouvoir modifer des utilisateurs.");
        }

        /// <summary>
        /// Test pour verifier la propriété InfomartionUtilisateurConnecte
        /// </summary>
        [TestMethod()]
        public async Task InformationsUtilisateurConnecteTest()
        {
            // Arrange
            IImmoBLL immoAdmin = ConnecterCommeAdmin();

            Client client = this._ClientsFaker.Generate();
            GestionnaireDeBiens gestionnaireDeBiens = this._GestionnairesDeBiensFaker.Generate();
            GestionnaireDeVentes gestionnaireDeVentes = this._GestionnairesDeVentesFaker.Generate();

            await immoAdmin.AjouterUtilisateurAsync(client);
            await immoAdmin.AjouterUtilisateurAsync(gestionnaireDeBiens);
            await immoAdmin.AjouterUtilisateurAsync(gestionnaireDeVentes);

            IImmoBLL immoClient = new ImmoBLL(client.Mail, client.MotDePasse, this._Db);
            IImmoBLL immoVente = new ImmoBLL(gestionnaireDeVentes.Mail, gestionnaireDeVentes.MotDePasse, this._Db);
            IImmoBLL immoBien = new ImmoBLL(gestionnaireDeBiens.Mail, gestionnaireDeBiens.MotDePasse, this._Db);

            // Act
            IInfoUtilisateur infoAdmin = immoAdmin.InformationsUtilisateurConnecte;
            IInfoUtilisateur infoClient = immoClient.InformationsUtilisateurConnecte;
            IInfoUtilisateur infoBien = immoBien.InformationsUtilisateurConnecte;
            IInfoUtilisateur infoVente = immoVente.InformationsUtilisateurConnecte;

            // Assert
            Assert.AreEqual(infoAdmin.Type, Classes.TypeUtilisateur.SuperAdmin, "Le type pour le superadmin n'est pas correct.");

            Assert.AreEqual(infoVente.Nom, gestionnaireDeVentes.Nom, "Le nom du gestionnaire n'était pas bien retourné.");
            Assert.AreEqual(infoVente.Prenom, gestionnaireDeVentes.Prenom, "Le prenom du gestionnaire n'était pas bien retourné.");
            Assert.AreEqual(infoVente.Type, Classes.TypeUtilisateur.GestionnaireDeVentes, "Le type pour le gestionnaire de vente n'est pas correct.");

            Assert.AreEqual(infoBien.Nom, gestionnaireDeBiens.Nom, "Le nom du gestionnaire n'était pas bien retourné.");
            Assert.AreEqual(infoBien.Prenom, gestionnaireDeBiens.Prenom, "Le prenom du gestionnaire n'était pas bien retourné.");
            Assert.AreEqual(infoBien.Type, Classes.TypeUtilisateur.GestionnaireDeBiens, "Le type pour le gestionnaire de bien n'est pas correct.");

            Assert.AreEqual(infoClient.Nom, client.Nom, "Le nom du gestionnaire n'était pas bien retourné.");
            Assert.AreEqual(infoClient.Prenom, client.Prenom, "Le prenom du gestionnaire n'était pas bien retourné.");
            Assert.AreEqual(infoClient.Type, Classes.TypeUtilisateur.Client, "Le type pour le client n'est pas correct.");
        }

        [TestMethod()]
        public async Task DemanderContactAsyncTest()
        {
            IImmoBLL immoAdmin = ConnecterCommeAdmin();

            List<Client> clients = this._ClientsFaker.Generate(2);
            List<GestionnaireDeVentes> gestionnaireDeVentes = this._GestionnairesDeVentesFaker.Generate(2);
     
            await immoAdmin.AjouterUtilisateurAsync(clients[0]);
            await immoAdmin.AjouterUtilisateurAsync(clients[1]);
            await immoAdmin.AjouterUtilisateurAsync(gestionnaireDeVentes[0]);
            await immoAdmin.AjouterUtilisateurAsync(gestionnaireDeVentes[1]);

            IImmoBLL immoClient1 = new ImmoBLL(clients[0].Mail, clients[0].MotDePasse, this._Db);
            IImmoBLL immoClient2 = new ImmoBLL(clients[1].Mail, clients[1].MotDePasse, this._Db);

            IInfoUtilisateurContact contact1a = await immoClient1.DemanderContactAsync();
            IInfoUtilisateurContact contact1b = await immoClient1.DemanderContactAsync();
            IInfoUtilisateurContact contact2 = await immoClient2.DemanderContactAsync();

            Assert.AreEqual(contact1a.Id, contact1b.Id);
            Assert.AreNotEqual(contact1a.Id, contact2.Id);
        }

        #endregion

        #region Annonce

        // Guillaume
        [TestMethod()]
        public async Task AjouterAnnonceTest()
        {
            //On se connecte comme admin pour créer les comptes
            IImmoBLL immoAdmin = this.ConnecterCommeAdmin();

            Client clients = this._ClientsFaker.Generate();
            GestionnaireDeBiens gestionnairesDeBiens = this._GestionnairesDeBiensFaker.Generate();
            await immoAdmin.AjouterUtilisateurAsync(gestionnairesDeBiens);
            await immoAdmin.AjouterUtilisateurAsync(clients);
            IImmoBLL immoBiens = new ImmoBLL(gestionnairesDeBiens.Mail, gestionnairesDeBiens.MotDePasse, this._Db);

            List<Annonce> annonces = _AnnonceFaker.Generate(5);

            //annonces[1].ReferenceAnnonce = annonces[0].ReferenceAnnonce;
            Annonce annonceManuelle = new Annonce()
            {
                IdAnnonce = Guid.NewGuid(),
                ReferenceAnnonce = "LHK-55632",
                Titre = "Un chateau en Espagne",
                CodePostal = "33000",
                Description = "Une annonce très spéciale",
                Surface = 634.42M,
                NombreDePieces = 13,
                PrixDesire = 640000M,
                PrixMinimum = 240000M,
                IdClientVendeur = clients.Id,
                Statut = PropertyStatus.Not_Reserved,
            };

            annonceManuelle.IdClientVendeur = clients.Id;
            annonces[1].IdClientVendeur = clients.Id;
            annonces[2].IdClientVendeur = clients.Id;
            annonces[3].IdClientVendeur = clients.Id;
            // TODO Faire Fonctionner TEST
            await immoBiens.AjouterAnnonceAsync(annonceManuelle);
            await immoBiens.AjouterAnnonceAsync(annonces[1]);
            await immoBiens.AjouterAnnonceAsync(annonces[2]);
            await immoBiens.AjouterAnnonceAsync(annonces[3]);

            annonces[3].ReferenceAnnonce = annonces[2].ReferenceAnnonce;
            // On voit si l'erreur en cas de deux références identiques est bien levée
        }

        // Guillaume
        [TestMethod()]
        public async Task ModifierAnnonceTest()
        {
            //Création de plusieurs annonces
            IImmoBLL immoAdmin = this.ConnecterCommeAdmin();

            GestionnaireDeBiens gestionnairesDeBiens = this._GestionnairesDeBiensFaker.Generate();
            await immoAdmin.AjouterUtilisateurAsync(gestionnairesDeBiens);

            Annonce annonce = this._AnnonceFaker.Generate();
            await immoAdmin.AjouterAnnonceAsync(annonce);

            IImmoBLL immoBiens = new ImmoBLL(gestionnairesDeBiens.Mail, gestionnairesDeBiens.MotDePasse, this._Db);

            //Modification d'une annonce
            Annonce annonceModifiee = this._AnnonceFaker.Generate();
            IAnnonce immoBiensModifie = await immoBiens.ModifierAnnonceAsync(annonceModifiee);
        }

        // Guillaume
        [TestMethod()]
        public void ListerAnnoncesTest()
        {
        }

        #endregion

        #region Visite

        //Benoit
        [TestMethod()]
        public async Task AjouterVisiteTest()
        {
            IImmoBLL immoAdmin = this.ConnecterCommeAdmin();

            Client client = this._ClientsFaker.Generate();
            GestionnaireDeVentes gestionnaireDeVentes = this._GestionnairesDeVentesFaker.Generate();
            GestionnaireDeBiens gestionnairesDeBiens = this._GestionnairesDeBiensFaker.Generate();
            await immoAdmin.AjouterUtilisateurAsync(gestionnairesDeBiens);
            await immoAdmin.AjouterUtilisateurAsync(gestionnaireDeVentes);
            await immoAdmin.AjouterUtilisateurAsync(client);

            IImmoBLL immoVente = new ImmoBLL(gestionnaireDeVentes.Mail, gestionnaireDeVentes.MotDePasse, this._Db);
            IImmoBLL immoBiens = new ImmoBLL(gestionnairesDeBiens.Mail, gestionnairesDeBiens.MotDePasse, this._Db);
            IImmoBLL immoClient = new ImmoBLL(client.Mail, client.MotDePasse, this._Db);

            List<Annonce> annonce = _AnnonceFaker.Generate(5);

            annonce[0].IdClientVendeur = client.Id;
            annonce[1].IdClientVendeur = client.Id;
            annonce[2].IdClientVendeur = client.Id;
            annonce[3].IdClientVendeur = client.Id;

            await immoBiens.AjouterAnnonceAsync(annonce[0]);
            await immoBiens.AjouterAnnonceAsync(annonce[1]);
            await immoBiens.AjouterAnnonceAsync(annonce[2]);
            await immoBiens.AjouterAnnonceAsync(annonce[3]);

            List<Visite> visite = this._VisiteFaker.Generate(5);
            visite[0].IdClient = client.Id;
            visite[0].IdAnnonce = annonce[0].IdAnnonce;
            visite[1].IdClient = client.Id;
            visite[1].IdAnnonce = annonce[0].IdAnnonce;
            visite[2].IdClient = client.Id;
            visite[2].IdAnnonce = annonce[2].IdAnnonce;
            visite[3].IdClient = client.Id;
            visite[3].IdAnnonce = annonce[1].IdAnnonce;
            visite[4].IdClient = client.Id;
            visite[4].IdAnnonce = annonce[1].IdAnnonce;
            await immoVente.AjouterVisiteAsync(visite[0]);
            await immoVente.AjouterVisiteAsync(visite[1]);
            await immoVente.AjouterVisiteAsync(visite[2]);
            await immoVente.AjouterVisiteAsync(visite[3]);
            await immoVente.AjouterVisiteAsync(visite[4]);

            Assert.IsNotNull(visite[2].IdClient, "L'idClient n'est pas enregistré dans la BDD");
        }

        //Benoit
        [TestMethod()]
        public async Task ModifierVisiteTest()
        {
            IImmoBLL immoAdmin = this.ConnecterCommeAdmin();

            Client client = this._ClientsFaker.Generate();
            GestionnaireDeVentes gestionnaireDeVentes = this._GestionnairesDeVentesFaker.Generate();
            GestionnaireDeBiens gestionnairesDeBiens = this._GestionnairesDeBiensFaker.Generate();
            await immoAdmin.AjouterUtilisateurAsync(gestionnairesDeBiens);
            await immoAdmin.AjouterUtilisateurAsync(gestionnaireDeVentes);
            await immoAdmin.AjouterUtilisateurAsync(client);

            IImmoBLL immoVente = new ImmoBLL(gestionnaireDeVentes.Mail, gestionnaireDeVentes.MotDePasse, this._Db);
            IImmoBLL immoBiens = new ImmoBLL(gestionnairesDeBiens.Mail, gestionnairesDeBiens.MotDePasse, this._Db);

            List<Annonce> annonce = _AnnonceFaker.Generate(5);

            annonce[0].IdClientVendeur = client.Id;
            annonce[1].IdClientVendeur = client.Id;
            annonce[2].IdClientVendeur = client.Id;
            annonce[3].IdClientVendeur = client.Id;

            await immoBiens.AjouterAnnonceAsync(annonce[0]);
            await immoBiens.AjouterAnnonceAsync(annonce[1]);
            await immoBiens.AjouterAnnonceAsync(annonce[2]);
            await immoBiens.AjouterAnnonceAsync(annonce[3]);

            List<Visite> visite = this._VisiteFaker.Generate(5);
            visite[0].IdClient = client.Id;
            visite[0].IdAnnonce = annonce[0].IdAnnonce;
            visite[1].IdClient = client.Id;
            visite[1].IdAnnonce = annonce[0].IdAnnonce;
            visite[2].IdClient = client.Id;
            visite[2].IdAnnonce = annonce[2].IdAnnonce;
            visite[3].IdClient = client.Id;
            visite[3].IdAnnonce = annonce[1].IdAnnonce;
            visite[4].IdClient = client.Id;
            visite[4].IdAnnonce = annonce[1].IdAnnonce;
            await immoVente.AjouterVisiteAsync(visite[0]);
            await immoVente.AjouterVisiteAsync(visite[1]);
            await immoVente.AjouterVisiteAsync(visite[2]);
            await immoVente.AjouterVisiteAsync(visite[3]);
            await immoVente.AjouterVisiteAsync(visite[4]);
            visite[1].Commentaire = visite[0].Commentaire;
            await immoVente.ModifierVisiteAsync(visite[1]);
            var vis1 = await immoVente.VisualiserVisiteAsync(visite[1].Id);
            Assert.AreEqual(visite[0].Commentaire, vis1.Commentaire, "Le commentaire de l'annonce n'a pas été bien modifié.");
        }


        [TestMethod()]
        public async Task ListerVisitesTest()
        {
            IImmoBLL immoAdmin = this.ConnecterCommeAdmin();

            Client client = this._ClientsFaker.Generate();
            GestionnaireDeVentes gestionnaireDeVentes = this._GestionnairesDeVentesFaker.Generate();
            GestionnaireDeBiens gestionnairesDeBiens = this._GestionnairesDeBiensFaker.Generate();
            await immoAdmin.AjouterUtilisateurAsync(gestionnairesDeBiens);
            await immoAdmin.AjouterUtilisateurAsync(gestionnaireDeVentes);
            await immoAdmin.AjouterUtilisateurAsync(client);

            IImmoBLL immoVente = new ImmoBLL(gestionnaireDeVentes.Mail, gestionnaireDeVentes.MotDePasse, this._Db);
            IImmoBLL immoBiens = new ImmoBLL(gestionnairesDeBiens.Mail, gestionnairesDeBiens.MotDePasse, this._Db);

            List<Annonce> annonce = _AnnonceFaker.Generate(5);

            annonce[0].IdClientVendeur = client.Id;
            annonce[1].IdClientVendeur = client.Id;
            annonce[2].IdClientVendeur = client.Id;
            annonce[3].IdClientVendeur = client.Id;

            await immoBiens.AjouterAnnonceAsync(annonce[0]);
            await immoBiens.AjouterAnnonceAsync(annonce[1]);
            await immoBiens.AjouterAnnonceAsync(annonce[2]);
            await immoBiens.AjouterAnnonceAsync(annonce[3]);

            List<Visite> visite = this._VisiteFaker.Generate(5);
            visite[0].IdClient = client.Id;
            visite[0].IdAnnonce = annonce[0].IdAnnonce;
            visite[1].IdClient = client.Id;
            visite[1].IdAnnonce = annonce[0].IdAnnonce;
            visite[2].IdClient = client.Id;
            visite[2].IdAnnonce = annonce[2].IdAnnonce;
            visite[3].IdClient = client.Id;
            visite[3].IdAnnonce = annonce[1].IdAnnonce;
            visite[4].IdClient = client.Id;
            visite[4].IdAnnonce = annonce[1].IdAnnonce;
            await immoVente.AjouterVisiteAsync(visite[0]);
            await immoVente.AjouterVisiteAsync(visite[1]);
            await immoVente.AjouterVisiteAsync(visite[2]);
            await immoVente.AjouterVisiteAsync(visite[3]);
            await immoVente.AjouterVisiteAsync(visite[4]);
        }


        #endregion

        #region Photo
        [TestMethod()]
        public async Task PhotoTest()
        {
            IImmoBLL immoAdmin = ConnecterCommeAdmin();

            Client client = this._ClientsFaker.Generate();
            GestionnaireDeBiens gestionnaireDeBiens = this._GestionnairesDeBiensFaker.Generate();

            await immoAdmin.AjouterUtilisateurAsync(client);
            await immoAdmin.AjouterUtilisateurAsync(gestionnaireDeBiens);

            IImmoBLL immoBien = new ImmoBLL(gestionnaireDeBiens.Mail, gestionnaireDeBiens.MotDePasse, this._Db);
            IImmoBLL immoClient = new ImmoBLL(client.Mail, client.MotDePasse, this._Db);

            Annonce annonce1 = this._AnnonceFaker.Generate();
            Annonce annonce2 = this._AnnonceFaker.Generate();
            annonce1.IdClientVendeur = client.Id;

            annonce2.IdClientVendeur = client.Id;
            annonce2.Statut = PropertyStatus.Sold;

            List<Photo> photos1 = this._PhotoFaker.Generate(10);
            List<Photo> photos2 = this._PhotoFaker.Generate(10);

            await immoBien.AjouterAnnonceAsync(annonce1);
            await immoBien.AjouterAnnonceAsync(annonce2);

            await immoBien.AjouterPhotosAAnnonceAsync(photos1, annonce1.IdAnnonce);
            await immoBien.AjouterPhotosAAnnonceAsync(photos2, annonce2.IdAnnonce);

            IEnumerable<IInfoItem> infoPhoto1 = await immoBien.ListerPhotosAnnonceAsync(annonce1.IdAnnonce);

            Assert.AreEqual(infoPhoto1.Count(), photos1.Count, "Pas toutes les photos associées à l'annonce étaient trouvées.");

            foreach (IPhoto p in photos1)
            {
                IInfoItem info = infoPhoto1.FirstOrDefault(c => c.Id == p.IdPhoto);
                Assert.IsNotNull(info, "Une photo n'était pas bien sauvgardé.");
                Assert.AreEqual(info.Libelle, p.Titre, "Le titre de la photo ne correspond pas à la titre dans InfoItem.");

                IPhoto photoCharge = await immoBien.VisualiserPhotoAsync(p.IdPhoto);
                Assert.IsNotNull(photoCharge, "Une photo n'était pas bien sauvgardé.");
                Assert.AreEqual(p.Titre, photoCharge.Titre, "Le titre de la photo n'est pas bien saugardé.");
                Assert.AreEqual(p.Description, photoCharge.Description, "La description de la photo n'est pas bien saugardé.");
                Assert.AreEqual(p.Image, photoCharge.Image, "L'image n'est pas bien saugardé.");
            }

            Exception accesClientPhotosAnnonceVendue = null;
            try
            {
                await immoClient.ListerPhotosAnnonceAsync(annonce2.IdAnnonce);
            }
            catch (Exception ex)
            {
                accesClientPhotosAnnonceVendue = ex;
            }

            Assert.IsNotNull(accesClientPhotosAnnonceVendue, "Le client ne doit pas avoir accès aux photos d'une annonce vendue.");
            ReculerChangementsNonSauvagardesDansContext();

            await immoBien.SupprimerPhotoAsync(photos1[0].IdPhoto);

            Assert.AreEqual(photos1.Count - 1, (await immoBien.ListerPhotosAnnonceAsync(annonce1.IdAnnonce)).Count(),
                "Une photo supprimé ne doit plus être associée à une annonce.");

            Exception accesPhotoSpprimee = null;
            try
            {
                await immoBien.VisualiserPhotoAsync(photos1[0].IdPhoto);
            }
            catch (Exception ex)
            {
                accesPhotoSpprimee = ex;
            }

            Assert.IsNotNull(accesPhotoSpprimee, "Une photo supprimée existe encore dans la BDD.");
            ReculerChangementsNonSauvagardesDansContext();
        }

        #endregion

     
    }
}