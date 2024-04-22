using BookingApp.Domain.Serializer;
using System;
using System.Diagnostics;

namespace BookingApp.Domain.Models
{
    public enum UserType
    {
        Owner,
        Guest,
        TourGuide,
        Tourist
    }
    public class User : ISerializable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string PersonalId { get; set; }
        public DateOnly BirthDate { get; set; }
        public UserType Type { get; set; }
        public string AvatarPath { get; set; }


        public User() { }

        public User(string username, string password, UserType type, string fullName, string personalId,DateOnly birthDate, string avatarPath)
        {
            BirthDate = birthDate;
            Username = username;
            Password = password;
            Type = type;
            FullName = fullName;
            PersonalId = personalId;
            AvatarPath = avatarPath;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, Password, Type.ToString(),FullName,PersonalId,BirthDate.ToString(),AvatarPath};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            Password = values[2];
            Type = (UserType)Enum.Parse(typeof(UserType), values[3]);
            FullName = values[4];
            PersonalId = values[5];
            BirthDate = DateOnly.Parse(values[6]);
            AvatarPath = values[7];
        }
    }
}
