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

        static void Main(string[] args)
        {
            View terminal = new View();

            MultipleChoice options = new MultipleChoice();
            options.addOption("First", (int)Choices.FIRST);
            options.addOption("Second", (int)Choices.SECOND);
            options.addOption("Third", (int)Choices.THIRD);

            terminal.multipleChoice(options, "Please which order you want to go in?");

            switch ((Choices) options.getSelection())
            {
                case Choices.FIRST:
                    terminal.writeBock("You will go first.");
                    break;
                case Choices.SECOND:
                    terminal.writeBock("You will go second.");
                    break;
                case Choices.THIRD:
                    terminal.writeBock("You will go first.");
                    break;
                default:
                    terminal.writeBock("There was a problem.");
                    break;
            }
        }
    }
}
