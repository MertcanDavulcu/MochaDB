Bir s�tundaki tek bir veriyi temsil eder. Verileri "object" olarak tutar. <br>
Kodda kullan�lacak bir nesnedir.


### �rnekler

"String" veri tipinde "Mertcan" de�eri ile veri olu�turur.
```C#
MochaData myData = new MochaData(MochaDataType.String,"Mertcan");
```

Veri "Merhaba D�nya!" olarak ayarlan�r.
```C#
myData.Data = "Hello World!";
```

Bununla birlikte, veri t�r� d���nda bir veri girmeye �al���rsan�z, Exception al�rs�n�z. Misal;
```C#
myData.Data = 18;
```

Veri t�r�n� "Int32" olarak ayarlar. Veri yeni veri tipiyle uyumluysa ayn� kalacak, de�ilse uyumlu varsay�lan de�ere s�f�rlanacakt�r.
```C#
myData.DataType = MochaDataType.Int32;
```
