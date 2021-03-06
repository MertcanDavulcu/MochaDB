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

namespace server

open System
open System.Collections.Generic

open MochaDB

/// <summary>
/// Config settings.
/// </summary>
[<Class>]
type configs() =
  class
  /// <summary>
  /// Name of server.
  /// </summary>
  static member val name:string = "MochaDB Server" with get, set

  /// <summary>
  /// IP address.
  /// </summary>
  static member val address:string = null with get, set

  /// <summary>
  /// Port.
  /// </summary>
  static member val port:int = 8085 with get, set

  /// <summary>
  /// The maximum length of the pending connections queue.
  /// </summary>
  static member val listen:int = 10 with get, set

  /// <summary>
  /// Lock server connection with password.
  /// </summary>
  static member val lock:bool = false with get, set

  /// <summary>
  /// Protection password of server connection.
  /// </summary>
  static member val key:string = String.Empty with get, set

  /// <summary>
  /// Connected databases of server connection.
  /// </summary>
  static member val connections:System.Collections.Generic.List<MochaDatabase> =
    new System.Collections.Generic.List<MochaDatabase>() with get, set
  end
