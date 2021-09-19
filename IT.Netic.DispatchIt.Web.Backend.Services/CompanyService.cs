using AutoMapper;
using IT.Netic.DispatchIt.Web.Backend.DataContracts.BaseDto;
using IT.Netic.DispatchIt.Web.Backend.Domain.Entities;
using IT.Netic.DispatchIt.Web.Backend.Domain.Repository;
using IT.Netic.DispatchIt.Web.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace IT.Netic.DispatchIt.Web.Backend.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private EventGridClient eventGridClient;
        private string topicHostname;

        public CompanyService(ICompanyRepository companyRepository, IAddressRepository addressRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _addressRepository = addressRepository;
            _mapper = mapper;

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = configBuilder.Build();

            var topicEndpoint = configuration.GetConnectionString("TopicEndpoint");
            var topicKey = configuration.GetConnectionString("TopicKey");

            topicHostname = new Uri(topicEndpoint).Host;

            TopicCredentials topicCredentials = new TopicCredentials(topicKey);
            eventGridClient = new EventGridClient(topicCredentials);
        }

        public async Task<IActionResult> SendCreateAddressEvent(AddressBaseDto address)
        {
            if (string.IsNullOrEmpty(address.Streetname) || string.IsNullOrEmpty(address.Number) || string.IsNullOrEmpty(address.Zipcode) || string.IsNullOrEmpty(address.Country))
            {
                return new BadRequestObjectResult("Address is incomplete.");
            }

            try
            {
                EventGridEvent eventGridEvent = new EventGridEvent()
                {
                    Id = Guid.NewGuid().ToString(),
                    EventType = "Addresses.Create",
                    Data = address,
                    EventTime = DateTime.Now,
                    Subject = "addresses/create",
                    DataVersion = "1.0"
                };
                List<EventGridEvent> eventsList = new List<EventGridEvent>();
                eventsList.Add(eventGridEvent);
                await eventGridClient.PublishEventsAsync(topicHostname, eventsList);
                return new OkObjectResult($"Successfully sent add addresses event");
            }
            catch (Exception)
            {
                return new BadRequestObjectResult("Failed to send event");
            }
        }

        public async Task<IActionResult> SendCreateCompanyEvent(CompanyBaseDto company)
        {
            if (string.IsNullOrEmpty(company.VatNr))
            {
                return new BadRequestObjectResult("Property VAT-number can not be null or empty");
            }

            var companyInformation = await _companyRepository.GetByVat(company.VatNr);
            if (companyInformation != null)
            {

                return new BadRequestObjectResult($"Company with VAT Number {company.VatNr} already exists");
            }

            try
            {
                EventGridEvent eventGridEvent = new EventGridEvent()
                {
                    Id = Guid.NewGuid().ToString(),
                    EventType = "Companies.Create",
                    Data = company,
                    EventTime = DateTime.Now,
                    Subject = "companies/create",
                    DataVersion = "1.0"
                };
                List<EventGridEvent> eventsList = new List<EventGridEvent>();
                eventsList.Add(eventGridEvent);
                await eventGridClient.PublishEventsAsync(topicHostname, eventsList);
                return new OkObjectResult($"Successfully sent add company event");
            }
            catch (Exception)
            {
                return new BadRequestObjectResult("Failed to send event");
            }
        }

        public async Task<IActionResult> CreateAddress(AddressBaseDto address)
        {
            var entity = _mapper.Map<Address>(address);
            await _addressRepository.Create(entity);
            return new OkObjectResult($"Successfully added address");
        }

        public async Task<IActionResult> CreateCompany(CompanyBaseDto company)
        {
            var entity = _mapper.Map<Company>(company);
            await _companyRepository.Create(entity);
            return new OkObjectResult($"Successfully created company {company.Name}");
        }

        public async Task<IActionResult> GetCompanyByOwner(string owner)
        {
            var companies = await _companyRepository.GetByOwner(owner);
            
            if (companies == null)
            {
                return new BadRequestObjectResult("No data found!");
            }

            foreach(Company c in companies)
            {
                c.addresses = await _addressRepository.GetByCompany(c.CompanyId);
            }

            return new OkObjectResult(companies);
        }

        public async Task<IActionResult> GetCompany(int id)
        {
            var company = await _companyRepository.GetById(id);
            company.addresses = await _addressRepository.GetByCompany(company.CompanyId);
            return new OkObjectResult(company);
        }

        public async Task<IActionResult> PutCompany(CompanyBaseDto company)
        {
            var entity = _mapper.Map<Company>(company);
            await _companyRepository.Update(entity, t => t.CompanyId == company.CompanyId);
            return new OkObjectResult($"Successfully Updated company {company.Name}");
        }

        public async Task<IActionResult> DeleteCompany(int company)
        {
            await _companyRepository.Delete(t => t.CompanyId == company);
            return new OkObjectResult($"Successfully Deleted company {company}");
        }
    }
}
