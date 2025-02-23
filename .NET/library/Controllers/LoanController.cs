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
        public IActionResult Get()
        {
            var activeLoans = _loanRepository.GetActiveLoans();
            return Ok(activeLoans);
        }

        [HttpPut("return/{bookStockId}")]
        public IActionResult ReturnBook(Guid bookStockId)
        {
            var bookStock = _loanRepository.ReturnBook(bookStockId);

            if (bookStock == null)
            {
                return NotFound("Book stock not found or not on loan.");
            }

            return Ok($"Book '{bookStock.Book.Name}' has been returned successfully.");
        }
    }
}