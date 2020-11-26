Veritaban�ndaki bir tabloyu temsil eder. S�tun ve sat�rlardan olu�ur. <br>
Kodda kullan�lacak bir nesnedir.

### �rnekler

``Personels`` ad�nda yeni bir bo� tablo olu�turur.
```C#
MochaTable PersonelTable = new MochaTable("Personels");
```

``ID``, ``Name`` ve ``Age`` ad�nda s�tunlar ekler.
```C#
PersonelTable.Columns.Add("ID",MochaDataType.AutoInt);
PersonelTable.Columns.Add("Name",MochaDataType.String);
PersonelTable.Columns.Add("Age",MochaDataType.Int32);
```

``Mertcan`` ve ``18`` verilerini ``Name`` ve ``Age`` s�tunlar�na ekler. `` ID '' s�tunu AutoInt oldu�u i�in atama yap�lam�yor, ancak yine de belirtmemiz gerekiyor (Veri say�s� s�tun say�s�yla ayn� olmal�d�r), bu y�zden AutoInt olan s�tuna Null de�er belirtiyorum. Null de�er i�lenmeyecek ve AutoInt taraf�ndan atanan numara ile de�i�tirilecektir.
```C#
PersonelTable.Rows.Add(null,"Mertcan",18);
```
