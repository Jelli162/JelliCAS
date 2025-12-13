public class Token
{
    private TokenType type {get;}
    private string value {get;}

    public Token(TokenType type, string value)
    {
        this.type = type;
        this.value = value;
    }
}

public enum TokenType
{
    NUM,
    VAR,
    OP,
    FUNC,
    PAR_O,
    PAR_C,
    EOF
}