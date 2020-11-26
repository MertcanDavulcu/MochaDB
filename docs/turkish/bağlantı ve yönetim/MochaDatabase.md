MochaDatabase, MochaDB veritaban� dosyalar�na eri�ir ve bunlar� y�netme olana�� sunar. <br>
Fonksiyonlar yard�m�yla kolay y�netim sa�lar. <br>
Kodda kullan�lacak bir nesnedir.

### �rnekler
MochaDB veri taban�na ba�lanmak i�in ba�lant� dizesini kullan�n.
```C#
MochaDatabase db = new MochaDatabase("connection string");
```

Yaln�zca dosya yolunu ve parolay� belirterek ba�lan�n.
```C#
MochaDatabase db = new MochaDatabase("path","password");
```

Ba�lant� kapal�yken herhangi bir i�lem yapmaya �al��mak bir hatayla sonu�lanacakt�r. ��lemler s�ras�nda ba�lant� a��k olmal�d�r.
```C#
db.Connect(); // Ba�lant�y� a�ar.
db.Disconnect(); // Ba�lant�y� kapar.
```

### S�k kullan�lan Fonksiyonlar
#### Genel
<table border="1">
    <tr>
        <td><strong>GetPassword()</strong></td>
        <td width="100%">Veritaban�n�n �ifresini d�nd�r�r.</td>
    </tr>
    <tr>
        <td><strong>SetPassword(string)</strong></td>
        <td width="100%">Veritaban�n�n �ifresini ayarlar.</td>
    </tr>
    <tr>
        <td><strong>GetDescription()</strong></td>
        <td width="100%">Veritaban�n�n a��klamas�n� d�nd�r�r.</td>
    </tr>
    <tr>
        <td><strong>SetDescription(string)</strong></td>
        <td width="100%">Veritaban�n�n a��klamas�n� ayarlar.</td>
    </tr>
    <tr>
        <td><strong>Reset()</strong></td>
        <td width="100%">MochaDB, veritaban� dosyas�n�n varl���n� kontrol eder ve yoksa yeni bir dosya olu�turur. T�M VER�LER KAYBOLUR!</td>
    </tr>
    <tr>
        <td><strong>GetXML()</strong></td>
        <td width="100%">Veritaban�n�n XML �emas�n� d�nd�r�r.</td>
    </tr>
</table>

#### Tablolar
<table border="1">
    <tr>
        <td><strong>AddTable(MochaTable)</strong></td>
        <td width="100%">Bir tablo ekler.</td>
    </tr>
    <tr>
        <td><strong>RemoveTable(string)</strong></td>
        <td width="100%">Ad�n� kullanarak bir tabloyu siler.</td>
    </tr>
    <tr>
        <td><strong>GetTable(string)</strong></td>
        <td width="100%">Ad�n� kullanarak bir tabloyu al�r.</td>
    </tr>
    <tr>
        <td><strong>GetTables()</strong></td>
        <td width="100%">Veritaban�ndaki t�m tablolar� al�r.</td>
    </tr>
    <tr>
        <td><strong>ExistsTable(string)</strong></td>
        <td width="100%">Ad�na g�re tablonun olup olmad���n� kontrol eder.</td>
    </tr>
</table>

#### S�tunlar
<table border="1">
    <tr>
        <td><strong>AddColumn(string,MochaColumn)</strong></td>
        <td width="100%">Bir s�tun ekler.</td>
    </tr>
    <tr>
        <td><strong>RemoveColumn(string,string)</strong></td>
        <td width="100%">Ad�n� kullanarak bir s�tunu siler.</td>
    </tr>
    <tr>
        <td><strong>GetColumns(string)</strong></td>
        <td width="100%">Ad�n� kullanarak bir tablodaki t�m s�tunlar� al�r.</td>
    </tr>
    <tr>
        <td><strong>ExistsColumn(string,string)</strong></td>
        <td width="100%">Ad�na g�re s�tunun olup olmad���n� kontrol eder.</td>
    </tr>
</table>

#### Sat�rlar
<table border="1">
    <tr>
        <td><strong>AddRow(string,MochaRow)</strong></td>
        <td width="100%">Bir sat�r ekler.</td>
    </tr>
    <tr>
        <td><strong>RemoveRow(string,int)</strong></td>
        <td width="100%">�ndeks kullanarak bir sat�r� siler.</td>
    </tr>
    <tr>
        <td><strong>GetRow(string,int)</strong></td>
        <td width="100%">�ndeks kullanarak bir sat�r� al�r.</td>
    </tr>
    <tr>
        <td><strong>GetRows(string)</strong></td>
        <td width="100%">Ad�na g�re bir tablodaki t�m sat�rlar� al�r.</td>
    </tr>
</table>

#### Veriler
<table border="1">
    <tr>
        <td><strong>AddData(string,string,MochaData)</strong></td>
        <td width="100%">Veri ekler.</td>
    </tr>
    <tr>
        <td><strong>UpdateData(string,string,int,object)</strong></td>
        <td width="100%">�ndeks kullanarak veriyi g�nceller.</td>
    </tr>
    <tr>
        <td><strong>GetData(string,string,int)</strong></td>
        <td width="100%">�ndeks kullanarak veriyi al�r.</td>
    </tr>
    <tr>
        <td><strong>GetDatas(string,string)</strong></td>
        <td width="100%">Ad�n� kullanarak s�tundaki t�m verileri al�r.</td>
    </tr>
</table>
