using ImmoBLL.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImmoBLL.Interfaces
{
    // L'interface était créé par l'ensemble de l'équipe
    public interface IImmoBLL
    {
        /// <summary>
        /// Voir le nom, le prenom et le type de l'utilisateur connecté.
        /// </summary>
        IInfoUtilisateur InformationsUtilisateurConnecte { get; }

        Task<IInfoUtilisateur> ConnecterAsync(string mail, string motDePasse);

        #region Utilisateur
        /// <summary>
        /// Ajouter un nouvel utilisateur dans la BDD. 
        /// Un superadmin peut ajouter des nouveaux gestionnaires et des clients..
        /// Un gestionnaire de vente peut aujouter des nouveaux clients.
        /// </summary>
        /// <param name="nouvelUtilisateur">Utilisateur à ajouter dans la BDD</param>
        /// <returns>Retourne l'utilisateur ajouté dans la BDD</returns>
        Task<IUtilisateur> AjouterUtilisateurAsync(IUtilisateur nouvelUtilisateur);

        /// <summary>
        /// Modifier un utilisateur déjà existant dans la BDD.
        /// Seulement un superadmin peut faire des modifications.
        /// </summary>
        /// <param name="utilisateurModifie">Utilisateur à modifié</param>
        /// <returns>Retourne l'utilisateur modifié dans la BDD</returns>
        Task<IUtilisateur> ModifierUtilisateurAsync(IUtilisateur utilisateurModifie);

        // MarquerGestionnaireInactif ressemblerai beaucoup à ModifierCompteUtilisateur, sauf qu'il ne modifierai 
        // que le paramètre "actif" et déplacerai les clients d'un gestionnaire innactif vers un actif
        /// <summary>
        /// Marquer un gestionnaire inactif et reassigner ses annonces (gestionnaire de biens) ou ses clients (gestionnaire de ventes) à un remplacant.
        /// </summary>
        /// <param name="idUtilisateur">Id du gestionnaire</param>
        Task MarquerGestionnaireInactifAsync(Guid idUtilisateur);

        Task<IEnumerable<IInfoUtilisateur>> ListerGestionnairesAsync();

        Task<IEnumerable<IInfoUtilisateur>> ListerClientsAsync();

        Task<IGestionnaire> VisualiserGestionnaire(Guid id);
        #endregion

        #region Annonces
        /// <summary>
        /// Ajouter une nouvelle annonce dans la BDD
        /// </summary>
        /// <param name="annonce">Annonce à ajouter</param>
        /// <returns>Retourne la nouvelle annonce</returns>
        Task<IAnnonce> AjouterAnnonceAsync(IAnnonce annonce);

        /// <summary>
        /// Modifier une annonce existante
        /// </summary>
        /// <param name="annonce">Annonce à modifier</param>
        /// <returns>L'annonce modifiée</returns>
        Task<IAnnonce> ModifierAnnonceAsync(IAnnonce annonce);

        // TODO reflechir comment on veut faire le retour - le client ne peut pas voir toutes les details de l'annonce
        /// <summary>
        /// Visualiser l'intégralité d'une annonce
        /// </summary>
        /// <param name="idAnnonce">Id de l'annonce</param>
        /// <returns>L'annonce qui correspond à l'Id</returns>
        Task<IAnnonce> VisualiserAnnonceAsync(Guid idAnnonce);

        /// <summary>
        /// Lister les annonces avec un nombre de colonnes resteintes
        /// </summary>
        /// <param name="chaineRecherche">Introduire une chaine de caractères de recherche</param>
        /// <returns>Les annonces correspondant à la recherche</returns>
        Task<IEnumerable<IInfoAnnonce>> ListerAnnoncesAsync(string chaineRecherche = null);

        Task<IEnumerable<IInfoAnnonce>> ListerAnnoncesReserveesAsync();

        Task MarquerAnnonceVenduAsync(Guid idAnnonce);
        #endregion


        #region Visites
        /// <summary>
        /// Ajouter une visite dans la BDD.
        /// Seul un gestionnaire de ventes peut ajouter une visite.
        /// </summary>
        /// <param name="nouvelleVisite"></param>
        /// <returns>Retourne la visite dans la BDD</returns>
        Task<IVisite> AjouterVisiteAsync(IVisite visite);

        /// <summary>
        /// Modifie une visite dans la BDD
        /// Seul un gestionnaire de ventes peut modifier une visite
        /// </summary>
        /// <param name="visite"></param>
        /// <returns>Retourne la visite modifiée dans la BDD</returns>
        Task<IVisite> ModifierVisiteAsync(IVisite visite);

        /// <summary>
        /// Permet d'extraire les données d'une visite de la BDD
        /// </summary>
        /// <param name="idVisite"></param>
        /// <returns></returns>
        Task<IInfoVisitesDetaillees> VisualiserVisiteAsync(Guid idVisite);

        Task<IEnumerable<IInfoVisites>> ListerVisitesAsync(string chaineRecherche = null);

        /// <summary>
        /// Donne les information du gestionnaire de vente responsable pour le client connecté.
        /// Si le client n'a pas encore un gestionnaire de vente est associé au client.
        /// </summary>
        /// <returns>Retourne des information de contact du gestionnaire de vente</returns>
        Task<IInfoUtilisateurContact> DemanderContactAsync();
        #endregion


        #region Photo

        /// <summary>
        /// Ajouter une liste de photos à une annonce
        /// </summary>
        /// <param name="photos">Une liste de photos à ajouter</param>
        /// <param name="idAnnonce">L'annonce à laquelle les photos seront ajoutées</param>
        Task AjouterPhotosAAnnonceAsync(IEnumerable<IPhoto> photos, Guid idAnnonce);

        /// <summary>
        /// Modifier une photo déjà existant dans la BDD
        /// </summary>
        /// <param name="photo">La photo modifiée</param>
        /// <returns>La photo qui était modifiée</returns>
        Task<IPhoto> ModifierPhotoAsync(IPhoto photo);

        /// <summary>
        /// Visualier les details d'une photo
        /// </summary>
        /// <param name="idPhoto"></param>
        /// <returns>La photo detaillée</returns>
        Task<IPhoto> VisualiserPhotoAsync(Guid idPhoto);

        /// <summary>
        /// Supprimer une photo
        /// </summary>
        /// <param name="idPhoto">id de la photo à supprimer</param>
        Task SupprimerPhotoAsync(Guid idPhoto);

        /// <summary>
        /// Lister toutes les photos pour une annonce.
        /// Les clients ne peuvent pas voir des photos pour des annonces vendues.
        /// </summary>
        /// <param name="idAnnonce"></param>
        /// <returns>Une liste de photos</returns>
        Task<IEnumerable<IInfoItem>> ListerPhotosAnnonceAsync(Guid idAnnonce);

        #endregion
    }
}
