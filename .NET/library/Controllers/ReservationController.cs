using Microsoft.AspNetCore.Mvc;
using OneBeyondApi.DataAccess;
using OneBeyondApi.Model;
using OneBeyondApi.Services;
using System.Collections;

namespace OneBeyondApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ILogger<ReservationController> _logger;
        private readonly IReservationRepository _reservationRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IBorrowerRepository _borrowerRepository;

        public ReservationController(ILogger<ReservationController> logger, IReservationRepository reservationRepository, IBookRepository bookRepository, IBorrowerRepository borrowerRepository)
        {
            _logger = logger;
            _reservationRepository = reservationRepository;
            _bookRepository = bookRepository;
            _borrowerRepository = borrowerRepository;
        }

        [HttpPost("reserve")]
        public IActionResult ReserveBook([FromBody] ReservationRequest request)
        {
            var book = _bookRepository.GetBookById(request.BookId);
            var borrower = _borrowerRepository.GetBorrowerById(request.BorrowerId);

            if (book == null || borrower == null)
            {
                return BadRequest("Invalid book or borrower.");
            }

            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                BorrowerId = request.BorrowerId,
                BookId = request.BookId,
                ReservationDate = DateTime.UtcNow
            };

            _reservationRepository.AddReservation(reservation);
            return Ok(new { message = "Reservation successful.", expectedAvailability = reservation.ExpectedAvailabilityDate });
        }

        [HttpGet("borrower/{borrowerId}")]
        public IActionResult GetReservationsForBorrower(Guid borrowerId)
        {
            var reservations = _reservationRepository.GetReservationsByBorrower(borrowerId);
            return Ok(reservations);
        }
    }
}