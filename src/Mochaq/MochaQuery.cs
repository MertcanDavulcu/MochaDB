﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using MochaDB.Logging;
using MochaDB.Querying;

namespace MochaDB.Mochaq {
  /// <summary>
  /// It offers query usage and management with MochaQ.
  /// </summary>
  public class MochaQuery {
    #region Fields

    private MochaDatabase database;

    #endregion Fields

    #region Internal Constructors

    /// <summary>
    /// Create new MochaQuery.
    /// </summary>
    /// <param name="database">MochaDatabase object to use querying.</param>
    /// <param name="embedded">Is embedded query in database.</param>
    internal MochaQuery(MochaDatabase database,bool embedded) {
      this.database = database;
      MochaQ = string.Empty;
      IsDatabaseEmbedded=embedded;
    }

    #endregion Internal Constructors

    #region Constructors

    /// <summary>
    /// Create new MochaQuery.
    /// </summary>
    /// <param name="database">MochaDatabase object to use querying.</param>
    public MochaQuery(MochaDatabase database) {
      IsDatabaseEmbedded=false;
      Database = database;
      MochaQ = string.Empty;
    }

    /// <summary>
    /// Create new MochaQuery.
    /// </summary>
    /// <param name="database">MochaDatabase object to use querying.</param>
    /// <param name="mochaQ">MochaQuery to use.</param>
    public MochaQuery(MochaDatabase database,string mochaQ)
      : this(database) => MochaQ = mochaQ;

    #endregion Constructors

    #region Operators

    public static explicit operator string(MochaQuery value) =>
        value.ToString();

    #endregion Operators

    #region Members

    #region ExecuteCommand

    /// <summary>
    /// Detect command type and execute. Returns result if exists returned result.
    /// </summary>
    /// <param name="mochaQ">MochaQ to be set as the active MochaQ Query.</param>
    public IMochaResult ExecuteCommand(string mochaQ) {
      MochaQ=mochaQ;
      return ExecuteCommand();
    }

    /// <summary>
    /// Detect command type and execute. Returns result if exists returned result.
    /// </summary>
    /// <param name="mochaQ">MochaQ to be set as the active MochaQ Query.</param>
    public IMochaResult ExecuteCommand(MochaDatabase database,string mochaQ) {
      Database=database;
      MochaQ=mochaQ;
      return ExecuteCommand();
    }

    /// <summary>
    /// Detect command type and execute. Returns result if exists returned result.
    /// </summary>
    public IMochaResult ExecuteCommand() {
      if(MochaQ.IsRunQuery()) {
        Run();
        return null;
      } else if(MochaQ.IsGetRunQuery()) {
        return GetRun();
      } else if(MochaQ.IsDynamicQuery()) {
        return Dynamic();
      } else
        throw new MochaException("This command is a not valid MochaQ command!");
    }

    #endregion ExecuteCommands

    #region Dynamic

    /// <summary>
    /// If the value is returned, it returns the function and performs the function; if not, it just performs the function.
    /// </summary>
    /// <param name="mochaQ">MochaQ to be set as the active MochaQ Query.</param>
    public IMochaResult Dynamic(string mochaQ) {
      MochaQ=mochaQ;
      return Dynamic();
    }

    /// <summary>
    /// If the value is returned, it returns the function and performs the function; if not, it just performs the function.
    /// </summary>
    /// <param name="database">MochaDatabase object that provides management of the targeted MochaDB database.</param>
    /// <param name="mochaQ">MochaQ to be set as the active MochaQ Query.</param>
    public IMochaResult Dynamic(MochaDatabase database,string mochaQ) {
      Database = database;
      MochaQ=mochaQ;
      return Dynamic();
    }

    /// <summary>
    /// If the value is returned, it returns the function and performs the function; if not, it just performs the function.
    /// </summary>
    public IMochaResult Dynamic() {
      if(!MochaQ.IsDynamicQuery())
        throw new MochaException(@"This MochaQ command is not ""Dynamic"" type command.");

      Database.OnConnectionCheckRequired(this,new EventArgs());

      //Check null.
      if(string.IsNullOrEmpty(MochaQ))
        throw new MochaException("This MochaQ query is empty, invalid!");

      try {
        string[] parts = MochaQ.Command.Split(' ');
        parts[0]=parts[0].ToUpperInvariant();
        parts[2]=parts[2].ToUpperInvariant();

        if(parts[0] == "SELECT") {
          string[] selectedColumns = parts[1].Split(',');

          if(parts[2] == "FROM") {
            string tableName = parts[3];

            MochaTable table = new MochaTable(tableName);

            for(int index = 0; index < selectedColumns.Length; index++) {
              table.Columns.Add(Database.GetColumn(tableName,selectedColumns[index]));
            }

            return new MochaResult<MochaTable>(table);
          } else
            throw new MochaException("Table not specified!");
        } else
          throw new MochaException("The first syntax is wrong, there is no such function.");
      } catch { return null; }
    }

    #endregion Dynamic

    #region Run

    /// <summary>
    /// Runs the active MochaQ query. Even if there is an incoming value, it will not return.
    /// </summary>
    /// <param name="mochaQ">MochaQ to be set as the active MochaQ Query.</param>
    public void Run(string mochaQ) {
      MochaQ=mochaQ;
      Run();
    }

    /// <summary>
    /// Runs the active MochaQ query. Even if there is an incoming value, it will not return.
    /// </summary>
    /// <param name="database">MochaDatabase object that provides management of the targeted MochaDB database.</param>
    /// <param name="mochaQ">MochaQ to be set as the active MochaQ Query.</param>
    public void Run(MochaDatabase database,string mochaQ) {
      Database = database;
      MochaQ=mochaQ;
      Run();
    }

    /// <summary>
    /// Runs the active MochaQ query. Even if there is an incoming value, it will not return.
    /// </summary>
    public void Run() {
      if(!MochaQ.IsRunQuery())
        throw new MochaException(@"This MochaQ command is not ""Run"" type command.");

      Database.OnConnectionCheckRequired(this,new EventArgs());

      if(Database.Provider.Readonly)
        throw new MochaException("This connection is can read only, cannot task of write!");

      //Check null.
      if(string.IsNullOrEmpty(MochaQ))
        throw new MochaException("This MochaQ query is empty, invalid!");

      string[] queryPaths = MochaQ.Command.Split(':');
      queryPaths[0]=queryPaths[0].ToUpperInvariant();

      if(queryPaths.Length == 1) {
        if(queryPaths[0] == "RESETMOCHA") {
          Database.Reset();
          return;
        } else if(queryPaths[0] == "RESETTABLES") {
          Database.OnChanging(this,new EventArgs());
          IEnumerable<XElement> tableRange = Database.CDoc.Root.Element("Tables").Elements();
          for(int index = 0; index < tableRange.Count(); index++) {
            tableRange.ElementAt(index).Elements().Remove();
          }

          Database.Save();
          return;
        } else if(queryPaths[0] == "CLEARTABLES") {
          Database.ClearTables();
          return;
        } else if(queryPaths[0] == "CLEARALL") {
          Database.ClearAll();
          return;
        } else if(queryPaths[0] == "CLEARLOGS") {
          Database.ClearLogs();
          return;
        } else if(queryPaths[0] == "RESTORETOFIRSTLOG") {
          Database.RestoreToFirstLog();
          return;
        } else if(queryPaths[0] == "RESTORETOLASTLOG") {
          Database.RestoreToLastLog();
          return;
        } else
          throw new MochaException("Invalid query. The content of the query could not be processed, wrong!");
      } else if(queryPaths.Length == 2) {
        if(queryPaths[0] == "REMOVETABLE") {
          Database.RemoveTable(queryPaths[1]);
          return;
        } else if(queryPaths[0] == "CREATETABLE") {
          Database.CreateTable(queryPaths[1]);
          return;
        } else if(queryPaths[0] == "SETPASSWORD") {
          Database.SetPassword(queryPaths[1]);
          return;
        } else if(queryPaths[0] == "SETDESCRIPTION") {
          Database.SetDescription(queryPaths[1]);
          return;
        } else if(queryPaths[0] == "RESTORETOLOG") {
          Database.RestoreToLog(queryPaths[1]);
          return;
        } else if(queryPaths[0] == "CLEARROWS") {
          Database.ClearRows(queryPaths[1]);
          return;
        } else if(queryPaths[0] == "RESETTABLE") {
          if(!Database.ExistsTable(queryPaths[1]))
            throw new MochaException("Table not found in this name!");
          Database.OnChanging(this,new EventArgs());
          Database.CDoc.Root.Element("Tables").Elements(queryPaths[1]).Elements().Remove();
          Database.Save();
          return;
        } else if(queryPaths[0] == "CREATEMOCHA") {
          MochaDatabase.CreateMochaDB(queryPaths[1],string.Empty,string.Empty);
          return;
        } else
          throw new MochaException("Invalid query. The content of the query could not be processed, wrong!");
      } else if(queryPaths.Length == 3) {
        if(queryPaths[0] == "REMOVECOLUMN") {
          Database.RemoveColumn(queryPaths[1],queryPaths[2]);
          return;
        } else if(queryPaths[0] == "SETTABLEDESCRIPTION") {
          Database.SetTableDescription(queryPaths[1],queryPaths[2]);
          return;
        } else if(queryPaths[0] == "REMOVEROW") {
          Database.RemoveRow(queryPaths[1],int.Parse(queryPaths[2]));
          return;
        } else if(queryPaths[0] == "RENAMETABLE") {
          Database.RenameTable(queryPaths[1],queryPaths[2]);
          return;
        } else if(queryPaths[0] == "CREATECOLUMN") {
          Database.CreateColumn(queryPaths[1],queryPaths[2]);
          return;
        } else if(queryPaths[0] == "CREATEMOCHA") {
          MochaDatabase.CreateMochaDB(Path.Combine(queryPaths[1] + queryPaths[2]),string.Empty,string.Empty);
          return;
        } else
          throw new MochaException("Invalid query. The content of the query could not be processed, wrong!");
      } else if(queryPaths.Length == 4) {
        if(queryPaths[0] == "RENAMECOLUMN") {
          Database.RenameColumn(queryPaths[1],queryPaths[2],queryPaths[3]);
          return;
        } else if(queryPaths[0] == "SETCOLUMNDESCRIPTION") {
          Database.SetColumnDescription(queryPaths[1],queryPaths[2],queryPaths[3]);
          return;
        } else if(queryPaths[0] == "SETCOLUMNDATATYPE") {
          Database.SetColumnDataType(queryPaths[1],queryPaths[2],MochaData.GetDataTypeFromName(queryPaths[3]));
          return;
        } else if(queryPaths[0] == "ADDDATA") {
          if(queryPaths[3] != string.Empty)
            Database.AddData(queryPaths[1],queryPaths[2],queryPaths[3]);
          else
            Database.AddData(queryPaths[1],queryPaths[2],null);
          return;
        } else if(queryPaths[0] == "UPDATEFIRSTDATA") {
          Database.UpdateFirstData(queryPaths[1],queryPaths[2],queryPaths[3]);
          return;
        } else if(queryPaths[0] == "UPDATELASTDATA") {
          Database.UpdateLastData(queryPaths[1],queryPaths[2],queryPaths[3]);
          return;
        } else
          throw new MochaException("Invalid query. The content of the query could not be processed, wrong!");
      } else if(queryPaths.Length == 5) {
        if(queryPaths[0] == "UPDATEDATA") {
          Database.UpdateData(queryPaths[1],queryPaths[2],int.Parse(queryPaths[3]),queryPaths[4]);
          return;
        } else
          throw new MochaException("Invalid query. The content of the query could not be processed, wrong!");
      }
    }

    #endregion Run

    #region GetRun

    /// <summary>
    /// Runs the active MochaQ query. Returns the incoming value.
    /// </summary>
    /// <param name="mochaQ">MochaQ to be set as the active MochaQ Query.</param>
    public IMochaResult GetRun(string mochaQ) {
      MochaQ=mochaQ;
      return GetRun();
    }

    /// <summary>
    /// Runs the active MochaQ query. Returns the incoming value.
    /// </summary>
    /// <param name="database">MochaDatabase object that provides management of the targeted MochaDB database.</param>
    /// <param name="mochaQ">MochaQ to be set as the active MochaQ Query.</param>
    public IMochaResult GetRun(MochaDatabase database,string mochaQ) {
      Database = database;
      MochaQ=mochaQ;
      return GetRun();
    }

    /// <summary>
    /// Runs the active MochaQ query. Returns the incoming value.
    /// </summary>
    public IMochaResult GetRun() {
      if(!MochaQ.IsGetRunQuery())
        throw new MochaException(@"This MochaQ command is not ""GetRun"" type command.");

      Database.OnConnectionCheckRequired(this,new EventArgs());

      //Check null.
      if(string.IsNullOrEmpty(MochaQ))
        throw new MochaException("This MochaQ query is empty, invalid!");

      string[] queryPaths = MochaQ.Command.Split(':');
      queryPaths[0]=queryPaths[0].ToUpperInvariant();

      if(queryPaths.Length == 1) {
        if(queryPaths[0] == "GETTABLES") {
          return new MochaCollectionResult<MochaTable>(Database.GetTables());
        } else if(queryPaths[0] == "GETPASSWORD") {
          return new MochaResult<string>(Database.GetPassword());
        } else if(queryPaths[0] == "GETDESCRIPTION") {
          return new MochaResult<string>(Database.GetDescription());
        } else if(queryPaths[0] == "GETLOGS") {
          return new MochaCollectionResult<MochaLog>(Database.GetLogs());
        } else if(queryPaths[0] == "GETDATAS") {
          List<MochaData> datas = new List<MochaData>();
          IEnumerable<XElement> tableRange = Database.Doc.Root.Element("Tables").Elements();
          for(int index = 0; index < tableRange.Count(); index++) {
            datas.AddRange(GETDATAS(tableRange.ElementAt(index).Name.LocalName).collection);
          }
          return new MochaCollectionResult<MochaData>(datas);
        } else if(queryPaths[0] == "TABLECOUNT") {
          return new MochaResult<int>(Database.Doc.Root.Elements().Count());
        } else
          throw new MochaException("Invalid query. The content of the query could not be processed, wrong!");
      } else if(queryPaths.Length == 2) {
        if(queryPaths[0] == "GETTABLE") {
          return new MochaResult<MochaTable>(Database.GetTable(queryPaths[1]));
        } else if(queryPaths[0] == "GETCOLUMNS") {
          return new MochaCollectionResult<MochaColumn>(Database.GetColumns(queryPaths[1]));
        } else if(queryPaths[0] == "GETFIRSTCOLUMN_NAME") {
          return GETFIRSTCOLUMN_NAME(queryPaths[1]);
        } else if(queryPaths[0] == "EXISTSLOG") {
          return new MochaResult<bool>(Database.ExistsLog(queryPaths[1]));
        } else if(queryPaths[0] == "GETROWS") {
          return new MochaCollectionResult<MochaRow>(Database.GetRows(queryPaths[1]));
        } else if(queryPaths[0] == "GETDATAS") {
          return GETDATAS(queryPaths[1]);
        } else if(queryPaths[0] == "GETTABLEDESCRIPTION") {
          return new MochaResult<string>(Database.GetTableDescription(queryPaths[1]));
        } else if(queryPaths[0] == "COLUMNCOUNT") {
          return COLUMNCOUNT(queryPaths[1]);
        } else if(queryPaths[0] == "ROWCOUNT") {
          try {
            return new MochaResult<int>(Database.Doc.Root.Element("Tables").Elements(queryPaths[1]).Elements(
                GETFIRSTCOLUMN_NAME(queryPaths[1]).Value).Elements().Count());
          } catch(Exception excep) {
            throw excep;
          }
        } else if(queryPaths[0] == "DATACOUNT") {
          return new MochaResult<int>(Database.GetDataCount(queryPaths[1],GETFIRSTCOLUMN_NAME(queryPaths[1]))
              * COLUMNCOUNT(queryPaths[1]));
        } else if(queryPaths[0] == "EXISTSTABLE") {
          return new MochaResult<bool>(Database.ExistsTable(queryPaths[1]));
        } else if(queryPaths[0] == "#REMOVETABLE") {
          return new MochaResult<bool>(Database.RemoveTable(queryPaths[1]));
        } else
          throw new MochaException("Invalid query. The content of the query could not be processed, wrong!");
      } else if(queryPaths.Length == 3) {
        if(queryPaths[0] == "GETCOLUMN") {
          return new MochaResult<MochaColumn>(Database.GetColumn(queryPaths[1],queryPaths[2]));
        } else if(queryPaths[0] == "DATACOUNT") {
          return new MochaResult<int>(Database.GetDataCount(queryPaths[1],queryPaths[2]));
        } else if(queryPaths[0] == "EXISTSCOLUMN") {
          return new MochaResult<bool>(Database.ExistsColumn(queryPaths[1],queryPaths[2]));
        } else if(queryPaths[0] == "GETDATAS") {
          return new MochaCollectionResult<MochaData>(Database.GetDatas(queryPaths[1],queryPaths[2]));
        } else if(queryPaths[0] == "GETCOLUMNDESCRIPTION") {
          return new MochaResult<string>(Database.GetColumnDescription(queryPaths[1],queryPaths[2]));
        } else if(queryPaths[0] == "GETCOLUMNDATATYPE") {
          return new MochaResult<MochaDataType>(Database.GetColumnDataType(queryPaths[1],queryPaths[2]));
        } else if(queryPaths[0] == "#REMOVECOLUMN") {
          return new MochaResult<bool>(Database.RemoveColumn(queryPaths[1],queryPaths[2]));
        } else if(queryPaths[0] == "#REMOVEROW") {
          return new MochaResult<bool>(Database.RemoveRow(queryPaths[1],int.Parse(queryPaths[2])));
        } else
          throw new MochaException("Invalid query. The content of the query could not be processed, wrong!");
      } else if(queryPaths.Length == 4) {
        if(queryPaths[0] == "EXISTSDATA") {
          return new MochaResult<bool>(Database.ExistsData(queryPaths[1],queryPaths[2],queryPaths[3]));
        } else if(queryPaths[0] == "GETDATA") {
          return new MochaResult<MochaData>(Database.GetData(queryPaths[1],queryPaths[2],int.Parse(queryPaths[3])));
        } else
          throw new MochaException("Invalid query. The content of the query could not be processed, wrong!");
      } else
        throw new MochaException("Invalid query. The content of the query could not be processed, wrong!");
    }

    #endregion GetRun

    #endregion Members

    #region Private Members

    /// <summary>
    /// Return column count of table.
    /// </summary>
    /// <param name="name">Name of table.</param>
    private MochaResult<int> COLUMNCOUNT(string name) {
      if(!Database.ExistsTable(name))
        throw new MochaException("Table not found in this name!");

      IEnumerable<XElement> columnElements = Database.Doc.Root.Element("Tables").Elements(name).Elements();
      return columnElements.Count();
    }

    /// <summary>
    /// Return all datas of table.
    /// </summary>
    /// <param name="name">Name of table.</param>
    private MochaCollectionResult<MochaData> GETDATAS(string name) {
      if(!Database.ExistsTable(name))
        throw new MochaException("Table not found in this name!");

      List<MochaData> datas = new List<MochaData>();

      IEnumerable<XElement> columnRange = Database.Doc.Root.Element("Tables").Elements(name).Elements();
      for(int index = 0; index < columnRange.Count(); index++) {
        datas.AddRange(Database.GetDatas(name,columnRange.ElementAt(index).Name.LocalName));
      }

      return new MochaCollectionResult<MochaData>(datas);
    }

    /// <summary>
    /// Return first column name of table.
    /// </summary>
    /// <param name="name">Name of table.</param>
    private MochaResult<string> GETFIRSTCOLUMN_NAME(string name) {
      if(!Database.ExistsTable(name))
        throw new MochaException("Table not found in this name!");

      XElement firstColumn = (XElement)Database.Doc.Root.Element("Tables").Element(name).FirstNode;
      return firstColumn == null ? null : firstColumn.Name.LocalName;
    }

    #endregion Private Members

    #region Overrides

    /// <summary>
    /// Returns command property value of <see cref="MochaQ"/>.
    /// </summary>
    public override string ToString() =>
      MochaQ.Command;

    #endregion Overrides

    #region Properties

    /// <summary>
    /// Is embedded query in database.
    /// </summary>
    public bool IsDatabaseEmbedded { get; private set; }

    /// <summary>
    /// MochaDatabase object to use querying.
    /// </summary>
    public MochaDatabase Database {
      get => database;
      set {
        if(IsDatabaseEmbedded)
          throw new MochaException("This is embedded in database, can not set database!");

        if(value == null)
          throw new MochaException("This MochaDatabase is not affiliated with a database!");

        if(value == database)
          return;

        database = value;
      }
    }

    /// <summary>
    /// Active MochaQ query.
    /// </summary>
    public MochaQCommand MochaQ { get; set; }

    #endregion Properties
  }
}
