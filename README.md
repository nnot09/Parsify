Xml
```xml
<?xml version="1.0" encoding="utf-8"?>
<ParsifyModule xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Name>TestTextFormat</Name>
  <Version>1.6</Version>
  <Line StartsWith="HEAD">
    <Field Type="string" Name="NOTE" Position="5" Length="10" />
    <Field Type="string" Name="FLAG" Position="15" Length="2" />
    <Field Type="string" Name="ALPHA" Position="17" Length="2" />
    <Field Type="string" Name="BETA" Position="19" Length="6" Optional="true" />
    <Field Type="string" Name="GAMMA" Position="25" Length="1" />
  </Line>
  <Line StartsWith="POS">
    <Field Type="int" Name="QUANTITY" Position="4" Length="2">
      <Translate Value="^42$" Display="answer to everything" IgnoreCase="true" SearchMode="Regex" />
      <Translate Value="43" Display="missed the answer" SearchMode="StartsWith" />
    </Field>
  </Line>
  <Line StartsWith="DELTA">
	<Field Type="string" Name="TEST" Position="6" Length="3" />
  </Line>
</ParsifyModule>
```
### Foreground highlighting
![fg-highlight](https://i.gyazo.com/abeb5b694453e0ea73a3382f4d586ac4.png)

### Background highlighting
![bg-highlight](https://i.gyazo.com/d12aef5196f31b041698da6598f5aa78.png)

### App configuration
![app-config](https://i.gyazo.com/2efcd2be8e8e8a12163044d106a1c0ba.png)

### Error messages
![error-msg](https://i.gyazo.com/4fe150aa09f5fff6dc0ba177576dec70.png)

### Mandatory and optional fields are missing ('?' ones are mandatory)
![error-msg](https://i.gyazo.com/0c9a65e016885148793a7daeec9dddfe.png)
