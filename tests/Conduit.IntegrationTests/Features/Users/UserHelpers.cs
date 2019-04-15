using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Conduit.Features.Users;

namespace Conduit.IntegrationTests.Features.Users
{
    public static class UserHelpers
    {
        public static readonly string DefaultUserName = "username";
        public static UserEnvelope UserEnvelope;
        /// <summary>
        /// creates a default user to be used in different tests
        /// </summary>
        /// <param name="fixture"></param>
        /// <returns></returns>
        public static async Task<User> CreateDefaultUser(SliceFixture fixture)
        {
            if (UserEnvelope != null)
            {
                return UserEnvelope.User;
            }

            var command = new Create.Command()
            {
                User = new Create.UserData()
                {
                    Email = "email",
                    Password = "password",
                    Username = DefaultUserName
                }
            };

            UserEnvelope = await fixture.SendAsync(command);
            return UserEnvelope.User;
        }
    }
}
