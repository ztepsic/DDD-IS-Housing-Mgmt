using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using ISHousingMgmt.Domain.BusinessRulesAndValidation;
using ISHousingMgmt.Domain.PersonsAndRoles;
using Moq;
using NUnit.Framework;
using ISHousingMgmt.Domain.MembershipAndRoles;

namespace ISHousingMgmt.Domain.Tests.MembershipsAndRoles {
	[TestFixture]
	public class UserTests {

		#region Members

		private IPasswordCoder passwordCoder;
		private string password = "password";
		private string fakePassword = "fakePassword";

		#endregion

		#region Constructors and Init

		[SetUp]
		public void SetUp() {
			Mock<IPasswordCoder> passwordCoderMock = new Mock<IPasswordCoder>();

			passwordCoderMock.Setup(pc => pc.PasswordFormat).Returns(MembershipPasswordFormat.Clear);

			var passwordData = Encoding.Default.GetBytes(password);
			passwordCoderMock.Setup(pc => pc.Encode(passwordData))
				.Returns(passwordData);

			passwordCoderMock.Setup(pc => pc.Decode(passwordData))
				.Returns(passwordData);

			var fakePasswordData = Encoding.Default.GetBytes(password);
			passwordCoderMock.Setup(pc => pc.Encode(fakePasswordData))
				.Returns(fakePasswordData);

			passwordCoder = passwordCoderMock.Object;
		}

		#endregion

		#region Methods

		[Test]
		public void If_Time_Of_Tracking_Bad_Attempts_Is_Passed_Counters_Must_Be_Reset() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			user.IncrementFailedPasswordAttemptCount();
			user.IncrementFailedPasswordAttemptCount();
			user.IncrementFailedPasswordAnswerAttemptCount();

			PropertyInfo passwordAnswerProperty = user.GetType().GetProperty("FailedPasswordAnswerAttemptWindowStart",
				BindingFlags.Public | BindingFlags.Instance);

			passwordAnswerProperty.SetValue(user, DateTime.Now.AddMinutes(-30), null);

			PropertyInfo passwordProperty = user.GetType().GetProperty("FailedPasswordAttemptWindowStart",
				BindingFlags.Public | BindingFlags.Instance);
			passwordProperty.SetValue(user, DateTime.Now.AddMinutes(-30), null);

			// Act
			user.CheckTrackingFailedAttempts(1);

			// Assert
			Assert.AreEqual(0, user.FailedPasswordAnswerAttemptCount, "FailedPasswordAnswerAttempt counter isn't reset.");
			Assert.AreEqual(DateTime.MinValue.ToLocalTime(), user.FailedPasswordAnswerAttemptWindowStart, "FailedPasswordAnswerAttemptWindowStart isn't reset to min datetime.");
			Assert.AreEqual(0, user.FailedPasswordAttemptCount, "FailedPasswordAttempt counter isn't reset.");
			Assert.AreEqual(DateTime.MinValue.ToLocalTime(), user.FailedPasswordAttemptWindowStart, "FailedPasswordAttemptWindowStart isn't reset to min datetime.");

		}

		[Test]
		public void If_Time_Of_Tracking_Bad_Attempts_Is_Not_Passed_Counters_Must_Be_Reset() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			user.IncrementFailedPasswordAttemptCount();
			user.IncrementFailedPasswordAttemptCount();

			// Act
			user.CheckTrackingFailedAttempts(30);

			// Assert
			Assert.AreEqual(0, user.FailedPasswordAnswerAttemptCount, "FailedPasswordAnswerAttempt counter is reset.");
			Assert.AreEqual(2, user.FailedPasswordAttemptCount, "FailedPasswordAttempt counter is reset.");

		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Cant_ComparePasswordAnswerTo_If_QuestionAndAnswer_Arent_Defined() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			// Act
			var isEqual = user.ComparePasswordAnswerTo("otherPasswordAnser", passwordCoder);

			// Assert
		}


		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Cant_GetPassword_If_QuestionAndAnswer_Arent_Defined() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			// Act
			var newPassword = user.GetPassword("otherPasswordAnswer", passwordCoder);

			// Assert
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Cant_ResetPassword_If_QuestionAndAnswer_Arent_Defined() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			// Act
			var newPassword = user.ResetPassword("otherPasswordAnswer", 7, passwordCoder);

			// Assert
		}

		[Test]
		public void Can_Get_Password_If_PasswordCoder_Supports_It() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			// Act
			var retrievedPassword = user.GetPassword(passwordCoder);

			// Assert
			Assert.AreEqual(password, retrievedPassword, "Passwords aren't equal.");
		}

		[Test]
		public void If_PasswordCoder_Is_HashPasswordCoder_PasswordSalt_Must_Be_Set_On_In_Ctor() {
			// Arrange
			Mock<IPasswordCoder> passwordCoderMock = new Mock<IPasswordCoder>();
			passwordCoderMock.Setup(pc => pc.PasswordFormat).Returns(MembershipPasswordFormat.Hashed);
			var passwordCoder = passwordCoderMock.Object;

			User user = new User("username", password, passwordCoder);

			// Act

			// Assert
			Assert.IsNotNullOrEmpty(user.PasswordSalt, "PasswordSalt isn't set in constructor");
		}

		[Test]
		public void Can_Set_PasswordQuestion_In_ClearText() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			string question = "Question for password";
			string answer = "Answer for password question";

			// Act
			user.ChangePasswordQuestionAndAnswer(password, question, answer, passwordCoder);			

			// Assert
			Assert.AreEqual(question, user.PasswordQuestion, "PasswordQuestion isn't set in clear text.");
		}

		[Test]
		public void Cant_Change_PasswordQuestionAndAnswer_When_User_Is_Locked() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			var newPasswordQuestion = "New password question";
			var newPasswordAnswer = "New password answer";

			user.ChangePasswordQuestionAndAnswer(fakePassword, newPasswordQuestion, newPasswordAnswer, passwordCoder);
			user.ChangePasswordQuestionAndAnswer(fakePassword, newPasswordQuestion, newPasswordAnswer, passwordCoder);

			// ovdje bi korisnik trebao biti zakljucan
			user.TryToLockoutUser(2);

			// Act
			var result = user.ChangePasswordQuestionAndAnswer(password, newPasswordQuestion, newPasswordAnswer, passwordCoder);

			// Assert
			Assert.IsFalse(result, "Change of PasswordQuestionAndAnswer was successful.");
			Assert.AreNotEqual(newPasswordQuestion, user.PasswordQuestion, "User has successfuly changed password question.");
			Assert.IsTrue(user.IsLockedOut, "User isn't lockedout.");
		}

		[Test]
		public void Attempt_To_Change_PasswordQuestionAndAnswer_With_Wrong_Password_Must_Increment_Failiure_Count() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			var failedPasswordAttemptCount = user.FailedPasswordAttemptCount;

			var newPasswordQuestion = "New password question";
			var newPasswordAnswer = "New password answer";

			// Act
			var result = user.ChangePasswordQuestionAndAnswer(fakePassword, newPasswordQuestion, newPasswordAnswer, passwordCoder);

			// Assert
			Assert.IsFalse(result, "Change of PasswordQuestionAndAnswer was successful.");
			Assert.AreNotEqual(newPasswordQuestion, user.PasswordQuestion, "User has successfuly changed password question.");
			Assert.IsTrue(failedPasswordAttemptCount < user.FailedPasswordAttemptCount, "Change with wrong password didn't rise failed password attempt count.");
		}

		[Test]
		public void Email_Must_Be_Set_To_Lower() {
			// Arrange
			string email = "Name.Surname@GMAIL.com";
			User user = new User("username", password, passwordCoder) {
				Email = email
			};

			string loweredEmail = email.ToLower();

			// Act

			// Assert
			Assert.AreEqual(loweredEmail, user.Email, "Email isn't lowered.");
		}

		[Test]
		[Ignore("Ignore a test")]
		[ExpectedException(typeof(BusinessRulesException<User>))]
		public void Cant_Set_InValid_Email_Address() {
			// Arrange

			// Act

			// Assert
		}

		[Test]
		[Ignore("Ignore a test")]
		[ExpectedException(typeof(BusinessRulesException<User>))]
		public void Cant_Set_InValid_UserName() {
			// provjeru napraviti i u NHibernateMembershipProvideru

			// Arrange

			// Act

			// Assert
		}

		[Test]
		public void CreationDate_Is_Set_By_Constructor() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			// Act

			// Assert
			Assert.IsTrue(user.CreationDate > DateTime.MinValue.ToLocalTime(), "CreationDate isn't set by constructor.");
		}

		[Test]
		public void CreationDate_Must_Be_Set_In_UTC_And_Returned_In_LocalTime() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			// Act

			// Assert
			Assert.AreEqual(DateTime.Now.Kind, user.CreationDate.Kind, "CreationDate isn't returned in local time.");
		}

		[Test]
		public void LastLoginDate_Must_Be_Set_In_UTC_And_Retured_In_LocalTime() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			var localTime = DateTime.Now;

			// Act
			user.LastLoginDate = localTime;

			// Assert
			Assert.AreEqual(localTime, user.LastLoginDate, "LastLoginDate isn't returned in local time.");
		}

		[Test]
		public void On_User_Creation_LastPasswordChangedDate_Must_Have_The_Same_Value_As_CreatedDate() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			// Act

			// Assert
			Assert.AreEqual(user.CreationDate, user.LastPasswordChangedDate, "LastPasswordChangedDate and CreationDate aren't equal.");
		}

		[Test]
		[Ignore("Ignore a test")]
		public void Password_Change_Must_Set_New_LastPasswordChangedDate() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			// Act
			var isPasswordChanged = user.ChangePassword(password, fakePassword, passwordCoder);

			// Assert
			Assert.IsTrue(isPasswordChanged, "Password isn't changed.");
			Assert.AreNotEqual(user.CreationDate, user.LastPasswordChangedDate, "LastPasswordChangedDate and CreationDate are equal.");
		}

		[Test]
		public void When_User_Is_Declared_As_LockedOut_LastLockoutDate_Must_Be_Set() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			user.IncrementFailedPasswordAttemptCount();
			user.IncrementFailedPasswordAttemptCount();

			// Act
			user.TryToLockoutUser(2);

			// Assert
			Assert.IsTrue(user.IsLockedOut, "User isn't locked out.");
			Assert.AreNotEqual(DateTime.MinValue.ToLocalTime(), user.LastLockoutDate, "LastLockoutDate isn't set when user is declared as locked out.");
		}

		[Test]
		public void Can_Increment_FailedPasswordAttemptCount() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			// Act
			user.IncrementFailedPasswordAttemptCount();
			user.IncrementFailedPasswordAttemptCount();
			user.IncrementFailedPasswordAttemptCount();

			// Assert
			Assert.AreEqual(3, user.FailedPasswordAttemptCount, "Can't increment FailedPasswordAttemptCount.");
		}

		[Test]
		public void When_User_Is_Unlocked_FailedPasswordAttemptCount_Must_Be_Reset_To_Zero() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			user.IncrementFailedPasswordAttemptCount();
			user.IncrementFailedPasswordAttemptCount();

			// Act
			user.UnlockUser();

			// Assert
			Assert.AreEqual(0, user.FailedPasswordAnswerAttemptCount, "FailedPasswordAttemptCount isn't set to zero.");
		}

		[Test]
		public void Can_Increment_FailedPasswordAnswerAttemptCount() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			// Act
			user.IncrementFailedPasswordAnswerAttemptCount();
			user.IncrementFailedPasswordAnswerAttemptCount();
			user.IncrementFailedPasswordAnswerAttemptCount();

			// Assert
			Assert.AreEqual(3, user.FailedPasswordAnswerAttemptCount, "Can't increment FailedPasswordAnswerAttemptCount.");
		}

		[Test]
		public void When_User_Is_Unlocked_FailedPasswordAnswerAttemptCount_Must_Be_Reset_To_Zero() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			user.IncrementFailedPasswordAnswerAttemptCount();
			user.IncrementFailedPasswordAnswerAttemptCount();

			// Act
			user.UnlockUser();

			// Assert
			Assert.AreEqual(0, user.FailedPasswordAnswerAttemptCount, "FailedPasswordAnswerAttemptCount isn't set to zero.");
		}

		[Test]
		public void Can_Reset_FailedPasswordAttemptCount() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			user.IncrementFailedPasswordAttemptCount();
			user.IncrementFailedPasswordAttemptCount();

			// Act
			user.ResetFailedPasswordAttemptCount();

			// Assert
			Assert.AreEqual(0, user.FailedPasswordAttemptCount, "FailedPasswordAttemptCount isn't set to zero.");
		}

		[Test]
		public void Can_Reset_FailedPasswordAnswerAttemptCount() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			user.IncrementFailedPasswordAnswerAttemptCount();
			user.IncrementFailedPasswordAnswerAttemptCount();

			// Act
			user.ResetFailedPasswordAnswerAttemptCount();

			// Assert
			Assert.AreEqual(0, user.FailedPasswordAnswerAttemptCount, "FailedPasswordAnswerAttemptCount isn't set to zero.");
		}

		[Test]
		public void When_User_Is_Unlocked_FailedPasswordAttemptWindowStart_Must_Be_Reset_To_Default() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			// Act

			user.IncrementFailedPasswordAttemptCount();
			user.IncrementFailedPasswordAttemptCount();
			user.TryToLockoutUser(2);
			var wasLockedOut = user.IsLockedOut;

			user.UnlockUser();

			// Assert
			Assert.IsTrue(wasLockedOut, "User wasn't ever locked out.");
			Assert.AreEqual(DateTime.MinValue.ToLocalTime(), user.FailedPasswordAttemptWindowStart, "FailedPasswordAttemptWindowstart isn't set to default.");
			Assert.IsTrue(DateTime.MinValue.ToLocalTime() == user.FailedPasswordAttemptWindowStart, "FailedPasswordAttemptWindowstart isn't set to default.");
		}

		[Test]
		public void When_User_Is_Unlocked_FailedPasswordAnswerAttemptWindowStart_Must_Be_Reset_To_Default() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			user.IncrementFailedPasswordAnswerAttemptCount();
			user.IncrementFailedPasswordAnswerAttemptCount();
			user.TryToLockoutUser(2);
			var wasLockedOut = user.IsLockedOut;

			// Act
			user.UnlockUser();

			// Assert
			Assert.IsTrue(wasLockedOut, "User wasn't ever locked out.");
			Assert.AreEqual(DateTime.MinValue.ToLocalTime(), user.FailedPasswordAnswerAttemptWindowStart, "FailedPasswordAnswerAttemptWindowstart isn't set to default.");
			Assert.IsTrue(DateTime.MinValue.ToLocalTime() == user.FailedPasswordAnswerAttemptWindowStart, "FailedPasswordAnswerAttemptWindowstart isn't set to default.");
		}

		[Test]
		public void When_Incrementing_FailedPasswordAttemptCount_Must_Set_Its_StartWindow() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			// Act
			user.IncrementFailedPasswordAttemptCount();
			user.IncrementFailedPasswordAttemptCount();

			// Assert
			Assert.IsTrue(user.FailedPasswordAttemptWindowStart > DateTime.MinValue.ToLocalTime(), "FailedPasswordAttemptWindowStart is not changed.");
			Assert.AreNotEqual(DateTime.MinValue.ToLocalTime(), user.FailedPasswordAttemptWindowStart, "FailedPasswordAttemptWindowStart and DateTime.MinValue are equal.");
		}

		[Test]
		public void When_Incrementing_FailedPasswordAnswerAttemptCount_Must_Set_Its_StartWindow() {
			// Arrange
			User user = new User("username", password, passwordCoder);

			user.UnlockUser();

			// Act
			
			user.IncrementFailedPasswordAnswerAttemptCount();
			user.IncrementFailedPasswordAnswerAttemptCount();

			// Assert
			Assert.IsTrue(user.FailedPasswordAnswerAttemptWindowStart > DateTime.MinValue.ToLocalTime(), "FailedPasswordAnswerAttemptWindowStart is not changed.");
			Assert.AreNotEqual(DateTime.MinValue.ToLocalTime(), user.FailedPasswordAnswerAttemptWindowStart, "FailedPasswordAnswerAttemptWindowStart and DateTime.MinValue are equal.");
		}

		#endregion

	}
}
