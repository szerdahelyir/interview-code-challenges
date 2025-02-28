using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public interface IReservationRepository
    {
        public void AddReservation(Reservation reservation);
        public List<Reservation> GetReservationsByBorrower(Guid borrowerId);
        public Reservation GetNextReservation(Guid bookId);
        public void RemoveReservation(Guid reservationId);
    }
}
