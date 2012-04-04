using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using ISHousingMgmt.Domain;
using ISHousingMgmt.Domain.BuildingMaintenance;
using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Web.Models;
using ISHousingMgmt.Web.Models.BuildingMaintenance;
using ISHousingMgmt.Web.Models.BuildingManagement;
using ISHousingMgmt.Web.Models.Legislature;
using ISHousingMgmt.Web.Models.PersonsAndRoles;
using ISHousingMgmt.Web.Models.Finances;
using ISHousingMgmt.Domain.Finances;

namespace ISHousingMgmt.Web.App_Start {
	public static class AutoMapperBootstrapper {

		public static void Bootstrap() {
			Mapper.CreateMap<City, CityModel>();
			Mapper.CreateMap<Cadastre, CadastreJsonModel>();
			Mapper.CreateMap<Cadastre, CadastreDetailModel>();
			Mapper.CreateMap<CadastralParticle, CadastralParticleDetailModel>();
			Mapper.CreateMap<LandRegistry, LandRegistryListModel>()
				.ForMember(dest => dest.NumberOfPartitionSpaces, opt => opt.MapFrom(src => src.PartitionSpaces.Count));
			Mapper.CreateMap<LandRegistry, LandRegistryDetailModel>();
			Mapper.CreateMap<Person, PersonModel>();
			Mapper.CreateMap<LegalPerson, PhysicalPersonModel>()
				.ForMember(dest => dest.Surname, opt => opt.Ignore());
			Mapper.CreateMap<LegalPerson, LegalPersonModel>();
			Mapper.CreateMap<PhysicalPerson, PhysicalPersonModel>();
			Mapper.CreateMap<PersonSnapshot, PersonModel>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.Name, opt => opt.Ignore())
				.ForMember(dest => dest.Telephones, opt => opt.Ignore());
			Mapper.CreateMap<IPartitionSpace, PartitionSpaceDetailModel>()
				.ForMember(dest => dest.LandRegistryId, opt => opt.MapFrom(src => src.LandRegistry.Id));
			Mapper.CreateMap<IPartitionSpace, PartitionSpaceListModel>();
			Mapper.CreateMap<Address, AddressModel>()
				.ForMember(dest => dest.Cities, opt => opt.Ignore());
			Mapper.CreateMap<Building, BuildingListModel>()
				.ForMember(dest => dest.SurfaceArea, opt => opt.MapFrom(src => src.LandRegistry.CadastralParticle.SurfaceArea))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.LandRegistry.CadastralParticle.Description));
			Mapper.CreateMap<Building, BuildingDetailModel>();
			Mapper.CreateMap<AdministrationJobsVoting, VotingVoteModel>()
				.ForMember(dest => dest.Vote, opt => opt.Ignore());
			Mapper.CreateMap<RepairService, RepairServiceModel>();
			Mapper.CreateMap<Contractor, ContractorModel>();
			Mapper.CreateMap<BuildingManager, BuildingManagerModel>();
			Mapper.CreateMap<AdministrationJobsVoting, AdminJobsVotingListModel>()
				.ForMember(dest => dest.Voted, opt => opt.MapFrom(src => src.OwnerVotes.Count));
			Mapper.CreateMap<AdministrationJobsVoting, AdminJobsVotingDetailModel>()
				.ForMember(dest => dest.OwnerVotesCount, opt => opt.MapFrom(src => src.OwnerVotes.Count));
			Mapper.CreateMap<MaintenanceRequest, MaintenanceRequestModel>();
			Mapper.CreateMap<MaintenanceRemark, MaintenanceDetailModel.MaintenanceRemarkModel>();
			Mapper.CreateMap<Maintenance, MaintenanceDetailModel>()
				.ForMember(dest => dest.BuildingManager, opt => opt.MapFrom(src => src.BuildingManager.LegalPerson));
			Mapper.CreateMap<BillItem, BillItemModel>();
			Mapper.CreateMap<Bill, BillModel>();
			Mapper.CreateMap<Reserve, ReserveModel>();
		}

	}
}