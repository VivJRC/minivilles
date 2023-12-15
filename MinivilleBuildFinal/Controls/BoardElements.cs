using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MinivilleBuildFinal.Controls
{
    class BoardElements
    {
        Random rnd = new Random();

        public bool Updated = true;

        // This class uses the same State variable principle as Form1

        public string State = "WaitPlayerChoice";
        // "WaitPlayerChoice" : nothing
        // "PlayerSwapAnim" : Change the player's deck's position. Once done, set to "WaitDice" and set Form1State to "InitDice"
        // "AskDice" : Ask the player the number of dices they want to use. Set to "DiceWait"
        //       The button will set and Form1State to "ThrowDice"
        // "WaitDice" : nothing
        // "DiceInit" : Initialise Dice elements. Set to "DiceAnim"
        // "DiceAnim" : Animate the dice. Once dice is settled, set to "GainsWait". Set Form1 to "GainsInit"
        // "GainsWait" : Nothing - During this state, BoardElements will be called multiple time by Form1 through other methods.

        // "ShopAnim" : Bring up shop. Once done, set to "Shop"
        // "Shop" : nothing     in this state we're waiting for the player to click a button. Once the button is clicked, it will call a function in GameManager to add the card to the deck or not, and it will change State to "ShopClean"
        // "ShopClean" : Hide shop. Once done, set to "PlayerSwapAnim"

        // "Form1NewTurn" : inform Form1 to set it's state correctly

        //public List<Card>[] PlayerCards;
        public bool[] ActivePlayerMonument;

        public bool isPlayerDeckSwitched;

        public PlayerChoiceButton[] PlayerChoices;
        public DiceForm[] Dices;
        public PlayerDeckForm[] PlayerDecks;
        public ShopForm Shop;
        public GainForm[] Gains;

        public InfoPanel InfoPanelForm;

        Image[] diceImages;
        public bool anotherTurn = false;

        bool endOfGame;

        // here we setup the board and all it's elements
        public BoardElements()
        {
            PlayerChoices = new PlayerChoiceButton[4];
            for (int i = 0; i < PlayerChoices.Length; i++)
            {
                PlayerChoices[i] = new PlayerChoiceButton(i);
                PlayerChoices[i].sprite.pos = new Point(144 + (i * 192), 336);
            }
            diceImages = new Image[6];
            for (int i = 0; i < diceImages.Length; i++)
            {
                diceImages[i] = Image.FromFile(String.Format("Sprites/Dice{0}.png", i + 1));
            }
            Gains = new GainForm[4];
            for (int i = 0; i < Gains.Length; i++)
            {
                Gains[i] = new GainForm();
            }
            Shop = new ShopForm();
        }

        // What happens in this method depends on "State"
        // This method is called by Form1 to display all the board elements. It then returns a list of all sprites to render
        public List<Sprite> UpdateBoard()
        {
            List<Sprite> BOARDELEMENTS = new List<Sprite>();

            // If the users have yet to chose the number of players, this code will add the choice buttons to the list of sprites to display
            if (State == "WaitPlayerChoice")
            {
                for (int i = 0; i < PlayerChoices.Length; i++)
                {
                    BOARDELEMENTS.Add(PlayerChoices[i].sprite);
                }
            }
            // Else, this code is called to render the player's cards first...
            else
            {
                foreach (PlayerDeckForm pd in PlayerDecks)
                {
                    pd.UpdatePlayerDeckForm();
                    if (endOfGame)
                    {
                        for(int i = 0; i < pd.PlayerCards.Count; i++)
                        {
                            switch (pd.IntendedRota)
                            {
                                case (0):
                                    pd.cardIntendedPos[i] = new Point(pd.cardIntendedPos[i].X, 912);
                                    break;
                                case (1):
                                    pd.cardIntendedPos[i] = new Point(1008, pd.cardIntendedPos[i].Y);
                                    break;
                                case (2):
                                    pd.cardIntendedPos[i] = new Point(pd.cardIntendedPos[i].X, -96);
                                    break;
                                case (3):
                                    pd.cardIntendedPos[i] = new Point(-96, pd.cardIntendedPos[i].Y);
                                    break;
                            }
                        }
                    }
                    List<Sprite> ONEPLAYER = pd.moveCards();
                    if (!pd.testCardPos()) { Updated = true; } else { }
                    foreach (Sprite s in ONEPLAYER)
                    {
                        BOARDELEMENTS.Add(s);
                    }
                }
            }
            if (State == "showPlayers")
            {
                State = "none";
            }
            // ...then to process the dices...
            if (State == "DiceInit")
            {
                Dices[0].InitAnim(new Point(rnd.Next(-120, -96),rnd.Next(150, 200)), rnd.Next(27, 35));
                if(Dices.Length == 2)
                {
                    Dices[1].InitAnim(new Point(rnd.Next(-120, -96), rnd.Next(300, 350)), rnd.Next(27, 35));
                }
                State = "DiceAnim";
                Updated = true;
            }
            // ...to diplay them...
            if (State == "DiceAnim" || State == "GainsWait")
            {
                bool isDone = true;
                foreach (DiceForm d in Dices)
                {
                    BOARDELEMENTS.Add(d.dice);
                    if (!d.Anim())
                    {
                        isDone = false;
                    }
                }
                if (isDone)
                {
                    State = "GainsWait";
                }
            }
            // ...to process the shop being raised...
            if (State == "ShopAnim")
            {
                if (Shop.animTimer < 126)
                {
                    Shop.RaiseShopAnim();
                    Updated = true;
                }
                else
                {
                    State = "Shop";
                }
            }
            // ... and lowered...
            if (State == "ShopClean")
            {
                if (Shop.animTimer < 252)
                {
                    Shop.CleanShopAnim();
                    Updated = true;
                }
                else
                {
                    Updated = true;
                    isPlayerDeckSwitched = false;
                    if (anotherTurn)
                    {
                        State = "setupNewTurn";
                    }
                    else
                    {
                        foreach (PlayerDeckForm pd in PlayerDecks)
                        {
                            pd.IntendedRota--;
                            if (pd.IntendedRota == -1) { pd.IntendedRota = PlayerDecks.Length - 1; }
                            foreach (CardForm c in pd.PlayerCards)
                            {
                                c.sprite.sprite.RotateFlip(RotateFlipType.Rotate90FlipNone); 
                                if (PlayerDecks.Length == 3 && pd.IntendedRota == 2) { c.sprite.sprite.RotateFlip(RotateFlipType.Rotate90FlipNone); }
                                if (PlayerDecks.Length == 2 && pd.IntendedRota == 1) { c.sprite.sprite.RotateFlip(RotateFlipType.Rotate180FlipNone); }
                            }
                        }
                        State = "PlayerSwapAnim";
                    }

                }
            }

            // ..and the player's position being swapped around.
            if (State == "PlayerSwapAnim")
            {
                bool test = true;
                foreach (PlayerDeckForm pd in PlayerDecks)
                {
                    if (!pd.testCardPos()) { test = false; }
                }
                if (test)
                {
                    State = "Form1NewTurn";
                }
            }
            return BOARDELEMENTS;
        }

        // This method updates the player's board separatly for the sake of clarity
        public List<Sprite> UpdatePlayerBoard()
        {
            List<Sprite> PLAYERCARDS = new List<Sprite>();
            foreach (PlayerDeckForm pd in PlayerDecks)
            {
                pd.UpdatePlayerDeckForm();
                List<Sprite> CURRENTCARDS = pd.moveCards();
                foreach (Sprite s in CURRENTCARDS)
                {
                    PLAYERCARDS.Add(s);
                }
            }
            return PLAYERCARDS;
        }

        public void CleanCard()
        {
            endOfGame = true;
        }
    }
}
