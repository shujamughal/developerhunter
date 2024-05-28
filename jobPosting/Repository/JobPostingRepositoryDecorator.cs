using jobPosting.Models;
using jobPosting.Repository;

public abstract class JobPostingRepositoryDecorator : IJobPostingRepository
{
    protected readonly IJobPostingRepository _inner;

    public JobPostingRepositoryDecorator(IJobPostingRepository inner)
    {
        _inner = inner;
    }

    public virtual Task<IEnumerable<JobPosting>> GetAllJobPosts()
    {
        return _inner.GetAllJobPosts();
    }

    public virtual Task<JobPosting> AddJobPosts(JobPosting jobPosting)
    {
        return _inner.AddJobPosts(jobPosting);
    }

    public virtual Task<JobPosting> GetJobPostById(int id)
    {
        return _inner.GetJobPostById(id);
    }

    public virtual Task<IEnumerable<JobPosting>> GetJobPostsByCompany(string company)
    {
        return _inner.GetJobPostsByCompany(company);
    }

    public virtual Task<bool> DeleteJobPost(int id)
    {
        return _inner.DeleteJobPost(id);
    }

    public virtual Task<bool> UpdateJobPost(JobPosting jobPosting)
    {
        return _inner.UpdateJobPost(jobPosting);
    }
}
