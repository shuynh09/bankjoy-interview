using BankingApi.Models;
using BankingApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstitutionController : ControllerBase
    {
        private readonly IRepository _repository;

        public InstitutionController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: api/<InstitutionController>
        [HttpGet]
        public async Task<List<Institution>> Get()
        {
            var model = await _repository.ReadDataAsync();
            return model.Institutions;
        }

        // GET api/<InstitutionController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Institution>> Get(int id)
        {
            var model = await _repository.ReadDataAsync();
            var institution = model.Institutions.SingleOrDefault(x => x.InstitutionId == id);
            if (institution == null)
                return NotFound();

            return institution;
        }

        // POST api/<InstitutionController>
        [HttpPost]
        public async Task<IActionResult> Post(Institution institutionDTO)
        {
            var model = await _repository.ReadDataAsync();
            model.Institutions.Add(new Institution { InstitutionId = model.Institutions.Max(x => x.InstitutionId + 1), Name = institutionDTO.Name });
            _repository.SaveDataAsync(model);
            return Accepted();
        }

        // POST: api/institution/Transfer
        [HttpPost("transfer/{id}")]
        public async Task<IActionResult> Transfer(TransferModel transferDTO)
        {
            var model = await _repository.ReadDataAsync();
            var member = model.Members.SingleOrDefault(x => x.MemberId == transferDTO.MemberId);
            if (member == null)
            {
                return NotFound();
            }

            var account = member.Accounts.SingleOrDefault(x => x.AccountId == transferDTO.AccountId);
            if (account == null)
            {
                return NotFound();
            }

            if (account.Balance < transferDTO.TransferAmount)
            {
                return Conflict();
            }

            var transferMember = model.Members.SingleOrDefault(x => x.MemberId == transferDTO.TransferMemberId);
            if (transferMember == null)
            {
                return NotFound();
            }

            var transferAccount = member.Accounts.SingleOrDefault(x => x.AccountId == transferDTO.AccountId);
            if (transferAccount == null)
            {
                return NotFound();
            }

            if (member.InstitutionId != transferMember.InstitutionId)
            {
                return Conflict();
            }

            account.Balance -= transferDTO.TransferAmount;
            transferAccount.Balance += transferDTO.TransferAmount;

            _repository.SaveDataAsync(model);

            return Accepted();
        }
    }
}
