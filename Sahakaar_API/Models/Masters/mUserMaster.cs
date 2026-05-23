using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sahakaar_API.Models.Masters
{
    [Table("AspNetUsers", Schema = "dbo")]

    public class mUserMaster
    {
        [Required(ErrorMessage = "User Name is required.")]
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public decimal F_UserType { get; set; }
        public decimal UserId { get; set; }
    }
    public class mUserMasterOld
    {
        [Key]
        [Required(ErrorMessage = "User Id is required.")]
        public string UserId
        {
            get;
            set;
        }
        [Required(ErrorMessage = "User Name is required.")]
        public string UserName
        {
            get;
            set;
        }
        [Required(ErrorMessage = "User Full Name is required.")]
        public string FullName
        {
            get;
            set;
        }
        [Required(ErrorMessage = "User Email is required.")]
        public string Email
        {
            get;
            set;
        }
        [Required(ErrorMessage = "User Image URL is required.")]
        public string ImageURL
        {
            get;
            set;
        }
        [Required(ErrorMessage = "User Miles is required.")]
        public string Miles
        {
            get;
            set;
        }
        [Required(ErrorMessage = "User Address is required.")]
        public string Address
        {
            get;
            set;
        }
        [Required(ErrorMessage = "User Location Lat is required.")]
        public string Lat
        {
            get;
            set;
        }
        [Required(ErrorMessage = "User Location Long is required.")]
        public string Long
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Country is required.")]
        public string Country
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Age is required.")]
        public int Age
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Gender is required.")]
        public int Gender
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Education Level is required.")]
        public string EducationLevel
        {
            get;
            set;
        }
        [Required(ErrorMessage = "About Bio is required.")]
        public string AboutBio
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Recruiting From is required.")]
        public string RecruitingFrom
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Sport Type is required.")]
        public string SportType
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Sport Type Id is required.")]
        public decimal SportTypeId
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Profile Type is required.")]
        public string ProfileType
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Profile Type Id is required.")]
        public decimal ProfileTypeId
        {
            get;
            set;
        }
    }
    public class mUserMaster_ProfileSettings
    {
        [Required(ErrorMessage = "User Full Name is required.")]
        public string FullName
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Miles is required.")]
        public string Miles
        {
            get;
            set;
        }
    }
    public class mUserMaster_Profile
    {
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Address is required.")]
        public string Address
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Date Of is required.")]
        public string DOB
        {
            get;
            set;
        }
    }
    public class mUserMaster_ImageData
    {
        [Required(ErrorMessage = "User Image Data is required.")]
        public string ImageData
        {
            get;
            set;
        }
    }
    public class mUserMaster_Address
    {
        [Required(ErrorMessage = "User Address is required.")]
        public string Address
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Country is required.")]
        public string Country
        {
            get;
            set;
        }
        [Required(ErrorMessage = "User Location Latitude is required.")]
        public string Lat
        {
            get;
            set;
        }
        [Required(ErrorMessage = "User Location Longitude Long is required.")]
        public string Long
        {
            get;
            set;
        }
    }
}
