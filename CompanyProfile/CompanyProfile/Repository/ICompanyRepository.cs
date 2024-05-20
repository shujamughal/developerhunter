using CompanyProfile.Models;
using Microsoft.AspNetCore.Mvc;

namespace CompanyProfile.Repository
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetAllCompaniesAsync();
        Task<Company> GetCompanyByEmailAsync(string email);
        Task<int> AddCompanyAsync(Company company);
        Task UpdateCompanyAsync(Company company);
        Task <string> createUserCookie(string email);
        Task DeleteCompanyAsync(string email);
        Task<bool> UpdateCompanyEmailInCompanyProfileAsync(string existingEmail, string newEmail);
        Task SaveChangesAsync();
    }
}
