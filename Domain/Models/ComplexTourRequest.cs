using System;
using BookingApp.Domain.Serializer;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.RightsManagement;

namespace BookingApp.Domain.Models
{
    public class ComplexTourRequest : ISerializable
    {
        public int Id { get; set; }
        public STATE Status { get; set; }
        public int TouristId { get; set; }
        public List<TourRequest> Requests { get; set; }
        public ComplexTourRequest()
        {
            Requests = new List<TourRequest>();
        }

        public ComplexTourRequest(int id, STATE status, int touristId)
        {
            Requests = new List<TourRequest>();
            Id = id;
            Status = status;
            TouristId = touristId;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Status = (STATE)Enum.Parse(typeof(STATE), values[1]);
            TouristId = Convert.ToInt32(values[2]);
        }

        public string[] ToCSV()
        {
            return new string[] { Id.ToString(), Status.ToString(), TouristId.ToString() };
        }

        public bool IsAcceptable()
        {
            foreach(TourRequest tR in  Requests)
            {
                if(!tR.IsAcceptable() && tR.Status == STATE.PENDING)
                    return false;
            }
            return true;
        }
    }
}
