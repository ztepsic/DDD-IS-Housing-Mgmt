using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using ISHousingMgmt.Domain.MembershipAndRoles;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.Repositories.MembershipAndRoles;
using ISHousingMgmt.Infrastructure.Tests.DataAccess.NHibernate;
using Moq;
using NHibernate;
using NUnit.Framework;

namespace ISHousingMgmt.Infrastructure.Tests.DataAccess.Repositories.MembershipAndRoles {
	[TestFixture]
	public class UsersNHRepositoryTests : NHibernateFixture {

		[Test]
		public void Can_Save_Person_To_DB() {
			// Arrange
			var password = "password";
			var passwordCoderMock = new Mock<IPasswordCoder>();
			passwordCoderMock.Setup(pc => pc.PasswordFormat).Returns(MembershipPasswordFormat.Clear);
			var encodedPassword = Encoding.Default.GetBytes(password);
			passwordCoderMock.Setup(pc => pc.Encode(encodedPassword))
			                              	.Returns(encodedPassword);

			HousingMgmtUser user = new HousingMgmtUser("username", password, passwordCoderMock.Object) {
				Email = "mail@mail.com"
			};

			LegalPerson legalPerson = new LegalPerson("12345678902", "FER");

			user.Person = legalPerson;


			UsersNHRepository usersNHRepository = new UsersNHRepository(SessionFactory);

			// Act
			using (var session = SessionFactory.OpenSession()) {
				using (var tx = Session.BeginTransaction()) {
					usersNHRepository.SaveOrUpdate(user);
					tx.Commit();
				}
			}

			HousingMgmtUser fetchedUser = null;
			using (var session = SessionFactory.OpenSession()) {
				using (var tx = Session.BeginTransaction()) {
					fetchedUser = usersNHRepository.GetById(user.Id) as HousingMgmtUser;
					tx.Commit();
				}

			}

			// Assert
			Assert.IsTrue(fetchedUser != null, "FetchedUser is null.");
			Assert.AreEqual(legalPerson, fetchedUser.Person, "Persons aren't equal.");
			Assert.AreEqual(DateTime.MinValue.ToLocalTime(), fetchedUser.LastLockoutDate, "Date and Time aren't equal.");
		}

		[Test]
		public void Can_Delete_User() {
			// Arrange
			var password = "password";
			var passwordCoderMock = new Mock<IPasswordCoder>();
			passwordCoderMock.Setup(pc => pc.PasswordFormat).Returns(MembershipPasswordFormat.Clear);
			var encodedPassword = Encoding.Default.GetBytes(password);
			passwordCoderMock.Setup(pc => pc.Encode(encodedPassword))
											.Returns(encodedPassword);

			HousingMgmtUser user = new HousingMgmtUser("username", password, passwordCoderMock.Object) {
				Email = "mail@mail.com"
			};

			LegalPerson legalPerson = new LegalPerson("12345678902", "FER");

			user.Person = legalPerson;


			UsersNHRepository usersNHRepository = new UsersNHRepository(SessionFactory);

			// Act
			using (var session = SessionFactory.OpenSession()) {
				using (var tx = Session.BeginTransaction()) {
					usersNHRepository.SaveOrUpdate(user);
					tx.Commit();
				}
			}

			using (var session = SessionFactory.OpenSession()) {
				using (var tx = Session.BeginTransaction()) {
					usersNHRepository.Delete(user);
					tx.Commit();
				}
			}

			HousingMgmtUser fetchedUser = null;
			using (var session = SessionFactory.OpenSession()) {
				using (var tx = Session.BeginTransaction()) {
					fetchedUser = usersNHRepository.GetById(user.Id) as HousingMgmtUser;
					tx.Commit();
				}

			}

			// Assert
			Assert.IsTrue(fetchedUser == null, "FetchedUser isn't null.");
		}

		[Test]
		public void Can_Delete_User_By_UserName() {
			// Arrange
			var password = "password";
			var passwordCoderMock = new Mock<IPasswordCoder>();
			passwordCoderMock.Setup(pc => pc.PasswordFormat).Returns(MembershipPasswordFormat.Clear);
			var encodedPassword = Encoding.Default.GetBytes(password);
			passwordCoderMock.Setup(pc => pc.Encode(encodedPassword))
											.Returns(encodedPassword);

			HousingMgmtUser user = new HousingMgmtUser("username", password, passwordCoderMock.Object) {
				Email = "mail@mail.com"
			};

			LegalPerson legalPerson = new LegalPerson("12345678902", "FER");
			user.Person = legalPerson;

			UsersNHRepository usersNHRepository = new UsersNHRepository(SessionFactory);

			// Act
			using (var session = SessionFactory.OpenSession()) {
				using (var tx = Session.BeginTransaction()) {
					usersNHRepository.SaveOrUpdate(user);
					tx.Commit();
				}
			}

			bool isDeleted = false;
			using (var session = SessionFactory.OpenSession()) {
				using (var tx = Session.BeginTransaction()) {
					isDeleted = usersNHRepository.DeleteUserByUserName(user.UserName);
					tx.Commit();
				}
			}

			HousingMgmtUser fetchedUser = null;
			using (var session = SessionFactory.OpenSession()) {
				using (var tx = Session.BeginTransaction()) {
					fetchedUser = usersNHRepository.GetById(user.Id) as HousingMgmtUser;
					tx.Commit();
				}

			}

			// Assert
			//Assert.IsTrue(fetchedUser == null, "FetchedUser isn't null.");
			Assert.IsTrue(isDeleted, "User isn't deleted.");
		}

	}
}
