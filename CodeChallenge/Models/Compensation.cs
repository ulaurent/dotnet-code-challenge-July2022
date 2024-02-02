using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        
        // PK for DT unique identifier
        [Key]
        public String CompensationId { get; set; }

        public float Salary { get; set; }
        public DateTime EffectiveDate { get; set; }

        // Foreign Key stablishes the linq between two tables,
        // Prevents any actions that would destroy this relationship
        // Benefical when querying and joining a table
        // https://learn.microsoft.com/en-us/ef/core/modeling/relationships
        public String EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; } 
    }
}
