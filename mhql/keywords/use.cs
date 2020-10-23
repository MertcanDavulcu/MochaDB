using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using MochaDB.framework;
using MochaDB.mhql.engine;
using MochaDB.Mhql;
using MochaDB.Querying;

namespace MochaDB.mhql.keywords {
  /// <summary>
  /// MHQL USE keyword.
  /// </summary>
  internal class Mhql_USE:MhqlKeyword {
    #region Constructors

    /// <summary>
    /// Create a new Mhql_USE.
    /// </summary>
    /// <param name="db">Target database.</param>
    public Mhql_USE(MochaDatabase db) {
      Command = string.Empty;
      Tdb = db;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Returns use command.
    /// </summary>
    /// <param name="final">Command of removed use commands.</param>
    public string GetUSE(out string final) {
      int usedex = Command.IndexOf($"USE{Mhql_LEXER.ALL_OPERATOR}",StringComparison.OrdinalIgnoreCase);
      if(usedex == -1) {
        usedex = Command.IndexOf("USE ",StringComparison.OrdinalIgnoreCase);
        if(usedex == -1)
          throw new MochaException("USE command is cannot processed!");
      }
      Regex pattern = new Regex($@"\s+{Mhql_GRAMMAR.MainKeywords}(\s+.*|$)",
        RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
      string command = Command.Substring(3);
      int index = command.IndexOf(Mhql_LEXER.LBRACE);
      index = index == -1 ? command.IndexOf(Mhql_LEXER.RBRACE) : index;
      index = index == -1 ? 0 : index;
      int count = index == 0 ? 0 : 1;
      for(; index < command.Length; index++) {
        char currentChar = command[index];
        if(count == 0) {
          Match match = pattern.Match(command.Substring(index));
          if(match.Success) {
            final = command.Substring(match.Index).Trim();
            return command.Substring(0,match.Index).Trim();
          }
        } else if(currentChar == Mhql_LEXER.LBRACE)
          count++;
        else if(currentChar == Mhql_LEXER.RBRACE)
          count--;
      }
      final = string.Empty;
      return command;
    }

    /// <summary>
    /// Returns table by use command.
    /// </summary>
    /// <param name="usecommand">Use command.</param>
    public MochaTableResult GetTable(string usecommand,bool from) {
      MochaColumn GetColumn(string cmd,MochaColumn[] cols) {
        var name = Mhql_AS.GetAS(ref cmd);
        if(Mhql_GRAMMAR.UseFunctions.MatchKey(cmd)) {
          MochaColumn column = new MochaColumn();
          column.MHQLAsText = name;
          column.Tag =
              Mhql_GRAMMAR.UseFunctions.GetValueByMatchKey(cmd);
          if(column.Tag != "COUNT")
            column.Description =
                Mhql_GRAMMAR.GetIndexOfColumn(MhqlEng_EDITOR.DecomposeBrackets(cmd),cols,from).ToString();
          return column;
        } else {
          string colname = cmd.StartsWith("$") ? cmd.Substring(1).Trim() : cmd;
          IEnumerable<MochaColumn> result = cols.Where(x => x.Name == colname);
          if(result.Count() == 0)
            throw new MochaException($"Could not find a column with the name '{cmd}'!");
          MochaColumn column = result.First();
          column.Tag = colname != cmd ? "$" : null;
          column.MHQLAsText = name;
          return column;
        }
      }

      var columns = new List<MochaColumn>();
      var resulttable = new MochaTableResult();

      if(from) {
        var dex = Mhql_FROM.GetIndex(ref usecommand);
        var tablename = usecommand.Substring(dex+5).Trim();
        var parts = Mhql_LEXER.SplitUseParameters(usecommand.Substring(0,dex));
        var _columns = Tdb.GetColumns(tablename);
        if(parts.Length == 1 && parts[0].Trim() == "*")
          columns.AddRange(_columns);
        else {
          if(parts[0].TrimStart().StartsWith("$") &&
             parts[0].TrimStart().Substring(1).TrimStart().StartsWith($"{Mhql_LEXER.LBRACE}"))
            throw new MochaException("Cannot be used with subquery FROM keyword!");
          for(var index = 0; index < parts.Length; index++)
            columns.Add(GetColumn(parts[index].Trim(),_columns));
        }
      } else {
        var parts = Mhql_LEXER.SplitUseParameters(usecommand);
        for(var index = 0; index < parts.Length; index++) {
          var callcmd = parts[index].Trim();
          if(callcmd == "*") {
            var tables = Tdb.GetTables();
            for(int tindex = 0; tindex < tables.Length; tindex++)
              columns.AddRange(tables[tindex].Columns);
            continue;
          }
          int obrace = callcmd.IndexOf(Mhql_LEXER.LBRACE);
          if(obrace != -1) {
            string mode =
              callcmd.Substring(0, obrace).TrimStart().StartsWith("$") ?
                "$" : string.Empty;
            MochaColumn[] _cols = Tdb.ExecuteScalarTable(Mhql_LEXER.RangeBrace(
              callcmd.Substring(obrace).Trim(),Mhql_LEXER.LBRACE,Mhql_LEXER.RBRACE)).Columns;
            for(int cindex = 0; cindex < _cols.Length; cindex++)
              columns.Add(GetColumn($"{mode}{_cols[cindex].Name}",_cols));
            continue;
          }
          if(callcmd.StartsWith("$")) {
            callcmd = callcmd.Substring(1).Trim();
            MochaColumn[] _cols = Tdb.GetColumns(callcmd);
            for(int cindex = 0; cindex < _cols.Length; cindex++)
              columns.Add(GetColumn($"${_cols[cindex].Name}", _cols));
            continue;
          }

          var callparts = Mhql_LEXER.SplitSubCalls(callcmd);
          if(callparts.Length>2)
            throw new MochaException($"'{callcmd}' command is cannot processed!");
          for(byte partindex = 0; partindex < callparts.Length; partindex++)
            callparts[partindex] = callparts[partindex].Trim();
          var _columns = Tdb.GetColumns(callparts[0]);
          if(callparts.Length == 1)
            columns.AddRange(_columns);
          else
            columns.Add(GetColumn(callparts[1],_columns));
        }
      }

      resulttable.Columns = columns.ToArray();
      resulttable.SetRowsByDatas();

      return resulttable;
    }

    #endregion
  }
}
