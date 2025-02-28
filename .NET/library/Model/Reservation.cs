using OneBeyondApi.Model;

namespace OneBeyondApi.Model
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public Guid BorrowerId { get; set; }
        public Guid BookId { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime? ExpectedAvailabilityDate { get; set; }
        public int QueuePosition { get; set; }
    }
}
