using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Dtos.Employee
{
    public class EmployeeDto
    {
        public string PersonCode { get; set; }
        public string TranDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string CertificateNo { get; set; }
        public string NationalCode { get; set; }
        public string EmploymentType { get; set; }
        public string EmploymentTypeDesc { get; set; }
        public string BankAccountNo { get; set; }
        public string BankNo { get; set; }
        public string BankNoDesc { get; set; }
        public string BankBranch { get; set; }
        public string BankBranchDesc { get; set; }
        public string EnterVal_Salary { get; set; }
        public string CmptVal_Salary { get; set; }
        public string EnterVal_NetPay { get; set; }
        public string CmptVal_NetPay { get; set; }
        public string CmptVal_RePayment { get; set; }
        public string Cmnd_Total { get; set; }
    }

}
