using OthelloView;
using OthelloModel;

namespace Othello
{
    class Program
    {
        static void Main(string[] args)
        {
            Controller game = new Controller();

            game.runGame();
        }
    }

    class Controller
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

        private string quitMessage = "Goodbye";

        private View terminal;
        private HAL9000 hal;

        public void runGame()
        {
            terminal = new View(this, "q");

            terminal.writeBock(greeting);

            int difficulty = terminal.getInt(
                "What level of difficulty would you like? (lower numbers are easier)"
            );
            terminal.writeBock("jk this game will destroy you anyway\n");

            hal = new HAL9000();

            bool stillPlaying = true;
            do
            {
                terminal.writeBock(render(hal.currentState()));

                NumPair coordinates = terminal.getPair("Where would you like to move?");
                while (!hal.makeMove(coordinates[0], coordinates[1]))
                {
                    terminal.writeBock("That is not a valid move. Please try again.");
                    coordinates = terminal.getPair("Where would you like to move?");
                }
            } while (stillPlaying);

            terminal.writeBock("Goodbye");
        }

        private static string render(GameState game)
        {
            string view = "";

            for (int x = 0; x < game.getXLength(); x++)
            {
                for (int y = 0; y < game.getYLength(); y++)
                {
                    switch (game[x, y])
                    {
                        case Player.HUMAN:
                            view += "X";
                            break;
                        case Player.COMPUTER:
                            view += "O";
                            break;
                        case Player.UNOCCUPIED:
                            view += ".";
                            break;
                    }
                }
                view += "\n";
            }

            return view;
        }

        public void shutDown()
        {
            terminal.writeBock(quitMessage);
            //shut down the game threads here as well
            System.Environment.Exit(0);
        }
    }
}
