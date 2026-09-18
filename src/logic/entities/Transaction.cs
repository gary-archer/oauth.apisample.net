namespace FinalApi.Logic.Entities
{
    /*
     * A single transaction
     */
    public class Transaction
    {
        public required string Id { get; set; }

        public required string InvestorId { get; set; }

        public required double AmountUsd { get; set; }
    }
}