using Microsoft.EntityFrameworkCore;
using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public class ReservationRepository : IReservationRepository
    {
        public ReservationRepository()
        {
        }

        //Add functionality to allow a borrower to reserve a particular title that is currently on loan (also consider the case of multiple borrowers all wanting to borrow the same book). The borrower should also be able to query via the API to find out when the book will be available for them.
        public void AddReservation(Reservation reservation)
        {
            using (var context = new LibraryContext())
            {
                var reservations = context.Reservations
                    .Where(r => r.BookId == reservation.BookId)
                    .OrderBy(r => r.QueuePosition)
                    .ToList();

                reservation.QueuePosition = reservations.Count + 1;

                var lastReservation = reservations.LastOrDefault();
                if (lastReservation != null)
                {
                    reservation.ExpectedAvailabilityDate = lastReservation.ExpectedAvailabilityDate?.AddDays(14);
                }
                else
                {
                    var bookLoan = context.Catalogue.FirstOrDefault(bs => bs.Book.Id == reservation.BookId && bs.OnLoanTo != null);
                    reservation.ExpectedAvailabilityDate = bookLoan?.LoanEndDate ?? DateTime.UtcNow.AddDays(14);
                }

                context.Reservations.Add(reservation);
                context.SaveChanges();
            }
        }

        public List<Reservation> GetReservationsByBorrower(Guid borrowerId)
        {
            using (var context = new LibraryContext())
            {
                return [.. context.Reservations
                    .AsNoTracking()
                    .Where(r => r.BorrowerId == borrowerId)
                    .OrderBy(r => r.ReservationDate)];
            }
        }

        public Reservation GetNextReservation(Guid bookId)
        {
            using (var context = new LibraryContext())
            {
                return context.Reservations
                .Where(r => r.BookId == bookId)
                .OrderBy(r => r.QueuePosition)
                .FirstOrDefault();
            }
        }

        public void RemoveReservation(Guid reservationId)
        {
            using (var context = new LibraryContext())
            {
                var reservation = context.Reservations.Where(r=>r.Id == reservationId).FirstOrDefault();
                if (reservation != null)
                {
                    context.Reservations.Remove(reservation);
                    context.SaveChanges();
                }
            }
        }


    }
}
