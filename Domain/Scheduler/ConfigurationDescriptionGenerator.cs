using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public static class ConfigurationDescriptionGenerator
    {
        public static string GenerateDescription(Configuration configuration)
        {
            string output = 
                    GetStartingDateDescription(configuration.StartDate) +
                    GetEndingDateDescription(configuration.EndDate);
            return output;
        }

        private static string GetStartingDateDescription(DateOnly startingDate)
        {
            return $"starting on {startingDate:g}";
        }
        private static string GetEndingDateDescription(DateOnly? endingDate)
        {
            string endingDateDescription =  endingDate.HasValue 
                ? $"ending on {endingDate:g}"
                : string.Empty;

            return endingDateDescription;
        }   
    }
}
