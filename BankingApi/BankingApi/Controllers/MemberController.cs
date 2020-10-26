using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApi.Models;
using BankingApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BankingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IRepository _repository;

        public MemberController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Member
        [HttpGet]
        public async Task<IEnumerable<MemberModel>> Get()
        {
            var model = await _repository.ReadDataAsync();
            return model.Members;
        }

        // GET: api/Member/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<MemberModel>> Get(int id)
        {
            var model = await _repository.ReadDataAsync();
            var member = model.Members.SingleOrDefault(x => x.MemberId == id);
            if (member == null)
                return NotFound();

            return member;
        }

        // POST: api/Member
        [HttpPost]
        public async Task<IActionResult> Post(MemberModel memberDTO)
        {
            var model = await _repository.ReadDataAsync();
            var institution = model.Institutions;
            var exist = institution.Any(x => x.InstitutionId == memberDTO.InstitutionId);
            if (!exist)
                throw new ArgumentException("Institution does not exist");

            model.Members.Add(new MemberModel { MemberId = model.Members.Max(x => x.MemberId + 1), GivenName = memberDTO.GivenName, Surname = memberDTO.Surname, InstitutionId = memberDTO.InstitutionId, Accounts = memberDTO.Accounts });
            _repository.SaveDataAsync(model);
            return Accepted();
        }

        // PUT: api/Member/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, MemberModel memberDTO)
        {
            if (id != memberDTO.MemberId)
            {
                return BadRequest();
            }

            var model = await _repository.ReadDataAsync();
            var member = model.Members.SingleOrDefault(x => x.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            member.GivenName = memberDTO.GivenName;
            member.Surname = memberDTO.Surname;
            _repository.SaveDataAsync(model);

            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _repository.ReadDataAsync();
            var member = model.Members.SingleOrDefault(x => x.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            model.Members.Remove(member);
            _repository.SaveDataAsync(model);

            return NoContent();
        }

        [HttpPut("updatebalance/{id}")]
        public async Task<IActionResult> UpdateBalance(int id, BalanceModel accountDTO)
        {
            if (id != accountDTO.MemberId)
            {
                return BadRequest();
            }

            var model = await _repository.ReadDataAsync();
            var member = model.Members.SingleOrDefault(x => x.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            var account = member.Accounts.SingleOrDefault(x => x.AccountId == accountDTO.AccountId);
            if (account == null)
            {
                return NotFound();
            }

            account.Balance = accountDTO.Balance;
            _repository.SaveDataAsync(model);

            return NoContent();
        }
    }
}
