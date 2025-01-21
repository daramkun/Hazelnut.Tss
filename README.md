## Hazelnut.Tss
This is simple ANSI Escape code convert/generate library.

### Usage
#### ANSI Escape code to HTML
```csharp
var converter = new AnsiCodeConverter();
var html = converter.Convert("\e[31mHello, world!\e[0m");
Debug.Assert(html.Equals("<span style=\"color: #800000\">Hello, world!</span>"));
```

#### Escape ANSI Escape code (to Plain Text)
```csharp
var converter = new AnsiCodeConverter(PlainTextStringifier.SharedInstance);
var text = converter.Convert("\e[31mHello, world!\e[0m");
Debug.Assert(text.Equals("Hello, world!"));
```

#### Make ANSI Escape coded format text
```csharp
using var generator = new AnsiCodeGenerator();
var generated = generator
    .SetBold()
    .SetItalic()
    .Append("Hello, world!")
    .Reset()
    .ToString();
Debug.Assert(generated.Equals("\e[1m\e[3mHello, world!\e[0m"));
```