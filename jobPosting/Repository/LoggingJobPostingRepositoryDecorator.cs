using jobPosting.Models;
using jobPosting.Repository;

public class LoggingJobPostingRepositoryDecorator : JobPostingRepositoryDecorator
{
    public LoggingJobPostingRepositoryDecorator(IJobPostingRepository inner)
        : base(inner)
    {
    }

    public override async Task<IEnumerable<JobPosting>> GetAllJobPosts()
    {
        Console.WriteLine("Getting all jobs");
        var result = await base.GetAllJobPosts();
        Console.WriteLine("Retrieved all jobs");
        return result;
    }

    public override async Task<JobPosting> AddJobPosts(JobPosting jobPosting)
    {
        Console.WriteLine($"Adding job post: {jobPosting.JobTitle}");
        var result = await base.AddJobPosts(jobPosting);
        Console.WriteLine("Added job");
        return result;
    }

    public override async Task<JobPosting> GetJobPostById(int id)
    {
        Console.WriteLine($"Getting job post by ID: {id}");
        var result = await base.GetJobPostById(id);
        Console.WriteLine("Retrieved job");
        return result;
    }

    public override async Task<IEnumerable<JobPosting>> GetJobPostsByCompany(string company)
    {
        Console.WriteLine($"Getting job posts for company: {company}");
        var result = await base.GetJobPostsByCompany(company);
        Console.WriteLine("Retrieved job");
        return result;
    }

    public override async Task<bool> DeleteJobPost(int id)
    {
        Console.WriteLine($"Deleting job post by ID: {id}");
        var result = await base.DeleteJobPost(id);
        Console.WriteLine(result ? $"Deleted job post: {id}" : $"Failed to delete job post: {id}");
        return result;
    }

    public override async Task<bool> UpdateJobPost(JobPosting jobPosting)
    {
        Console.WriteLine($"Updating job: {jobPosting.JobTitle}");
        var result = await base.UpdateJobPost(jobPosting);
        Console.WriteLine(result ? "Job Updated" : "Failed to update job");
        return result;
    }
}
