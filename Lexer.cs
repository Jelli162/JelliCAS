
public class Lexer
{
    private string input;
    private int position;
    private int readPosition;
    private char currentChar;

    public Lexer(string input)
    {
        this.input = input;
        this.position = 0;
        this.readPosition = 0;
        this.currentChar = '\0';

        readChar();
    }

    public void readChar()
    {
        if(this.readPosition >= this.input.Length)
        {
            this.currentChar = '\0';
            return;
        }

        else
        {
            this.currentChar = this.input[this.readPosition];
        }

        this.position = this.readPosition;
        this.readPosition += 1;
    }

    private char peekChar()
    {
        if(this.readPosition >= this.input.Length)
        {
            return '\0';
        }

        else
        {
            return this.input[this.readPosition];
        }
    }

    public Token getToken()
    {
        Token tok;

        switch(this.currentChar)
        {
            case ' ':
                tok = new Token(TOKEN_TYPES.WHITESPACE, this.currentChar);
                break;

            case '(':
                tok = new Token(TOKEN_TYPES.LPAREN, this.currentChar);
                break;

            case ')':
                tok = new Token(TOKEN_TYPES.RPAREN, this.currentChar);
                break;

            case '\0':
                tok = new Token(TOKEN_TYPES.EOF, this.currentChar);
                break;

            default:
                if(char.IsLetter(this.currentChar))
                {
                    tok = new Token(TOKEN_TYPES.ALPHA, readID(false));
                }

                else if(char.IsDigit(this.currentChar))
                {
                    tok = new Token(TOKEN_TYPES.NUMBER, readID(true));
                }
                
                else if(Operators.getInstance().isOperator(this.currentChar))
                {
                    tok = new Token(TOKEN_TYPES.OP, this.currentChar);
                }

                else
                {
                    tok = new Token(TOKEN_TYPES.ILLEGAL, this.currentChar);
                }

                break;
        }

        return tok;
    }

    private string readID(bool isNumber)
    {
        int start = this.position;

        if(isNumber)
        {
            char next = peekChar();
            while(char.IsDigit(next) || next == '.')
            {
                readChar();
                next = peekChar();
            }

            return this.input.Substring(start, this.position - start);
        }

        while(char.IsLetter(peekChar()))
        {
            readChar();
        }

        return this.input.Substring(start, this.position - start);
    }

}

public class Token
{
    public TOKEN_TYPES type { get; }
    public string literal { get; }

    public Token(TOKEN_TYPES type, string literal)
    {
        this.type = type;
        this.literal = literal;
    }

    public Token(TOKEN_TYPES type, char literal)
    {
        this.type = type;
        this.literal = literal.ToString();
    }
}


public enum TOKEN_TYPES
{
    EOF,
    ILLEGAL,
    WHITESPACE,
    NUMBER,
    ALPHA,
    LPAREN,
    RPAREN,
    OP
}