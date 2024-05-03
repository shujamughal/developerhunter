using jobPosting.Models;
using Microsoft.EntityFrameworkCore;
using jobPosting.DbContexts;
using Microsoft.AspNetCore.Builder;

namespace jobPosting.Repository
{
    public class JobPostingRepository : IJobPostingRepository
    {
        private readonly JobPostingContext _context;

        public JobPostingRepository(JobPostingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobPosting>> GetAllJobPosts()
        {
            return await _context.JobPosts.ToListAsync();
        }

        public async Task<JobPosting> AddJobPosts(JobPosting jobPosting)
        {
            _context.JobPosts.Add(jobPosting);
            await _context.SaveChangesAsync();
            return jobPosting;
        }

        public async Task<JobPosting> GetJobPostById(int id)
        {
            return await _context.JobPosts.FindAsync(id);
        }

        public async Task<IEnumerable<JobPosting>> GetJobPostsByCompany(string company)
        {
            return await _context.JobPosts.Where(jp => jp.Company == company).ToListAsync();
        }
    }
}
