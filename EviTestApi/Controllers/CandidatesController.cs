using EviTestApi.Models;
using EviTestApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EviTestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly CandidateDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CandidatesController(CandidateDbContext _context, IWebHostEnvironment
       _env, IConfiguration configuration)
        {
            this._context = _context;
            this._env = _env;
            this.configuration = configuration;
        }
        [HttpGet]
        [Route("GetSkills")]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<Skill>>> GetSkills()
        {
            return await _context.Skills.ToListAsync();
        }
        [HttpGet]
        [Route("GetCandidates")]
        public async Task<ActionResult<IEnumerable<Candidate>>> GetCandidates()
        {
            return await _context.Candidates.ToListAsync();
        }
        [HttpGet]
        [Route("GetCandidates/{id}")] //Get candidates by Id.(extra added)
        public async Task<ActionResult<Candidate>> GetCandidates(int id)
        {
            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate == null)
            {
                return NotFound();
            }
            return Ok(candidate);
        }
        [HttpGet]
        [Route("CandidateInfo")]
        public async Task<ActionResult<IEnumerable<CandidateVM>>> GetCandidateSkills()
        {
            List<CandidateVM> candidateSkills = new List<CandidateVM>();
            var allCandidates = _context.Candidates.ToList();
            foreach (var candidate in allCandidates)
            {
                var skillList = _context.CandidateSkills
                .Where(x => x.CandidateId == candidate.CandidateId)
               .Select(x => new Skill { SkillId = x.SkillId })
                .ToList();
                candidateSkills.Add(new CandidateVM
                {
                    CandidateId = candidate.CandidateId,
                    CandidateName = candidate.CandidateName,
                    BirthDate = candidate.BirthDate,
                    PhoneNo = candidate.PhoneNo,
                    Fresher = candidate.Fresher,
                    Picture = candidate.Picture,
                    SkillList = skillList.ToList()
                });
            }
            return candidateSkills;
        }
        [HttpGet]
        [Route("CandidateInfo/{id}")] //Get candidatesInfo by Id.(extra added)
        public async Task<ActionResult<CandidateVM>> GetCandidateInfo(int id)
        {
            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate == null)
            {
                return NotFound();
            }
            var skillList = _context.CandidateSkills
            .Where(x => x.CandidateId == candidate.CandidateId)
            .Select(x => new Skill { SkillId = x.SkillId })
            .ToList();
            var candidateVM = new CandidateVM
            {
                CandidateId = candidate.CandidateId,
                CandidateName = candidate.CandidateName,
                BirthDate = candidate.BirthDate,
                PhoneNo = candidate.PhoneNo,
                Fresher = candidate.Fresher,
                Picture = candidate.Picture,
                SkillList = skillList.ToList()
            };
            return Ok(candidateVM);
        }
        [HttpPost]
        [Route("Post")]
        public async Task<ActionResult<CandidateSkill>>
        PostCandidateSkills([FromForm] CandidateVM VM)
        {
            var skillItems =
           JsonConvert.DeserializeObject<Skill[]>(VM.SkillStringify);
            Candidate candidate = new Candidate
            {
                CandidateName = VM.CandidateName,
                BirthDate = VM.BirthDate,
                PhoneNo = VM.PhoneNo,
                Fresher = VM.Fresher
            };
            if (VM.PictureFile != null)
            {
                var webroot = _env.WebRootPath;
                var fileName = Guid.NewGuid().ToString() +
               Path.GetExtension(VM.PictureFile.FileName);
                var filePath = Path.Combine(webroot, "Images", fileName);
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                await VM.PictureFile.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                fileStream.Close();
                candidate.Picture = fileName;
            }
            foreach (var item in skillItems)
            {
                var candidateskill = new CandidateSkill
                {
                    Candidate = candidate,
                    CandidateId = candidate.CandidateId,
                    SkillId = item.SkillId
                };
                _context.Add(candidateskill);
            }
            await _context.SaveChangesAsync();
            return Ok(candidate);
        }
        [HttpPut]
        [Route("Update/{id}")]
        public async Task<ActionResult<CandidateSkill>> UpdateCandidateSkills(int
    id, [FromForm] CandidateVM vm)
        {
            var skillItems =
           JsonConvert.DeserializeObject<Skill[]>(vm.SkillStringify);
            Candidate candidate = await _context.Candidates.FindAsync(id);
            if (candidate == null)
            {
                return NotFound();
            }
            candidate.CandidateName = vm.CandidateName;
            candidate.BirthDate = vm.BirthDate;
            candidate.PhoneNo = vm.PhoneNo;
            candidate.Fresher = vm.Fresher;
            if (vm.PictureFile != null)
            {
                var webroot = _env.WebRootPath;
                var fileName = Guid.NewGuid().ToString() +
               Path.GetExtension(vm.PictureFile.FileName);
                var filePath = Path.Combine(webroot, "Images", fileName);
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                await vm.PictureFile.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                fileStream.Close();
                candidate.Picture = fileName;
            }
            // Delete existing skills
            var existingSkills = _context.CandidateSkills.Where(x => x.CandidateId
           == candidate.CandidateId).ToList();
            foreach (var item in existingSkills)
            {
                _context.CandidateSkills.Remove(item);
            }
            // Add newly added skills
            foreach (var item in skillItems)
            {
                var candidateSkill = new CandidateSkill
                {
                    CandidateId = candidate.CandidateId,
                    SkillId = item.SkillId
                };
                _context.Add(candidateSkill);
            }
            _context.Entry(candidate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(candidate);
        }
        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<ActionResult<string>> DeleteCandidateSkill(int id)
        {
            Candidate candidate = _context.Candidates.Find(id);
            var existingSkills = _context.CandidateSkills.Where(x => x.CandidateId
           == candidate.CandidateId).ToList();
            foreach (var item in existingSkills)
            {
                _context.CandidateSkills.Remove(item);
            }
            _context.Entry(candidate).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            string message = $"All data for ID number {id} is deleted successfully.";
            return Ok(message);
        }
        [Route("CheckLogin")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CheckLogin(UserModels models)
        {
            if (models.LoginID == "admin" && models.Password == "password")
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("UserId", models.LoginID)
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    configuration["jwt:Issuer"],
                    configuration["jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: signIn);
                models.UserMessage = "Login Success";
                models.UserToken = new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                models.UserMessage = "Login Failed";
            }
            return Ok(models);
        }
    }
}
