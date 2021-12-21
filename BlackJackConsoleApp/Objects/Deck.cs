namespace BlackJack.Service.Objects
{
    public class Deck
    {
        public List<Card> Cards { get; set; } = new List<Card>();

        public void Shuffle()
        {
            Cards = Cards.OrderBy(a => Guid.NewGuid()).ToList();
        }

        public Card TakeCard()
        {
            var card = Cards.First();
            Cards.RemoveAt(0);
            return card;
        }

        public Hand TakeHand()
        {
            return new Hand { Cards = new List<Card> { TakeCard(), TakeCard() } };
        }
    }
}