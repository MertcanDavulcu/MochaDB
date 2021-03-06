﻿//
// MIT License
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
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE
// OR OTHER DEALINGS IN THE SOFTWARE.

namespace terminal

open System

/// <summary>
/// Terminal declare.
/// </summary>
[<Class>]
type terminal() =
  class
  /// <summary>
  /// Print message to screen with color.
  /// </summary>
  /// <param name="msg">Message.</param>
  /// <param name="color">Color of message.</param>
  static member printc(msg:string, color:ConsoleColor) : unit =
    let realColor:ConsoleColor = Console.ForegroundColor
    Console.ForegroundColor <- color
    printf "%s" msg
    Console.ForegroundColor <- realColor

  /// <summary>
  /// Print message and new line to screen with color.
  /// </summary>
  /// <param name="msg">Message.</param>
  /// <param name="color">Color of message.</param>
  static member printnc(msg:string, color:ConsoleColor) : unit =
    terminal.printc(msg, color)
    printfn ""

  /// <summary>
  /// Print error message.
  /// </summary>
  /// <param name="msg">Message.</param>
  static member printError(msg:string) : unit =
    terminal.printnc(msg, ConsoleColor.Red)

  /// <summary>
  /// Returns input with msg.
  /// </summary>
  /// <param name="msg">Message.</param>
  /// <returns>Input.</returns>
  static member getInput(msg:string) : string =
    printf "%s" msg
    Console.ReadLine().TrimStart()

  /// <summary>
  /// Returns input with msg and color.
  /// </summary>
  /// <param name="msg">Message.</param>
  /// <param name="color">Color of message.</param>
  /// <returns>Input.</returns>
  static member getInput(msg:string, color:ConsoleColor) : string =
    terminal.printc(msg, color)
    Console.ReadLine().TrimStart()

  /// <summary>
  /// Returns input with pwd.
  /// </summary>
  /// <returns>Input.</returns>
  static member getInput() : string =
    terminal.getInput(terminal.pwd + " ", ConsoleColor.White)

  /// <summary>
  /// Working directory.
  /// </summary>
  static member val pwd:string = Environment.CurrentDirectory with get, set

  /// <summary>
  /// Version of terminal.
  /// </summary>
  static member val version:string = "0.0.1" with get

  /// <summary>
  /// Start arguments of terminal.
  /// </summary>
  static member val startArgs:string[] = [| String.Empty |] with get, set

  /// <summary>
  /// Terminal execute in start arguments only mode.
  /// </summary>
  static member val argMode:bool = false with get, set

  /// <summary>
  /// Last index of start arguments.
  /// </summary>
  static member val argsIndex:int = 0 with get, set
  end
