using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace SalesWebMvc.Models
{
    public class Seller
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)] //configura o tipo e-mail
        public string Email { get; set; }
        [Display(Name = "Birth Date")] //configura label no display
        [DataType(DataType.Date)] //configura o tipo somente data sem as horas
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")] //configura o formato da data  
        public DateTime Birth { get; set; } 
        [Display(Name = "Base Salary")] //configura label no display
       [DisplayFormat(DataFormatString = "{0:F2}")] //Configura o formato do valor com 2 casas decimais
        public double BaseSalary { get; set; }
        public Department Department { get; set; }
        public int DepartmentId { get; set; }
        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        public Seller()
        {

        }

        public Seller(int id, string name, string email, DateTime birth, double baseSalary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            Birth = birth;
            BaseSalary = baseSalary;
            Department = department;
        }

        public void AddSales(SalesRecord sr)
        {
            Sales.Add(sr);
        }

        public void RemoveSales(SalesRecord sr)
        {
            Sales.Remove(sr);
        }

        public double TotalSales(DateTime initial, DateTime final)
        {
            return Sales.Where(sr => sr.Date >= initial && sr.Date <= final).Sum(sr => sr.Amount);
        }
    }
}
