using BookingApp.Domain.Serializer;
using System;
using System.Diagnostics;
using System.Globalization;

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
        public string MacAddress { get; set; }
        public bool? IsSuperUser { get; set; }
        public bool HasJob {  get; set; }

        public User() { }

        public User(string username, string password, UserType type, string fullName, string personalId,DateOnly birthDate, string avatarPath, string macAddress,bool? isSuperUser,bool hasJob)
        {
            BirthDate = birthDate;
            Username = username;
            Password = password;
            Type = type;
            FullName = fullName;
            PersonalId = personalId;
            AvatarPath = avatarPath;
            MacAddress = macAddress;
            IsSuperUser = isSuperUser;
            HasJob = hasJob;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, Password, Type.ToString(),FullName,PersonalId,BirthDate.ToString("d/M/yyyy", CultureInfo.InvariantCulture),AvatarPath,MacAddress,IsSuperUser.ToString(),HasJob.ToString()};
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
            BirthDate = DateOnly.ParseExact(values[6], "d/M/yyyy", CultureInfo.InvariantCulture);
            AvatarPath = values[7];
            MacAddress = values[8];
            IsSuperUser = (values[9] == "") ? (bool?)null : Convert.ToBoolean(values[9]);
            HasJob = bool.Parse(values[10]);
        }

        internal bool IsOwner()
        {
            return Type==UserType.Owner;
        }
        public bool IsSuper()
        {
            return IsSuperUser == true;
        }
    }
}
