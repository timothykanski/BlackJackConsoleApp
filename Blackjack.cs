
//C# BlackJack Console App

using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
	// TODO:  Betting.  Including: Insurance, splitting, and double down.
	public static void Main()
	{
		var a = PlayGame(14);
		Console.WriteLine(a);
	}
	
	public class ResultSet{
		public ResultSet (byte playerStandsOn, byte dealerShowingCard, (byte,byte) playerHand, bool playerWon, bool didDealerBlackJackOrPush = false){
		    PlayerStandsOn = playerStandsOn;
			DealerShowingCard = dealerShowingCard;
			PlayerHand = playerHand;
			PlayerWon = playerWon;
			DidDealerBlackJackOrPush = didDealerBlackJackOrPush;
		}
        public byte PlayerStandsOn {get;set;} // 4-21
		public byte DealerShowingCard {get;set;} // 1-13
		public (byte,byte) PlayerHand {get;set;} // 1-13, 1-13
		public bool PlayerWon {get;set;}
		public bool DidDealerBlackJackOrPush {get;set;}
		
		public override string ToString() => $"{PlayerStandsOn}|{DealerShowingCard}|{PlayerHand}|{PlayerWon}|{DidDealerBlackJackOrPush}";
		
	}
	
	
	public static ResultSet PlayGame(byte playerStandsOn)
	{
		// It's the Generator!
		var gen = new Generator();
		
		// Get some cards together.
		var shoe = gen.GetShoe(5);
		
		//var deck = gen.GetDeck();
		//deck.Cards.ForEach(x=>Console.WriteLine(x));

		
		// Game
		Hand playerHand = shoe.TakeHand();
		Hand dealerHand = shoe.TakeHand();
		
		Card dealersShowingCard = dealerHand.Cards.First();
		
		Console.WriteLine($"Player has {playerHand}");
		Console.WriteLine($"Dealer shows {dealersShowingCard}");
		
		if(dealersShowingCard.CardType == CardType.Ace){
		    //TODO offer insurance
		}
		
		ResultSet ret(bool playerWon, bool dealerBjOrPush){
		    return new ResultSet(playerStandsOn, (byte)(dealersShowingCard.CardType + 1), ((byte)(playerHand.Cards[0].CardType + 1), (byte)(playerHand.Cards[1].CardType + 1)), playerWon, dealerBjOrPush);
		}
		
		if(dealersShowingCard.CardType == CardType.Ace || (byte)dealersShowingCard.CardType > 8){
			// Dealer checks for BlackJack
			Console.WriteLine("Dealer Checks for BlackJack...");
			
			if(dealerHand.GetValue() == 21){
			    Console.WriteLine("Dealer has BlackJack...");
				DealerWins(21);
				return ret(false, true);
			} else {
			  Console.WriteLine("Dealer Does not have BlackJack...");
			}
		}
		// Player Plays
		Console.WriteLine($"Player has a {playerHand.GetValue()}.  Hit?");
		while(playerHand.GetValue() < playerStandsOn){
			var c = shoe.TakeCard();
		   	playerHand.Cards.Add(c);
			Console.WriteLine($"Player hits...  It's a {c} | {playerHand.GetValue()}");
		}
        
		// Check for player bust
		if(playerHand.GetValue() > 21){
			Console.WriteLine("Player busts!");
		    DealerWins(dealerHand.GetValue()); // Todo - draw cards for dealer - don't show this value as final!
			return ret(false, false);
		} else {
			Console.WriteLine($"Player stands with a {playerHand.GetValue()}");
		}
		
		Console.WriteLine($"Dealer reveals his hole card.  Dealer has {dealerHand}");
		
		// Dealer Plays
		while(dealerHand.GetValue() < 17){
			var c = shoe.TakeCard();
			dealerHand.Cards.Add(c);
			Console.WriteLine($"Dealer hits...  It's a {c} | {dealerHand.GetValue()}");
		}
        
		// Check for dealer bust
		if(dealerHand.GetValue() > 21){
			Console.WriteLine("Dealer busts!");
		    PlayerWins(playerHand.GetValue());
			return ret(true, false);
		} else {
			Console.WriteLine($"Dealer stands with a {dealerHand.GetValue()}");			
		}
		
		if(playerHand.GetValue() > dealerHand.GetValue()){
		    PlayerWins(playerHand.GetValue());
			return ret(true, false);
		} else {
			// Check for Push
			if(playerHand.GetValue() == dealerHand.GetValue()){
				Push(playerHand.GetValue());
				return ret(true, true);
			}
		    DealerWins(dealerHand.GetValue());
			return ret(false,false);
		}
	}
	public static void DealerWins(byte val){
	    Console.WriteLine($"Dealer wins with a {val}.");
	}
	public static void PlayerWins(byte val){
	    Console.WriteLine($"Player wins with a {val}.");
	}
	public static void Push(byte val){
	    Console.WriteLine($"Push! {val}");
	}
	
	
	public class Hand {
	    public List<Card> Cards {get;set;}
		
		public override string ToString(){
			var str = "";
		    Cards.ForEach(x => str += x + " | ");
			str += GetValue();
			return str;
		}
		
		public byte GetValue(){
			byte val = 0;
			byte numAces = 0;
			foreach(var card in Cards){
			   if(card.CardType == CardType.Ace){
			      numAces++;
			   } else {
			      val += Math.Min((byte)10, (byte)(card.CardType + 1));
			   }
			}
			for(var i = 0; i < numAces; i++){
				val += (byte)((i == numAces - 1 && val <= 10) ? 11 : 1);
			}
			return val;
		}
	}
	
	public class Generator {
		
	    public Deck GetDeck() {
		    // Empty Deck
			var deck = new Deck{
			   Cards = new List<Card> {}
			};
			// Populate with Cards
			for(var i = 0; i < 13; i++){
				for(var j = 0; j < 4; j++){
				    deck.Cards.Add(new Card{
					    Suit = (Suit)j,
						CardType = (CardType)i
					});
				}
			}
			deck.Shuffle();
			return deck;
		}
		
		public Deck GetShoe(byte numberOfDecks){
		    // Empty Shoe
			var shoe = new Deck{
			   Cards = new List<Card> {}
			};
			for(var x = 0; x < numberOfDecks; x++){
				// Populate with Cards
				for(var i = 0; i < 13; i++){
					for(var j = 0; j < 4; j++){
						shoe.Cards.Add(new Card{
							Suit = (Suit)j,
							CardType = (CardType)i
						});
					}
				}
			}
			shoe.Shuffle();
			return shoe;
		}
	}
	
	public class Deck {
	    public List<Card> Cards {get;set;}
		
		public void Shuffle(){
		    Cards = Cards.OrderBy(a => Guid.NewGuid()).ToList();
		}
		
		public Card TakeCard(){
		    var card = Cards.First();
			Cards.RemoveAt(0);
			return card;
		}
		
		public Hand TakeHand(){
			return new Hand{ Cards = new List<Card>{ TakeCard(), TakeCard()}};
		}
	}
	
	public class Card {
	    public Suit Suit {get;set;}
		public CardType CardType {get;set;}
		public override string ToString() => $"{CardType} of {Suit}";
	}
	
	public enum Suit : byte {
	    Hearts, Spades, Clubs, Diamonds
	}
	
	public enum CardType : byte {
	    Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King
	}
}
