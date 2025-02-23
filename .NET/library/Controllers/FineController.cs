using Microsoft.AspNetCore.Mvc;
using OneBeyondApi.DataAccess;
using OneBeyondApi.Model;
using System.Collections;

namespace OneBeyondApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FineController : ControllerBase
    {
        private readonly ILogger<LoanController> _logger;
        private readonly IFineRepository _fineRepository;

        public FineController(ILogger<LoanController> logger, IFineRepository fineRepository)
        {
            _logger = logger;
            _fineRepository = fineRepository;   
        }

        [HttpGet("{borrowerId}")]
        public IActionResult GetFines(Guid borrowerId)
        {
            var fines = _fineRepository.GetFinesByBorrower(borrowerId);

            if (fines.Count == 0)
            {
                return Ok("No fines found for this borrower.");
            }

            return Ok(fines);
        }
    }
}