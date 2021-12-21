using BlackJack.Service.Enums;

namespace BlackJack.Service.Objects
{
    public class Hand
    {
        public List<Card> Cards { get; set; } = new List<Card>();

        public override string ToString()
        {
            var str = "";
            Cards.ForEach(x => str += x + " | ");
            str += GetValue();
            return str;
        }

        public byte GetValue()
        {
            byte val = 0;
            byte numAces = 0;
            foreach (var card in Cards)
            {
                if (card.CardType == CardType.Ace)
                {
                    numAces++;
                }
                else
                {
                    val += Math.Min((byte)10, (byte)(card.CardType + 1));
                }
            }
            for (var i = 0; i < numAces; i++)
            {
                val += (byte)((i == numAces - 1 && val <= 10) ? 11 : 1);
            }
            return val;
        }
    }
}