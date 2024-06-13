using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.Domain.Models
{
    public class ComplexSimpleRequestPair : ISerializable
    {
        public int ComplexRequestId { get; set; }
        public int SimpleRequestId { get; set; }

        public ComplexSimpleRequestPair() { }

        public ComplexSimpleRequestPair(int complexRequestId, int simpleRequestId)
        {
            ComplexRequestId = complexRequestId;
            SimpleRequestId = simpleRequestId;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { ComplexRequestId.ToString(), SimpleRequestId.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            ComplexRequestId = Convert.ToInt32(values[0]);
            SimpleRequestId = Convert.ToInt32(values[1]);            
        }

    }
}
