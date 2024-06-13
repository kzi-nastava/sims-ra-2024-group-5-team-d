using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.Domain.Models
{
    public class CheckPoint : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TourId { get; set; }
        public bool IsChecked { get; set; }
        public CheckPoint() { }

        public CheckPoint(string name, int tourId, bool isChecked)
        {
            Name = name;
            TourId = tourId;
            IsChecked = isChecked;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            TourId = Convert.ToInt32(values[2]);
            IsChecked = Convert.ToBoolean(values[3]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, TourId.ToString(), IsChecked.ToString() };
            return csvValues;
        }

    }
}
