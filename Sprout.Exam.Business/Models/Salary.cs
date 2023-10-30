using System;
using Sprout.Exam.Business.Exceptions;
using Sprout.Exam.Common.Enums;

namespace Sprout.Exam.Business.Models
{
    public class Salary 
    { 
        private const double MONTHLY_SALARY = 20000;
        private const double CONTRACTUAL_RATE_PER_DAY = 500;
        private const double PROBATIONARY_RATE = 0.9;
        private const double PART_TIME_RATE_PER_HOUR = 50;
        private const double TAX_RATE = 0.12;
        private const double NUMBER_OF_WORKDAYS_IN_MONTH = 22; 

        public Salary()
        {
            
        }
        public Salary(int typeId) : base()
        {
            TypeId = (EmployeeType) typeId;
        }

        public EmployeeType TypeId { get; set; }  

        public double GetDailyRate()
        {
            double dailyRate = 0;
            if(TypeId == EmployeeType.Regular)
                dailyRate = MONTHLY_SALARY / NUMBER_OF_WORKDAYS_IN_MONTH;
            if(TypeId == EmployeeType.Probationary)
                dailyRate = MONTHLY_SALARY * PROBATIONARY_RATE / NUMBER_OF_WORKDAYS_IN_MONTH;
            if(TypeId == EmployeeType.Contractual)
                dailyRate = CONTRACTUAL_RATE_PER_DAY;
            if(TypeId == EmployeeType.PartTime)
                dailyRate = PART_TIME_RATE_PER_HOUR * 24; 

            return Math.Round(dailyRate,2);
            
        }  
        public static double GetTax(double salary) => Math.Round(salary * TAX_RATE,2); 
        public double CalculateSalary(decimal workedDays, decimal absentDays, decimal hoursWorked)
        {
            switch (TypeId)
            {
                case EmployeeType.Regular: 
                    double regularDailyRate = GetDailyRate();
                    double regularSalary = MONTHLY_SALARY - (regularDailyRate * (double)absentDays);
                    double taxDeduction = GetTax(MONTHLY_SALARY);
                    return regularSalary - taxDeduction;
                case EmployeeType.Contractual:
                    return CONTRACTUAL_RATE_PER_DAY * (double)workedDays;
                case EmployeeType.Probationary:
                    double probationaryDailyRate = GetDailyRate();
                    double probationaryMonthlySalary =  MONTHLY_SALARY * PROBATIONARY_RATE;
                    double probationarySalary = probationaryMonthlySalary - (probationaryDailyRate * (double)absentDays);
                    double probationaryTaxDeduction = GetTax(probationaryMonthlySalary);                    
                    return probationarySalary - probationaryTaxDeduction;
                case EmployeeType.PartTime:
                    return PART_TIME_RATE_PER_HOUR * (double)hoursWorked;
                default:
                    throw new NotFoundException("Employee type not found.");
            }
        }

    }
}