namespace MochaDB.mhql.engine.value {
  using System;
  using System.Text.RegularExpressions;

  /// <summary>
  /// Char value engine of MHQL.
  /// </summary>
  internal static class MhqlEngVal_STRING {
    /// <summary>
    /// Process value.
    /// </summary>
    /// <param name="val">Value.</param>
    public static void Process(ref string val) {
      val = val.Trim();

      if(val == string.Empty)
        throw new ArgumentException("String is not defined!");
      if(!val.StartsWith("\""))
        throw new ArgumentException("String is not declare!");
      if(!val.EndsWith("\""))
        throw new ArgumentException("String end is not declared!");

      val = val.Substring(1,val.Length-2);
      for(int index = 0; index < MhqlEngVal_LEXER.Escapes.Length/2; ++index) {
        Regex pattern = new Regex(MhqlEngVal_LEXER.Escapes[index,1],
            RegexOptions.Multiline);
        val = pattern.Replace(val,MhqlEngVal_LEXER.Escapes[index,0]);
      }
    }
  }
}
