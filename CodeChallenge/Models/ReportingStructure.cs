using System;

namespace CodeChallenge.Models
{
    public class ReportingStructure
    {
        public Employee Employee { get; set; }

        /// <summary>
        /// Total number of reports under a given employee
        /// </summary>
        public int NumberOfReports { get; set; }
    }
}
