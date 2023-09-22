using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EviTestApi.Models.ViewModels
{
    public class CandidateVM
    {
        public int CandidateId { get; set; }
        public string CandidateName { get; set; } = default!;
        [Column(TypeName = "date"),DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        public DateTime BirthDate { get; set; }
        public string? PhoneNo { get; set; }
        public string? Picture { get; set; }
        public IFormFile PictureFile { get; set; }
        public bool Fresher { get; set; }
        public string? SkillStringify { get; set; }
        public List<Skill> SkillList { get; set; }
        
    }
    public class UserModels
    {
        public string LoginID { get; set; }
        public string Password { get; set; }
        public string UserMessage { get; set; }
        public string UserToken { get; set; }
    }
}
