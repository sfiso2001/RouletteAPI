using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.Models.Common
{
    public static class AssertExtensions
    {
        public static T AssertIsNotNull<T>(this T parameterValue, string parameterName)
        {
            if (parameterValue == null)
                throw new ArgumentNullException(parameterName ?? "N/A");

            return parameterValue;
        }

        public static string AssertIsNotNullOrEmpty(this string parameterValue, string parameterName)
        {
            parameterValue.AssertIsNotNull(parameterName);

            if (parameterValue == "")
                throw new ArgumentException($"Parameter {parameterName} cannot be an empty string");

            return parameterValue;
        }
    }
}
