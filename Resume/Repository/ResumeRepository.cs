using Microsoft.EntityFrameworkCore;
using Resume.Controllers;

namespace Resume.Repository
{
    public class ResumeRepository:IResumeRepository
    {
        private readonly ResumeContext _context;
        private readonly ILogger<ResumeController> _logger;
        public ResumeRepository(ResumeContext context)
        {
            _context = context;
        }   

        public async Task<ResumePdf> AddResume(ResumePdf resumePdf)
        {
           _context.resumes.Add(resumePdf);
            await _context.SaveChangesAsync();
            return resumePdf;
        }
        public async Task<IEnumerable<ResumePdf>> getAllResumes()
        {
            return await _context.resumes.ToListAsync();
        }
        public async Task<ResumePdf?> getResumebyid(int id)
        {
            try
            {
                var resume = await _context.resumes.FindAsync(id);
                _logger.LogInformation(resume != null ? $"Resume found with ID: {id}" : $"Resume not found with ID: {id}");
                return resume;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting resume by ID {id}: {ex.Message}");
                throw; // Rethrow the exception after logging
            }
        }
    }
}
