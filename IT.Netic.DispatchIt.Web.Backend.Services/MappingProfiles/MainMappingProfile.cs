using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using IT.Netic.DispatchIt.Web.Backend.DataContracts.BaseDto;
using IT.Netic.DispatchIt.Web.Backend.Domain.Entities;

namespace IT.Netic.DispatchIt.Web.Backend.Services.MappingProfiles
{
    public class MainMappingProfile : Profile
    {
        public MainMappingProfile()
        {
            CreateMap<CompanyBaseDto, Company>()
                .ForMember(company => company.CompanyId, opt => opt.DoNotAllowNull())
                .ForMember(company => company.VatNr, opt => opt.DoNotAllowNull())
                .ForMember(company => company.Name, opt => opt.DoNotAllowNull())
                .ForMember(company => company.Email, opt => opt.DoNotAllowNull())
                .ForMember(company => company.Owner, opt => opt.DoNotAllowNull());
            CreateMap<AddressBaseDto, Address>()
                .ForMember(address => address.AddressId, opt => opt.DoNotAllowNull())
                .ForMember(address => address.CompanyId, opt => opt.DoNotAllowNull());
            CreateMap<DeliveryAddressBaseDto, DeliveryAddress>()
                .ForMember(address => address.Country, opt => opt.DoNotAllowNull())
                .ForMember(address => address.Zipcode, opt => opt.DoNotAllowNull())
                .ForMember(address => address.City, opt => opt.NullSubstitute(""))
                .ForMember(address => address.Streetname, opt => opt.DoNotAllowNull())
                .ForMember(address => address.Number, opt => opt.DoNotAllowNull())
                .ForMember(address => address.Addition, opt => opt.NullSubstitute(""));
            CreateMap<ProgressBaseDto, Progress>()
                .ForMember(prog => prog.longitude, opt => opt.NullSubstitute(""))
                .ForMember(prog => prog.latitude, opt => opt.NullSubstitute(""));
            CreateMap<DeliveryrequestBaseDto, Deliveryrequest>()
                .ForMember(delivery => delivery.pickupAddressId, opt => opt.DoNotAllowNull())
                .ForMember(delivery => delivery.deliveryAddress, opt => opt.DoNotAllowNull())
                .ForMember(delivery => delivery.companyVat, opt => opt.DoNotAllowNull())
                .ForMember(delivery => delivery.companyId, opt => opt.DoNotAllowNull())
                .ForMember(delivery => delivery.deliveryId, opt => opt.DoNotAllowNull())
                .ForMember(delivery => delivery.title, opt => opt.NullSubstitute(""));
            CreateMap<MessageDto, Message>()
                .ForMember(prog => prog.Title, opt => opt.DoNotAllowNull())
                .ForMember(prog => prog.MessageId, opt => opt.DoNotAllowNull())
                .ForMember(prog => prog.Content, opt => opt.NullSubstitute(""))
                .ForMember(prog => prog.CreationDate, opt => opt.DoNotAllowNull())
                .ForMember(prog => prog.Read, opt => opt.DoNotAllowNull())
                .ForMember(prog => prog.User, opt => opt.DoNotAllowNull());
        }
    }
}
