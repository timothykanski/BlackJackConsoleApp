using BlackJack.Service.Objects;
using BlackJack.Service.Enums;

namespace BlackJack.Services
{
    public class BlackJackService
    {

        public static ResultSet PlayGame(byte playerStandsOn)
        {
            // Get some cards together.
            var shoe = GetShoe(5);

            //var deck = gen.GetDeck();
            //deck.Cards.ForEach(x=>Console.WriteLine(x));


            // Game
            Hand playerHand = shoe.TakeHand();
            Hand dealerHand = shoe.TakeHand();  

            Card dealerShowingCard = dealerHand.Cards.First();
            Card dealerHoleCard = dealerHand.Cards[1];

            Console.WriteLine($"Player has {playerHand}");
            Console.WriteLine($"Dealer shows {dealerShowingCard}");

            if (dealerShowingCard.CardType == CardType.Ace)
            {
                //TODO offer insurance
            }

            // TODO Rewrite or explain..
            ResultSet ret(bool playerWon, bool dealerBjOrPush)
            {
                return new ResultSet(playerStandsOn, (byte)(dealerShowingCard.CardType + 1), ((byte)(playerHand.Cards[0].CardType + 1), (byte)(playerHand.Cards[1].CardType + 1)), playerWon, dealerBjOrPush);
            }

            if (dealerShowingCard.CardType == CardType.Ace || dealerShowingCard.CardType > CardType.Nine)
            {
                // Dealer checks for BlackJack
                Console.WriteLine("Dealer Checks for BlackJack...");

                if (dealerHand.GetValue() == 21)
                {
                    Console.WriteLine("Dealer has BlackJack...");
                    DealerWins(21);
                    return ret(false, true);
                }
                else
                {
                    Console.WriteLine("Dealer Does not have BlackJack...");
                }
            }
            // Player Plays
            Console.WriteLine($"Player has a {playerHand.GetValue()}.  Hit?");
            while (playerHand.GetValue() < playerStandsOn)
            {
                var c = shoe.TakeCard();
                playerHand.Cards.Add(c);
                Console.WriteLine($"Player hits...  It's a {c} | {playerHand.GetValue()}");
            }

            // Check for player bust
            if (playerHand.GetValue() > 21)
            {
                Console.WriteLine("Player busts!");
                DealerWins(dealerHand.GetValue()); // Todo - draw cards for dealer - don't show this value as final!
                return ret(false, false);
            }
            else
            {
                Console.WriteLine($"Player stands with a {playerHand.GetValue()}");
            }

            Console.WriteLine($"Dealer reveals a {dealerHoleCard}.  Dealer has {dealerHand}");

            // Dealer Plays
            while (dealerHand.GetValue() < 17)
            {
                var c = shoe.TakeCard();
                dealerHand.Cards.Add(c);
                Console.WriteLine($"Dealer hits...  It's a {c} | {dealerHand.GetValue()}");
            }

            // Check for dealer bust
            if (dealerHand.GetValue() > 21)
            {
                Console.WriteLine("Dealer busts!");
                PlayerWins(playerHand.GetValue());
                return ret(true, false);
            }
            else
            {
                Console.WriteLine($"Dealer stands with a {dealerHand.GetValue()}");
            }

            if (playerHand.GetValue() > dealerHand.GetValue())
            {
                PlayerWins(playerHand.GetValue());
                return ret(true, false);
            }
            else
            {
                // Check for Push
                if (playerHand.GetValue() == dealerHand.GetValue())
                {
                    Push(playerHand.GetValue());
                    return ret(true, true);
                }
                DealerWins(dealerHand.GetValue());
                return ret(false, false);
            }
        }

        public static void DealerWins(byte val)
        {
            Console.WriteLine($"Dealer wins with a {val}.");
        }
        public static void PlayerWins(byte val)
        {
            Console.WriteLine($"Player wins with a {val}.");
        }
        public static void Push(byte val)
        {
            Console.WriteLine($"Push! {val}");
        }

        public static Deck GetDeck()
        {
            // Empty Deck
            var deck = new Deck
            {
                Cards = new List<Card> { }
            };
            // Populate with Cards
            for (var i = 0; i < 13; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    deck.Cards.Add(new Card
                    {
                        Suit = (SuitType)j,
                        CardType = (CardType)i
                    });
                }
            }
            deck.Shuffle();
            return deck;
        }
        public static Deck GetShoe(byte numberOfDecks)
        {
            // Empty Shoe
            var shoe = new Deck
            {
                Cards = new List<Card> { }
            };
            for (var x = 0; x < numberOfDecks; x++)
            {
                // Populate with Cards
                for (var i = 0; i < 13; i++)
                {
                    for (var j = 0; j < 4; j++)
                    {
                        shoe.Cards.Add(new Card
                        {
                            Suit = (SuitType)j,
                            CardType = (CardType)i
                        });
                    }
                }
            }
            shoe.Shuffle();
            return shoe;
        }
    }
}