namespace BlackJack.Service.Objects
{
    public class ResultSet
    {
        public ResultSet(byte playerStandsOn, byte dealerShowingCard, (byte, byte) playerHand, bool playerWon, bool didDealerBlackJackOrPush = false)
        {
            PlayerStandsOn = playerStandsOn;
            DealerShowingCard = dealerShowingCard;
            PlayerHand = playerHand;
            PlayerWon = playerWon;
            DidDealerBlackJackOrPush = didDealerBlackJackOrPush;
        }
        public byte PlayerStandsOn { get; set; } // 4-21
        public byte DealerShowingCard { get; set; } // 1-13
        public (byte, byte) PlayerHand { get; set; } // 1-13, 1-13
        public bool PlayerWon { get; set; }
        public bool DidDealerBlackJackOrPush { get; set; }

        public override string ToString() => $"{PlayerStandsOn}|{DealerShowingCard}|{PlayerHand}|{PlayerWon}|{DidDealerBlackJackOrPush}";

    }
}