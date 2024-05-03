using System.Collections.Generic;

namespace Resume.Repository
{
    public interface IResumeRepository
    {
        Task<List<ResumePdf>> getAllResumes();
        Task <ResumePdf>getResumebyid(int resumeid);
        Task<ResumePdf> AddResume(ResumePdf resumePdf);
        Task<List<ResumePdf>> getResumesbyEmail(string email);
        Task<int> DeleteResume(int resumeid);
    }
}
