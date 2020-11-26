Connection strings are used for connections. They are processed by ``MochaProvider``.  It is an object to use in code.

### �rnekler
```C#
"Path = .\\Databases\\Personels.mhdb"
"Path = .\\Databases\\Personels; Password=1234"
"Password= 1234;Path =.\\Databases\\Personels.mhdb; AutoConnect= True"
```

### �znitelikler
�znitelikler, ba�lant� dizesindeki �zelliklerdir. <br>
Ba�lant� dizesinin olu�turulmas�na izin veren ana ��elerdir ve hepsinin kendi benzersiz i�levleri vard�r. <br>
Bir �znitelik, ba�lant� dizesinde en fazla bir kez tan�mlanabilir. Bir ba�lant� dizesi "Path" �zniteli�ini tan�mlam�� olmal�d�r. <br>
Bir �znitelik tan�mlanmam��sa veya yanl�� tan�mlanm��sa, varsay�lan de�eri atan�r. <br>
Birden fazla �znitelik tan�mlanm��sa �znitelikler ``;`` ile ayr�lmal�d�r.
<table border="1">
    <tr>
        <td width="15%"><strong>Ad</strong></td>
        <td width="75%"><strong>�znitelik</strong></td>
        <td width="20%"><strong>De�er tipi</strong></td>
    </tr>
    <tr>
        <td>Path</td>
        <td>Veritaban�n�n yolunu g�sterir.</td>
        <td>String</td>
    </tr>
    <tr>
        <td>Password</td>
        <td>Veritaban�n�n �ifresini belirtir.</td>
        <td>String</td>
    </tr>
    <tr>
        <td>AutoConnect</td>
        <td">E�er true olarak ayarlanm��sa, otomatik olarak ba�lant� a��l�r.</td>
        <td>True/False</td>
    </tr>
    <tr>
        <td>Readonly</td>
        <td">E�er true olarak ayarlan�rsa veritaban�na o ba�lant� �st�nden veri yazamazs�n�z.</td>
        <td>True/False</td>
    </tr>
    <tr>
        <td>AutoCreate</td>
        <td">True ise, her ba�lant� a��ld���nda veritaban� yoksa yeni bir bo� veritaban� olu�turulur.</td>
        <td>True/False</td>
    </tr>
    <tr>
        <td>Logs</td>
        <td">If true, a copy of the database is kept in database whenever the content changes.</td>
        <td>True/False</td>
    </tr>
</table>

### Fonksiyonlar
Fonksiyonlar, i�inizi kolayla�t�rmak i�in kullanabilece�iniz basit ve kullan�m� kolay alternatiflerdir.<br>

- ```>SOURCEDIR<subcount``` <br>
Bu i�levi yaln�zca ``Path`` �zniteli�inde kullanabilirsiniz. Komut dosyas�na eri�ilen konumu d�nd�r�r. Alt hesap parametresi ise daha y�ksek bir dizine ka� kez gidece�ini g�steren say�d�r. Konuma do�rudan eri�im sa�lamak istiyorsan�z, ``0`` yazabilirsiniz. Hi� belirtilmezse, i�lev tan�nmaz ve bir hataya neden olabilece�inden ``Path`` �zniteli�inin i�eri�ini etkileyebilir.
<br><b>Examples</b><br>
```>SOURCEDIR<0```: �unu d�nd�r�r ```C:\Users\user\Desktop``` <br>
```Path= >SOURCEDIR<3\testdocs\testdb.mhdb``` -- D�nd�r�r: ```C:\``` <br>
```Path= >SOURCEDIR<0\testdocs\testdb.mhdb``` -- D�nd�r�r: ```C:\Users\user\Desktop``` <br>
```Path= >SOURCEDIR<1\testdocs\testdb.mhdb``` -- D�nd�r�r: ```C:\Users\user``` <br>
```Path= >SOURCEDIR<\testdocs\testdb.mhdb``` -- ��kt� Path: ```>SOURCEDIR<\testdocs\testdb.mhdb```
