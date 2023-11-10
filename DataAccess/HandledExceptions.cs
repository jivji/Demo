using System;
using System.Text.RegularExpressions;

namespace DataAccess
{
    public class HandledExceptions
    {
        public SystemException CheckItemNotFound(Exception ex, SystemException exception)
        {
            string pattern = @"The UPDATE statement conflicted with the FOREIGN KEY constraint";

            // Use Regex to find a match
            Match match = Regex.Match(ex.Message, pattern);

            if (match.Success)
            {
                string duplicateValue = match.Groups[1].Value;
                exception = new SystemException("It is not allowed to update the GrandParentId to a value that does not exist in the GrandParents table.");
            }

            return exception;
        }

        public  SystemException CheckDuplicateNameException(Exception ex, SystemException exception)
        {
            string pattern = @"The duplicate key value is \((.*?)\)";

            // Use Regex to find a match
            Match match = Regex.Match(ex.Message, pattern);

            if (match.Success)
            {
                string duplicateValue = match.Groups[1].Value;
                exception = new SystemException($"It is not allowed duplicate value for Name : {duplicateValue}");
            }

            return exception;
        }
    }
}