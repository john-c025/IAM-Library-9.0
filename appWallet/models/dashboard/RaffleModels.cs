using System;

namespace IAM_Library.appWallet.models.dashboard
{
    /// <summary>
    /// Raffle row returned by GET /util/v1/LoadRaffle.
    /// </summary>
    public class RaffleListItem
    {
        public int raffleID { get; set; }
        public string raffleName { get; set; }
    }

    /// <summary>
    /// Member raffle ticket row returned by GET /util/v1/LoadMemberRaffleTickets.
    /// </summary>
    public class MemberRaffleTicket
    {
        public string refNo { get; set; }
        public int raffleID { get; set; }
        public DateTime dateGenerated { get; set; }
        public DateTime tranDate { get; set; }
        public string tranDescription { get; set; }
        public string idNumber { get; set; }
        public string name { get; set; }
        public string prodCode { get; set; }
        public string prodName { get; set; }
        public int prodQty { get; set; }
        public string ticketNumber { get; set; }
        public int ticketCtr { get; set; }
        public DateTime winDate { get; set; }
        public bool isWinner { get; set; }
        public DateTime inclusionDate { get; set; }
    }

    /// <summary>
    /// Ticket count returned by GET /util/v1/GetRaffleTicketCtr.
    /// </summary>
    public class RaffleTicketCounter
    {
        public int raffleID { get; set; }
        public string raffleName { get; set; }
        public int totalTickets { get; set; }
    }
}
