﻿using System.Text.RegularExpressions;

namespace MochaDB.engine {
    /// <summary>
    /// Name engine of MochaDB.
    /// </summary>
    internal static class Engine_NAMES {
        /// <summary>
        /// Returns true if pass but returns false if not.
        /// </summary>
        public static bool Check(string value) {
            var pattern = new Regex(
"^()$|.*( |\\n|\\r|\\$|#|\\-|<|>|\\?|\\*|\\\\|\\{|\\}|\\[|\\]|\\(|\\)|\\&|`|´|=|%|\\+|'|\"|\\^|!|/|\\.|;|¨|~|:|₺|€|\\||£).*");
            return !pattern.IsMatch(value);
        }

        /// <summary>
        /// Check name and give exception if not pass.
        /// </summary>
        /// <param name="value">Value.</param>
        public static void CheckThrow(string value) {
            if(!Check(value))
                throw new MochaException("The name did not meet the naming conventions!");
        }

        /// <summary>
        /// Returns true if pass but returns false if not.
        /// </summary>
        public static bool AttributeCheck(string value) {
            var pattern = new Regex(
"^()$|.*(\\n|\\r|\\$|#|\\-|<|>|\\?|\\*|\\\\|\\{|\\}|\\[|\\]|\\(|\\)|\\&|`|´|=|%|\\+|'|\"|\\^|!|/|\\.|;|¨|~|:|₺|€|\\||£).*");
            return !pattern.IsMatch(value);
        }

        /// <summary>
        /// Check name and give exception if not pass.
        /// </summary>
        /// <param name="value">Value.</param>
        public static void AttributeCheckThrow(string value) {
            if(!AttributeCheck(value))
                throw new MochaException("The name did not meet the naming conventions!");
        }
    }
}
