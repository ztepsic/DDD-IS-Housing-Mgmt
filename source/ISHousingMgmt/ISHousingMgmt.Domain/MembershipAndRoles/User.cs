using System;
using Iesi.Collections.Generic;
using System.Text;
using System.Web.Security;
using ISHousingMgmt.Domain.Abstractions;

namespace ISHousingMgmt.Domain.MembershipAndRoles {

	/// <summary>
	/// Razred koji predstavlja korisnika
	/// </summary>
	public class User : NHibernateEntity {

		#region Members

		/// <summary>
		/// Preporucena duljina sistemski izgenerirane lozinke
		/// </summary>
		private const int SYSTEM_GENERATED_PASSWORD_LENGHT = 7;

		/// <summary>
		/// Preporuceni broj ne alfanumerickih znakova
		/// </summary>
		private const int NBR_OF_NON_ALPHANUMBERIC_CHARACTERS = 3;

		/// <summary>
		/// Korisnicko ime (dio primarnog kljuca)
		/// </summary>
		private string userName;

		/// <summary>
		/// Dohvaca korisnicko ime (dio primarnog kljuca)
		/// </summary>
		public virtual string UserName {
			get { return userName; }
			set { userName = value; }
		}

		/// <summary>
		/// Lozinka
		/// </summary>
		private string password;

		/// <summary>
		/// Dohvaca lozinku
		/// </summary>
		public virtual string Password {
			get { return password; }
		}

		/// <summary>
		/// Password salt - ukoliko je za format lozinke odabran sazetak lozinke (hashed password),
		/// generira se slucajna 16 byte vrijednost koja se lijepi na originalnu lozinku i originalni
		/// odgovor.
		/// </summary>
		private string passwordSalt;

		/// <summary>
		/// Dohvaca Password salt - ukoliko je za format lozinke odabran sazetak lozinke (hashed password),
		/// generira se slucajna vrijednost koja se lijepi na originalnu lozinku i originalni
		/// odgovor.
		/// </summary>
		public virtual string PasswordSalt {
			get { return passwordSalt; }
		}

		/// <summary>
		/// Indicira format spremanja lozinki
		/// </summary>
		private MembershipPasswordFormat passwordFormat;

		/// <summary>
		/// Dohvaca format spremanja lozinki
		/// </summary>
		public virtual MembershipPasswordFormat PasswordFormat {
			get { return passwordFormat; }
		}

		/// <summary>
		/// E-mail adresa korisnika
		/// </summary>
		private string email;

		/// <summary>
		/// Dohvaca ili postavlja e-mail adresu korisnika
		/// </summary>
		public virtual string Email {
			get { return email; }
			set { email = value.ToLower(); }
		}

		/// <summary>
		/// Opceniti komentari/informacije o korisnicku
		/// </summary>
		public virtual string Comment { get; set; }

		/// <summary>
		/// Datum i vrijeme kada je korisnik po prvi puta stvoren.
		/// </summary>
		private DateTime creationDate;
		
		/// <summary>
		/// Dohvaca datum i vrijeme kada je korisnik po prvi puta stvoren.
		/// [Protected] Postavlja datum i vrijeme u lokalnom vremenu kada je korisnik
		/// po prvi puta stvoren u UTC vremenu.
		/// </summary>
		public virtual DateTime CreationDate {
			get { return creationDate.ToLocalTime(); }
			protected set { 
				creationDate = value.ToUniversalTime();
				LastPasswordChangedDate = value;
			}
		}

		/// <summary>
		/// Indicira da li je korisniku dozvoljena prijava na sustav.
		/// Ukoliko korisnik unese ispravne podatke za prijavu/login
		/// prijava mora biti neuspjesna ako je IsApproved postavljen na
		/// false.
		/// </summary>
		public virtual bool IsApproved { get; set; }

		/// <summary>
		/// Indicira da li je korisnicki racun zakljucan zbog povrde sigurnosti.
		/// </summary>
		private bool isLockedOut;

		/// <summary>
		/// Dohvaca indikaciju da li je korisnicki racun zakljucan zbog povrde sigurnosti.
		/// </summary>
		public virtual bool IsLockedOut { get { return isLockedOut; } }

		/// <summary>
		/// Pitanje za korisnika u slucaju resetiranja ili dohvacanja(plain-text) lozinke
		/// Pitanje je moguce definirati tako da ga korisnik definira ili je
		/// definirano od strane sustava.
		/// </summary>
		private string passwordQuestion;

		/// <summary>
		/// Dohvaca pitanje za korisnika u slucaju resetiranja ili dohvacanja(plain-text) lozinke
		/// Pitanje je moguce definirati tako da ga korisnik definira ili je
		/// definirano od strane sustava.
		/// </summary>
		public virtual string PasswordQuestion { get { return passwordQuestion; } }

		/// <summary>
		/// Odgovor na passwordQuestion.
		/// </summary>
		private string passwordAnswer;

		/// <summary>
		/// Posljednji datum i vrijeme kada je korisnik bio aktivan.
		/// </summary>
		private DateTime lastActivityDate;

		/// <summary>
		/// Dohvaca posljednji datum i vrijeme kada je korisnik bio aktivan u lokalnom vremenu.
		/// Postavlja posljednji datum i vrijeme u lokalnom vremenu
		/// kada je korisnik bio aktivan u UTC vremenu.
		/// </summary>
		public virtual DateTime LastActivityDate {
			get { return lastActivityDate.ToLocalTime(); }
			set { lastActivityDate = value.ToUniversalTime(); }
		}

		/// <summary>
		/// Posljednji datum i vrijeme uspjesne prijave/logina korisnika.
		/// </summary>
		private DateTime lastLoginDate;

		/// <summary>
		/// Dohvaca posljednji datum i vrijeme uspjesne prijave/logina korisnika u lokalnom vremenu.
		/// Postavlja posljednji datum i vrijeme u lokalnom vremenz uspjesne prijave/logina korisnika
		/// u UTC vremenu.
		/// </summary>
		public virtual DateTime LastLoginDate {
			get { return lastLoginDate.ToLocalTime(); }
			set { lastLoginDate = value.ToUniversalTime(); }
		}

		/// <summary>
		/// Posljednji datum i vrijeme promjene lozinke.
		/// </summary>
		private DateTime lastPasswordChangedDate;

		/// <summary>
		/// Dohvaca posljedni datum i vrijeme promjene lozinke u lokalnom vremenu.
		/// [Protected] Postavlja posljednji datum i vrijeme u lokalnom vremenu promjene lozinke
		/// u UTC vremenu.
		/// </summary>
		public virtual DateTime LastPasswordChangedDate {
			get { return lastPasswordChangedDate.ToLocalTime(); }
			protected set { lastPasswordChangedDate = value.ToUniversalTime(); }
		}

		/// <summary>
		/// Posljednji datum i vrijeme zakljucavanja korisnickog racuna.
		/// </summary>
		private DateTime lastLockoutDate;

		/// <summary>
		/// Dohvaca posljednji datum i vrijeme zakljucavanja korisnickog racuna u lokalnom vremenu.
		/// Postavlja posljednji datum i vrijeme u lokalnom vremenu zakljucavanja
		/// korisnickog racuna u UTC vremenu.
		/// </summary>
		public virtual DateTime LastLockoutDate {
			get { return lastLockoutDate.ToLocalTime(); }
			protected set { lastLockoutDate = value.ToUniversalTime(); }
		}

		/// <summary>
		/// Broj pokusaja prijave sa pogresnom lozinkom.
		/// Broj ne sadrzi broj pokusaja dohvacanja ili resetiranja lozinke krivim odgovorom.
		/// </summary>
		private int failedPasswordAttemptCount = 0;

		/// <summary>
		/// Dohvaca broj pokusaja prijave sa pogresnom lozinkom.
		/// Broj ne sadrzi broj pokusaja dohvacanja ili resetiranja lozinke krivim odgovorom.
		/// </summary>
		public virtual int FailedPasswordAttemptCount {
			get { return failedPasswordAttemptCount; }
		}

		/// <summary>
		/// Broj pokusaja dohvacanja ili resetiranja lozinke krivim odgovorom.
		/// Broj ne sadrzi broj pokusaja prijave sa pogresnom lozinkom.
		/// </summary>
		private int failedPasswordAnswerAttemptCount = 0;

		/// <summary>
		/// Dohvaca broj pokusaja dohvacanja ili resetiranja lozinke krivim odgovorom.
		/// Broj ne sadrzi broj pokusaja prijave sa pogresnom lozinkom.
		/// </summary>
		public virtual int FailedPasswordAnswerAttemptCount {
			get { return failedPasswordAnswerAttemptCount; }
		}

		/// <summary>
		/// Pocetak vremena (UTC) od prvog pokusaja prijave pogresnom lozinkom
		/// </summary>
		private DateTime failedPasswordAttemptWindowStart;

		/// <summary>
		/// Dohvaca pocetak vremena (local time) od prvog pokusaja prijave pogresnom lozinkom.
		/// [Protected] Postavlja pocetak (UTC) od prvog pokusaja prijave pogresnom lozinkom.
		/// </summary>
		public virtual DateTime FailedPasswordAttemptWindowStart {
			get { return failedPasswordAttemptWindowStart.ToLocalTime(); }
			protected set { failedPasswordAttemptWindowStart = value.ToUniversalTime(); }
		}

		/// <summary>
		/// Pocetak vremena (UTC) od prvog pokusaja dohvacanja ili resetiranja lozinke pogresnim odgovorom
		/// </summary>
		private DateTime failedPasswordAnswerAttemptWindowStart;

		/// <summary>
		/// Dohvaca pocetak vremena (local time) od prvog pokusaja dohvacanja ili resetiranja lozinke pogresnim odgovorom.
		/// [Protected] Postavlja pocetak vremena (UTC) od prvog pokusaja dohvacanja ili resetiranja lozinke
		/// pogresnim odgovorom.
		/// </summary>
		public virtual DateTime FailedPasswordAnswerAttemptWindowStart {
			get { return failedPasswordAnswerAttemptWindowStart.ToLocalTime(); }
			protected set { failedPasswordAnswerAttemptWindowStart = value.ToUniversalTime(); }
		}

		private Iesi.Collections.Generic.ISet<Role> roles;

		public virtual Iesi.Collections.Generic.ISet<Role> Roles {
			get { return roles; }
		}

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		protected User() { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="username">korisnicko ime</param>
		/// <param name="password">lozinka</param>
		/// <param name="passwordCoder">koder lozinke pomocu kojeg se lozinka kodira</param>
		public User(string username, string password, IPasswordCoder passwordCoder) {
			this.userName = username;
			roles = new HashedSet<Role>();

			passwordFormat = passwordCoder.PasswordFormat;

			if(passwordFormat == MembershipPasswordFormat.Hashed) {
				passwordSalt = generatePasswordSalt();	
			}


			this.password = Encode(password, passwordCoder);

			CreationDate = DateTime.Now.ToUniversalTime();

		}

		#endregion

		#region Methods

		/// <summary>
		/// Dodaje ulogu korisniku
		/// </summary>
		/// <param name="role">uloga</param>
		public virtual void AddRole(Role role) {
			roles.Add(role);
		}

		/// <summary>
		/// Brise ulogu korisniku
		/// </summary>
		/// <param name="role">uloga</param>
		public virtual void RemoveRole(Role role) {
			roles.Remove(role);
		}

		/// <summary>
		/// Usporeduje zadanu lozinku sa trenutnom lozinkom instance
		/// </summary>
		/// <param name="otherPassword">lozinka koja se usporeduje s trenutnom lozinkom instance</param>
		/// <param name="passwordCoder">koder lozinke pomocu kojeg se lozinka moze dekodirati</param>
		/// <returns>true ukoliko su lozinke jednake, inace false</returns>
		public virtual bool ComparePasswordTo(string otherPassword, IPasswordCoder passwordCoder) {
			if (passwordFormat != passwordCoder.PasswordFormat) {
				throw new ArgumentException("PasswordCoder isn't suited for this password.");
			}

			var encodedPassword = Encode(otherPassword, passwordCoder);
			return encodedPassword == password;
		}

		/// <summary>
		/// Usporeduje zadani odgovor sa trenutnim odgovorom instance
		/// </summary>
		/// <param name="otherPasswordAnswer">odgovor koji se usporeduje s trenutnim odgovorom instance</param>
		/// <param name="passwordCoder">koder lozinke pomocu kojeg se lozinka moze dekodirati</param>
		/// <returns>true ukoliko su lozinke jednake, inace false</returns>
		public virtual bool ComparePasswordAnswerTo(string otherPasswordAnswer, IPasswordCoder passwordCoder) {
			if(string.IsNullOrEmpty(passwordQuestion)) {
				throw new InvalidOperationException("Password question and answer aren't set/defined.");
			}

			if (passwordFormat != passwordCoder.PasswordFormat) {
				throw new ArgumentException("PasswordCoder isn't suited for this password.");
			}

			var encodedPasswordAnswer = Encode(otherPasswordAnswer, passwordCoder);
			return encodedPasswordAnswer == passwordAnswer;
		}

		/// <summary>
		/// Dohvaca lozinku u plain-textu ukoliko je to moguce (ako nije hashirana).
		/// Metoda baca ArgumentException iznimku ukoliko koder ne odgovara formatu lozinke, te
		/// baca NotSupportedException ukoliko je lozinka hashirana.
		/// </summary>
		/// <param name="passwordCoder">koder lozinke pomocu kojeg se lozinka moze dekodirati</param>
		/// <returns>loznka u plain-textu ukoliko korisnik nije zakljucan, inace null</returns>
		public virtual string GetPassword(IPasswordCoder passwordCoder) {
			if(passwordFormat != passwordCoder.PasswordFormat) {
				throw new ArgumentException("PasswordCoder isn'd suited for this password.");
			}

			if(passwordFormat == MembershipPasswordFormat.Hashed) {
				throw new NotSupportedException("For hashed password retreival isn't supported.");
			}

			return !IsLockedOut
			       	? Encoding.Default.GetString(passwordCoder.Decode(Convert.FromBase64String(password)))
			       	: null;
		}

		/// <summary>
		/// Dohvaca lozinku u plain-textu ukoliko je to moguce i ukoliko je predan
		/// ispravan odgovor.
		/// Metoda baca ArgumentException iznimku ukoliko koder ne odgovara formatu lozinke, te
		/// baca NotSupportedExcepton ukoliko je lozinka hashirana.
		/// </summary>
		/// <param name="otherPasswordAnswer">odgovor za dohvacanje lozinke</param>
		/// <param name="passwordCoder">koder lozinke pomocu kojeg se lozinka moze dekodirati</param>
		/// <returns>loznka u plain-textu ukoliko je dohvacanje uspjesno, inace null</returns>
		public virtual string GetPassword(string otherPasswordAnswer, IPasswordCoder passwordCoder) {
			if (passwordFormat != passwordCoder.PasswordFormat) {
				throw new ArgumentException("PasswordCoder isn't suited for this password.");
			}

			if (passwordFormat == MembershipPasswordFormat.Hashed) {
				throw new NotSupportedException("For hashed password retreival isn't supported.");
			}

			if(!IsLockedOut) {
				if (ComparePasswordAnswerTo(otherPasswordAnswer, passwordCoder)) {
					return GetPassword(passwordCoder);
				} else {
					IncrementFailedPasswordAnswerAttemptCount();
				}	
			}

			return null;
			
		}

		/// <summary>
		/// Postavlja novu lozinku.
		/// </summary>
		/// <param name="oldPassword">stara lozinka</param>
		/// <param name="newPassword">nova lozinka</param>
		/// <param name="passwordCoder">koder lozinke pomocu kojeg se lozinka kodira</param>
		/// <returns>true ukoliko je promjena uspjela, false inace</returns>
		public virtual bool ChangePassword(string oldPassword, string newPassword, IPasswordCoder passwordCoder) {
			if (passwordFormat != passwordCoder.PasswordFormat) {
				throw new ArgumentException("PasswordCoder isn't suited for this password.");
			}

			if (IsLockedOut) {
				return false;
			}

			if (ComparePasswordTo(oldPassword, passwordCoder)) {
				password = Encode(newPassword, passwordCoder);
				LastPasswordChangedDate = DateTime.Now;
				return true;
			} else {
				IncrementFailedPasswordAttemptCount();
				return false;
			}
		}

		/// <summary>
		/// Mijenja pitanje i odgovor za dohvacanje ili resetiranje lozinke.
		/// </summary>
		/// <param name="password">trenutna lozinka</param>
		/// <param name="newPasswordQuestion">novo pitanje za dohvacanje ili resetiranje lozinke</param>
		/// <param name="newPasswordAnswer">novi odgovor na pitanje za dohvacanje ili resetiranje lozinke </param>
		/// <returns>true ukoliko je promjena uspjela, false inace</returns>
		public virtual bool ChangePasswordQuestionAndAnswer(string password,
			string newPasswordQuestion, string newPasswordAnswer, IPasswordCoder passwordCoder) {

			if (passwordFormat != passwordCoder.PasswordFormat) {
				throw new ArgumentException("PasswordCoder isn't suited for this password.");
			}

			if (IsLockedOut) {
				return false;
			}

			if (ComparePasswordTo(password, passwordCoder)) {
				passwordQuestion = newPasswordQuestion;

				var encodedNewPasswordAnswer = Encode(newPasswordAnswer, passwordCoder);
				passwordAnswer = encodedNewPasswordAnswer;
				return true;
			} else {
				IncrementFailedPasswordAttemptCount();
				return false;
			}
			
		}

		/// <summary>
		/// Resetira lozinku tako da izgenerira slucajan znakovni niz uz ispravan
		/// odgovor.
		/// </summary>
		/// <param name="passwordAnswer">odgovor na pitanje</param>
		/// <param name="minPasswordLenght">minimalna duljina lozinke</param>
		/// <param name="passwordCoder">koder lozinke pomocu kojeg se lozinka kodira</param>
		/// <returns></returns>
		public virtual string ResetPassword(string passwordAnswer, int minPasswordLenght, IPasswordCoder passwordCoder) {
			if (passwordFormat != passwordCoder.PasswordFormat) {
				throw new ArgumentException("PasswordCoder isn't suited for this password.");
			}

			if(IsLockedOut) {
				return null;
			}

			if(ComparePasswordAnswerTo(passwordAnswer, passwordCoder)) {
				return ResetPassword(minPasswordLenght, passwordCoder);
			} else {
				IncrementFailedPasswordAnswerAttemptCount();
				return null;
			}
		}

		/// <summary>
		/// Resetira lozinku tako da izgenerira slucajan znakovni niz
		/// </summary>
		/// <param name="minPasswordLenght">minimalna duljina lozinke</param>
		/// <param name="passwordCoder">koder lozinke pomocu kojeg se lozinka kodira</param>
		/// <returns>nova izgenerirana lozinka, null ako resetiranje nije uspjelo</returns>
		public virtual string ResetPassword(int minPasswordLenght, IPasswordCoder passwordCoder) {
			if (passwordFormat != passwordCoder.PasswordFormat) {
				throw new ArgumentException("PasswordCoder isn't suited for this password.");
			}

			if(IsLockedOut) {
				return null;
			}

			var passwordLenght = minPasswordLenght <= SYSTEM_GENERATED_PASSWORD_LENGHT
			                     	? SYSTEM_GENERATED_PASSWORD_LENGHT
			                     	: minPasswordLenght;
			var newPassword = generateRandomString(passwordLenght, NBR_OF_NON_ALPHANUMBERIC_CHARACTERS);
			password = Encode(newPassword, passwordCoder);

			return newPassword;

		}

		/// <summary>
		/// Pokusava zakljucati korisnika ukoliko je broj pokusaja davanja pogresne
		/// lozinke ili odgovora veci ili jednak od zadanog broja.
		/// </summary>
		/// <param name="maxInvalidPasswordAttempts">maksimalni broj mogucih pogresnih davanja lozinke ili odgovora</param>
		/// <returns>true ukoliko je zakljucavanje uspjelo, inace false.</returns>
		public virtual bool TryToLockoutUser(int maxInvalidPasswordAttempts) {
			if(maxInvalidPasswordAttempts <= 0) {
				throw new ArgumentException("The maxInvalidPasswordAttempts parameter must be greater than zero.");
			}

			if(!IsLockedOut && ((FailedPasswordAttemptCount >= maxInvalidPasswordAttempts) 
				|| (FailedPasswordAnswerAttemptCount >= maxInvalidPasswordAttempts))) {
				isLockedOut = true;
				LastLockoutDate = DateTime.Now;
			}

			return IsLockedOut;
		}

		/// <summary>
		/// Odkljucavanje korisnika
		/// </summary>
		/// <returns></returns>
		public virtual bool UnlockUser() {
			ResetFailedPasswordAttemptCount();
			ResetFailedPasswordAnswerAttemptCount();

			isLockedOut = false;

			return true;
		}

		/// <summary>
		/// Provjerava da li je vrijeme u kojem se cuvaju vrijednosti neuspjelih pokusaja
		/// proslo. Ukoliko je vrijeme proslo, brojila se restiraju na pocetne vrijednosti.
		/// </summary>
		/// <param name="passwordAttemptWindow">razdoblje u minutama u kojem treba cuvati vrijednosti neuspjelih
		/// pokusaja</param>
		public virtual void CheckTrackingFailedAttempts(int passwordAttemptWindow) {
			if ((FailedPasswordAttemptCount > 0) && (FailedPasswordAttemptWindowStart.AddMinutes(passwordAttemptWindow) < DateTime.Now)) {
				ResetFailedPasswordAttemptCount();
			}

			if((FailedPasswordAnswerAttemptCount > 0) && (FailedPasswordAnswerAttemptWindowStart.AddMinutes(passwordAttemptWindow) < DateTime.Now)) {
				ResetFailedPasswordAnswerAttemptCount();
			}
		}

		/// <summary>
		/// Povecava za jedan broj pokusaja prijave s pogresnom lozinkom.
		/// </summary>
		public virtual void IncrementFailedPasswordAttemptCount() {
			failedPasswordAttemptCount++;
			FailedPasswordAttemptWindowStart = DateTime.Now;
		}

		/// <summary>
		/// Resetira na nulu broj pokusaja prijave sa pogresnom lozinkom
		/// </summary>
		public virtual void ResetFailedPasswordAttemptCount() {
			failedPasswordAttemptCount = 0;
			FailedPasswordAttemptWindowStart = DateTime.MinValue;
		}

		/// <summary>
		/// Povecava za jedan broj pokusaja dohvacanja ili resetiranja lozinke pogresnim odgovorom
		/// </summary>
		public virtual void IncrementFailedPasswordAnswerAttemptCount() {
			failedPasswordAnswerAttemptCount++;
			FailedPasswordAnswerAttemptWindowStart = DateTime.Now;
		}

		/// <summary>
		/// Resetira na nulu broj pokusaja dohvacanja ili resetiranja lozinke pogresnim odgovorom
		/// </summary>
		public virtual void ResetFailedPasswordAnswerAttemptCount() {
			failedPasswordAnswerAttemptCount = 0;
			FailedPasswordAnswerAttemptWindowStart = DateTime.MinValue;
		}

		/// <summary>
		/// Indicira da li je korisnik aktivan unutar posljednjih Membership.UserIsOnlineTimeWindow
		/// minuta. Korisnik se razmatra da li je aktivan ukoliko je posljednje vrijeme
		/// aktivnosti jednako ili vece od posljednje uspjesne prijave.
		/// </summary>
		/// <param name="userIsOnlineTimeWindow">specificira broj minuta od posljednjem datuma i vremena aktivnosti untar kojih
		/// se korisnik smatra da je online</param>
		public virtual bool IsOnline(int userIsOnlineTimeWindow) {
			if(userIsOnlineTimeWindow <= 0) {
				throw new ArgumentException("The userIsOnlineTimeWindow parameter must be greater than zero.");
			}

			return (LastActivityDate >= LastLoginDate) 
				&& (DateTime.Now <= LastActivityDate.AddMinutes(userIsOnlineTimeWindow));

		}

		/// <summary>
		/// Postavlja datum i vrijeme posljednje aktivnosti na trenutnu vrijednost datuma i vremena
		/// </summary>
		public virtual void SetLastActivityDate() {
			LastActivityDate = DateTime.Now;
		}

		/// <summary>
		/// Postavlja datum i vrijeme posljednje uspjesne prijave na trenutnu vrijednost datuma i vremena
		/// </summary>
		public virtual void SetLastLoginDate() {
			LastLoginDate = DateTime.Now;
		}

		/// <summary>
		/// Izracunava novi oblik/zapis lozinke pomocu zadane lozinke i salta
		/// </summary>
		/// <param name="password">lozinka</param>
		/// <param name="passwordSalt">salt</param>
		/// <returns>novi oblik/zapis lozinke</returns>
		private byte[] calculatePasswordWithSalt(string password, string passwordSalt) {
			var passwordData = Encoding.Default.GetBytes(password + passwordSalt);
			//for (int i = 0; i < passwordData.Length; i++) {
			//    int byteToInt = Convert.ToInt32(passwordData[i]);
			//    byteToInt = (byteToInt ^ 31);
			//    passwordData[i] = Convert.ToByte(byteToInt);
			//}

			return passwordData;
		}

		/// <summary>
		/// Kodira lozinku ili odgovor odgovarajucim koderom
		/// </summary>
		/// <param name="stringForEncoding">string koja se kodira</param>
		/// <param name="passwordCoder">koder lozinke pomocu kojeg se lozinka kodira</param>
		/// <returns>kodiran string</returns>
		public virtual string Encode(string stringForEncoding, IPasswordCoder passwordCoder) {
			if (passwordCoder.PasswordFormat == MembershipPasswordFormat.Hashed) {
				if(string.IsNullOrEmpty(passwordSalt)) {
					throw new ApplicationException("Password salt can't be null or empty.");
				}

				var passwordData = calculatePasswordWithSalt(stringForEncoding, passwordSalt);
				return Convert.ToBase64String(passwordCoder.Encode(passwordData));
			} else {
				return Convert.ToBase64String(passwordCoder.Encode(Encoding.Default.GetBytes(stringForEncoding)));
			}
		}

		/// <summary>
		/// Generira salt lozinke
		/// </summary>
		/// <returns>salt lozinke</returns>
		private string generatePasswordSalt() {
			return generateRandomString(5, 2);
		}

		/// <summary>
		/// Generira slucajan niz znakova zadane duljine
		/// </summary>
		/// <param name="lenght">duljina znakovnog niza</param>
		/// <param name="numberOfNonAlphanumericCharacters">najmanji broj ne alfanumberičkih znakova</param>
		/// <returns></returns>
		private string generateRandomString(int lenght, int numberOfNonAlphanumericCharacters) {
			return Membership.GeneratePassword(lenght, numberOfNonAlphanumericCharacters);
		}

		#endregion
	}
}
