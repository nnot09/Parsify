readme is in progress

# Parsify - Notepad++ Plugin for Text-Based Document Visualization

Parsify is a powerful plugin for Notepad++ that enables users to visualize electronic documents in a text-based format, such as EDIFACT or GDV. With Parsify, users can create custom format definitions using XML files, allowing them to precisely interpret and display various parts of the document.

## Features

- **Custom Format Definitions**: Define custom format definitions using XML files to parse text-based documents accurately.
- **Flexible Parsing**: Specify the structure of each line and the fields within them, including optional fields and translations for specific values.
- **Visualization**: Visualize parsed documents as a tree of nodes, displaying all lines and fields along with their corresponding values.
- **Much more** (WIP)

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
## Credits
- [BdR76](https://github.com/BdR76) and his [CSVLint](https://github.com/BdR76/CSVLint) project to understand highlighting/lexing in Notepad++.
- [Chri-s](https://github.com/Chri-s) with his helpful PR's fixing various things and having great ideas to enhance the usability of this project.

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
