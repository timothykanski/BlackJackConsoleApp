using BlackJack.Service.Enums;

namespace BlackJack.Service.Objects
{
    public class Card
    {
        public SuitType Suit { get; set; }
        public CardType CardType { get; set; }
        public override string ToString() => $"{CardType} of {Suit}";
    }
}