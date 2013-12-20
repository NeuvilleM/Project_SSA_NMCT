using DBHelper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace FestivalRegistratie.Models.DAL
{
    public class ReservationRepository
    {
        public static List<Reservation> GetReservations()
        {
            List<Reservation> reservations = new List<Reservation>();
            string sql = "SELECT *, tickettype.Id AS ticketID,orderedtickets.Id AS orderID FROM orderedtickets INNER JOIN tickettype ON orderedtickets.TicketType=tickettype.Id";
            DbDataReader reservationReader = Database.GetData(sql);
            while (reservationReader.Read())
            {
                reservations.Add(MakeReservation(reservationReader));
            }
            reservationReader.Close();
            return reservations;
        }
        private static Reservation MakeReservation(DbDataReader reservationReader)
        {
            Reservation r = new Reservation();
            r.ID = reservationReader["orderID"].ToString();
            r.Email = reservationReader["Email"].ToString();
            r.FirstName = reservationReader["Holder"].ToString();
            r.LastName = reservationReader["HolderLast"].ToString();
            r.Number = Convert.ToInt32(reservationReader["Number"].ToString());
            r.Ticket = CreateTicket(reservationReader);
            r.TotalCost = r.Number * Convert.ToInt32(r.Ticket.Price);
            return r;
        }
        private static TicketType CreateTicket(DbDataReader reservationReader)
        {
            TicketType t = new TicketType();
            t.Name = reservationReader["TicketName"].ToString();
            t.Available = reservationReader["Available"].ToString();
            t.Price = reservationReader["price"].ToString();
            t.Id = reservationReader["ticketID"].ToString();
            t.Number = reservationReader["Available"].ToString();
            int reserved = NumberOfReservedTickets(t.Id);
            int startavailable = Convert.ToInt32(t.Available);
            t.Available = Convert.ToString(startavailable - reserved);
            return t;
        }
        public static int NumberOfReservedTickets(string TicketID)
        {
            string sql = "SELECT SUM(Number) AS NumberOfReservedTickets FROM orderedtickets WHERE TicketType = @tID";
            DbParameter tID = Database.AddParameter("@tID", TicketID);
            DbDataReader reader = Database.GetData(sql, tID);
            reader.Read();
            int iReserved = 0;
            Int32.TryParse(reader["NumberOfReservedTickets"].ToString(), out iReserved);
            reader.Close();
            return iReserved;
        }
    }
}