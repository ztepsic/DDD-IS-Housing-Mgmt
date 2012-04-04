USE HousingMgmt;
GO

CREATE TABLE Cities (
	Id INT IDENTITY PRIMARY KEY,
	Version INT NOT NULL DEFAULT 0,
	PostalCode INT NOT NULL UNIQUE,
	Name NVARCHAR(50) NOT NULL,
	CONSTRAINT CKPostalCode
		CHECK(PostalCode BETWEEN 10000 AND 60000)	
);

INSERT INTO Cities
VALUES
	(DEFAULT, 10000, 'Zagreb'),
	(DEFAULT, 21000, 'Split'),
	(DEFAULT, 31000, 'Osijek'),
	(DEFAULT, 51000, 'Rijeka');
GO

CREATE TABLE BuildingMaintenance.RepairServices (
	Id INT IDENTITY PRIMARY KEY,
	Version INT NOT NULL DEFAULT 0,
	Name NVARCHAR(256) NOT NULL
);

INSERT INTO BuildingMaintenance.RepairServices
VALUES
	(DEFAULT, 'Lièenje i bojanje'),
	(DEFAULT, 'Zamjena i popravak pokrova'),
	(DEFAULT, 'Keramièarski radovi'),
	(DEFAULT, 'Zamjena i popravak stolarije'),
	(DEFAULT, 'Održavanje rasvjete i drugih elektriènih ureðaja'),
	(DEFAULT, 'Održavanje nasada i staza'),
	(DEFAULT, 'Servis ureðaja za grijanje'),
	(DEFAULT, 'Servis dizala'),
	(DEFAULT, 'Servis na instalacijama vodovoda i kanalizacije'),
	(DEFAULT, 'Servis na instalacijama plina'),
	(DEFAULT, 'Servis na instalacijama el. energije');

GO

CREATE TABLE PersonsAndRoles.Persons (
	Id INT IDENTITY PRIMARY KEY,
	Version INT NOT NULL DEFAULT 0,
	OType NVARCHAR(30) NOT NULL,
	Oib NCHAR(11) NOT NULL UNIQUE,
	Name NVARCHAR(30) NOT NULL,
	PP_Surname NVARCHAR(30) NULL,
	LP_NumberOfBankAccount NVARCHAR(17) NULL,
	CityId INTEGER NULL,
	StreetAddress NVARCHAR(50) NOT NULL,
	StreetAddressNumber NVARCHAR(10) NOT NULL
	CONSTRAINT FK_Cities_CityId
		FOREIGN KEY (CityId) REFERENCES Cities(Id)
			ON DELETE SET NULL
			ON UPDATE CASCADE
);
    
    
CREATE TABLE PersonsAndRoles.PersonTelephones (
	PersonId INT NOT NULL,
	NameOfTelephoneNumber NVARCHAR(30) NOT NULL,
	TelephoneNumber NVARCHAR(30) NOT NULL,
	CONSTRAINT PK_PersonTelephones
		PRIMARY KEY(PersonId, NameOfTelephoneNumber, TelephoneNumber),
	CONSTRAINT FK_Persons_PersonId
		FOREIGN KEY (PersonId) REFERENCES PersonsAndRoles.Persons(Id)
			ON DELETE CASCADE
			ON UPDATE CASCADE
);

GO

CREATE TABLE PersonsAndRoles.Contractors(
	Id INT IDENTITY PRIMARY KEY,
	Version INT NOT NULL DEFAULT 0,
	LegalPersonId INTEGER NOT NULL,
	CONSTRAINT FK_Contractors_Persons_PersonId
		FOREIGN KEY (LegalPersonId) REFERENCES PersonsAndRoles.Persons(Id)
			ON DELETE CASCADE
			ON UPDATE CASCADE
);

CREATE TABLE PersonsAndRoles.ContractorRepairServices(
	ContractorId INT NOT NULL,
	RepairServiceId INT NOT NULL,
	CONSTRAINT PK_ContractorId_RepairServiceId
		PRIMARY KEY(ContractorId, RepairServiceId),
	CONSTRAINT FK_ContactorRS_Contractors_ContactorId
		FOREIGN KEY (ContractorId) REFERENCES PersonsAndRoles.Contractors(Id)
			ON DELETE CASCADE
			ON UPDATE CASCADE,
	CONSTRAINT FK_ContractorRS_RepairServices_RepairServiceId
		FOREIGN KEY (RepairServiceId) REFERENCES BuildingMaintenance.RepairServices(Id)
			ON DELETE CASCADE
			ON UPDATE CASCADE
);
    
GO

CREATE TABLE PersonsAndRoles.BuildingManagers(
	Id INT IDENTITY PRIMARY KEY,
	Version INT NOT NULL DEFAULT 0,
	LegalPersonId INTEGER NOT NULL,
	CONSTRAINT FK_BuildMagmts_Persons_PersonId
		FOREIGN KEY (LegalPersonId) REFERENCES PersonsAndRoles.Persons(Id)
			ON DELETE CASCADE
			ON UPDATE CASCADE
);

CREATE TABLE PersonsAndRoles.BuildingManagerContractors(
	BuildingManagerId INT NOT NULL,
	ContractorId INT NOT NULL,
	CONSTRAINT PK_BuildingMgmtCont
		PRIMARY KEY(BuildingManagerId, ContractorId),
	CONSTRAINT FK_BMC_BuildingManagers_BuildingManagerId
		FOREIGN KEY (BuildingmanagerId) REFERENCES PersonsAndRoles.BuildingManagers(Id)
			ON DELETE NO ACTION
			ON UPDATE NO ACTION,
	CONSTRAINT FK_BuildignMgmtCont_Contractors_ContactorId
		FOREIGN KEY (ContractorId) REFERENCES PersonsAndRoles.Contractors(Id)
			ON DELETE CASCADE
			ON UPDATE CASCADE,
	
);

GO

CREATE TABLE MembershipAndRoles.Users(
	Id INT IDENTITY PRIMARY KEY,
	Version INT NOT NULL DEFAULT 0,
	OType NVARCHAR(30) NOT NULL,
	UserName NVARCHAR(50) NOT NULL UNIQUE,
	Password NVARCHAR(128) NOT NULL,
	PasswordFormat INT NOT NULL DEFAULT 1,
	PasswordSalt NCHAR(5),
	Email NVARCHAR(100) UNIQUE,
	Comment NTEXT,
	CreationDate DATETIME2 NOT NULL,
	IsApproved BIT NOT NULL DEFAULT 0,
	IsLockedOut BIT NOT NULL DEFAULT 0,
	PasswordQuestion NVARCHAR(256),
	PasswordAnswer NVARCHAR(128),
	LastActivityDate DATETIME2 NOT NULL,
	LastLoginDate DATETIME2 NOT NULL,
	LastPasswordChangedDate DATETIME2 NOT NULL,
	LastLockoutDate DATETIME2 NOT NULL,
	FailedPasswordAttemptCount INT NOT NULL DEFAULT 0,
	FailedPasswordAnswerAttemptCount INT NOT NULL DEFAULT 0,
	FailedPasswordAttemptWindowStart DATETIME2 NOT NULL,
	FailedPasswordAnswerAttemptWindowStart DATETIME2 NOT NULL
);

ALTER TABLE MembershipAndRoles.Users ADD
	PersonId INT,
	CONSTRAINT FK_Persons_PersonId
		FOREIGN KEY (PersonId) REFERENCES PersonsAndRoles.Persons(Id)
			ON DELETE CASCADE
			ON UPDATE CASCADE;
			
CREATE TABLE MembershipAndRoles.Roles(
	Id INT IDENTITY PRIMARY KEY,
	Version INT NOT NULL DEFAULT 0,
	Name NVARCHAR(100) NOT NULL UNIQUE
);

INSERT INTO MembershipAndRoles.Roles
VALUES
	(DEFAULT, 'BuildingManager'),
	(DEFAULT, 'Contractor'),
	(DEFAULT, 'Owner'),
	(DEFAULT, 'Representative');

CREATE TABLE MembershipAndRoles.UserRoles(
	UserId INT NOT NULL,
	RoleId INT NOT NULL,
	CONSTRAINT PK_UserRoles
		PRIMARY KEY(UserId, RoleId),
	CONSTRAINT FK_UR_UserId
		FOREIGN KEY (UserId) REFERENCES MembershipAndRoles.Users(Id)
			ON DELETE CASCADE
			ON UPDATE CASCADE,
	CONSTRAINT FK_UR_RoleId			
		FOREIGN KEY (RoleId) REFERENCES MembershipAndRoles.Roles(Id)
				ON DELETE CASCADE
				ON UPDATE CASCADE
);

GO

CREATE TABLE Legislature.Cadastres(
	Id INT IDENTITY PRIMARY KEY,
	Version INT NOT NULL DEFAULT 0,
	CadastralDistrict NVARCHAR(100) NOT NULL,
	Mbr INT NOT NULL,
	CityId INT NOT NULL,
	CONSTRAINT FK_Cadastr_Cities_CityId
		FOREIGN KEY(CityId) REFERENCES Cities(Id)
			ON DELETE CASCADE
			ON UPDATE CASCADE
);

INSERT INTO Legislature.Cadastres
VALUES
	(DEFAULT, 'Èrnomerec', '335266', 1),
	(DEFAULT, 'Dubrava', '335304', 1),
	(DEFAULT, 'Maksimir', '335339', 1),
	(DEFAULT, 'Podsused', '335584', 1),
	(DEFAULT, 'Trešnjevka', '335622', 1),
	(DEFAULT, 'Trnje', '335649', 1)
;

GO

CREATE TABLE Legislature.LandRegistries(
	Id INT IDENTITY PRIMARY KEY,
	Version INT NOT NULL DEFAULT 0,
	CadastreId INTEGER NOT NULL,
	NumberOfCadastralParticle NVARCHAR(100) NOT NULL,
	SurfaceArea DECIMAL(16,6) NOT NULL,
	Description NTEXT NOT NULL,
	Locked BIT NOT NULL DEFAULT 0,
	CONSTRAINT FK_LR_Cadastres_CadastreId
		FOREIGN KEY(CadastreId) REFERENCES Legislature.Cadastres(Id)
			ON DELETE CASCADE
			ON UPDATE CASCADE	
);

CREATE TABLE Legislature.PartitionSpaces(
	Id INT IDENTITY PRIMARY KEY,
	Version INT NOT NULL DEFAULT 0,
	RegistryNumber NVARCHAR(100) NOT NULL UNIQUE,
	OrdinalNumber INTEGER NOT NULL,
	SurfaceArea DECIMAL(16,6) NOT NULL,
	Description NTEXT NOT NULL,
	PersonId INTEGER,
	LandRegistryId INTEGER NOT NULL,
	CONSTRAINT FK_PS_Persons_PersonId
		FOREIGN KEY(PersonId) REFERENCES PersonsAndRoles.Persons(Id)
			ON DELETE CASCADE
			ON UPDATE CASCADE,
	CONSTRAINT FK_PS_LandRegistries_LandRegistryId
		FOREIGN KEY(LandRegistryId) REFERENCES Legislature.LandRegistries(Id)
			ON DELETE NO ACTION
			ON UPDATE NO ACTION
);

GO

CREATE TABLE BuildingManagement.Buildings(
	Id INT IDENTITY PRIMARY KEY,
	Version INT NOT NULL DEFAULT 0,
	CityId INTEGER NULL,
	StreetAddress NVARCHAR(50) NOT NULL,
	StreetAddressNumber NVARCHAR(10) NOT NULL,
	BuildingManagerId INT,
	ReprePersonId INT,
	LandRegistryId INT,
	ReserveCoefficient DECIMAL(9, 2),
	CONSTRAINT FK_B_Cities_Id
		FOREIGN KEY (CityId) REFERENCES Cities(Id)
			ON DELETE SET NULL
			ON UPDATE CASCADE,
	CONSTRAINT FK_B_BuildingManager_Id
		FOREIGN KEY(BuildingManagerId) REFERENCES PersonsAndRoles.BuildingManagers(Id)
			ON DELETE SET NULL
			ON UPDATE NO ACTION,
	CONSTRAINT FK_B_Persons_Id
		FOREIGN KEY(ReprePersonId) REFERENCES PersonsAndRoles.Persons(Id)
			ON DELETE NO ACTION
			ON UPDATE NO ACTION,
	CONSTRAINT FK_B_LandRegistries_Id
		FOREIGN KEY(LandRegistryId) REFERENCES Legislature.LandRegistries(Id)
			ON DELETE NO ACTION
			ON UPDATE NO ACTION
);

GO

CREATE TABLE BuildingManagement.AdministrationJobsVotings(
	Id INT IDENTITY PRIMARY KEY,
	Version INT NOT NULL DEFAULT 0,
	AdministrationJobsType INT NOT NULL,
	Subject NVARCHAR(50) NOT NULL,
	Description NTEXT NOT NULL,
	StartDateTime DATETIME2 NOT NULL,
	EndDateTime DATETIME2 NOT NULL,
	NumberOfOwners INT NOT NULL,
	IsFinished BIT NOT NULL DEFAULT 0,
	IsAccepted BIT NOT NULL DEFAULT 0,
	BuildingId INT NOT NULL,
	CONSTRAINT FK_AJV_Buildings_Id
		FOREIGN KEY(BuildingId) REFERENCES BuildingManagement.Buildings(Id)
			ON DELETE CASCADE
			ON UPDATE CASCADE
);

CREATE TABLE BuildingManagement.OwnerVotes(
	AdminJobsVotingId INT NOT NULL,	
	Vote BIT NOT NULL DEFAULT 0,
	ShareOfTotalOwnership DECIMAL(11, 10) NOT NULL DEFAULT 0,
	FullName NVARCHAR(100) NOT NULL,
	Oib NCHAR(11) NOT NULL,
	CityId INT,
	StreetAddress NVARCHAR(50) NOT NULL,
	StreetAddressNumber NVARCHAR(10) NOT NULL,
	CONSTRAINT PK_OwnerVotes
		PRIMARY KEY(AdminJobsVotingId, Oib),
	CONSTRAINT FK_OV_AdminJobsVoting_Id
		FOREIGN KEY (AdminJobsVotingId) REFERENCES BuildingManagement.AdministrationJobsVotings(Id)
			ON DELETE CASCADE
			ON UPDATE CASCADE,
	CONSTRAINT FK_OV_Cities_Id
		FOREIGN KEY (CityId) REFERENCES Cities(Id)
			ON DELETE NO ACTION
			ON UPDATE NO ACTION
);
  
GO

CREATE TABLE Finances.Reserves (
	Id INT IDENTITY PRIMARY KEY,
	Version INT NOT NULL DEFAULT 0,
	Money DECIMAL(18, 2) NOT NULL,
	BuildingId INT NOT NULL UNIQUE,
	CONSTRAINT FK_R_Buildings_Id
	FOREIGN KEY (BuildingId) REFERENCES BuildingManagement.Buildings(Id)
		ON DELETE CASCADE
		ON UPDATE CASCADE
);

GO

CREATE TABLE Finances.Bills (
	Id INT IDENTITY PRIMARY KEY,
	Version INT NOT NULL DEFAULT 0,
	DateTimeIssued DATETIME2 NOT NULL,
	Tax INT NOT NULL,
	ReferenceNumber NVARCHAR(100) NOT NULL,
	PaymentDescription NTEXT NOT NULL,
	IsPaid BIT NOT NULL,
	PaidDateTime DATETIME2,
	Reserve INT NOT NULL,
	FromFullName NVARCHAR(100),
	FromOib NCHAR(11),
	FromNumberOfBankAccount NVARCHAR(17),
	FromCityId INT,
	FromStreetAddress NVARCHAR(50),
	FromStreetAddressNumber NVARCHAR(10),
	ToFullName  NVARCHAR(100),
	ToOib NCHAR(11),
	ToCityId INT,
	ToStreetAddress NVARCHAR(50),
	ToStreetAddressNumber NVARCHAR(10),
	CONSTRAINT FK_B_Reserves_Id
	FOREIGN KEY (Reserve) REFERENCES Finances.Reserves(Id)
		ON DELETE NO ACTION
		ON UPDATE CASCADE,
	CONSTRAINT FK_BF_Cities_Id
	FOREIGN KEY (FromCityId) REFERENCES Cities(Id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION,
	CONSTRAINT FK_BT_Cities_Id
	FOREIGN KEY (ToCityId) REFERENCES Cities(Id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
);
  
CREATE NONCLUSTERED INDEX IX_B_ToOib ON Finances.Bills(ToOib);
CREATE NONCLUSTERED INDEX IX_B_FromOib ON Finances.Bills(FromOib);

CREATE TABLE Finances.BillItems (
	Id INT IDENTITY PRIMARY KEY,
	BillId INT NOT NULL,
	Quantity INT NOT NULL,
	Price DECIMAL(18, 2) NOT NULL,
	Description NTEXT NOT NULL,	
	CONSTRAINT FK_BI_Bills_Id
	FOREIGN KEY (BillId) REFERENCES Finances.Bills(Id)
		ON DELETE CASCADE
		ON UPDATE CASCADE
);

GO

CREATE TABLE BuildingMaintenance.Maintenances (
	Id INT IDENTITY PRIMARY KEY,
	Version INT NOT NULL DEFAULT 0,
	Urgency INT NOT NULL DEFAULT 1,
	StatusOfMaintenance INT NOT NULL DEFAULT 0,
	Instructions NTEXT,
	CompletitionDateTime	DATETIME2,
	ServiceType INT NOT NULL,
	BuildingId INT NOT NULL,
	Bill INT,
	ContractorsConclusion NTEXT,
	ContractorFullName NVARCHAR(100),
	ContractorOib NCHAR(11),
	ContractorCityId INT,
	ContractorStreetAddress NVARCHAR(50),
	ContractorStreetAddressNumber NVARCHAR(10),
	SubmitterFullName NVARCHAR(100) NOT NULL,
	SubmitterOib NCHAR(11) NOT NULL,
	SubmitterCityId INT,
	SubmitterStreetAddress NVARCHAR(50) NOT NULL,
	SubmitterStreetAddressNumber NVARCHAR(10) NOT NULL,
	Subject NVARCHAR(100) NOT NULL,
	Description NTEXT NOT NULL,
	Location NVARCHAR(100) NOT NULL,
	DateTimeOfRequest DATETIME2 NOT NULL,
	CONSTRAINT FK_M_RepairServices_Id
		FOREIGN KEY (ServiceType) REFERENCES BuildingMaintenance.RepairServices(Id)
			ON DELETE NO ACTION
			ON UPDATE CASCADE,
	CONSTRAINT FK_M_Buildings_Id
		FOREIGN KEY (BuildingId) REFERENCES BuildingManagement.Buildings(Id)
			ON DELETE NO ACTION
			ON UPDATE CASCADE,
	CONSTRAINT FK_M_Bills_Id
		FOREIGN KEY (Bill) REFERENCES Finances.Bills(Id)
			ON DELETE NO ACTION
			ON UPDATE NO ACTION,
	CONSTRAINT FK_Mc_Cities_Id
		FOREIGN KEY (ContractorCityId) REFERENCES Cities(Id)
			ON DELETE NO ACTION
			ON UPDATE NO ACTION,
		CONSTRAINT FK_MS_Cities_Id
		FOREIGN KEY (SubmitterCityId) REFERENCES Cities(Id)
			ON DELETE NO ACTION
			ON UPDATE NO ACTION
);

CREATE NONCLUSTERED INDEX IX_M_ContractorOib ON BuildingMaintenance.Maintenances(ContractorOib);
CREATE NONCLUSTERED INDEX IX_M_SubmitterOib ON BuildingMaintenance.Maintenances(SubmitterOib);

CREATE TABLE BuildingMaintenance.MaintenanceRemarks(
	Id INT IDENTITY PRIMARY KEY,
	MaintenanceId INT NOT NULL,
	Remark NTEXT NOT NULL,
	RemarkDateTime DATETIME2 NOT NULL,
	CONSTRAINT FK_MR_Maintenances_Id
	FOREIGN KEY (MaintenanceId) REFERENCES BuildingMaintenance.Maintenances(Id)
		ON DELETE CASCADE
		ON UPDATE CASCADE
);

GO
