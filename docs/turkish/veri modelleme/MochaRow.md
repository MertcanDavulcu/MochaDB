Tablodaki bir sat�r� temsil eder. Her veri, o sat�rdaki bir s�tunun verilerini g�sterir. <br>
Kodda kullan�lacak bir nesnedir.

### �rnekler

Yeni bo� bir sat�r olu�turur.
```C#
MochaRow myRow = new MochaRow();
```

"String" veri t�r�nde bir MochaData nesnesi olu�turur, de�erini "Mertcan" olarak ayarlar ve sat�r verilerine ekler. Veri t�r� "String" oldu�undan, bu s�tunun veri t�r� de "String" anlam�na gelir.
```C#
myRow.Add(new MochaData(MochaDataType.String,"Mertcan"));
```

"0" dizinindeki veriyi siler.
```C#
myRow.RemoveDataAt(0);
```
