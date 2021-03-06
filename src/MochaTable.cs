﻿namespace MochaDB {
  using System;

  using MochaDB.engine;

  /// <summary>
  /// This is table object for MochaDB.
  /// </summary>
  public class MochaTable {
    #region Fields

    private string name;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Create new instance of <see cref="MochaTable"/>.
    /// </summary>
    /// <param name="name">Name of table.</param>
    /// <param name="description">Description of table.</param>
    public MochaTable(string name,string description = "") {
      Name = name;
      Description = description;
      Columns = new MochaColumnCollection();
      Columns.Changed+=Column_Changed;
      Rows = new MochaRowCollection();
      Rows.Changed+=Row_Changed;
      Rows.RowChanged+=Row_Changed;
    }

    #endregion Constructors

    #region Events

    /// <summary>
    /// This happens after name changed;
    /// </summary>
    public event EventHandler<EventArgs> NameChanged;
    protected virtual void OnNameChanged(object sender,EventArgs e) {
      //Invoke.
      NameChanged?.Invoke(sender,e);
    }

    #endregion Events

    #region Content Events

    private void Column_Changed(object sender,EventArgs e) =>
      SetRowsByDatas();

    private void Row_Changed(object sender,EventArgs e) =>
      SetDatasByRows();

    #endregion Content Events

    #region Internal Members

    /// <summary>
    /// Set datas by row datas.
    /// </summary>
    internal protected virtual void SetDatasByRows() {
      for(int columnIndex = 0; columnIndex < Columns.Count; ++columnIndex)
        Columns[columnIndex].Datas.collection.Clear();

      for(int rowIndex = 0; rowIndex < Rows.Count; ++rowIndex) {
        MochaRow currentRow = Rows[rowIndex];

        if(currentRow.Datas.Count != Columns.Count)
          throw new ArgumentException("The number of data must be equal to the number of columns!");

        for(int columnIndex = 0; columnIndex < Columns.Count; ++columnIndex) {
          MochaColumn currentColumn = Columns[columnIndex];

          if(currentColumn.DataType!=MochaDataType.AutoInt)
            currentColumn.Datas.collection.Add(currentRow.Datas[columnIndex]);
          else {
            MochaData data = new MochaData {
              data=currentColumn.Datas.Count > 0 ?
                1 + (int)currentColumn.Datas[currentColumn.Datas.Count - 1].Data : 1,
              dataType=MochaDataType.AutoInt
            };
            currentColumn.Datas.collection.Add(data);
            currentRow.Datas.collection[columnIndex]= data;
          }
        }
      }
    }

    /// <summary>
    /// Set rows by column datas.
    /// </summary>
    internal protected virtual void SetRowsByDatas() {
      Rows.collection.Clear();
      if(Columns.Count == 0)
        return;
      MochaData[] datas = new MochaData[Columns.Count];
      for(int dataIndex = 0; dataIndex < Columns[0].Datas.Count; ++dataIndex) {
        for(int columnIndex = 0; columnIndex < Columns.Count; ++columnIndex) {
          MochaColumn currentColumn = Columns[columnIndex];
          if(currentColumn.Datas.Count < dataIndex + 1)
            datas[columnIndex] = new MochaData {
              dataType = currentColumn.DataType,
              data = MochaData.TryGetData(currentColumn.DataType,string.Empty)
            };
          else
            datas[columnIndex] = currentColumn.Datas[dataIndex];
        }
        Rows.collection.Add(new MochaRow(datas));
      }
    }

    #endregion Internal Members

    #region Members

    /// <summary>
    /// Short datas.
    /// </summary>
    /// <param name="index">Index of column.</param>
    public virtual void ShortDatas(int index) {
      Rows.collection.Sort((x,y) => x.Datas[index].ToString().CompareTo(y.Datas[index].ToString()));
      SetDatasByRows();
    }

    /// <summary>
    /// Sort columns by name.
    /// </summary>
    public virtual void ShortColumns() {
      Columns.collection.Sort((X,Y) => X.Name.CompareTo(Y.Name));
      SetRowsByDatas();
    }

    /// <summary>
    /// Returns table empty state.
    /// </summary>
    public virtual bool Any() =>
      Rows.Count == 0;

    #endregion Members

    #region Properties

    /// <summary>
    /// Name.
    /// </summary>
    public virtual string Name {
      get => name;
      set {
        value=value.Trim();
        if(string.IsNullOrWhiteSpace(value))
          throw new NullReferenceException("Name is cannot null or whitespace!");

        Engine_NAMES.CheckThrow(value);

        if(value==name)
          return;

        name=value;
        OnNameChanged(this,new EventArgs());
      }
    }

    /// <summary>
    /// Description.
    /// </summary>
    public virtual string Description { get; set; }

    /// <summary>
    /// Columns of table.
    /// </summary>
    public virtual MochaColumnCollection Columns { get; }

    /// <summary>
    /// Rows of table.
    /// </summary>
    public virtual MochaRowCollection Rows { get; }

    #endregion Properties
  }
}
