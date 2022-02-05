using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab_2
{
    /// <summary>
    /// ConsoleValidation class.
    /// </summary>
    public static class ConsoleValidation
    {
        /// <summary>
        /// Validates the binary.
        /// </summary>
        /// <param name="inputMessege">The input messege.</param>
        /// <returns></returns>
        public static string ValidateBinary(string inputMessege)
        {
            Console.Write(inputMessege);
            var input = Console.ReadLine();

            var binary = new Regex("^[01]{1,32}$", RegexOptions.Compiled);

            return binary.IsMatch(input) && input.Length == 10 ? input : ValidateBinary(inputMessege);
        }
    }
}
