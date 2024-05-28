using CompanyProfile.Models;
using CompanyProfile.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyProfile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "CompanyAdmin")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly JwtClass _jwtClass;

        public CompanyController(ICompanyRepository companyRepository, JwtClass jwtClass)
        {
            _companyRepository = companyRepository;
            _jwtClass = jwtClass;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetAllCompanies()
        {
            var companies = await _companyRepository.GetAllCompaniesAsync();
            return Ok(companies);
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<Company>> GetCompanyByEmail(string email)
        {
            var company = await _companyRepository.GetCompanyByEmailAsync(email);

            if (company == null)
            {
                return NotFound();
            }

            return Ok(company);
        }
        [HttpPost]
        public async Task<ActionResult> AddCompany([FromBody] Company company)
        {
            var existingCompany = await _companyRepository.GetCompanyByEmailAsync(company.Email);
            if (existingCompany != null)
            {
                return Conflict("Company with the same email already exists.");
            }
            var addCompanyResponse = await _companyRepository.AddCompanyAsync(company);
            if (addCompanyResponse == 200)
            {
                Console.WriteLine("yes its added ");
                return CreatedAtAction(nameof(GetCompanyByEmail), new { email = company.Email }, new { Message = "Company added successfully", StatusCode = 201, Company = company });

            }
            else
            {
                return StatusCode(addCompanyResponse);
            }
        }

        [HttpPut("{email}")]
        public async Task<bool> UpdateCompanyAsync(string email, [FromBody] Company newCompany)
        {
            try
            {
                var existingCompany = await _companyRepository.GetCompanyByEmailAsync(email);
                if (existingCompany == null)
                {
                    return false;
                }
                var emailExists = await _companyRepository.GetCompanyByEmailAsync(newCompany.Email);
                if (emailExists != null)
                {
                    return false;
                }
                await _companyRepository.DeleteCompanyAsync(email);
                var updatedCompany = new Company
                {
                    Email = newCompany.Email,
                    Username = newCompany.Username,
                };

                await _companyRepository.AddCompanyAsync(updatedCompany);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpDelete("{email}")]
        public async Task<ActionResult> DeleteCompany(string email)
        {
            await _companyRepository.DeleteCompanyAsync(email);
            return NoContent();
        }
    }
}
