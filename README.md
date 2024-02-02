Xml
```xml
<?xml version="1.0" encoding="utf-8"?>
<ParsifyModule xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Name>TestTextFormat</Name>
  <Version>1.6</Version>
  <Line StartsWith="HEAD">
    <Field Type="string" Name="NOTE" Position="5" Length="10" />
    <Field Type="string" Name="FLAG" Position="15" Length="2" />
  </Line>
  <Line StartsWith="POS">
    <Field Type="int" Name="QUANTITY" Position="4" Length="2">
      <Translate Value="^42$" Display="answer to everything" IgnoreCase="true" SearchMode="Regex" />
      <Translate Value="43" Display="missed the answer" IgnoreCase="false" SearchMode="StartsWith" />
    </Field>
  </Line>
</ParsifyModule>
```
