using System;

namespace Sprout.Exam.Business.Exceptions
{
    public class NotFoundException :  Exception
    {
        public NotFoundException()
        {
            
        }

        public NotFoundException(string message) : base(message)
        {
        } 
    }
}