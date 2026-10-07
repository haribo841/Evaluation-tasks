using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace WebApplication1.Models
{
    [Serializable]
    [XmlRoot("User")]
    public class User
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("FirstName")]
        [Required(ErrorMessage = "Imię jest polem wymaganym.")]
        [Display(Name = "Imię")]
        [MaxLength(50, ErrorMessage = "Imię nie może przekraczać 50 znaków.")]
        public string Name { get; set; }

        [XmlElement("LastName")]
        [Required(ErrorMessage = "Nazwisko jest polem wymaganym.")]
        [Display(Name = "Nazwisko")]
        [MaxLength(150, ErrorMessage = "Nazwisko nie może przekraczać 150 znaków.")]
        public string Surname { get; set; }

        [XmlElement("Gender")]
        [Required(ErrorMessage = "Płeć jest polem wymaganym.")]
        [Display(Name = "Płeć")]
        public Gender Gender { get; set; }

        [XmlElement("BirthDate")]
        [Required(ErrorMessage = "Data urodzenia jest polem wymaganym.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data urodzenia")]
        [BirthDateNotInFuture(ErrorMessage = "Data urodzenia nie może być z przyszłości.")]
        public DateTime BirthDate { get; set; }

        [XmlElement("TelephoneNumber")]
        [Display(Name = "Numer telefonu")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "Numer telefonu musi składać się z 9 cyfr.")]
        public int? TelephoneNumber { get; set; }

        [XmlElement("Position")]
        [Display(Name = "Stanowisko")]
        [MaxLength(100, ErrorMessage = "Stanowisko nie może przekraczać 100 znaków.")]
        public string? Position { get; set; }

        [XmlElement("ShoeSize")]
        [Display(Name = "Numer buta")]
        [Range(0, 100, ErrorMessage = "Numer buta musi być liczbą między 0 a 100.")]
        public int? ShoeSize { get; set; }

        public User()
        {
        }
    }

    public enum Gender
    {
        Male,
        Female,
        Other
    }
}
