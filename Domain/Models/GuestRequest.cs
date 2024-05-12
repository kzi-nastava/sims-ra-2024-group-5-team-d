using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public enum STATUS
    {
        APPROVED,
        REJECTED,
        INPROCESS
    }
    public class GuestRequest : ISerializable
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public DateTime NewReservedFrom { get; set; }
        public DateTime NewReservedTo {  get; set; }
        public string Comment {  get; set; }
        public STATUS Status { get; set; }

        public GuestRequest() 
        { 

        }
        public GuestRequest(int id, int idReservation, DateTime newFrom, DateTime newTo, string comment, STATUS status)
        {
            Id = id;
            ReservationId = idReservation;
            NewReservedFrom = newFrom;
            NewReservedTo = newTo;
            Comment = comment;
            Status = status;
        }
        public GuestRequest( int idReservation, DateTime newFrom, DateTime newTo, string comment, STATUS status)
        {

            ReservationId = idReservation;
            NewReservedFrom = newFrom;
            NewReservedTo = newTo;
            Comment = comment;
            Status = status;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), ReservationId.ToString(), NewReservedFrom.ToString("d/M/yyyy h:mm:ss tt"), NewReservedTo.ToString("d/M/yyyy h:mm:ss tt"), Comment, Status.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            ReservationId = Convert.ToInt32(values[1]);
            NewReservedFrom = DateTime.ParseExact(values[2], "d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            NewReservedTo = DateTime.ParseExact(values[3], "d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            Comment = values[4];
            Status = (STATUS)Enum.Parse(typeof(STATUS), values[5]);
        }
        public bool IsApproved()
        {
            return Status == STATUS.APPROVED;
        }

        public bool IsRejected()
        {
            return Status == STATUS.REJECTED;
        }

        public bool IsInProcess()
        {
            return Status == STATUS.INPROCESS;
        }

    }
}
