using BookingApp.Domain.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public enum Type{
        LIVETOUR,
        REQUEST,
        CANCEL,
        RATE,
        FORUM,
        NEWTOUR,
        TOURREQUEST,
        VOUCHER
    }
    public class Notification :ISerializable
    {
        public int Id { get; set; }
        public int ReceiverId { get; set; }
        public int LinkId { get; set; }
        public Type Type { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsRead { get; set; }
        public Notification(int id,int receiverId, int linkId, Type type, DateTime date, bool isRead)
        {
            Id = id;
            ReceiverId = receiverId;
            LinkId = linkId;
            Type = type;
            DateCreated = date;
            IsRead = isRead;
        }
        public Notification(int receiverId, int linkId, Type type, DateTime date, bool isRead)
        {
            ReceiverId = receiverId;
            LinkId = linkId;
            Type = type;
            DateCreated = date;
            IsRead = isRead;
        }
        public Notification()
        {
        }


        public string[] ToCSV()
        {
            return new string[] { Id.ToString(),ReceiverId.ToString(),LinkId.ToString(), Type.ToString(), DateCreated.ToString("d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture), IsRead.ToString() };
        }

        public void FromCSV(string[] values)
        {
            
            Id = int.Parse(values[0]);
            ReceiverId = int.Parse(values[1]);
            LinkId = int.Parse(values[2]);
            Type = (Type)Enum.Parse(typeof(Type), values[3]);
            DateCreated = DateTime.ParseExact(values[4], "d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            IsRead = bool.Parse(values[5]);
        }
        public bool isRequest()
        {
                return Type.REQUEST==Type;
        }
        public bool IsRate()
        {
            return Type == Type.RATE;
        }

        internal bool isForum()
        {
            return Type == Type.FORUM;
        }
    }

}
