``MochaQuery`` MochaQ sorgular�n� i�lemenizi sa�lar. <br>
Bir ``MochaDatabase`` veritaban� temsilcisine ba�lanmal�d�r.

### �rnekler
Yeni bir ``MochaQuery`` olu�turur ve bir ``MochaDatabase`` olu�turup referans ekler.
```C#
MochaQuery query = new MochaQuery(new MochaDatabase("path=.\\mydb.mhdb"));
```

<strong>UYARI</strong><br>
Normal bir �ekilde olu�turulmas� gerekmedi�inde tehlikeli olabilir. <br>
`` MochaDatabase`` nesnesine sahip olman�z� gerektirir, ancak ``MochaDatabase`` nesnesinin `` Query`` �zelli�inin zaten kendisine e�lenmi� bir ``MochaQuery`` nesnesi d�nd�rd���n� unutmay�n.

### S�k kullan�lan fonksiyonlar
<table border="1">
    <tr>
        <td><strong>Run()</strong></td>
        <td width="100%">Etkin MochaQ sorgusunu �al��t�r�r. Gelen bir de�er olsa bile geri d�nmeyecektir. MochaQ "Run" komutlar� i�in.</td>
    </tr>
    <tr>
        <td><strong>GetRun()</strong></td>
        <td width="100%">Etkin MochaQ sorgusunu �al��t�r�r. Gelen de�eri d�nd�r�r. "GetRun" MochaQ komutlar� i�in.</td>
    </tr>
    <tr>
        <td><strong>Dynamic()</strong></td>
        <td width="100%">De�er d�nd�r�l�rse, i�levi d�nd�r�r ve i�levi yerine getirir; de�ilse, sadece i�levi yerine getirir. "Dynamic" MochaQ komutlar� i�in.</td>
    </tr>
</table>
