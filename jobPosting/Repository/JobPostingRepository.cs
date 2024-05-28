using jobPosting.Models;
using Microsoft.EntityFrameworkCore;
using jobPosting.DbContexts;
using Microsoft.AspNetCore.Builder;
using jobPosting.Repository;

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

    public async Task<bool> DeleteJobPost(int id)
    {
        var jobPosting = await _context.JobPosts.FindAsync(id);
        if (jobPosting == null)
        {
            return false;
        }

        _context.JobPosts.Remove(jobPosting);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateJobPost(JobPosting jobPosting)
    {
        var existingJobPost = await _context.JobPosts.FindAsync(jobPosting.Id);
        if (existingJobPost == null)
        {
            return false;
        }

        _context.Entry(existingJobPost).CurrentValues.SetValues(jobPosting);
        await _context.SaveChangesAsync();
        return true;
    }
}
