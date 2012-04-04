using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Web.Security;
using ISHousingMgmt.Domain.MembershipAndRoles;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.Repositories.MembershipAndRoles;

namespace ISHousingMgmt.Infrastructure.MembershipAndRoles {
	/// <summary>
	/// NHibernate membership provider
	/// </summary>
	public class NHibernateMembershipProvider : MembershipProvider {

		#region Members

		#region Config

		private const string MAX_INVALID_PASSWORD_ATTEMPTS = "maxInvalidPasswordAttempts";
		private const string PASSWORD_ATTEMPT_WINDOW = "passwordAttemptWindow";
		private const string MIN_REQUIRED_NON_ALPHANUM_CHARACTERS = "minRequiredNonAlphanumericCharacters";
		private const string MIN_REQUIRED_PASSWORD_LENGTH = "minRequiredPasswordLenght";
		private const string PASSWORD_STRENGHT_REGEX = "passwordStrengthRegularExpression";
		private const string ENABLE_PASSWORD_RESET = "enablePasswordReset";
		private const string ENABLE_PASSWORD_RETRIEVAL = "enablePasswordRetrieval";
		private const string REQUIRES_QUESTION_AND_ANSWER = "requiresQuestionAndAnswer";
		private const string REQUIRES_UNIQUE_EMAIL = "requiresUniqueEmail";
		private const string PASSWORD_FORMAT = "passwordFormat";
		private const string AUTO_UNLOCK_TIME = "autoUnlockTime";

		/// <summary>
		/// Defaultne konfiguracijske vrijednosti
		/// </summary>
		private static readonly NameValueCollection defaultConfig = new NameValueCollection {
			{ MAX_INVALID_PASSWORD_ATTEMPTS, "5" },
			{ PASSWORD_ATTEMPT_WINDOW, "10" },
			{ MIN_REQUIRED_NON_ALPHANUM_CHARACTERS , "1"},
			{ MIN_REQUIRED_PASSWORD_LENGTH, "7"},
			{ PASSWORD_STRENGHT_REGEX, string.Empty },
			{ ENABLE_PASSWORD_RESET, "true"},
			{ ENABLE_PASSWORD_RETRIEVAL, "true"},
			{ REQUIRES_QUESTION_AND_ANSWER, "false"},
			{ REQUIRES_UNIQUE_EMAIL, "true"},
			{ PASSWORD_FORMAT, "Hashed"},
			{ AUTO_UNLOCK_TIME, "30"}
		};

		#endregion

		#region Overrides of MembershipProvider

		/// <summary>
		/// The minimum lenght required for a password.
		/// </summary>
		private int minRequiredPasswordLength = 1;

		/// <summary>
		/// Gets the minimum length required for a password.
		/// </summary>
		/// <returns>
		/// The minimum length required for a password. 
		/// </returns>
		public override int MinRequiredPasswordLength {
			get { return minRequiredPasswordLength; }
		}

		/// <summary>
		/// The minimum number of special characters that must be present in a valid password.
		/// </summary>
		private int minRequiredNonAlphanumericCharacters;

		/// <summary>
		/// Gets the minimum number of special characters that must be present in a valid password.
		/// </summary>
		/// <returns>
		/// The minimum number of special characters that must be present in a valid password.
		/// </returns>
		public override int MinRequiredNonAlphanumericCharacters {
			get { return minRequiredNonAlphanumericCharacters; }
		}

		/// <summary>
		/// A regular expression used to evaluate a password.
		/// </summary>
		private string passwordStrengthRegularExpression = string.Empty;

		/// <summary>
		/// Gets the regular expression used to evaluate a password.
		/// </summary>
		/// <returns>
		/// A regular expression used to evaluate a password.
		/// </returns>
		public override string PasswordStrengthRegularExpression {
			get { return passwordStrengthRegularExpression;  }
		}

		/// <summary>
		/// A value indicating whether the membership provider is configured to require a unique e-mail address for each user name.
		/// </summary>
		private bool requiresUniqueEmail;

		/// <summary>
		/// Gets a value indicating whether the membership provider is configured to require a unique e-mail address for each user name.
		/// </summary>
		/// <returns>
		/// true if the membership provider requires a unique e-mail address; otherwise, false. The default is true.
		/// </returns>
		public override bool RequiresUniqueEmail {
			get { return requiresUniqueEmail; }
		}

		/// <summary>
		/// The number of invalid password or password-answer attempts allowed before the membership user is locked out.
		/// </summary>
		private int maxInvalidPasswordAttempts = Int32.MaxValue;

		/// <summary>
		/// Gets the number of invalid password or password-answer attempts allowed before the membership user is locked out.
		/// </summary>
		/// <returns>
		/// The number of invalid password or password-answer attempts allowed before the membership user is locked out.
		/// </returns>
		public override int MaxInvalidPasswordAttempts {
			get { return maxInvalidPasswordAttempts; }
		}

		/// <summary>
		/// The number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.
		/// </summary>
		private int passwordAttemptWindow;

		/// <summary>
		/// Gets the number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.
		/// </summary>
		/// <returns>
		/// The number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.
		/// </returns>
		public override int PasswordAttemptWindow {
			get { return passwordAttemptWindow; }
		}

		/// <summary>
		/// Indicates whether the membership provider is configured to allow users to retrieve their passwords.
		/// </summary>
		private bool enablePasswordRetrieval;

		/// <summary>
		/// Indicates whether the membership provider is configured to allow users to retrieve their passwords.
		/// </summary>
		/// <returns>
		/// true if the membership provider is configured to support password retrieval; otherwise, false. The default is false.
		/// </returns>
		public override bool EnablePasswordRetrieval {
			get { return enablePasswordRetrieval; }
		}

		/// <summary>
		/// Indicates whether the membership provider is configured to allow users to reset their passwords.
		/// </summary>
		private bool enablePasswordReset;

		/// <summary>
		/// Indicates whether the membership provider is configured to allow users to reset their passwords.
		/// </summary>
		/// <returns>
		/// true if the membership provider supports password reset; otherwise, false. The default is true.
		/// </returns>
		public override bool EnablePasswordReset {
			get { return enablePasswordReset; }
		}

		/// <summary>
		/// A value indicating whether the membership provider is configured to require the user to answer a password question for password reset and retrieval.
		/// </summary>
		private bool requiresQuestionAndAnswer;

		/// <summary>
		/// Gets a value indicating whether the membership provider is configured to require the user to answer a password question for password reset and retrieval.
		/// </summary>
		/// <returns>
		/// true if a password answer is required for password reset and retrieval; otherwise, false. The default is true.
		/// </returns>
		public override bool RequiresQuestionAndAnswer {
			get { return requiresQuestionAndAnswer; }
		}

		/// <summary>
		/// A value indicating the format for storing passwords in the membership data store.
		/// </summary>
		private MembershipPasswordFormat passwordFormat;

		/// <summary>
		/// Gets a value indicating the format for storing passwords in the membership data store.
		/// </summary>
		/// <returns>
		/// One of the <see cref="T:System.Web.Security.MembershipPasswordFormat"/> values indicating the format for storing passwords in the data store.
		/// </returns>
		public override MembershipPasswordFormat PasswordFormat {
			get { return passwordFormat; }
		}

		/// <summary>
		/// The name of the application using the custom membership provider.
		/// </summary>
		/// <returns>
		/// The name of the application using the custom membership provider.
		/// </returns>
		public override string ApplicationName {
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		#endregion

		/// <summary>
		/// Specificira vrijeme u minutama nakon kojeg je korisnika moguce automatski odkljucati
		/// </summary>
		private int autoUnlockTime;

		/// <summary>
		/// Dohvaca vrijeme u minutama nakon kojeg je korisnika moguce automatski odkljucati
		/// </summary>
		public int AutoUnlockTime {
			get { return autoUnlockTime; }
		}

		/// <summary>
		/// Indicira da li je omoguceno automatsko odkljucavanje
		/// Atomatsko odkljucavanje je moguce ukoliko je broj minuta za automatsko odkljucavanje veci od 0
		/// </summary>
		public bool EnableAutoUnlock {
			get { return autoUnlockTime > 0; }
		}

		/// <summary>
		/// Repozitorij korisnika
		/// </summary>
		private IUsersRepository usersRepository;

		/// <summary>
		/// Koder lozinke.
		/// Koristiti PasswordCoder property zbog lazy evaluacije
		/// </summary>
		private IPasswordCoder passwordCoder;

		/// <summary>
		/// Dohvaca koder lozinke.
		/// </summary>
		protected IPasswordCoder PasswordCoder {
			get {
				if(passwordCoder == null) {
					setPasswordCoder();
				}

				return passwordCoder;
			}
		}

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Inicijalizacija providera
		/// </summary>
		/// <param name="name">naziv providera</param>
		/// <param name="config">konfiguracija providera</param>
		public override void Initialize(string name, NameValueCollection config) {
			if (config == null) {
				throw new ArgumentNullException("config");
			}

			if (String.IsNullOrEmpty(config["description"])) {
				config.Remove("description");
				config.Add("description", "NHibernate Membership Provider");
			}

			name = String.IsNullOrEmpty(name) ? "NHibernateMembershipProvider" : name;

			base.Initialize(name, config);

			ValidatingPassword += NHibernateMembershipProvider_ValidatingPassword;
			setConfigurationProperties(config);

			usersRepository = new UsersNHRepository();
			//setPasswordCoder(); // koristiti jedino ona ako se u toj metodi ne koristi Membership.HashAlgorithmType
		}

		/// <summary>
		/// Postavlja konfiguracijske postavke na temelju zadane konfiguracije.
		/// Ukoliko neke postavke nisu postavljene u zadanoj konfiguraciji koriste
		/// se defaultne postavke.
		/// </summary>
		/// <param name="config">zadana konfiguracija</param>
		private void setConfigurationProperties(NameValueCollection config) {
			minRequiredPasswordLength = Convert.ToInt32(getConfigValue(MIN_REQUIRED_PASSWORD_LENGTH, config));
			minRequiredNonAlphanumericCharacters = Convert.ToInt32(getConfigValue(MIN_REQUIRED_NON_ALPHANUM_CHARACTERS, config));
			passwordStrengthRegularExpression = getConfigValue(PASSWORD_STRENGHT_REGEX, config);
			//requiresUniqueEmail = Convert.ToBoolean(getConfigValue(REQUIRES_UNIQUE_EMAIL, config));
			requiresUniqueEmail = Convert.ToBoolean(defaultConfig[REQUIRES_UNIQUE_EMAIL]);
			maxInvalidPasswordAttempts = Convert.ToInt32(getConfigValue(MAX_INVALID_PASSWORD_ATTEMPTS, config));
			passwordAttemptWindow = Convert.ToInt32(getConfigValue(PASSWORD_ATTEMPT_WINDOW, config));
			enablePasswordRetrieval = Convert.ToBoolean(getConfigValue(ENABLE_PASSWORD_RETRIEVAL, config));
			enablePasswordReset = Convert.ToBoolean(getConfigValue(ENABLE_PASSWORD_RESET, config));
			requiresQuestionAndAnswer = Convert.ToBoolean(getConfigValue(REQUIRES_QUESTION_AND_ANSWER, config));
			passwordFormat = getPasswordFormat(getConfigValue(PASSWORD_FORMAT, config));
			autoUnlockTime = Convert.ToInt32(getConfigValue(AUTO_UNLOCK_TIME, config));
		}

		#endregion

		#region Methods

		/// <summary>
		/// Dohvaca format za spremanje/baratanje sa lozinkama
		/// </summary>
		/// <param name="passwordFormat">string reprezentacija formata za spremanje/baratanje sa lozinkama</param>
		/// <returns>format za spremanje/baratanje sa lozinkama</returns>
		private MembershipPasswordFormat getPasswordFormat(string passwordFormat) {
			MembershipPasswordFormat membershipPasswordFormat = 0;
			switch (passwordFormat) {
				case "Hashed":
					membershipPasswordFormat = MembershipPasswordFormat.Hashed;
					break;
				case "Encrypted":
					membershipPasswordFormat = MembershipPasswordFormat.Encrypted;
					break;
				case "Clear":
					membershipPasswordFormat = MembershipPasswordFormat.Clear;
					break;
				default:
					membershipPasswordFormat = MembershipPasswordFormat.Hashed;
					break;
			}

			return membershipPasswordFormat;
		}

		/// <summary>
		/// Dohvaca konfiguracijsku vrijednost. Ukoliko vrijednost iz konfiguracijske datoteke nije dosupna
		/// metoda vraca defaultnu konfiguracijsku vrijednost 
		/// </summary>
		/// <param name="configKey">kljuc za vrijednost konfiguracije</param>
		/// <param name="config">zadana konfiguracija</param>
		/// <returns>konfiguracijska vrijednost</returns>
		private static string getConfigValue(string configKey, NameValueCollection config) {
			return config[configKey] ?? defaultConfig[configKey];
		}

		/// <summary>
		/// Incijalizira i postavlja koder lozinke
		/// </summary>
		private void setPasswordCoder() {
			switch(passwordFormat) {
				case MembershipPasswordFormat.Clear:
					passwordCoder = new ClearPassCoder();
					break;
				case MembershipPasswordFormat.Encrypted:
					passwordCoder = new EncryptionPassCoder(EncryptPassword, DecryptPassword);
					break;
				case MembershipPasswordFormat.Hashed:
					// inace bolje je koristiti Membership.HashAlgorithmType, ali zbog toga sto se ova metoda
					// poziva u Initialize metodi, informacije iz Membership statickih metoda nisu dosupne (aplikacija puca)
					//Configuration cfg = WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
					//MembershipSection membershipSection = (MembershipSection) cfg.GetSection("system.web/membership");
					//passwordCoder = new HashPassCoder(HashAlgorithmFactory.Create(membershipSection.HashAlgorithmType));
					passwordCoder = new HashPassCoder(HashAlgorithmFactory.Create(Membership.HashAlgorithmType));
					break;
				default:
					throw new ProviderException("Unsupported password format.");
			}
		}

		#region Overrides of MembershipProvider

		/// <summary>
		/// Verifies that the specified user name and password exist in the data source.
		/// </summary>
		/// <returns>
		/// true if the specified username and password are valid; otherwise, false.
		/// </returns>
		/// <param name="username">The name of the user to validate. </param><param name="password">The password for the specified user. </param>
		public override bool ValidateUser(string username, string password) {
			User user = usersRepository.GetUser(username);
			if(user != null) {
				// provjeri da li je proslo vrijeme unutar kojeg se broje neuspjeli pokusaji
				user.CheckTrackingFailedAttempts(PasswordAttemptWindow);

				// provjeri da li je auto-unlock omogucen te da li je proslo dovoljno vremena za auto-unlock)
				if (user.IsLockedOut && EnableAutoUnlock && (user.LastLockoutDate.AddMinutes(AutoUnlockTime) < DateTime.Now)) {
					user.UnlockUser();
				}

				if(!user.IsLockedOut && user.IsApproved) {
					if (user.ComparePasswordTo(password, PasswordCoder)) {
						user.SetLastLoginDate();
						user.LastActivityDate = user.LastLoginDate;
						user.ResetFailedPasswordAttemptCount();
						user.ResetFailedPasswordAnswerAttemptCount();
						return true;
					} else {
						user.SetLastActivityDate();
						user.IncrementFailedPasswordAttemptCount();
						user.TryToLockoutUser(MaxInvalidPasswordAttempts);
					}	
				}
			}

			return false;
		}

		/// <summary>
		/// Adds a new membership user to the data source.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Web.Security.MembershipUser"/> object populated with the information for the newly created user.
		/// </returns>
		/// <param name="username">The user name for the new user.</param>
		/// <param name="password">The password for the new user.</param>
		/// <param name="email">The e-mail address for the new user.</param>
		/// <param name="passwordQuestion">The password question for the new user.</param>
		/// <param name="passwordAnswer">The password answer for the new user</param>
		/// <param name="isApproved">Whether or not the new user is approved to be validated.</param>
		/// <param name="providerUserKey">The unique identifier from the membership data source for the user.</param>
		/// <param name="status">A <see cref="T:System.Web.Security.MembershipCreateStatus"/> enumeration value indicating whether the user was created successfully.</param>
		public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion,
			string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status) {

			// validacija lozinke
			ValidatePasswordEventArgs validatePasswordEventArgs = new ValidatePasswordEventArgs(username, password, true);
			OnValidatingPassword(validatePasswordEventArgs);

			// ako username i lozinka ne zadovoljavaju
			if(validatePasswordEventArgs.Cancel) {
				status = MembershipCreateStatus.InvalidPassword;
				return null;
			}

			// provjera da li vec postoji ista email adresa ukoliko se trazi jedinstvena email adresa
			if(RequiresUniqueEmail) {
				string existedUserName = GetUserNameByEmail(email);
				if(!string.IsNullOrEmpty(existedUserName)) {
					status = MembershipCreateStatus.DuplicateEmail;
					return null;	
				}
			}

			// da li konfiguracija podrzava QuestionAndAnswer
			if(RequiresQuestionAndAnswer) {
				// ako da trebaju biti zadani pitanje i odgovor

				if(string.IsNullOrEmpty(passwordQuestion)) {
					status = MembershipCreateStatus.InvalidQuestion;
					return null;
				}

				if(string.IsNullOrEmpty(passwordAnswer)) {
					status = MembershipCreateStatus.InvalidAnswer;
					return null;
				}
			}

			// provjera da li vec posotji korisnik sa istim korisnickim imenom			
			User existingUser = usersRepository.GetUser(username);
			if(existingUser == null) {
				HousingMgmtUser user = new HousingMgmtUser(username, password, PasswordCoder) {
					Email = email,
					IsApproved = isApproved
				};

				if(RequiresQuestionAndAnswer) {
					user.ChangePasswordQuestionAndAnswer(password, passwordQuestion, passwordAnswer, PasswordCoder);	
				}

				usersRepository.SaveOrUpdate(user);
				status = MembershipCreateStatus.Success;

				NHibernateMembershipUser nHibernateMembershipUser = new NHibernateMembershipUser(user);

				return nHibernateMembershipUser;
			} else {
				status = MembershipCreateStatus.DuplicateUserName;
				return null;
			} 

		}


		/// <summary>
		/// Removes a user from the membership data source. 
		/// </summary>
		/// <returns>
		/// true if the user was successfully deleted; otherwise, false.
		/// </returns>
		/// <param name="username">The name of the user to delete.</param><param name="deleteAllRelatedData">true to delete data related to the user from the database; false to leave data related to the user in the database.</param>
		public override bool DeleteUser(string username, bool deleteAllRelatedData) {
			return usersRepository.DeleteUserByUserName(username);
		}

		/// <summary>
		/// Updates information about a user in the data source.
		/// </summary>
		/// <param name="user">A <see cref="T:System.Web.Security.MembershipUser"/> object that represents the user to update and the updated information for the user. </param>
		public override void UpdateUser(MembershipUser user) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Processes a request to update the password for a membership user.
		/// </summary>
		/// <returns>
		/// true if the password was updated successfully; otherwise, false.
		/// </returns>
		/// <param name="username">The user to update the password for. </param><param name="oldPassword">The current password for the specified user. </param><param name="newPassword">The new password for the specified user. </param>
		public override bool ChangePassword(string username, string oldPassword, string newPassword) {
			var user = usersRepository.GetUser(username);
			if(user == null) {
				return false;
			} else {
				return user.ChangePassword(oldPassword, newPassword, PasswordCoder);
			}
		}

		/// <summary>
		/// Processes a request to update the password question and answer for a membership user.
		/// </summary>
		/// <returns>
		/// true if the password question and answer are updated successfully; otherwise, false.
		/// </returns>
		/// <param name="username">The user to change the password question and answer for. </param><param name="password">The password for the specified user. </param><param name="newPasswordQuestion">The new password question for the specified user. </param><param name="newPasswordAnswer">The new password answer for the specified user. </param>
		public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer) {
			// da li je postavljena provjera pitanjem i odgovorom
			if(RequiresQuestionAndAnswer) {
				// dohvati korisnika za username
				// nad user objektnom pozovi promjenu pitanja i odgovora
				// ako metoda pozvana nad user objektom vrati false provjeri da li je potrebno korisnika zakljucati
				// vrati odgovor (true ili false)
				return false;
			} else {
				// ukoliko nije postavljena provjera pitanjem baci iznimku da operacija nije podrzana
				throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Gets the password for the specified user name from the data source.
		/// </summary>
		/// <returns>
		/// The password for the specified user name.
		/// </returns>
		/// <param name="username">The user to retrieve the password for. </param><param name="answer">The password answer for the user. </param>
		public override string GetPassword(string username, string answer) {
			// ako su lozinke spremljene u cistom obliku ili ako se modu dekriptirati
			if(EnablePasswordRetrieval &&
				(passwordFormat == MembershipPasswordFormat.Clear || passwordFormat == MembershipPasswordFormat.Encrypted)) {
				// dohvati korisnika za username

				// ako se zathjeva odgovor
				if(RequiresQuestionAndAnswer) {
					// nad korisnikom pozovi metodu getpassword sa passwordom i odgovorom
					// ako je odgovor metode null ili empty string provjeri da li je potrebno korisnika akljucati
				} else {
					// nad korisnikom pozovi metodu getpassword
				}

				// TODO: baca MembershipPasswordException ukoliko je upisan krivi odgovor
				// vrati password (null ili empty string ako nije tocan odgovor)
				return null;
			} else {
				// ukoliko dohvat lozinke nije podrzan/omogucen baci iznimku
				throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Resets a user's password to a new, automatically generated password.
		/// </summary>
		/// <returns>
		/// The new password for the specified user.
		/// </returns>
		/// <param name="username">The user to reset the password for. </param><param name="answer">The password answer for the specified user. </param>
		public override string ResetPassword(string username, string answer) {
			// ako sustav dozvoljava resetiranje lozinki
			if(EnablePasswordReset) {
				// TODO ukoliko sustav podrzava validaciju lozinke potreno je pozvati taj event: pogledaj metodu CreateUser o podizaju evenata

				// dohvati korisnika za username

				// ako se zahtjeva odgovor
				if(RequiresQuestionAndAnswer) {
					// nad korisnikom pozovi metodu resetpassword sa odgovorom
					// ako je odgovor metode null ili empty string provjeri da li je potrebno korisnika akljucati
				} else {
					// nad korisnikom pozovi metodu resetpassword
				}
				// TODO: baca MembershipPasswordException ukoliko je upisan krivi odgovor
				// vrati novu lozinku (vrati null ili empty string ako resetiranje nije uspjelo)
				return null;
			} else {
				// ukoliko resetiranje lozinke nije podrzano/omoguceno baci iznimkus
				throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Gets user information from the data source based on the unique identifier for the membership user. Provides an option to update the last-activity date/time stamp for the user.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Web.Security.MembershipUser"/> object populated with the specified user's information from the data source.
		/// </returns>
		/// <param name="providerUserKey">The unique identifier for the membership user to get information for.</param><param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
		public override MembershipUser GetUser(object providerUserKey, bool userIsOnline) {
			// TODO: ukoliko je userIsOnline postavljen na true potrebno je postaviti lastActivityDate
			// TODO: baca ProviderException ukoliko user nije pronaden ili ako nesto drugo nevalja
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets information from the data source for a user. Provides an option to update the last-activity date/time stamp for the user.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Web.Security.MembershipUser"/> object populated with the specified user's information from the data source.
		/// </returns>
		/// <param name="username">The name of the user to get information for. </param><param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user. </param>
		public override MembershipUser GetUser(string username, bool userIsOnline) {
			HousingMgmtUser user = usersRepository.GetUser(username) as HousingMgmtUser;
			if(user == null) {
				throw new ProviderException();
			}

			if(userIsOnline) {
				user.SetLastActivityDate();
			}


			NHibernateMembershipUser nHibernateMembershipUser = new NHibernateMembershipUser(user);

			return nHibernateMembershipUser;
		}

		/// <summary>
		/// Gets the user name associated with the specified e-mail address.
		/// </summary>
		/// <returns>
		/// The user name associated with the specified e-mail address. If no match is found, return null.
		/// </returns>
		/// <param name="email">The e-mail address to search for. </param>
		public override string GetUserNameByEmail(string email) {
			return usersRepository.GetUserNameByEmail(email);
		}


		/// <summary>
		/// Gets a collection of all the users in the data source in pages of data.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Web.Security.MembershipUserCollection"/> collection that contains a page of <paramref name="pageSize"/><see cref="T:System.Web.Security.MembershipUser"/> objects beginning at the page specified by <paramref name="pageIndex"/>.
		/// </returns>
		/// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex"/> is zero-based.</param><param name="pageSize">The size of the page of results to return.</param><param name="totalRecords">The total number of matched users.</param>
		public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the number of users currently accessing the application.
		/// </summary>
		/// <returns>
		/// The number of users currently accessing the application.
		/// </returns>
		public override int GetNumberOfUsersOnline() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets a collection of membership users where the user name contains the specified user name to match.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Web.Security.MembershipUserCollection"/> collection that contains a page of <paramref name="pageSize"/><see cref="T:System.Web.Security.MembershipUser"/> objects beginning at the page specified by <paramref name="pageIndex"/>.
		/// </returns>
		/// <param name="usernameToMatch">The user name to search for.</param><param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex"/> is zero-based.</param><param name="pageSize">The size of the page of results to return.</param><param name="totalRecords">The total number of matched users.</param>
		public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets a collection of membership users where the e-mail address contains the specified e-mail address to match.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Web.Security.MembershipUserCollection"/> collection that contains a page of <paramref name="pageSize"/><see cref="T:System.Web.Security.MembershipUser"/> objects beginning at the page specified by <paramref name="pageIndex"/>.
		/// </returns>
		/// <param name="emailToMatch">The e-mail address to search for.</param><param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex"/> is zero-based.</param><param name="pageSize">The size of the page of results to return.</param><param name="totalRecords">The total number of matched users.</param>
		public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Clears a lock so that the membership user can be validated.
		/// </summary>
		/// <returns>
		/// true if the membership user was successfully unlocked; otherwise, false.
		/// </returns>
		/// <param name="userName">The membership user whose lock status you want to clear.</param>
		public override bool UnlockUser(string userName) {
			User user = usersRepository.GetUser(userName);
			if (user != null) {
				user.UnlockUser();
				return true;
			}

			return false;
		}

		#endregion

		/// <summary>
		/// Validacija lozinke
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NHibernateMembershipProvider_ValidatingPassword(object sender, ValidatePasswordEventArgs e) {
			var errorMessage = "";
			var pwChar = e.Password.ToCharArray();

			//Check Length
			if (e.Password.Length < minRequiredPasswordLength) {
				errorMessage += "[Minimum length: " + minRequiredPasswordLength + "]";
				e.Cancel = true;
			}

			//Check Strength
			if (passwordStrengthRegularExpression != string.Empty) {
				Regex regex = new Regex(passwordStrengthRegularExpression);
				if (!regex.IsMatch(e.Password)) {
					errorMessage += "[Insufficient Password Strength]";
					e.Cancel = true;
				}
			}

			//Check Non-alpha characters
			Regex regexAlpha = new Regex(@"\w");
			int iNumNonAlpha = pwChar.Count(c => !char.IsLetterOrDigit(c));
			if (iNumNonAlpha < minRequiredNonAlphanumericCharacters) {
				errorMessage += "[Insufficient Non-Alpha Characters]";
				e.Cancel = true;
			}

			e.FailureInformation = new MembershipPasswordException(errorMessage);
		}

		#endregion
	}
}
