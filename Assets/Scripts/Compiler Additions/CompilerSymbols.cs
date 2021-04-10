public class CompilerSymbols
{

    //public const char outputFunctionArgumentSeperator = ':';
    //public const char outputFunctionSeperator = '|';

    public const string commentMarker = "//";
    public const string usingKeyword = "using";

    public static readonly string[] types =
    {
        "Integer",
        "Float",
        "Double",
        "String",
        "Character",
        "int",
        "float",
        "double",
        "string",
        "char",
        "public",
        "private",
        "protected",
        "const",
        "static",
        "readonly"
    };

    public static readonly string[] builtinFunctions = {
        "sqrt",
        "abs",
        "sign",
        "sin",
        "cos",
        "tan",
        "asin",
        "acos",
        "atan"
    };

    public static readonly string[] reservedNames = {
        "if",
        "for",
        "else",
        "while",
        "return"
    };

    public static readonly string[] compoundOperators = {
        "+=",
        "-=",
        "*=",
        "/=",
        "++",
        "--",
        "<=",
        ">=",
        "==",
        "elseif",
        commentMarker
    };

    const string mathOperatorsString = "+-*/%()";
    const string comparisonOperatorsString = "=<>";
    const string assignmentOperator = "=";
    const string logicOperators = "&|!";
    const string valueMarker = "#";

    public const string brackets = "(){}[]";

    public const string allMathOperators = mathOperatorsString + comparisonOperatorsString + assignmentOperator;

    public const string allOperators = mathOperatorsString + comparisonOperatorsString + assignmentOperator 
        + logicOperators + valueMarker;


    public static bool ArrayContainsString(string[] array, string s)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == s)
            {
                return true;
            }
        }
        return false;
    }
}