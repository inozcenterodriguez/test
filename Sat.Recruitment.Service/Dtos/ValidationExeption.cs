using System;
using System.Collections.Generic;
using System.Text;

namespace Sat.Recruitment.Service.Dtos
{
    public class ValidationExeption : BusinessException
    {
        public ValidationExeption(string message) : base(message)
        {
        }
    }
}
