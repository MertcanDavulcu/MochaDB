﻿namespace submodules.connection

open System

open MochaDB
open MochaDB.Mhql

open utils
open terminal

/// <summary>
/// MHQL module.
/// </summary>
type mhql() =
  /// <summary>
  /// Process command in module.
  /// </summary>
  /// <param name="db">Database connection.</param>
  /// <param name="cmd">Command.</param>
  static member proc(db:MochaDatabase, cmd:string) : unit =
    let mhql:MochaDbCommand = new MochaDbCommand(db)
    
    /// <summary>
    /// Execute MHQL command.
    /// </summary>
    /// <param name="query">Query.</param>
    let exec(query:string) : unit =
      let xml:bool = query.[0] = '$'
      mhql.Command <- if xml then query.Substring(1) else query
      try
        match mhql.Command.Split(" ", 2).[0].ToUpperInvariant() with
        | "USE" ->
          let table = mhql.ExecuteScalar()
          if table = null
          then printfn "NULL"
          else
            if xml then printfn "%s" (parser.parseTableToXmlString(table))
            else cli.printTable(table)
        | _ -> terminal.printError("MHQL query is invalid!")
      with
      | :? Exception as except ->
        terminal.printError(except.Message)

    if cmd = String.Empty then
      let mutable break:bool = false
      while break = false do
        let input:string = terminal.getInput(db.Name + "\MHQL ", ConsoleColor.White)
        if input = String.Empty then
          break <- true
        else
          exec(input)
    else
      exec(cmd)
