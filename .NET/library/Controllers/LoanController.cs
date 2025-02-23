using Microsoft.AspNetCore.Mvc;
using OneBeyondApi.DataAccess;
using OneBeyondApi.Model;
using System.Collections;

namespace OneBeyondApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoanController : ControllerBase
    {
        private readonly ILogger<LoanController> _logger;
        private readonly ILoanRepository _loanRepository;

        public LoanController(ILogger<LoanController> logger, ILoanRepository loanRepository)
        {
            _logger = logger;
            _loanRepository = loanRepository;   
        }

        [HttpGet]
        [Route("OnLoan")]
        public IList<LoanDetail> Get()
        {
            return _loanRepository.GetActiveLoans();
        }
    }
}