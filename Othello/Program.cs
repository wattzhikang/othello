using OthelloView;

namespace Othello
{
    class Program
    {
        static void Main(string[] args)
        {
            View output = new View();

            output.sayHello();

            NumPair pair = output.getPair("Please enter two numbers.");

            output.writeBock("The two numbers you have entered are " + pair[0] + " and " + pair[1] + ".");
        }
    }
}
