using jobPosting.Models;

namespace jobPosting.Repository
{
    public interface IJobPostingRepository
    {
        Task<IEnumerable<JobPosting>> GetAllJobPosts();
        Task<JobPosting> AddJobPosts(JobPosting jobPosting);
        Task<JobPosting> GetJobPostById(int id);
        Task<IEnumerable<JobPosting>> GetJobPostsByCompany(string email);
        Task<bool> DeleteJobPost(int id);
        Task<bool> UpdateJobPost(JobPosting jobPosting);
    }
}
