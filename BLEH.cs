//File to test Lexer

public class BLEH
{
    public static bool go()
    {
        string line = Console.ReadLine();

        if(line == null || line.Length == 0)
        {
            return true;
        }

        Lexer lexer = new Lexer(line);

        Token tok = lexer.getToken();

        while(tok.type != TOKEN_TYPES.EOF)
        {
            Console.WriteLine(tok.type);
            lexer.readChar();
            tok = lexer.getToken();
        }

        return false;
    }
}
