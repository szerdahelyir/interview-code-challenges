using Microsoft.AspNetCore.Mvc;
using OneBeyondApi.DataAccess;
using OneBeyondApi.Model;
using OneBeyondApi.Services;
using System.Collections;

namespace OneBeyondApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoanController : ControllerBase
    {
        private readonly ILogger<LoanController> _logger;
        private readonly ILoanRepository _loanRepository;
        private readonly ILoanService _loanService;

        public LoanController(ILogger<LoanController> logger, ILoanRepository loanRepository, ILoanService loanService)
        {
            _logger = logger;
            _loanRepository = loanRepository;   
            _loanService = loanService;
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
            var bookStock = _loanService.ReturnBook(bookStockId);
            if (bookStock == null)
            {
                return NotFound("Book not found or not on loan.");
            }

            return Ok($"Book '{bookStock.Book.Name}' has been returned successfully.");
        }
    }
}