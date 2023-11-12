using System;
using System.Text.RegularExpressions;

namespace DataAccess
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message)
        {
            
        }
    }
    
    public class SqlValidationHelper
    {
        public static void CheckItemNotFound(Exception ex)
        {
            const string pattern = @"The UPDATE statement conflicted with the FOREIGN KEY constraint";

            // Use Regex to find a match
            Match match = Regex.Match(ex.Message, pattern);

            if (!match.Success) 
                return;
            
            throw new ValidationException("It is not allowed to update the GrandParentId to a value that does not exist in the GrandParents table.");
        }

        public static void CheckDuplicateNameException(Exception ex)
        {
            const string pattern = @"The duplicate key value is \((.*?)\)";

            // Use Regex to find a match
            Match match = Regex.Match(ex.Message, pattern);

            if (!match.Success) 
                return;
            string duplicateValue = match.Groups[1].Value;
            throw new ValidationException($"It is not allowed duplicate value for Name : {duplicateValue}");
        }
    }
}