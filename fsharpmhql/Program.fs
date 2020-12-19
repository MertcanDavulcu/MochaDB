﻿// MIT License
// 
// Copyright (c) 2020 Mertcan Davulcu
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

// Libraries
open System
open System.Drawing
open System.Windows.Forms
open MochaDB
open MochaDB.Mhql

// Fields
let form : Form = new Form()
let codebox : RichTextBox = new RichTextBox()
let gridview : DataGridView = new DataGridView()

// Functions

/// <summary>
/// Keywdown event of codebox.
/// </summary>
/// <param name="e">Key arguments.</param>
let codebox_keydown(e: KeyEventArgs) : unit =
  if e.KeyData = Keys.F5 then
    let path : MochaPath = new MochaPath(__SOURCE_DIRECTORY__)
    path.ParentDirectory()
    path.ParentDirectory()
    let database : MochaDatabase = new MochaDatabase("path=" + path.ToString() + "/tests/testdb; autoconnect=true")
    try
      gridview.Columns.Clear()
      let command : MochaDbCommand = new MochaDbCommand(database)
      let result : MochaTableResult = command.ExecuteScalar(codebox.Text) :?> MochaTableResult
      let table : System.Data.DataTable = System.Data.DataTable()
      for column in result.Columns do
        table.Columns.Add(column.Name)
      for row in result.Rows do
        table.Rows.Add([| for data in row.Datas -> data.Data |])
        gridview.DataSource <- table
    with
    | :? MochaException as excep ->
      printfn "ERROR: %s" excep.Message
    | :? Exception as excep ->
      printfn "ERROR: %s" excep.Message
      database.Dispose()

/// <summary>
/// Entry point.
/// </summary>
/// <param name="argv">Arguments.</param>
/// <returns>Exit code.</returns>
[<EntryPoint>]
let main(argv:string[]) : int =
  Console.Title <- "F# MHQL Test Console"
  form.Text <- "F# based mhql test application"
  form.ShowIcon <- false
  form.Size <- new Size(1000,500)
  form.StartPosition <- FormStartPosition.CenterScreen
  codebox.BorderStyle <- BorderStyle.None
  codebox.Anchor <- AnchorStyles.Top ||| AnchorStyles.Bottom ||| AnchorStyles.Left ||| AnchorStyles.Right
  codebox.Location <- Point.Empty
  codebox.Size <- new Size(form.Width,form.Height/2)
  codebox.KeyDown.Add(codebox_keydown)
  form.Controls.Add(codebox)
  gridview.Anchor <- AnchorStyles.Bottom ||| AnchorStyles.Left ||| AnchorStyles.Right
  gridview.Location <- new Point(0,codebox.Height)
  gridview.Size <- new Size(form.Width,form.Height/2)
  form.Controls.Add(gridview)
  Application.Run(form)
  0
