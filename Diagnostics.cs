namespace OIC;

/// <summary>
/// Diagnostic severity levels
/// </summary>
public enum DiagnosticSeverity
{
    Info,
    Warning,
    Error
}

/// <summary>
/// Source location for diagnostics
/// </summary>
public record SourceLocation(string FilePath, int Line, int Column, int Length = 0)
{
    public override string ToString() => $"{FilePath}:{Line}:{Column}";
}

/// <summary>
/// Diagnostic message (error, warning, or info)
/// </summary>
public record Diagnostic(
    DiagnosticSeverity Severity,
    string Code,
    string Message,
    SourceLocation? Location = null)
{
    public override string ToString()
    {
        var prefix = Severity switch
        {
            DiagnosticSeverity.Error => "error",
            DiagnosticSeverity.Warning => "warning",
            DiagnosticSeverity.Info => "info",
            _ => "diagnostic"
        };

        if (Location != null)
            return $"{Location}: {prefix} {Code}: {Message}";
        return $"{prefix} {Code}: {Message}";
    }

    public string ToColorizedString()
    {
        var severityColor = Severity switch
        {
            DiagnosticSeverity.Error => "\x1b[1;31m",    // Bold red
            DiagnosticSeverity.Warning => "\x1b[1;33m",  // Bold yellow
            DiagnosticSeverity.Info => "\x1b[1;36m",     // Bold cyan
            _ => ""
        };
        var reset = "\x1b[0m";
        var locationStr = Location != null ? $"{Location}: " : "";
        
        var severityText = Severity switch
        {
            DiagnosticSeverity.Error => "error",
            DiagnosticSeverity.Warning => "warning",
            DiagnosticSeverity.Info => "info",
            _ => "diagnostic"
        };

        return $"{locationStr}{severityColor}{severityText}{reset} {Code}: {Message}";
    }
}

/// <summary>
/// Collects diagnostics during compilation
/// </summary>
public class DiagnosticBag
{
    private readonly List<Diagnostic> _diagnostics = new();
    private readonly string _filePath;

    public DiagnosticBag(string filePath = "<input>")
    {
        _filePath = filePath;
    }

    public IReadOnlyList<Diagnostic> Diagnostics => _diagnostics;
    public int ErrorCount => _diagnostics.Count(d => d.Severity == DiagnosticSeverity.Error);
    public int WarningCount => _diagnostics.Count(d => d.Severity == DiagnosticSeverity.Warning);
    public bool HasErrors => ErrorCount > 0;

    public void AddError(string code, string message, int line = 0, int column = 0, int length = 0)
    {
        var location = line > 0 ? new SourceLocation(_filePath, line, column, length) : null;
        _diagnostics.Add(new Diagnostic(DiagnosticSeverity.Error, code, message, location));
    }

    public void AddWarning(string code, string message, int line = 0, int column = 0, int length = 0)
    {
        var location = line > 0 ? new SourceLocation(_filePath, line, column, length) : null;
        _diagnostics.Add(new Diagnostic(DiagnosticSeverity.Warning, code, message, location));
    }

    public void AddInfo(string code, string message, int line = 0, int column = 0, int length = 0)
    {
        var location = line > 0 ? new SourceLocation(_filePath, line, column, length) : null;
        _diagnostics.Add(new Diagnostic(DiagnosticSeverity.Info, code, message, location));
    }

    public void AddDiagnostic(Diagnostic diagnostic)
    {
        _diagnostics.Add(diagnostic);
    }

    public void Merge(DiagnosticBag other)
    {
        _diagnostics.AddRange(other._diagnostics);
    }

    public void PrintAll(bool useColor = true)
    {
        foreach (var diagnostic in _diagnostics)
        {
            var output = useColor ? diagnostic.ToColorizedString() : diagnostic.ToString();
            
            if (diagnostic.Severity == DiagnosticSeverity.Error)
                Console.Error.WriteLine(output);
            else
                Console.WriteLine(output);
        }
    }

    public void Clear()
    {
        _diagnostics.Clear();
    }
}

/// <summary>
/// Common diagnostic codes
/// </summary>
public static class DiagnosticCodes
{
    // Lexical errors (L001-L099)
    public const string UnterminatedString = "L001";
    public const string UnterminatedChar = "L002";
    public const string InvalidEscapeSequence = "L003";
    public const string InvalidCharacter = "L004";
    public const string UnterminatedComment = "L005";

    // Syntax errors (S001-S099)
    public const string UnexpectedToken = "S001";
    public const string ExpectedToken = "S002";
    public const string MissingSemicolon = "S003";
    public const string MissingClosingBrace = "S004";
    public const string MissingClosingParen = "S005";
    public const string InvalidDeclaration = "S006";
    public const string InvalidExpression = "S007";
    public const string InvalidStatement = "S008";

    // Semantic errors (C001-C099)
    public const string UndefinedIdentifier = "C001";
    public const string TypeMismatch = "C002";
    public const string DuplicateDeclaration = "C003";
    public const string InvalidFunctionCall = "C004";
    public const string WrongArgumentCount = "C005";
    public const string InvalidReturnType = "C006";
    public const string UnresolvedExtern = "C007";

    // Warnings (W001-W099)
    public const string UnusedVariable = "W001";
    public const string ImplicitConversion = "W002";
    public const string MissingReturn = "W003";
    public const string UnreachableCode = "W004";
    public const string EmptyStatement = "W005";
    public const string ExternFunctionWithBody = "W006";
    public const string AssumedInt32 = "W007";
}
