using System.ComponentModel.DataAnnotations;

namespace ASP.NETCore_API.Entities
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }

        public string EmployeeCode { get; set; }

        public string EmployeeName { get; set; }


        public DateTime DateOfBirth { get; set; }

        public int IdentityNumber { get; set; }

        public DateTime IdentityDate { get; set; }

        public string IdentityPlace { get; set; }




    }
}
