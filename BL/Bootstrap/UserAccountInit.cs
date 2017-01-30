using BL.DTOs.Users;
using BL.Facades;
using BL.Services.UserAccounts;
using Castle.Windsor;

namespace BL.Bootstrap
{
    public static class UserAccountInit
    {
        /// <summary>
        ///     Initializes DB with various user accounts
        /// </summary>
        /// <param name="container"></param>
        public static void InitializeUserAccounts(IWindsorContainer container)
        {
            CreateUsers(container);
        }

        /// <summary>
        ///     Creates users (admin and users)
        /// </summary>
        /// <param name="container">The windsor container</param>
        private static void CreateUsers(IWindsorContainer container)
        {
            var userAccountManagementService = container.Resolve<IUserAccountService>();
            var userFacade = container.Resolve<UserFacade>();
            var playlistFacade = container.Resolve<PlaylistFacade>();
            bool success;

            userAccountManagementService.RegisterUserAccount(new UserRegistrationDTO
            {
                Email = "admin@musicLibrary.com",
                FirstName = "MusicLibrary",
                LastName = "Administrator",
                Password = "admin1234"
            }, true);

            userFacade.RegisterUser(new UserRegistrationDTO
            {
                Email = "michaela.bamburova@musicLibrary.com",
                FirstName = "Michaela",
                LastName = "Bamburová",
                Password = "michaela123" // same for the email account
            }, out success);

            userFacade.RegisterUser(new UserRegistrationDTO
            {
                Email = "silvia.borzova@musicLibrary.com", // password: silvia123
                FirstName = "Silvia",
                LastName = "Borzová",
                Password = "silvia123" // same for the email account
            }, out success);

            var user1 = userFacade.GetUserAccordingToEmail("michaela.bamburova@musicLibrary.com");
            var user2 = userFacade.GetUserAccordingToEmail("silvia.borzova@musicLibrary.com");

            playlistFacade.CreateInitPlaylist(user1.ID);
            playlistFacade.CreateInitPlaylist(user2.ID);
        }
    }
}