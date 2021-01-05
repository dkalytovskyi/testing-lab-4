using System;
using IIG.CoSFE.DatabaseUtils;
using IIG.PasswordHashingUtils;
using NUnit.Framework;
namespace lab4.tests
{
    [TestFixture]
    public class PasswordHashingUtilsWithAuthDBTest
    {
        private const string Server = @"localhost";
        private const string Database = @"IIG.CoSWE.AuthDB";
        private const bool IsTrusted = false;
        private const string Login = @"sa";
        private const string Password = @"qwerty12345";
        private const int ConnectionTime = 75;

        AuthDatabaseUtils authDatabaseUtils = new AuthDatabaseUtils(Server, Database, IsTrusted, Login, Password, ConnectionTime);

        [Test]
        public void AddCredentials()
        {
            string login1 = "login";
            string password1 = "password";

            string login2 = "логин";
            string password2 = "пароль";

            string login3 = "log-in_test";
            string password3 = "pass.word?";

            Assert.IsTrue(authDatabaseUtils.AddCredentials(login1, PasswordHasher.GetHash(password1)));
            Assert.IsTrue(authDatabaseUtils.AddCredentials(login2, PasswordHasher.GetHash(password2)));
            Assert.IsTrue(authDatabaseUtils.AddCredentials(login3, PasswordHasher.GetHash(password3)));

        }

        [Test]
        public void AddCredentialsSameLogin()
        {

            string login = "login_same";
            string password = "password";

            Assert.IsTrue(authDatabaseUtils.AddCredentials(login, PasswordHasher.GetHash(password)));
            Assert.IsFalse(authDatabaseUtils.AddCredentials(login, PasswordHasher.GetHash(password)));

        }

        [Test]
        public void AddCredentialsNullOrEmptyLogin()
        {
            Assert.IsFalse(authDatabaseUtils.AddCredentials(null, PasswordHasher.GetHash("nullpassword")));
            Assert.IsFalse(authDatabaseUtils.AddCredentials("", PasswordHasher.GetHash("emptypassword")));

        }

        [Test]
        public void AddCredentialsWithEmptyPassword()
        {
            string login = "login_empty_pass";
            string password = "";
            Assert.IsTrue(authDatabaseUtils.AddCredentials(login, PasswordHasher.GetHash(password)));
        }

        [Test]
        public void AddCredentialsWithNullPasswordThrows()
        {
            try
            {
                authDatabaseUtils.AddCredentials("login_null_pass", PasswordHasher.GetHash(null));
            }
            catch (ArgumentNullException e)
            {
                StringAssert.Contains("Value cannot be null", e.Message);
                return;
            }

            Assert.Fail("The ArgumentNullException was not thrown");

        }

        [Test]
        public void UpdateCredentialsWithPasswordChange()
        {
            string login = "login_upd_pass";
            string password = "password";
            string passwordNew = "password_upd";


            Assert.IsTrue(authDatabaseUtils.AddCredentials(login, PasswordHasher.GetHash(password)));
            Assert.IsTrue(authDatabaseUtils.UpdateCredentials(login, PasswordHasher.GetHash(password),
               login, PasswordHasher.GetHash(passwordNew)));

        }

        [Test]
        public void UpdateCredentialsWithLoginChange()
        {
            string login = "login_old";
            string password = "password";
            string loginNew = "login_upd";

            Assert.IsTrue(authDatabaseUtils.AddCredentials(login, PasswordHasher.GetHash(password)));
            Assert.IsTrue(authDatabaseUtils.UpdateCredentials(login, PasswordHasher.GetHash(password),
            loginNew, PasswordHasher.GetHash(password)));

        }

        [Test]
        public void UpdateCredentialsWithEmptyOrNullLogin()
        {
            string login = "login_empty";
            string password = "password";

            Assert.IsTrue(authDatabaseUtils.AddCredentials(login, PasswordHasher.GetHash(password)));
            Assert.IsFalse(authDatabaseUtils.UpdateCredentials(login, PasswordHasher.GetHash(password),
            "", PasswordHasher.GetHash(password)));

            Assert.IsFalse(authDatabaseUtils.UpdateCredentials(login, PasswordHasher.GetHash(password),
            null, PasswordHasher.GetHash(password)));

        }

        [Test]
        public void UpdateCredentialsReturnsFalseWithWrongLoginOrPassword()
        {

            string login = "login_wrong_case";
            string password = "password";
            string wrongLogin = "wronglogin";
            string wrongPassword = "wrongpassword";
            string newLogin = "login_new";

            Assert.IsTrue(authDatabaseUtils.AddCredentials(login, PasswordHasher.GetHash(password)));
            Assert.IsFalse(authDatabaseUtils.UpdateCredentials(login, PasswordHasher.GetHash(wrongPassword),
            newLogin, PasswordHasher.GetHash(password)));

            Assert.IsFalse(authDatabaseUtils.UpdateCredentials(wrongLogin, PasswordHasher.GetHash(password),
            newLogin, PasswordHasher.GetHash(password)));
        }

        [Test]
        public void CheckValidCredentials()
        {
            string login = "login_check_case";
            string password = "password26";
            Assert.IsTrue(authDatabaseUtils.AddCredentials(login, PasswordHasher.GetHash(password)));
            Assert.IsTrue(authDatabaseUtils.CheckCredentials(login, PasswordHasher.GetHash(password)));

        }

        [Test]
        public void CheckInvalidCredentials()
        {
            string login = "login_check_invalid";
            string password = "password";
            string wrongPassword = "wrongpassword";

            Assert.IsTrue(authDatabaseUtils.AddCredentials(login, PasswordHasher.GetHash(password)));
            Assert.IsFalse(authDatabaseUtils.CheckCredentials(login, PasswordHasher.GetHash(wrongPassword)));

        }

        public void CheckCredentialsNonExistentLogin()
        {
            string login = "login_nonexistent_case";
            string password = "password";
            Assert.IsFalse(authDatabaseUtils.CheckCredentials(login, PasswordHasher.GetHash(password)));

        }

        [Test]
        public void DeleteExistingCredentialsTest()
        {
            string login = "login_delete_case";
            string password = "password";

            Assert.IsTrue(authDatabaseUtils.AddCredentials(login, PasswordHasher.GetHash(password)));
            Assert.IsTrue(authDatabaseUtils.DeleteCredentials(login, PasswordHasher.GetHash(password)));
            Assert.IsFalse(authDatabaseUtils.CheckCredentials(login, PasswordHasher.GetHash(password)));
        }

        [Test]
        public void DeleteInvalidCredentials()
        {
            string login = "login_delete_invalid";
            string password = "password";
            string wrongPassword = "wrongpasswordDdDdDd";

            Assert.IsTrue(authDatabaseUtils.AddCredentials(login, PasswordHasher.GetHash(password)));
            Assert.IsFalse(authDatabaseUtils.DeleteCredentials(login, PasswordHasher.GetHash(wrongPassword)));
            Assert.IsTrue(authDatabaseUtils.CheckCredentials(login, PasswordHasher.GetHash(password)));
        }

        [Test]
        public void DeleteNonExistentCredentials()
        {
            string fakeLogin = "login_fake";
            string password = "password";

            Assert.IsFalse(authDatabaseUtils.DeleteCredentials(fakeLogin, PasswordHasher.GetHash(password)));
        }
    }
}
