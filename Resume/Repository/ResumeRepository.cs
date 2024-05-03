using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Resume.Repository
{
    public class ResumeRepository: IResumeRepository
    {
        private readonly ResumeContext _context;
        public ResumeRepository(ResumeContext context)
        {
            _context = context;
        }   

        public async Task<ResumePdf> AddResume(ResumePdf resumePdf)
        {
           _context.resumes.Add(resumePdf);
			Console.WriteLine("I am in repositor of resume");
            await _context.SaveChangesAsync();
            return resumePdf;
        }
        public async Task<List<ResumePdf>> getAllResumes()
        {
            return await _context.resumes.ToListAsync();
        }
		public async Task<ResumePdf?> getResumebyid(int id)
		{
			try
			{
				var resume = await _context.resumes.FindAsync(id);
				Console.WriteLine(resume.ResumeId);
				return resume;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine("Here it is exception");
				return null; 
			}
		}

		public async Task<List<ResumePdf>> getResumesbyEmail(string email)
		{
			// Query resumes where the email matches the provided email
			return await _context.resumes.Where(r => r.userEmail == email).ToListAsync();
		}
		public async Task<int> DeleteResume(int resumeId)
		{
			var resume = await _context.resumes.FindAsync(resumeId);
			if (resume == null)
			{
				return 0;
			}
			_context.resumes.Remove(resume);
			return await _context.SaveChangesAsync();

		}
	}
}
