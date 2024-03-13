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
