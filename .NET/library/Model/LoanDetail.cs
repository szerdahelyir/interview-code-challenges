﻿namespace OneBeyondApi.Model
{
    public class LoanDetail
    {
        public string BorrowerName { get; set; }
        public string BorrowerEmail { get; set; }
        public List<LoanedBook> BooksOnLoan { get; set; }
    }
}
