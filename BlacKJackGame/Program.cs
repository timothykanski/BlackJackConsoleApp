using BlackJack.Services;

var playAgain = "";

while (playAgain != "n")
{
    var a = BlackJackService.PlayGame(14);
    Console.WriteLine(a);

    Console.WriteLine("Play Again? (Y/N)");

    playAgain = Console.ReadLine();
}