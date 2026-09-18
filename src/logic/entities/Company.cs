namespace FinalApi.Logic.Entities
{
    /*
     * Details for a company
     */
    public class Company
    {
        public required int Id { get; set; }

        public required string Name { get; set; }

        public required string Region { get; set; }

        public required double TargetUsd { get; set; }

        public required double InvestmentUsd { get; set; }

        public required int NoInvestors { get; set; }
    }
}