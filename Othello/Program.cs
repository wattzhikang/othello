using OthelloView;

namespace Othello
{
    class Program
    {
        enum Choices
        {
            FIRST,
            SECOND,
            THIRD
        }

        private static string greeting =
@"

  ___ ___         .__  .__                        
 /   |   \   ____ |  | |  |   ____                
/    ~    \_/ __ \|  | |  |  /  _ \               
\    Y    /\  ___/|  |_|  |_(  <_> )              
 \___|_  /  \___  >____/____/\____/               
       \/       \/                                
  ________                                  .__   
 /  _____/  ____   ____   ________________  |  |  
/   \  ____/ __ \ /    \_/ __ \_  __ \__  \ |  |  
\    \_\  \  ___/|   |  \  ___/|  | \// __ \|  |__
 \______  /\___  >___|  /\___  >__|  (____  /____/
        \/     \/     \/     \/           \/      
 ____  __.                  ___.   .__            
|    |/ _|____   ____   ____\_ |__ |__|           
|      <_/ __ \ /    \ /  _ \| __ \|  |           
|    |  \  ___/|   |  (  <_> ) \_\ \  |           
|____|__ \___  >___|  /\____/|___  /__|           
        \/   \/     \/           \/               

";

        private static string dummyBoard =
@"
........
........
........
...XO...
...OX...
........
........
........
";

        static void Main(string[] args)
        {
            View terminal = new View();

            terminal.writeBock(greeting);

            int difficulty = terminal.getInt(
                "What level of difficulty would you like? (lower numbers are easier)"
            );
            terminal.writeBock("jk this game will destroy you anyway\n");

            for (int turn = 0; turn < 10; turn++)
            {
                terminal.writeBock(dummyBoard);

                NumPair coordinates = terminal.getPair("Where would you like to move?");
            }
        }
    }
}
