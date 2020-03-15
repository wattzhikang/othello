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

            if (options.getSelection() == (int)Choices.FIRST)
            {
                terminal.writeBock("Success");
            }
            else
            {
                terminal.writeBock("Failure");
            }
        }
    }
}
