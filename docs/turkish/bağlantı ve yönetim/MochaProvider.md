MochaProvider, ba�lant�larda kullan�lan ba�lant� dizelerini i�leyen bir yap�d�r. Kodda kullan�lacak bir nesnedir.

### �rnekler
Bu kod, dosya yolunu ve veritaban� parolas�n� belirten bir ba�lant� dizesine sahip yeni bir sa�lay�c� olu�turur.
```C#
MochaProvider provider =
    new MochaProvider("Path=.\\Databases\\Personels.mhdb; Password=MyDb045089");
```

Bu kod, belirtilen ba�lant� dizesinde belirtilen �zniteli�i d�nd�r�r.
```C#
MochaProviderAttribute password =
    MochaProvider.GetAttribute("Password",provider.ConnectionString);
```
