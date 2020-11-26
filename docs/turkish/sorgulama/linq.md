### �rnekler

"M" harfiyle ba�layan tablolar� al�r.
```C#
MochaDatabase db = new MochaDatabase("path=.\\mydb.mhdb");
var datas =
    from value in db.GetDatas("Personels","Name")
    where value.ToString()[0] == 'M'
    select value;
```

18-30 ya�lar� aras�ndaki kay�tlar� al�r ve k���kten b�y��e s�ralan�r.
```C#
MochaDatabase db = new MochaDatabase("path=.\\mydb.mhdb");
MochaTable personels = db.GetTable("Personels");
var datas =
    from value in personels.Rows
    where ((int)value.Datas[2]) >= 18 && 30 <= ((int)value.Datas[2])
    select value;
```
