namespace FinalApi.Logic.Entities
{
    using System.Collections.Generic;

    /*
     * A company and its transactions
     */
    public class CompanyTransactions
    {
        public required int Id { get; set; }

        public Company? Company { get; set; }

        public required IEnumerable<Transaction> Transactions { get; set; }
     }
}
