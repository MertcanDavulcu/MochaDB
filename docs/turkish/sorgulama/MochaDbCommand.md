MochaDbCommand MHQL sorgular�n� i�lemenizi sa�lar. <br>
Veritaban� temel olarak ayarlanmal� ve MochaDatabase bir veritaban�na ba�lanmal�d�r.

### �rnekler
Yeni bir MochaQuery olu�turur ve bir MochaDatabase olu�turur ve bir referans ekler.
```C#
MochaDatabase db = new MochaDatabase("path=.\\mydb.mhdb");
MochaDbCommand query = new MochaDbCommand("USE *",db);
```

Veritaban�ndaki sorguyu i�ler ve `` ExecuteScalar`` ile tek veriyi �eker. `` ExecuteScalar '' en mant�kl� se�imdir ��nk� `` USE '' anahtar kelimesi her zaman bir tablo d�nd�r�r. D�nd�r�c� her zaman ``nesne`` t�r�n� d�nd�rd��� i�in, ne t�r oldu�unu belirlememiz gerekir. ``USE`` MHQL sorgular�ndan d�nd�r�len tablo her zaman MochaTableResult'dur.
```C#
MochaDbCommand command = new MochaDbCommand("USE *",db);
MochaTableResult table = command.ExecuteScalar() as MochaTableResult;
```

T�m nesneleri �eker ve ``ExecuteReader`` ile al�r. Daha sonra sanki veriler okunmu� gibi ``while`` d�ng�s�n� d�nd�r�r ve de�erini ``MochaTable`` olarak al�r ve ad�n� bir mesaj olarak d�nd�r�r.
```C#
MochaDbCommand command = new MochaDbCommand("SELECT ()",db);
MochaReader<object> results = command.ExecuteReader();
while(results.Read())
  Console.WriteLine(((MochaTable)item).Name);
```

### S�k kullan�lan fonksiyonlar
<table border="1">
    <tr>
        <td><strong>ExecuteScalar()</strong></td>
        <td width="100%">D�nd�r�len sonu�lardan yaln�zca birini al�r. Tek sonu� d�nd�recek bir sorgu yazd�ysan�z en mant�kl� se�im budur.</td>
    </tr>
    <tr>
        <td><strong>ExecuteScalarTable()</strong></td>
        <td width="100%">Sonucu MochaTableResult olarak d�nd�r�r. Tek bir tablo alan sorgular i�in kullan��l�d�r.</td>
    </tr>
    <tr>
        <td><strong>ExecuteReader()</strong></td>
        <td width="100%">Gelen t�m sonu�lar� 'MochaReader' ile d�nd�r�r. Birden fazla sonu� olas�l��� olan sorgularda kullan�lmal�d�r.</td>
    </tr>
</table>
