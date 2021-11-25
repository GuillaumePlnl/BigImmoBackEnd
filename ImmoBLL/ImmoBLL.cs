using ImmoBLL.Classes;
using ImmoBLL.Interfaces;
using ImmoDAL;
using ImmoDAL.DAOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoBLL
{
    public class ImmoBLL : IImmoBLL
    {
        private readonly ImmoContext _Db;
        private IInfoUtilisateur _Utilisateur;

        // Irene
        public IInfoUtilisateur InformationsUtilisateurConnecte
        {
            get
            {
                if (this._Utilisateur == null)
                {
                    throw new ArgumentNullException("Aucun utilisateur n'est connecté.");
                }

                return _Utilisateur;
            }
        }

        // Irene
        public async Task<IInfoUtilisateur> ConnecterAsync(string mail, string motDePasse)
        {
            UserDAO utilisateur = await this._Db.Users.FirstOrDefaultAsync(c => c.Mail == mail && c.Password == motDePasse);

            if (utilisateur == null)
            {
                return null;
            }

            this._Utilisateur = new InfoUtilisateur()
            {
                Id = utilisateur.UserId,
                Nom = utilisateur.LastName,
                Prenom = utilisateur.FirstName,
                Type = GetTypeUserDAO(utilisateur)
            };

            return this._Utilisateur;
        }

        // Irene
        public ImmoBLL(ImmoContext db, IInfoUtilisateur? utilisateur)
        {
            this._Db = db;
            this._Utilisateur = utilisateur;
        }

        public ImmoBLL(string mail, string motDePasse, ImmoContext db)
        {
            this._Db = db;

            UserDAO utilisateur = this._Db.Users.FirstOrDefault(c => c.Mail == mail && c.Password == motDePasse);

            if (utilisateur == null)
            {
                throw new NullReferenceException("L'utilisateur n'existe pas.");
            }

            this._Utilisateur = new InfoUtilisateur()
            {
                Id = utilisateur.UserId,
                Nom = utilisateur.LastName,
                Prenom = utilisateur.FirstName,
                Type = GetTypeUserDAO(utilisateur)
            };
        }

        #region Utilisateur

        // Irene
        async public Task<IUtilisateur> AjouterUtilisateurAsync(IUtilisateur nouvelUtilisateur)
        {
            Helper.RefuserAcces(_Utilisateur, TypeUtilisateur.Client, TypeUtilisateur.GestionnaireDeBiens);

            if (nouvelUtilisateur is SuperAdmin)
            {
                throw new ArgumentException("Impossible d'ajouter un compte superadmin.");
            }

            if (nouvelUtilisateur is IGestionnaire)
            {
                Helper.RefuserAcces(_Utilisateur, TypeUtilisateur.GestionnaireDeVentes, TypeUtilisateur.GestionnaireDeBiens);
            }

            if (await this._Db.Users.FirstOrDefaultAsync(u => u.Mail == nouvelUtilisateur.Mail) != null)
            {
                throw new InvalidOperationException("L'adresse mail est déjà utilisée.");
            }

            if (nouvelUtilisateur is IClient client)
            {
                this._Db.Clients.Add(new ClientDAO()
                {
                    UserId = nouvelUtilisateur.Id,
                    Mail = nouvelUtilisateur.Mail,
                    Password = nouvelUtilisateur.MotDePasse,
                    FirstName = nouvelUtilisateur.Prenom,
                    LastName = nouvelUtilisateur.Nom,
                    PhoneNumber = nouvelUtilisateur.NumeroDeTelephone,
                    PostalCode = client.CodePostal,
                });
            }
            else if (nouvelUtilisateur is IGestionnaireDeVentes gestionnaireDeVentes)
            {
                this._Db.SalesManagers.Add(new SalesManagerDAO()
                {
                    UserId = nouvelUtilisateur.Id,
                    Mail = nouvelUtilisateur.Mail,
                    Password = nouvelUtilisateur.MotDePasse,
                    FirstName = nouvelUtilisateur.Prenom,
                    LastName = nouvelUtilisateur.Nom,
                    PhoneNumber = nouvelUtilisateur.NumeroDeTelephone,
                    CompanyName = gestionnaireDeVentes.RaisonSociale,
                    Siret = gestionnaireDeVentes.Siret,
                });
            }
            else if (nouvelUtilisateur is IGestionnaireDeBiens gestionnaireDeBiens)
            {
                this._Db.PropertyManagers.Add(new PropertyManagerDAO()
                {
                    UserId = nouvelUtilisateur.Id,
                    Mail = nouvelUtilisateur.Mail,
                    Password = nouvelUtilisateur.MotDePasse,
                    FirstName = nouvelUtilisateur.Prenom,
                    LastName = nouvelUtilisateur.Nom,
                    PhoneNumber = nouvelUtilisateur.NumeroDeTelephone,
                    CompanyName = gestionnaireDeBiens.RaisonSociale,
                    Siret = gestionnaireDeBiens.Siret
                });
            }
            else
            {
                throw new ArgumentException("Nouvel utilisateur n'est pas valide.");
            }

            await SauvegarderLaBDDAsync();

            return nouvelUtilisateur;
        }

        // Irene
        async public Task<IUtilisateur> ModifierUtilisateurAsync(IUtilisateur utilisateurModifie)
        {
            Helper.AuthoriserAcces(this._Utilisateur, TypeUtilisateur.SuperAdmin);

            if (utilisateurModifie is ISuperAdmin)
            {
                throw new ArgumentException("Impossible de modifier un superadmin.");
            }

            UserDAO utilisateur = await this._Db.Users.FindAsync(utilisateurModifie.Id);
            if (utilisateur == null)
            {
                throw new ArgumentNullException("L'utilisateur n'existe pas.");
            }

            utilisateur.FirstName = utilisateurModifie.Prenom;
            utilisateur.LastName = utilisateurModifie.Nom;
            utilisateur.PhoneNumber = utilisateurModifie.NumeroDeTelephone;
            utilisateur.Mail = utilisateurModifie.Mail;
            //utilisateur.Password = utilisateurModifie.MotDePasse;

            if (utilisateur is ClientDAO client)
            {
                client.PostalCode = ((IClient)utilisateurModifie).CodePostal;
            }
            else if (utilisateur is ManagerDAO manager)
            {
                manager.CompanyName = ((IGestionnaire)utilisateurModifie).RaisonSociale;
                manager.Siret = ((IGestionnaire)utilisateurModifie).Siret;

                //if (manager.IsActive && !((IGestionnaire)utilisateurModifie).Actif)
                //{
                //    await MarquerGestionnaireInactifAsync(manager.UserId);
                //}

                //manager.IsActive = ((IGestionnaire)utilisateurModifie).Actif;
            }

            await SauvegarderLaBDDAsync();

            return utilisateurModifie;
        }

        // Irene
        async public Task MarquerGestionnaireInactifAsync(Guid idUtilisateur)
        {
            Helper.AuthoriserAcces(this._Utilisateur, TypeUtilisateur.SuperAdmin);

            UserDAO utilisateur = await this._Db.Users.FindAsync(idUtilisateur);

            if (utilisateur == null)
            {
                throw new ArgumentNullException("Le gestionnaire n'existe pas.");
            }

            if (utilisateur is PropertyManagerDAO gestionnaireDeBiens)
            {
                await this._Db.Entry(gestionnaireDeBiens).Collection(m => m.Properties).LoadAsync();

                PropertyManagerDAO remplacant = await this._Db.PropertyManagers.Include(m => m.Properties).OrderBy(m => m.Properties.Count).FirstOrDefaultAsync();

                if (remplacant == null || utilisateur.UserId == remplacant.UserId)
                {
                    throw new InvalidOperationException("Aucun remplacant disponiple.");
                }

                gestionnaireDeBiens.IsActive = false;

                foreach (PropertyDAO a in gestionnaireDeBiens.Properties)
                {
                    remplacant.Properties.Add(a);
                }

                gestionnaireDeBiens.Properties.Clear();

                await SauvegarderLaBDDAsync();
            }
            else if (utilisateur is SalesManagerDAO gestionnaireDeVentes)
            {
                await this._Db.Entry(gestionnaireDeVentes).Collection(m => m.Clients).LoadAsync();
                SalesManagerDAO remplacant = await this._Db.SalesManagers.Include(m => m.Clients).OrderBy(m => m.Clients.Count).FirstOrDefaultAsync();

                if (remplacant == null || utilisateur.UserId == remplacant.UserId)
                {
                    throw new InvalidOperationException("Aucun remplacant disponiple.");
                }

                gestionnaireDeVentes.IsActive = false;

                foreach (ClientDAO c in gestionnaireDeVentes.Clients)
                {
                    remplacant.Clients.Add(c);
                }
                gestionnaireDeVentes.Clients.Clear();

                await SauvegarderLaBDDAsync();
            }
            else
            {
                throw new ArgumentException("Il n'est pas possible de marquer ce type d'utilisateur inactif.");
            }
        }

        public async Task<IEnumerable<IInfoUtilisateur>> ListerClientsAsync()
        {
            Helper.RefuserAcces(this._Utilisateur, TypeUtilisateur.Client);

            IEnumerable<ClientDAO> clients = await this._Db.Clients.ToListAsync();
            return clients.Select(m => new InfoUtilisateur()
            {
                Id = m.UserId,
                Nom = m.LastName,
                Prenom = m.FirstName,
                Type = GetTypeUserDAO(m)
            });
        }

        public async Task<IEnumerable<IInfoUtilisateur>> ListerGestionnairesAsync()
        {
            Helper.RefuserAcces(this._Utilisateur, TypeUtilisateur.Client);

            IEnumerable<ManagerDAO> managers = await this._Db.Managers.ToListAsync();
            return managers.Select(m => new InfoUtilisateur()
            {
                Id = m.UserId,
                Nom = m.LastName,
                Prenom = m.FirstName,
                Type = GetTypeUserDAO(m)
            });
        }

        public async Task<IGestionnaire> VisualiserGestionnaire(Guid id)
        {
            Helper.AuthoriserAcces(this._Utilisateur, TypeUtilisateur.SuperAdmin);

            ManagerDAO managerDAO = await this._Db.Managers.FindAsync(id);
            if (managerDAO == null)
            {
                throw new NullReferenceException("Le gestionnaire n'existe pas");
            }

            return new Gestionnaire()
            {
                Id = managerDAO.UserId,
                MotDePasse = null,
                Mail = managerDAO.Mail,
                Actif = managerDAO.IsActive,
                Nom = managerDAO.LastName,
                Prenom = managerDAO.FirstName,
                NumeroDeTelephone = managerDAO.PhoneNumber,
                RaisonSociale = managerDAO.CompanyName,
                Siret = managerDAO.Siret
            };
        }

        #endregion


        #region Annonce
        // Guillaume
        public async Task<IAnnonce> AjouterAnnonceAsync(IAnnonce annonce)
        {
            Helper.AuthoriserAcces(this._Utilisateur, TypeUtilisateur.GestionnaireDeBiens);

            ClientDAO clientDAO = await this._Db.Clients.FindAsync(annonce.IdClientVendeur);
            if (clientDAO == null)
            {
                throw new ArgumentNullException("Le client n'existe pas");
            }

            //annonce.IdClientVendeur = (await this._Db.Clients.FirstAsync()).UserId;

            PropertyDAO nouvelleAnnonce = new PropertyDAO()
            {
                PropertyId = annonce.IdAnnonce,
                ReferenceCode = annonce.ReferenceAnnonce,
                Title = annonce.Titre,
                Description = annonce.Description,
                PostalCode = annonce.CodePostal,
                Surface = annonce.Surface,
                NumberOfRooms = annonce.NombreDePieces,
                DesiredPrice = annonce.PrixDesire,
                MinimumPrice = annonce.PrixMinimum,
                Status = annonce.Statut,
                ClientSellerId = annonce.IdClientVendeur,
                PropertyManagerId = this._Utilisateur.Id,
            };

            _Db.Properties.Add(nouvelleAnnonce);
            await SauvegarderLaBDDAsync();
            return annonce;
        }

        // Guillaume
        async public Task<IAnnonce> ModifierAnnonceAsync(IAnnonce annonceModifiee)
        {
            Helper.AuthoriserAcces(this._Utilisateur, TypeUtilisateur.GestionnaireDeBiens);

            PropertyDAO annonce = await this._Db.Properties.FindAsync(annonceModifiee.IdAnnonce);
            if (annonce == null)
            {
                throw new ArgumentNullException("L'annonce n'existe pas.");
            }

            if (annonce.PropertyManagerId != _Utilisateur.Id)
            {
                throw new InvalidOperationException("Seul le responsable de l'annonce peut modifier ses annonces.");
            }

            annonce.ReferenceCode = annonceModifiee.ReferenceAnnonce;
            annonce.Title = annonceModifiee.Titre;
            annonce.Description = annonceModifiee.Description;
            annonce.PostalCode = annonceModifiee.CodePostal;
            annonce.Surface = annonceModifiee.Surface;
            annonce.NumberOfRooms = annonceModifiee.NombreDePieces;
            annonce.DesiredPrice = annonceModifiee.PrixDesire;
            annonce.MinimumPrice = annonceModifiee.PrixMinimum;
            annonce.Status = annonceModifiee.Statut;

            await SauvegarderLaBDDAsync();

            return annonceModifiee;
        }

        // Guillaume
        // TODO add access
        public async Task<IAnnonce> VisualiserAnnonceAsync(Guid idAnnonce)
        {
            PropertyDAO annonceDAO = await _Db.Properties.FindAsync(idAnnonce);
            if (annonceDAO == null)
            {
                throw new InvalidOperationException("Annonce inexistante.");
            }

            Annonce annonceSelectionnee = new Annonce()
            {
                CodePostal = annonceDAO.PostalCode,
                Description = annonceDAO.Description,
                PrixDesire = annonceDAO.DesiredPrice,
                PrixMinimum = annonceDAO.MinimumPrice,
                NombreDePieces = annonceDAO.NumberOfRooms,
                Statut = annonceDAO.Status,
                IdAnnonce = annonceDAO.PropertyId,
                IdClientVendeur = annonceDAO.ClientSellerId,
                ReferenceAnnonce = annonceDAO.ReferenceCode,
                Surface = annonceDAO.Surface,
                Titre = annonceDAO.Title,
            };

            return annonceSelectionnee;
        }

        // Guillaume
        async public Task<IEnumerable<IInfoAnnonce>> ListerAnnoncesAsync(string chaineRecherche = null)
        {
            List<PropertyDAO> annoncesDAO;

            if (this._Utilisateur == null || this._Utilisateur.Type == TypeUtilisateur.Client)
            {
                annoncesDAO = await this._Db.Properties.Where(a =>
                    (chaineRecherche == null || a.Title.Contains(chaineRecherche))
                    && a.Status != PropertyStatus.Sold).ToListAsync();
            }
            else if (this._Utilisateur.Type == TypeUtilisateur.GestionnaireDeBiens)
            {
                annoncesDAO = await this._Db.Properties.Where(a =>
                    a.PropertyManagerId == _Utilisateur.Id
                    && (chaineRecherche == null || a.Title.Contains(chaineRecherche))).ToListAsync();
            }
            else
            {
                annoncesDAO = await this._Db.Properties.Where(a => (chaineRecherche == null || a.Title.Contains(chaineRecherche))).ToListAsync();
            }

            List<IInfoAnnonce> listeAnnonces = new List<IInfoAnnonce>();
            foreach (PropertyDAO property in annoncesDAO)
            {
                listeAnnonces.Add(new InfoAnnonce() { Id = property.PropertyId, ReferenceAnnonce = property.ReferenceCode, Titre = property.Title });
            }

            return listeAnnonces;
        }

        // Guillaume
        // TODO access
        async public Task<IEnumerable<IInfoAnnonce>> ListerAnnoncesReserveesAsync()
        {
            Helper.AuthoriserAcces(this._Utilisateur, TypeUtilisateur.GestionnaireDeBiens);

            List<PropertyDAO> annoncesDAO = await this._Db.Properties
                .Where(a => a.PropertyManager.UserId == _Utilisateur.Id && a.Status == PropertyStatus.Reserved).ToListAsync();

            List<IInfoItem> listeAnnoncesReservees = new List<IInfoItem>();
            foreach (PropertyDAO b in annoncesDAO)
            {
                listeAnnoncesReservees.Add(new InfoItem() { Id = b.PropertyId, Description = b.Description, Libelle = b.ReferenceCode });
            }

            return (IEnumerable<IInfoAnnonce>)listeAnnoncesReservees;
        }

        // Guillaume
        async public Task MarquerAnnonceVenduAsync(Guid idAnnonce)
        {
            Helper.AuthoriserAcces(this._Utilisateur, TypeUtilisateur.GestionnaireDeBiens);

            PropertyDAO annonceVendue = await this._Db.Properties.FindAsync(idAnnonce);
            annonceVendue.Status = PropertyStatus.Sold;

            await SauvegarderLaBDDAsync();
        }

        #endregion


        #region Visite

        //Benoit
        async public Task<IVisite> AjouterVisiteAsync(IVisite nouvelleVisite)
        {
            Helper.AuthoriserAcces(this._Utilisateur, TypeUtilisateur.GestionnaireDeVentes);

            PropertyDAO annonce = await this._Db.Properties.FindAsync(nouvelleVisite.IdAnnonce);
            if (annonce == null)
            {
                throw new InvalidOperationException("L'annonce selectionnée n'est pas valide");
            }

            ClientDAO clientAcheteur = await this._Db.Clients.FindAsync(nouvelleVisite.IdClient);
            if (clientAcheteur == null)
            {
                throw new InvalidOperationException("Le client selectionné n'est pas valide");
            }

            if (nouvelleVisite.Offre >= annonce.MinimumPrice)
            {
                annonce.Status = PropertyStatus.Reserved;
            }

            this._Db.Visits.Add(new VisitDAO()
            {
                VisitId = nouvelleVisite.Id,
                VisitDate = nouvelleVisite.DateVisite,
                Comment = nouvelleVisite.Commentaire,
                Offer = nouvelleVisite.Offre,
                ClientId = nouvelleVisite.IdClient,
                PropertyId = nouvelleVisite.IdAnnonce,
                SalesManagerId = this._Utilisateur.Id,
            });

            await SauvegarderLaBDDAsync();

            return nouvelleVisite;
        }

        //Benoit
        async public Task<IVisite> ModifierVisiteAsync(IVisite visiteModifiee)
        {
            Helper.AuthoriserAcces(this._Utilisateur, TypeUtilisateur.GestionnaireDeBiens);

            VisitDAO visite = await this._Db.Visits.FindAsync(visiteModifiee.Id);
            if (visite == null)
            {
                throw new InvalidOperationException("La visite doit exister pour être modifiée");
            }

            if (this._Utilisateur.Id != visite.SalesManagerId)
            {
                throw new InvalidOperationException("Seul le gestionnaire de vente associé à la visite peut la modifier");
            }

            if (visiteModifiee.IdClient != visite.ClientId)
            {
                throw new InvalidOperationException("Le client associé à la visite ne peut pas être changé");
            }

            visite.VisitDate = visiteModifiee.DateVisite;
            visite.Comment = visiteModifiee.Commentaire;
            visite.Offer = visiteModifiee.Offre;

            PropertyDAO annonce = await this._Db.Properties.FindAsync(visiteModifiee.IdAnnonce);

            if (visiteModifiee.Offre >= annonce.MinimumPrice)
            {
                annonce.Status = PropertyStatus.Reserved;
            }

            await SauvegarderLaBDDAsync();

            return visiteModifiee;
        }

        //Benoit
        async public Task<IInfoVisitesDetaillees> VisualiserVisiteAsync(Guid idVisite)
        {
            Helper.RefuserAcces(this._Utilisateur, TypeUtilisateur.GestionnaireDeBiens, TypeUtilisateur.SuperAdmin);

            VisitDAO visiteDAO = await _Db.Visits.Include(v => new { v.Client, v.Property, v.SalesManager }).FirstOrDefaultAsync(v => v.VisitId == idVisite);
            if (visiteDAO == null)
            {
                throw new InvalidOperationException("La visite doit exister pour être modifiée");
            }

            if (!(this._Utilisateur.Id == visiteDAO.SalesManagerId || this._Utilisateur.Id == visiteDAO.ClientId))
            {
                throw new InvalidOperationException("Seul le gestionnaire de vente ou le client associé à la visite peut la consulter");
            }

            InfoUtilisateurContact infoClient = new InfoUtilisateurContact()
            {
                Id = visiteDAO.Client.UserId,
                Mail = visiteDAO.Client.Mail,
                Nom = visiteDAO.Client.LastName,
                Prenom = visiteDAO.Client.FirstName,
                Numero = visiteDAO.Client.PhoneNumber
            };

            InfoUtilisateurContact infoGestionnaire = new InfoUtilisateurContact()
            {
                Id = visiteDAO.SalesManager.UserId,
                Mail = visiteDAO.SalesManager.Mail,
                Nom = visiteDAO.SalesManager.LastName,
                Prenom = visiteDAO.SalesManager.FirstName,
                Numero = visiteDAO.SalesManager.PhoneNumber
            };

            InfoAnnonce infoAnnonce = new InfoAnnonce()
            {
                Id = visiteDAO.Property.PropertyId,
                ReferenceAnnonce = visiteDAO.Property.ReferenceCode,
                Titre = visiteDAO.Property.Title,
            };

            return new InfoVisitesDetaillees()
            {
                Id = visiteDAO.VisitId,
                DateVisite = visiteDAO.VisitDate,
                Commentaire = visiteDAO.Comment,
                Offre = visiteDAO.Offer,
                InfoClient = infoClient,
                InfoGestionnaireDeVentes = infoGestionnaire,
                InfoAnnonceVisualise = infoAnnonce,
            };
        }

        //Benoit
        async public Task<IEnumerable<IInfoVisites>> ListerVisitesAsync(string chaineRecherche = null)
        {
            //List<VisitDAO> visitDAO = this._Db.Visits
            //                .Where(a => a. == .Id && a.Title.Contains(chaineRecherche)).ToList();
            //List<IInfoAnnonce> listeVisites = new List<IInfoAnnonce>();

            //foreach (VisitDAO b in visitDAO)
            //{
            //    listeVisites.Add(new InfoAnnonce() { Id = b.IdAdvertisement, ReferenceAnnonce = b.ReferenceCode, Titre = b.Title });
            //}

            //return listeVisites;

            throw new NotImplementedException();
        }

        // Irene
        async public Task<IInfoUtilisateurContact> DemanderContactAsync()
        {
            Helper.AuthoriserAcces(this._Utilisateur, TypeUtilisateur.Client);

            ClientDAO client = await this._Db.Clients.Include(c => c.SalesManager).FirstOrDefaultAsync(c => c.UserId == this._Utilisateur.Id);

            if (client.SalesManager == null)
            {
                SalesManagerDAO salesManager = await this._Db.SalesManagers.OrderBy(m => m.Clients.Count()).FirstAsync();
                client.SalesManager = salesManager;
            }

            return new InfoUtilisateurContact()
            {
                Id = (Guid)client.SalesManager.UserId,
                Prenom = client.SalesManager.FirstName,
                Nom = client.SalesManager.LastName,
                Mail = client.SalesManager.Mail,
                Numero = client.SalesManager.PhoneNumber
            };
        }

        #endregion


        #region Photo
        // Irene
        async public Task AjouterPhotosAAnnonceAsync(IEnumerable<IPhoto> photos, Guid idAnnonce)
        {
            Helper.AuthoriserAcces(this._Utilisateur, TypeUtilisateur.GestionnaireDeBiens);

            PropertyDAO annonce = await this._Db.Properties.FindAsync(idAnnonce);
            if (annonce == null)
            {
                throw new ArgumentNullException("L'annonce n'existe pas.");
            }

            foreach (IPhoto p in photos)
            {
                this._Db.Photos.Add(new PhotoDAO()
                {
                    PhotoId = p.IdPhoto,
                    Title = p.Titre,
                    Description = p.Description,
                    Image = p.Image,
                    Property = annonce
                });
            }

            await SauvegarderLaBDDAsync();
        }

        // Irene
        async public Task<IPhoto> ModifierPhotoAsync(IPhoto photo)
        {
            Helper.AuthoriserAcces(this._Utilisateur, TypeUtilisateur.GestionnaireDeBiens);

            PhotoDAO photoDAO = await this._Db.Photos.FindAsync(photo.IdPhoto);
            if (photoDAO == null)
            {
                throw new ArgumentNullException("La photo n'existe pas.");
            }

            if (this._Utilisateur.Id != photoDAO.Property.PropertyManagerId)
            {
                throw new InvalidOperationException("Seulement le gestionnaire de biens responsable peut modifier une annonce et ses photos.");
            }

            photoDAO.Title = photo.Titre;
            photoDAO.Description = photo.Description;
            photoDAO.Image = photo.Image;

            await SauvegarderLaBDDAsync();

            return photo;
        }

        // Irene
        async public Task<IPhoto> VisualiserPhotoAsync(Guid idPhoto)
        {
            PhotoDAO photo = await this._Db.Photos.FindAsync(idPhoto);
            if (photo == null)
            {
                throw new ArgumentNullException("La photo n'existe pas.");
            }

            await this._Db.Entry(photo).Reference(p => p.Property).LoadAsync();
            if (this._Utilisateur is IClient && photo.Property.Status == PropertyStatus.Sold)
            {
                throw new InvalidOperationException("Un client ne peut pas voir des détails/photos d'une annonce vendue.");
            }

            return new Photo()
            {
                IdPhoto = photo.PhotoId,
                Titre = photo.Title,
                Description = photo.Description,
                Image = photo.Image
            };
        }

        // Irene
        async public Task SupprimerPhotoAsync(Guid idPhoto)
        {
            Helper.AuthoriserAcces(this._Utilisateur, TypeUtilisateur.GestionnaireDeBiens);

            PhotoDAO photo = await this._Db.Photos.FindAsync(idPhoto);
            if (photo == null)
            {
                throw new ArgumentNullException("La photo n'existe pas");
            }

            await this._Db.Entry(photo).Reference(p => p.Property).LoadAsync();
            if (photo.Property.PropertyManagerId != this._Utilisateur.Id)
            {
                throw new InvalidOperationException("Seulement le gestionnaire de biens responsable pour l'annonce peut effacer les photos.");
            }

            this._Db.Photos.Remove(photo);
            await SauvegarderLaBDDAsync();
        }

        // Irene
        async public Task<IEnumerable<IInfoItem>> ListerPhotosAnnonceAsync(Guid idAnnonce)
        {
            var annonce = await this._Db.Properties.Include(a => a.Photos).FirstOrDefaultAsync(a => a.PropertyId == idAnnonce);

            if (annonce == null)
            {
                throw new ArgumentNullException("L'annonce n'existe pas.");
            }

            if (annonce.Status == PropertyStatus.Sold)
            {
                Helper.RefuserAcces(this._Utilisateur, TypeUtilisateur.Client);
            }

            return annonce.Photos.Select(p => new InfoItem() { Id = p.PhotoId, Libelle = p.Title });
        }
        #endregion

        private TypeUtilisateur GetTypeUtilisateur(IUtilisateur utilisateur)
        {
            TypeUtilisateur type;

            if (utilisateur is ISuperAdmin)
            {
                type = TypeUtilisateur.SuperAdmin;
            }
            else if (utilisateur is IGestionnaireDeVentes)
            {
                type = TypeUtilisateur.GestionnaireDeVentes;
            }
            else if (utilisateur is IGestionnaireDeBiens)
            {
                type = TypeUtilisateur.GestionnaireDeBiens;
            }
            else
            {
                type = TypeUtilisateur.Client;
            }

            return type;
        }

        private TypeUtilisateur GetTypeUserDAO(UserDAO utilisateur)
        {
            TypeUtilisateur type = TypeUtilisateur.Client;

            if (utilisateur is SuperAdminDAO)
            {
                type = TypeUtilisateur.SuperAdmin;
            }
            else if (utilisateur is SalesManagerDAO)
            {
                type = TypeUtilisateur.GestionnaireDeVentes;
            }
            else if (utilisateur is PropertyManagerDAO)
            {
                type = TypeUtilisateur.GestionnaireDeBiens;
            }
            else
            {
                type = TypeUtilisateur.Client;
            }

            return type;
        }

        async private Task SauvegarderLaBDDAsync()
        {
            //await _Db.SaveChangesAsync();
            try
            {
                await _Db.SaveChangesAsync();
            }
            //catch (DbEntityValidationException ex)
            //{
            //    throw new ArgumentException(ex.Message);
            //}
            catch (Exception)
            {
                throw new ArgumentException("Une opération n'as pas réussi dans la BDD");
            }
        }
    }
}
