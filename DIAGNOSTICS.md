# OIC Diagnostics

The OIC compiler now includes a comprehensive diagnostic system for errors and warnings.

## Features

### Diagnostic Levels
- **Error** (red): Prevents compilation from completing
- **Warning** (yellow): Indicates potential issues but allows compilation
- **Info** (cyan): Informational messages

### Error Codes

#### Lexical Errors (L001-L099)
- `L001` - Unterminated string literal
- `L002` - Unterminated character literal
- `L003` - Invalid escape sequence (warning)
- `L004` - Invalid character
- `L005` - Unterminated block comment

#### Syntax Errors (S001-S099)
- `S001` - Unexpected token
- `S002` - Expected specific token
- `S003` - Missing semicolon
- `S004` - Missing closing brace
- `S005` - Missing closing parenthesis
- `S006` - Invalid declaration
- `S007` - Invalid expression
- `S008` - Invalid statement

#### Semantic Errors (C001-C099)
- `C001` - Undefined identifier
- `C002` - Type mismatch
- `C003` - Duplicate declaration
- `C004` - Invalid function call
- `C005` - Wrong argument count
- `C006` - Invalid return type
- `C007` - Unresolved extern

#### Warnings (W001-W099)
- `W001` - Unused variable
- `W002` - Implicit conversion
- `W003` - Missing return statement
- `W004` - Unreachable code
- `W005` - Empty statement
- `W006` - Extern function with body
- `W007` - Assumed int32 parameter types

## Examples

### Unterminated String Error
```c
int main() {
    printf("Hello World);  // Missing closing quote
    return 0;
}
```

Output:
```
test_error.c:2:12: error L001: Unterminated string literal
Compilation failed with 1 error(s).
```

### Parameter Type Warnings
```c
extern void MyFunc(int x);

int main() {
    MyFunc(42);  // Warns about assumed int32 types
    return 0;
}
```

Output:
```
warning W007: Assuming int32 parameter types for call to 'MyFunc'
Compilation completed with 1 warning(s).
```

### Colorized Output
By default, diagnostics are printed with ANSI color codes:
- Errors in **red**
- Warnings in **yellow**
- Info in **cyan**

Use `--quiet` flag to disable colorization.

## Exit Codes
- `0` - Success (no errors, warnings are ok)
- `1` - Compilation failed with errors

## API Usage

```csharp
using OIC;

var diagnostics = new DiagnosticBag("myfile.c");
var compiler = new CLanguageCompiler(diagnostics);

var module = compiler.CompileSource(sourceCode);

// Check for errors
if (diagnostics.HasErrors)
{
    diagnostics.PrintAll();
    Environment.Exit(1);
}

// Show warnings
Console.WriteLine($"Compiled with {diagnostics.WarningCount} warnings");
```

## Source Location Tracking
All diagnostics include:
- File path
- Line number
- Column number
- Length (for IDE integration)

Format: `<file>:<line>:<column>: <severity> <code>: <message>`
