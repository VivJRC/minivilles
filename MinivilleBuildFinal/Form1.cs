using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MinivilleBuildFinal.Controls;
using System.Windows.Input;
using MinivilleBuildFinal.Enum;
using System.Media;

namespace MinivilleBuildFinal
{
    public partial class Form1 : Form
    {
        GameManager GM = new GameManager();

        public int GLOBALTEST;

        // ---------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------
        // ------------------------------------------ SOUND PLAYER -------------------------------------------
        // ---------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------

        string forHoverSound;

        int volumeGloal;
        int volumeMusic;
        int volumeSound;
        bool sfx = true;
        bool music = true;
        bool main = true;
        bool menu = false;

        // ---------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------
        // ------------------------------------------ SOUND PLAYER -------------------------------------------
        // ---------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------



            bool isclicking = false;

        //EffectManager _effectManager = new EffectManager();

        // The State variable is used to keep track of what state the program is in. There needs to be a constant loop running the program, so using a variable
        // to keep track of the state and checking it each loop with if statements is a simple and effective way to get the actions to be performed correctly

        public string State = "PlayerChoiceWait";
        // "PlayerChoiceWait" : nothing     When button is pressed, it sets State to "InitDice"      
        // "InitDice" : Check if player can throw 2 dices. If yes, set BoardElements to "AskDice" and set state to "WaitDiceChoice". Else, set State to "ThrowDice"
        // "ThrowDice" : Call GameManager to throw the dice. Then, save dice value and set BoardElements to "DiceInit". Set State to "DiceAnimWait"
        // "DiceAnimWait" : nothing
        // "GainsInit" : Call EventManager and it'll return a list of gains and losses. Set to "Gains"
        // "Gains" : Process the gains and losses list and call BoardElements, which will animate. After calling, switch to "GainsWait". If the list is process, switch to "ShopWait" and set BoardElement to "ShopAnim"
        // "GainsAnim" : nothing    while in this state, an animation is happening. BoardElements will switch state to "Gains" once done.
        // "ShopWait" : nothing

        int SizeX = 976;
        int SizeY = 903;

        int SizeMultiplier = 6;

        int nbPlayers = 2;
        //Player[] GM._players;

        int ActivePlayer = 0;

        HUD HUDManager; // Manages all HUD elements
        BoardElements BoardElementsManager; // Manages all elements on the board
        List<Sprite> SpritesToRenderHUD;
        List<Sprite> SpritesToRenderBoard;
        List<Sprite> AdditionnalButtons = new List<Sprite>();

        bool showinfo = false;
        InfoPanel CardInfoPanel;

        List<List<int>> gains;
        int countingforgains;
        int animtimer;
        int[][] nbActivatedCard;

        Sprite Backdrop;
        Image backdrop;

        Sprite[] DiceButton;

        Timer t;

        bool CanReroll = false;
        int CountingStadiumGain;
        bool tradeChooseOwnCard;
        CardForm tradeplayercard;
        int idOfTradedPlayerCard;
        CardForm tradeothercard;
        int idOfOtherTradedCard;

        EndAnimClass endanim;
        public Form1()
        {
            InitializeComponent();

            // ---------------------------------------------------------------------------------------------------
            // ---------------------------------------------------------------------------------------------------
            // ------------------------------------------ SOUND PLAYER -------------------------------------------
            // ---------------------------------------------------------------------------------------------------
            // ---------------------------------------------------------------------------------------------------

            // Here the sound player and multiple variables related to it are initiated

            volumeGloal = 100;
            volumeMusic = 100;
            volumeSound = 100;
            btn_masterVolume.Image = Image.FromFile(@"volumeOnImage.png");
            btn_sfxVolume.Image = Image.FromFile(@"sfxOnImage.png");
            btn_musicVolume.Image = Image.FromFile(@"musicOnImage.png");

            //btn_son.Image = Image.FromFile(@"son.png");
            panelMenu.Visible = false;

            MXP.settings.volume = volumeMusic;
            MXP.settings.playCount = 9999; //repeats
            MXP.Ctlcontrols.stop();
            MXP.Visible = false;

            MXP_SOUND.settings.volume = volumeSound;
            MXP_SOUND.settings.playCount = 1; //doesn't repeat
            MXP_SOUND.Ctlcontrols.stop();
            MXP_SOUND.Visible = false;

            AdjustVolume();

            MXP.settings.playCount = 9999;
            MXP.Ctlcontrols.stop();
            MXP.Visible = false;

            // ---------------------------------------------------------------------------------------------------
            // ---------------------------------------------------------------------------------------------------
            // ------------------------------------------ SOUND PLAYER -------------------------------------------
            // ---------------------------------------------------------------------------------------------------
            // ---------------------------------------------------------------------------------------------------

            StartPosition = FormStartPosition.Manual;
            Location = new Point(64, 64);
            Size = new Size(SizeX, SizeY);
            FormBorderStyle = FormBorderStyle.None;
            AutoScaleMode = AutoScaleMode.None;

            BoardElementsManager = new BoardElements();
            backdrop = Image.FromFile("Sprites/BackDrop.png");

            CardInfoPanel = new InfoPanel(this);

            nbActivatedCard = new int[GM._deck.Count][];

            for(int i = 0; i < nbActivatedCard.Length; i++)
            {
                nbActivatedCard[i] = new int[nbPlayers];
            }

            endanim = new EndAnimClass();

            t = new Timer();
            t.Interval = 1;
            t.Tick += new EventHandler(UpdateScreen);

            t.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MXP.URL = @"menuMusic.wav";
            MXP.Ctlcontrols.play();

            SpritesToRenderHUD = new List<Sprite>();
            SpritesToRenderBoard = new List<Sprite>();
            Backdrop = new Sprite(backdrop, new Point(0, 0), 0);
            DiceButton = new Sprite[2];
            DiceButton[0] = new Sprite(Image.FromFile("Sprites/Dice1.png"), new Point(336, 336), 0);
            DiceButton[1] = new Sprite(Image.FromFile("Sprites/Dice2ButtonNot.png"), new Point(528, 336), 0);
            Paint += PaintSprites;
            this.DoubleBuffered = true;
        }

        // This function is called constantly using a timer with a short delay. Therefor, everything in this function is made to be called repeatedly, and the usage of a
        // "State" variable to determine which actions should be performed at each point of the loop is necessary.

        public void UpdateScreen(object sender, EventArgs e)
        {
            GC.Collect();

            List<CardForm> allCard = new List<CardForm>();
            bool islookingcard = false;

            // This condition chekcs if the users have yet to decide the number of players. If not, we can show most sprites like the HUD and most board elements

            if (State != "PlayerChoiceWait")
            {
                // Here we add every cardw we need to render from the HUD, decks and shop to a single list of CardForm

                foreach (PlayerHUD ph in HUDManager.PlayerHUDs)
                {
                    foreach (CardForm c in ph.cardForms)
                    {
                        allCard.Add(c);
                    }
                }
                foreach (PlayerDeckForm pd in BoardElementsManager.PlayerDecks)
                {
                    foreach (CardForm c in pd.PlayerCards)
                    {
                        allCard.Add(c);
                    }
                }
                foreach (List<CardForm> ls in BoardElementsManager.Shop.ShopCardForms)
                {
                    foreach (CardForm c in ls)
                    {
                        allCard.Add(c);
                    }
                }
            }

            // For each one of these cards, we check if they have been clicked on. if they have, we need to show the card's info panel

            foreach (CardForm c in allCard)
            {
                int Xadd = 0;
                int Yadd = 0;
                if (c.isSmall)
                {
                    if (c.sprite.rota == 0 || c.sprite.rota == 2)
                    {
                        Xadd = 48;
                        Yadd = 72;
                    }
                    else
                    {
                        Xadd = 72;
                        Yadd = 48;
                    }
                }
                else
                {
                    Xadd = 144;
                    Yadd = 192;
                }
                if (!isclicking && !showinfo && ButtonCheck(c.sprite.pos, Xadd, Yadd, true))
                {
                    islookingcard = true;
                    isclicking = true;
                    CardInfoPanel.CardType = c.CardType;
                    CardInfoPanel.IsBuy = c.isShop;
                }

            }

            // These instructions are only executed if the shop is currently openned

            if (BoardElementsManager.State == "Shop")
            {
                // Here we check if the player tries to buy a monument

                for (int i = 0; i < 4; i++)
                {
                    if (ButtonCheck(BoardElementsManager.Shop.ShopMonumentForms[i].pos, 48, 72) & !GM._players[ActivePlayer].monuments[i])
                    {
                        PlayMonumentSound();
                        switch (i)
                        {
                            case (0):
                                if (GM._players[ActivePlayer]._coins >= 4)
                                {
                                    GM._players[ActivePlayer]._coins -= 4;
                                    GM._players[ActivePlayer].monuments[i] = true;
                                    BoardElementsManager.State = "ShopClean";
                                    BoardElementsManager.Updated = true;
                                }
                                break;
                            case (1):
                                if (GM._players[ActivePlayer]._coins >= 10)
                                {
                                    GM._players[ActivePlayer]._coins -= 10;
                                    GM._players[ActivePlayer].monuments[i] = true;
                                    GM.Monument("Centre commercial", GM._players[ActivePlayer]);
                                    BoardElementsManager.State = "ShopClean";
                                    BoardElementsManager.Updated = true;
                                }
                                break;
                            case (2):
                                if (GM._players[ActivePlayer]._coins >= 22)
                                {
                                    GM._players[ActivePlayer]._coins -= 22;
                                    GM._players[ActivePlayer].monuments[i] = true;
                                    BoardElementsManager.State = "ShopClean";
                                    BoardElementsManager.Updated = true;
                                }
                                break;
                            case (3):
                                if (GM._players[ActivePlayer]._coins >= 16)
                                {
                                    GM._players[ActivePlayer]._coins -= 16;
                                    GM._players[ActivePlayer].monuments[i] = true;
                                    BoardElementsManager.State = "ShopClean";
                                    BoardElementsManager.Updated = true;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            // We check if the player is trying to skip the shop phase without buying anything and set our variables correctly if he is

            if (BoardElementsManager.State == "Shop" & ButtonCheck(BoardElementsManager.Shop.SkipButton.pos, 240, 96))
            {
                BoardElementsManager.State = "ShopClean";
                BoardElementsManager.Updated = true;
            }

            // Here we check if the player has already clicked on a card from the shop and is trying to buy it by clicking on the "Buy" button

            if (showinfo && CardInfoPanel.IsBuy && ButtonCheck(new Point(CardInfoPanel.backdrop.pos.X + 24, CardInfoPanel.backdrop.pos.Y + 286), 140, 48))
            {
                // If he is and has enough money, we give him the card

                if (GM._players[ActivePlayer]._coins >= CardInfoPanel.CardType._cost && (Control.MouseButtons & MouseButtons.Left) != 0 && BoardElementsManager.State == "Shop")
                {
                    PlayEtablishment();
                    BoardElementsManager.PlayerDecks[ActivePlayer].PlayerCards.Add(BoardElementsManager.Shop.buyCard(CardInfoPanel.CardType, GM._players[ActivePlayer], HUDManager.PlayerHUDs[ActivePlayer], GM));
                    BoardElementsManager.State = "ShopClean";
                }
            }

            // If the info panel is openned and the player clicks on the screen, we want to close it

            if (!isclicking && !islookingcard && showinfo && (Control.MouseButtons & MouseButtons.Left) != 0)
            {
                showinfo = false;
                BoardElementsManager.Updated = true;
            }

            // if the player is trying to look at a card, we show the info panel

            if (islookingcard)
            {
                showinfo = true;
                BoardElementsManager.Updated = true;
                if (MousePosition.X > 480 + Location.X)
                {
                    CardInfoPanel.backdrop.pos = new Point(48, 96);
                }
                else
                {
                    CardInfoPanel.backdrop.pos = new Point(480, 96);
                }
                CardInfoPanel.UpdateImage();

            }

            // This is used to denote the player clicking having just clicked. Since the code is run continuously, it's impossible to differenciate
            // between a new click and the player holding the mouse button without this little variable

            if ((Control.MouseButtons & MouseButtons.Left) == 0 && isclicking)
            {
                isclicking = false;
            }

            // All this code is used when the users are choogin how may players will play the game

            if (State == "PlayerChoiceWait")
            {
                // We check if they are hovering over any buttons

                if (MousePosition.X < Location.X + BoardElementsManager.PlayerChoices[1].sprite.pos.X + 2 * 8 * 6 && MousePosition.X > Location.X + BoardElementsManager.PlayerChoices[1].sprite.pos.X && MousePosition.Y < Location.Y + BoardElementsManager.PlayerChoices[1].sprite.pos.Y + 2 * 8 * 6 && MousePosition.Y > Location.Y + BoardElementsManager.PlayerChoices[1].sprite.pos.Y)
                {
                    // And if they are we play the correct sound and update the sprites

                    if(forHoverSound != "2p")
                    {
                        forHoverSound = "2p";
                        PlayHoverSound();
                    }
                    BoardElementsManager.PlayerChoices[0].sprite.sprite = BoardElementsManager.PlayerChoices[0].on;
                    BoardElementsManager.PlayerChoices[1].sprite.sprite = BoardElementsManager.PlayerChoices[1].on;
                    BoardElementsManager.PlayerChoices[2].sprite.sprite = BoardElementsManager.PlayerChoices[2].off;
                    BoardElementsManager.PlayerChoices[3].sprite.sprite = BoardElementsManager.PlayerChoices[3].off;

                    nbPlayers = 2;

                    BoardElementsManager.Updated = true;

                    if ((Control.MouseButtons & MouseButtons.Left) != 0)
                    {
                        PlayClickSound();

                        InitPlayers();
                        State = "InitDice";
                        BoardElementsManager.State = "none";
                    }
                }
                if (MousePosition.X < Location.X + BoardElementsManager.PlayerChoices[2].sprite.pos.X + 2 * 8 * 6 && MousePosition.X > Location.X + BoardElementsManager.PlayerChoices[2].sprite.pos.X && MousePosition.Y < Location.Y + BoardElementsManager.PlayerChoices[2].sprite.pos.Y + 2 * 8 * 6 && MousePosition.Y > Location.Y + BoardElementsManager.PlayerChoices[2].sprite.pos.Y)
                {
                    // And if they are we play the correct sound and update the sprites

                    if (forHoverSound != "3p")
                    {
                        forHoverSound = "3p";
                        PlayHoverSound();
                    }
                    BoardElementsManager.PlayerChoices[0].sprite.sprite = BoardElementsManager.PlayerChoices[0].on;
                    BoardElementsManager.PlayerChoices[1].sprite.sprite = BoardElementsManager.PlayerChoices[1].on;
                    BoardElementsManager.PlayerChoices[2].sprite.sprite = BoardElementsManager.PlayerChoices[2].on;
                    BoardElementsManager.PlayerChoices[3].sprite.sprite = BoardElementsManager.PlayerChoices[3].off;

                    nbPlayers = 3;

                    BoardElementsManager.Updated = true;

                    if ((Control.MouseButtons & MouseButtons.Left) != 0)
                    {
                        PlayClickSound();

                        InitPlayers();
                        State = "InitDice";
                        BoardElementsManager.State = "showPlayers";
                    }
                }
                if (MousePosition.X < Location.X + BoardElementsManager.PlayerChoices[3].sprite.pos.X + 2 * 8 * 6 && MousePosition.X > Location.X + BoardElementsManager.PlayerChoices[3].sprite.pos.X && MousePosition.Y < Location.Y + BoardElementsManager.PlayerChoices[3].sprite.pos.Y + 2 * 8 * 6 && MousePosition.Y > Location.Y + BoardElementsManager.PlayerChoices[3].sprite.pos.Y)
                {
                    // And if they are we play the correct sound and update the sprites

                    if (forHoverSound != "4p")
                    {
                        forHoverSound = "4p";
                        PlayHoverSound();
                    }
                    BoardElementsManager.PlayerChoices[0].sprite.sprite = BoardElementsManager.PlayerChoices[0].on;
                    BoardElementsManager.PlayerChoices[1].sprite.sprite = BoardElementsManager.PlayerChoices[1].on;
                    BoardElementsManager.PlayerChoices[2].sprite.sprite = BoardElementsManager.PlayerChoices[2].on;
                    BoardElementsManager.PlayerChoices[3].sprite.sprite = BoardElementsManager.PlayerChoices[3].on;

                    nbPlayers = 4;

                    BoardElementsManager.Updated = true;

                    if ((Control.MouseButtons & MouseButtons.Left) != 0)
                    {
                        PlayClickSound();

                        InitPlayers();
                        State = "InitDice";
                        BoardElementsManager.State = "none";
                    }
                }
            }

            // After having chosen the amount of players or simply having started a new turn, this code will be excecuted

            if (State == "InitDice")
            {
                // We give the player a reroll if he has the 3rd monument
                if (GM._players[ActivePlayer].monuments[2])
                {
                    CanReroll = true;
                }
                // We ask them if they want to roll one or two dices if they have the 1st monument
                if (GM._players[ActivePlayer].monuments[0])
                {
                    State = "WaitDiceChoice";
                    BoardElementsManager.Updated = true;
                }
                // and if not we just keep on going like normal
                else
                {
                    State = "ThrowDice";
                    BoardElementsManager.Dices = new DiceForm[1];
                }
            }
            // If the player has the 1st monument, we ask them if they want to throw one or two dices here
            if (State == "WaitDiceChoice")
            {
                // One dice button
                if (MousePosition.X < Location.X + 432 && MousePosition.X > Location.X + 336 && MousePosition.Y < Location.Y + 432 && MousePosition.Y > Location.Y + 336)
                {
                    DiceButton[1].sprite = Image.FromFile("Sprites/Dice2ButtonNot.png");

                    BoardElementsManager.Updated = true;

                    if ((Control.MouseButtons & MouseButtons.Left) != 0)
                    {
                        // Throw 1
                        BoardElementsManager.Dices = new DiceForm[1];
                        State = "ThrowDice";
                    }
                }
                // Two dice button
                if (MousePosition.X < Location.X + 624 && MousePosition.X > Location.X + 528 && MousePosition.Y < Location.Y + 432 && MousePosition.Y > Location.Y + 336)
                {
                    DiceButton[1].sprite = Image.FromFile("Sprites/Dice2.png");

                    BoardElementsManager.Updated = true;

                    if ((Control.MouseButtons & MouseButtons.Left) != 0)
                    {
                        // Throw 2
                        BoardElementsManager.Dices = new DiceForm[2];
                        State = "ThrowDice";
                    }
                }
            }
            // After having chosen how many dices are thrown, this code will be executed
            if (State == "ThrowDice")
            {
                PlayDiceSound();

                Random rnd = new Random();
                
                // Rolling one die
                BoardElementsManager.Dices[0] = new DiceForm(rnd.Next(1, 7));//--------------------------------------------------------------------------------------------------
                int rolltot = BoardElementsManager.Dices[0].value;
                // Rolling the second if there's another one
                if (BoardElementsManager.Dices.Length == 2)
                {
                    BoardElementsManager.Dices[1] = new DiceForm(rnd.Next(1, 7));
                    rolltot += BoardElementsManager.Dices[1].value;
                }

                // Here we interrogate the effectManager to get the list of card effects being activated. The effect manager will change the internal value and return
                // a list of ints containing for each gain the player that own the ativated card and the value of the gain.

                gains = new List<List<int>>();
                gains.Clear();
                gains = GM._effectManager.Effect(rolltot, GM._players[ActivePlayer], GM._players);

                // This code here is to avoid coin overflow, which would crash the game
                foreach(Player p in GM._players)
                {
                    if(p._coins > 99) { p._coins = 99; }
                }

                // And when we've initiated everything we can animate the dice
                State = "DiceAnimWait";
                BoardElementsManager.State = "DiceInit";
                BoardElementsManager.Updated = true;
            }

            // This code checks if the player has the 1st monument, the 4th one and has thrown 2 dices. In this case, and if the dices have the same
            // value, another turn will play after the current one
            if (BoardElementsManager.State == "DiceAnim")
            {
                if (GM._players[ActivePlayer].monuments[0] & GM._players[ActivePlayer].monuments[3] & BoardElementsManager.Dices.Length == 2)
                {
                    if (BoardElementsManager.Dices[0].value == BoardElementsManager.Dices[1].value)
                    {
                        BoardElementsManager.anotherTurn = true;
                    }
                }
            }

            // We're checking if the dice has been thrown, and if it's animation if over and the player can reroll he's dice, we can ask if he wants
            // to reroll the dice(s). Else we go ahead and initiate the gains animations
            if (State == "DiceAnimWait" && BoardElementsManager.State == "GainsWait")
            {
                if (CanReroll)
                {
                    State = "AskReroll";
                }
                else
                {
                    State = "GainsInit";
                }
            }

            // If the player can reroll their dices, this code is executed
            if (State == "AskReroll")
            {
                // We create and render the buttons
                AdditionnalButtons.Clear();
                Sprite rerollyes = new Sprite(Image.FromFile("sprites/rerollyes.png"), new Point(7 * 8 * 6, 5 * 8 * 6), 0);
                Sprite rerollno = new Sprite(Image.FromFile("sprites/rerollno.png"), new Point(11 * 8 * 6, 5 * 8 * 6), 0);

                AdditionnalButtons.Add(rerollyes);
                AdditionnalButtons.Add(rerollno);

                // And check which one he clicks on
                if (ButtonCheck(rerollyes.pos, 96, 96))
                {
                    AdditionnalButtons.Clear();
                    State = "GainsInit";
                    CanReroll = false;
                }
                if (ButtonCheck(rerollno.pos, 96, 96))
                {
                    AdditionnalButtons.Clear();
                    BoardElementsManager.State = "none";
                    State = "ThrowDice";
                    CanReroll = false;
                }
            }

            // Here we setup every thing to process the gains.
            if (State == "GainsInit")
            {
                // ID of player ||| ID of card from player deck ||| money gained and/or lost

                countingforgains = 0;

                CountingStadiumGain = 0;

                nbActivatedCard = new int[GM._deck.Count][];

                for (int i = 0; i < nbActivatedCard.Length; i++)
                {
                    nbActivatedCard[i] = new int[nbPlayers];
                }

                State = "Gains";
            }

            // This is the first step of every loop. For every gain the programm needs to process, it will first set Form1's state to "Gains", where it will initiate every variable and
            // set the gain form to the correct spot. It will then switch the state to "GainsAnim", where it will repeatedly animate the gain form until it's animation is complete, where
            // it will change the state back to "Gains" and process the next gain before animating it, so on and so forth.

            if (State == "Gains")
            {
                bool ispurple = false;

                if (countingforgains != gains.Count)
                {
                    PlayCoinSound();

                    int cardtodraw = 0;

                    int counter = 0;
                    foreach(Card c in GM._players[gains[countingforgains][0]].cards)
                    {
                        bool stop = false;

                        int forcountingpurposesindexcount = 0;
                        foreach (Card cd in GM.FORCOUNTINGPURPOSES)
                        {
                            if(c == cd && forcountingpurposesindexcount == gains[countingforgains][1])
                            {
                                if(counter == nbActivatedCard[gains[countingforgains][1]][gains[countingforgains][0]])
                                {
                                    stop = true;
                                    break;
                                }
                                else
                                {
                                    counter++;
                                }
                            }
                            forcountingpurposesindexcount++;
                        }
                        if (stop) { break; }
                        cardtodraw++;
                    }
                    nbActivatedCard[gains[countingforgains][1]][gains[countingforgains][0]]++;

                    GLOBALTEST = cardtodraw;

                    if(cardtodraw > GM._players[gains[countingforgains][0]].cards.Count)
                    {
                        //   yo y'a un pb
                    }
                    if (countingforgains > gains.Count)
                    {
                        //   yo y'a un pb
                    }
                    if (gains[countingforgains][0] > GM._players.Count)
                    {
                        //   yo y'a un pb
                    }
                    if(GM._players[gains[countingforgains][0]].cards[cardtodraw]._color == null)
                    {

                    }

                    switch (GM._players[gains[countingforgains][0]].cards[cardtodraw]._color)
                    {
                        case (ColorCard.red):

                            HUDManager.PlayerHUDs[ActivePlayer].money -= gains[countingforgains][2];
                            HUDManager.PlayerHUDs[gains[countingforgains][0]].money += gains[countingforgains][2];

                            BoardElementsManager.Gains[0].InitGain(-gains[countingforgains][2], new Point(6 * 6 * 8, 9 * 6 * 8));

                            switch (BoardElementsManager.PlayerDecks[gains[countingforgains][0]].IntendedRota)
                            {
                                case (1):
                                    BoardElementsManager.Gains[gains[countingforgains][0]].InitGain(gains[countingforgains][2], new Point(480, 502 - ((288 / BoardElementsManager.PlayerDecks[gains[countingforgains][0]].PlayerCards.Count)) * cardtodraw));
                                    break;
                                case (2):
                                    BoardElementsManager.Gains[gains[countingforgains][0]].InitGain(gains[countingforgains][2], new Point(480 - ((240 / BoardElementsManager.PlayerDecks[gains[countingforgains][0]].PlayerCards.Count)) * cardtodraw, 312));
                                    break;
                                case (3):
                                    BoardElementsManager.Gains[gains[countingforgains][0]].InitGain(gains[countingforgains][2], new Point(240, 240 + ((240 / BoardElementsManager.PlayerDecks[gains[countingforgains][0]].PlayerCards.Count)) * cardtodraw));
                                    break;
                                default:
                                    BoardElementsManager.Gains[gains[countingforgains][0]].InitGain(gains[countingforgains][2], new Point(528, 480 - ((240 / BoardElementsManager.PlayerDecks[gains[countingforgains][0]].PlayerCards.Count)) * cardtodraw));
                                    break;
                            }
                            BoardElementsManager.PlayerDecks[gains[countingforgains][0]].drawnCard = cardtodraw;
                            break;
                        case (ColorCard.blue):
                            HUDManager.PlayerHUDs[gains[countingforgains][0]].money += gains[countingforgains][2];
                            switch (BoardElementsManager.PlayerDecks[gains[countingforgains][0]].IntendedRota)
                            {
                                case (0):
                                    BoardElementsManager.Gains[gains[countingforgains][0]].InitGain(gains[countingforgains][2], new Point(144 + ((544 / BoardElementsManager.PlayerDecks[gains[countingforgains][0]].PlayerCards.Count)) * cardtodraw, 300));
                                    break;
                                case (1):
                                    BoardElementsManager.Gains[gains[countingforgains][0]].InitGain(gains[countingforgains][2], new Point(480, 502 - ((288 / BoardElementsManager.PlayerDecks[gains[countingforgains][0]].PlayerCards.Count)) * cardtodraw));
                                    break;
                                case (2):
                                    BoardElementsManager.Gains[gains[countingforgains][0]].InitGain(gains[countingforgains][2], new Point(480 - ((240 / BoardElementsManager.PlayerDecks[gains[countingforgains][0]].PlayerCards.Count)) * cardtodraw, 312));
                                    break;
                                case (3):
                                    BoardElementsManager.Gains[gains[countingforgains][0]].InitGain(gains[countingforgains][2], new Point(240, 288 + ((240 / BoardElementsManager.PlayerDecks[gains[countingforgains][0]].PlayerCards.Count)) * cardtodraw));
                                    break;
                                default:
                                    BoardElementsManager.Gains[gains[countingforgains][0]].InitGain(gains[countingforgains][2], new Point(528, 480 - ((240 / BoardElementsManager.PlayerDecks[gains[countingforgains][0]].PlayerCards.Count)) * cardtodraw));
                                    break;
                            }
                            BoardElementsManager.PlayerDecks[gains[countingforgains][0]].drawnCard = cardtodraw;
                            BoardElementsManager.PlayerDecks[gains[countingforgains][0]].displacementfordrawncard = 0;
                            break;
                        case (ColorCard.green):
                            HUDManager.PlayerHUDs[gains[countingforgains][0]].money += gains[countingforgains][2];
                            BoardElementsManager.Gains[gains[countingforgains][0]].InitGain(gains[countingforgains][2], new Point(144 + ((544 / BoardElementsManager.PlayerDecks[gains[countingforgains][0]].PlayerCards.Count)) * cardtodraw, 300));
                            BoardElementsManager.PlayerDecks[gains[countingforgains][0]].drawnCard = cardtodraw;
                            break;
                        case (ColorCard.purple):
                            if (GM._players[gains[countingforgains][0]].cards[cardtodraw]._name == "Centre d'affaires")
                            {
                                tradeChooseOwnCard = true;
                                ispurple = true;
                            }
                            if (GM._players[gains[countingforgains][0]].cards[gains[countingforgains][1]]._name == "Chaîne de télévision")
                            {
                                ispurple = true;
                            }
                            if (GM._players[gains[countingforgains][0]].cards[gains[countingforgains][1]]._name == "Stade")
                            {
                                switch (CountingStadiumGain)
                                {
                                    case (0):
                                        BoardElementsManager.Gains[1].InitGain(-gains[countingforgains][2], new Point(576, 312));
                                        BoardElementsManager.Gains[0].InitGain(gains[countingforgains][2], new Point(6 * 6 * 8, 9 * 6 * 8));
                                        break;
                                    case (1):
                                        BoardElementsManager.Gains[1].InitGain(-gains[countingforgains][2], new Point(384, 240));
                                        BoardElementsManager.Gains[0].InitGain(gains[countingforgains][2], new Point(6 * 6 * 8, 9 * 6 * 8));
                                        break;
                                    case (2):
                                        BoardElementsManager.Gains[1].InitGain(-gains[countingforgains][2], new Point(144, 312));
                                        BoardElementsManager.Gains[0].InitGain(gains[countingforgains][2], new Point(6 * 6 * 8, 9 * 6 * 8));
                                        break;
                                }
                                BoardElementsManager.PlayerDecks[gains[countingforgains][0]].drawnCard = cardtodraw;
                                BoardElementsManager.PlayerDecks[gains[countingforgains][0]].displacementfordrawncard = 0;
                                CountingStadiumGain++;
                            }
                            break;
                    }

                    animtimer = 0;
                    BoardElementsManager.Updated = true;
                    State = "GainAnim";
                    if (ispurple)
                    {
                        State = "purple";
                    }
                }
                // If every gain has been process, we go onto the shop phase and setup the shop in BoardElements
                else
                {
                    State = "ShopWait";
                    BoardElementsManager.State = "ShopAnim";
                    BoardElementsManager.Shop.InitShopAnim(GM._players[ActivePlayer]);
                    foreach (CardForm c in BoardElementsManager.PlayerDecks[ActivePlayer].PlayerCards)
                    {
                        c.setToSmall();
                    }
                    BoardElementsManager.Updated = true;
                }
            }

            // If the current card gain being processed is from a purple card, we need to tackle it specificaly in this if statement
            if (State == "purple")
            {
                // Code for the business center
                if (GM._players[gains[countingforgains][0]].cards[gains[countingforgains][1]]._name == "Centre d'affaires")
                {
                    AdditionnalButtons.Clear();

                    if (tradeChooseOwnCard)
                    {
                        AdditionnalButtons.Add(new Sprite(Image.FromFile("sprites/ChooseOrSkip.png"), new Point(648, 24), 0));
                        AdditionnalButtons.Add(BoardElementsManager.Shop.SkipButton);

                        int i = 0;
                        foreach (CardForm c in BoardElementsManager.PlayerDecks[ActivePlayer].PlayerCards)
                        {
                            if (ButtonCheck(c.sprite.pos, 3 * 8 * 6, 4 * 8 * 6, true))
                            {
                                tradeplayercard = c;

                                int cbm = 0;
                                foreach(Card card in GM.FORCOUNTINGPURPOSES)
                                {
                                    if(card == BoardElementsManager.PlayerDecks[ActivePlayer].PlayerCards[i].CardType) { idOfTradedPlayerCard = cbm; }
                                    cbm++;
                                }
                                tradeChooseOwnCard = false;
                            }
                            i++;
                        }
                        if (ButtonCheck(BoardElementsManager.Shop.SkipButton.pos, 5 * 8 * 6, 2 * 8 * 6))
                        {
                            State = "Gains";
                            countingforgains++;
                            AdditionnalButtons.Clear();
                        }
                    }
                    else
                    {
                        AdditionnalButtons.Add(new Sprite(Image.FromFile("sprites/ChooseOpponent.png"), new Point(648, 24), 0));

                        int playerforexchange = 0;
                        foreach (PlayerDeckForm pd in BoardElementsManager.PlayerDecks)
                        {
                            if (pd != BoardElementsManager.PlayerDecks[ActivePlayer])
                            {
                                int i = 0;
                                foreach (CardForm c in pd.PlayerCards)
                                {
                                    if (pd.IntendedRota == 0 || pd.IntendedRota == 2)
                                    {
                                        if (ButtonCheck(c.sprite.pos, 48, 72, true))
                                        {
                                            tradeothercard = c;

                                            int cbm = 0;
                                            foreach (Card card in GM.FORCOUNTINGPURPOSES)
                                            {
                                                if (card == pd.PlayerCards[i].CardType) { idOfOtherTradedCard = cbm; }
                                                cbm++;
                                            }

                                            pd.PlayerCards.Remove(tradeothercard);
                                            pd.PlayerCards.Add(new CardForm(tradeplayercard.CardType, true, false));
                                            GM._players[playerforexchange]._terrain[tradeothercard.CardType]--;
                                            GM._players[playerforexchange]._terrain[tradeplayercard.CardType]++;
                                            BoardElementsManager.PlayerDecks[ActivePlayer].PlayerCards.Remove(tradeplayercard);
                                            BoardElementsManager.PlayerDecks[ActivePlayer].PlayerCards.Add(new CardForm(tradeothercard.CardType, false, false));
                                            GM._players[ActivePlayer]._terrain[tradeplayercard.CardType]--;
                                            GM._players[ActivePlayer]._terrain[tradeothercard.CardType]++;

                                            State = "Gains";
                                            countingforgains++;
                                            AdditionnalButtons.Clear();

                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (ButtonCheck(c.sprite.pos, 72, 48, true))
                                        {
                                            tradeothercard = c;

                                            int cbm = 0;
                                            foreach (Card card in GM.FORCOUNTINGPURPOSES)
                                            {
                                                if (card == pd.PlayerCards[i].CardType) { idOfOtherTradedCard = cbm; }
                                                cbm++;
                                            }

                                            pd.PlayerCards.Remove(tradeothercard);
                                            pd.PlayerCards.Add(tradeplayercard);
                                            BoardElementsManager.PlayerDecks[ActivePlayer].PlayerCards.Remove(tradeplayercard);
                                            BoardElementsManager.PlayerDecks[ActivePlayer].PlayerCards.Add(tradeothercard);

                                            State = "Gains";
                                            countingforgains++;
                                            AdditionnalButtons.Clear();

                                            break;
                                        }
                                    }
                                    i++;
                                }
                            }
                            playerforexchange++;
                        }
                    }


                }
                else
                {
                    // Code for the TV channel
                    if (GM._players[gains[countingforgains][0]].cards[gains[countingforgains][1]]._name == "Chaîne de télévision")
                    {
                        AdditionnalButtons.Clear();

                        switch (GM._players.Count)
                        {
                            case (2):
                                BoardElementsManager.Gains[1].InitGain(-gains[countingforgains][2], new Point(576, 312));
                                BoardElementsManager.Gains[0].InitGain(gains[countingforgains][2], new Point(6 * 6 * 8, 9 * 6 * 8));
                                animtimer = 0;
                                BoardElementsManager.Updated = true;
                                State = "GainAnim";
                                AdditionnalButtons.Clear();
                                BoardElementsManager.PlayerDecks[gains[countingforgains][0]].drawnCard = gains[countingforgains][1];
                                BoardElementsManager.PlayerDecks[gains[countingforgains][0]].displacementfordrawncard = 0;

                                // SET CHOSEN PLAYERS
                                break;
                            case (3):
                                AdditionnalButtons.Add(new Sprite(Image.FromFile("sprites/choosePlayerOne.png"), new Point(7 * 8 * 6, 5 * 8 * 6), 0));
                                AdditionnalButtons.Add(new Sprite(Image.FromFile("sprites/choosePlayerTwo.png"), new Point(11 * 8 * 6, 5 * 8 * 6), 0));

                                if (ButtonCheck(new Point(7 * 8 * 6, 5 * 8 * 6), 96, 96))
                                {
                                    BoardElementsManager.Gains[1].InitGain(-gains[countingforgains][2], new Point(576, 312));
                                    BoardElementsManager.Gains[0].InitGain(gains[countingforgains][2], new Point(6 * 6 * 8, 9 * 6 * 8));
                                    animtimer = 0;
                                    BoardElementsManager.Updated = true;
                                    State = "GainAnim";
                                    AdditionnalButtons.Clear();
                                    BoardElementsManager.PlayerDecks[gains[countingforgains][0]].drawnCard = gains[countingforgains][1];
                                    BoardElementsManager.PlayerDecks[gains[countingforgains][0]].displacementfordrawncard = 0;
                                }
                                if (ButtonCheck(new Point(11 * 8 * 6, 5 * 8 * 6), 96, 96))
                                {
                                    BoardElementsManager.Gains[1].InitGain(-gains[countingforgains][2], new Point(384, 240));
                                    BoardElementsManager.Gains[0].InitGain(gains[countingforgains][2], new Point(6 * 6 * 8, 9 * 6 * 8));
                                    animtimer = 0;
                                    BoardElementsManager.Updated = true;
                                    State = "GainAnim";
                                    AdditionnalButtons.Clear();
                                    BoardElementsManager.PlayerDecks[gains[countingforgains][0]].drawnCard = gains[countingforgains][1];
                                    BoardElementsManager.PlayerDecks[gains[countingforgains][0]].displacementfordrawncard = 0;
                                    CountingStadiumGain++;
                                }

                                // SET CHOSEN PLAYERS
                                break;
                            case (4):
                                AdditionnalButtons.Add(new Sprite(Image.FromFile("sprites/choosePlayerOne.png"), new Point(6 * 8 * 6, 5 * 8 * 6), 0));
                                AdditionnalButtons.Add(new Sprite(Image.FromFile("sprites/choosePlayerTwo.png"), new Point(9 * 8 * 6, 5 * 8 * 6), 0));
                                AdditionnalButtons.Add(new Sprite(Image.FromFile("sprites/choosePlayerThree.png"), new Point(12 * 8 * 6, 5 * 8 * 6), 0));

                                if (ButtonCheck(new Point(6 * 8 * 6, 5 * 8 * 6), 96, 96))
                                {
                                    BoardElementsManager.Gains[1].InitGain(-gains[countingforgains][2], new Point(576, 312));
                                    BoardElementsManager.Gains[0].InitGain(gains[countingforgains][2], new Point(6 * 6 * 8, 9 * 6 * 8));
                                    animtimer = 0;
                                    BoardElementsManager.Updated = true;
                                    State = "GainAnim";
                                    AdditionnalButtons.Clear();
                                    BoardElementsManager.PlayerDecks[gains[countingforgains][0]].drawnCard = gains[countingforgains][1];
                                    BoardElementsManager.PlayerDecks[gains[countingforgains][0]].displacementfordrawncard = 0;
                                    CountingStadiumGain++;
                                }
                                if (ButtonCheck(new Point(9 * 8 * 6, 5 * 8 * 6), 96, 96))
                                {
                                    BoardElementsManager.Gains[1].InitGain(-gains[countingforgains][2], new Point(384, 240));
                                    BoardElementsManager.Gains[0].InitGain(gains[countingforgains][2], new Point(6 * 6 * 8, 9 * 6 * 8));
                                    animtimer = 0;
                                    BoardElementsManager.Updated = true;
                                    State = "GainAnim";
                                    AdditionnalButtons.Clear();
                                    BoardElementsManager.PlayerDecks[gains[countingforgains][0]].drawnCard = gains[countingforgains][1];
                                    BoardElementsManager.PlayerDecks[gains[countingforgains][0]].displacementfordrawncard = 0;
                                    CountingStadiumGain++;
                                }
                                if (ButtonCheck(new Point(12 * 8 * 6, 5 * 8 * 6), 96, 96))
                                {
                                    BoardElementsManager.Gains[1].InitGain(-gains[countingforgains][2], new Point(144, 312));
                                    BoardElementsManager.Gains[0].InitGain(gains[countingforgains][2], new Point(6 * 6 * 8, 9 * 6 * 8));
                                    animtimer = 0;
                                    BoardElementsManager.Updated = true;
                                    State = "GainAnim";
                                    AdditionnalButtons.Clear();
                                    BoardElementsManager.PlayerDecks[gains[countingforgains][0]].drawnCard = gains[countingforgains][1];
                                    BoardElementsManager.PlayerDecks[gains[countingforgains][0]].displacementfordrawncard = 0;
                                    CountingStadiumGain++;
                                }

                                // SET CHOSEN PLAYERS
                                break;
                        }
                    }
                }


            }

            // When we're done with the shop interactions, we setup an new turn here
            
            // This is used if the player has the monument that allows for them to play a new turn if they get a double
            if (BoardElementsManager.State == "setupNewTurn")
            {
                // We check if the player won
                if (GM._players[ActivePlayer].Win())
                {
                    BoardElementsManager.State = "none";
                    BoardElementsManager.CleanCard();
                    State = "WIN";
                }
                // If not we start a new turn
                else
                {
                    BoardElementsManager.State = "none";
                    State = "InitDice";

                    BoardElementsManager.anotherTurn = false;
                    BoardElementsManager.Updated = true;
                }
            }

            // This is used to simply switch the active player and setup a new turn for a new player
            if (BoardElementsManager.State == "Form1NewTurn")
            {
                // We check if the player won
                if (GM._players[ActivePlayer].Win())
                {
                    BoardElementsManager.State = "none";
                    BoardElementsManager.CleanCard();
                    State = "WINCLEAN";
                }
                // If not we start a new turn
                else
                {
                    BoardElementsManager.State = "none";
                    State = "InitDice";

                    // We change the active player
                    BoardElementsManager.anotherTurn = false;
                    ActivePlayer++;
                    if (ActivePlayer == GM._players.Count) { ActivePlayer = 0; }

                    // And setup the board accordingly
                    foreach (CardForm c in BoardElementsManager.PlayerDecks[ActivePlayer].PlayerCards)
                    {
                        c.setToBig();
                    }

                    for (int i = 0; i < GM._players.Count; i++)
                    {
                        HUDManager.PlayerHUDs[i].money = GM._players[i]._coins;
                    }
                }
            }
            // ------------------------------------------------------------------------------------------------------
            // ------------------------------------------ W I N C L E A N -------------------------------------------
            // ------------------------------------------------------------------------------------------------------

            // When the game is over, this is used the clean the board and remove all elements
            if (State == "WINCLEAN") 
            {
                BoardElementsManager.Updated = true;
                if (HUDManager.HUDOFFSET < 384)
                {
                    HUDManager.HUDOFFSET += 16;
                }
                else
                {
                    // when the board is cleared, we can use the special "EndAnimClass" class to display the winner
                    PlayWinSound();

                    for(int i = 0; i < endanim.intednedY.Count; i++)
                    {
                        endanim.intednedY[i] += 576;
                    }
                    endanim.numberform.ChangeNumber(ActivePlayer+1, 2);
                    endanim.NumberIntendedPos = 472;
                    State = "WIN";
                }
            }

            // ------------------------------------------------------------------------------------------------------
            // ------------------------------------------ G a i n A n i m -------------------------------------------
            // ------------------------------------------------------------------------------------------------------

            // This state animates the gains. When everything is setup, we simply displace the image every tick and setup another gain when it's done
            if (State == "GainAnim")
            {
                animtimer++;

                if (animtimer < 12)
                {
                    BoardElementsManager.PlayerDecks[gains[countingforgains][0]].displacementfordrawncard += (12 - animtimer) * 2;
                }
                /*
                if(animtimer>12 && animtimer < 36)
                {
                    BoardElementsManager.PlayerDecks[gains[countingforgains][0]].displacementfordrawncard = 0;
                }
                */
                if (animtimer > 36 && animtimer < 48)
                {
                    BoardElementsManager.PlayerDecks[gains[countingforgains][0]].displacementfordrawncard -= (12 - (animtimer - 36)) * 2;
                }

                if (animtimer > 48)
                {
                    /*
                    switch (BoardElementsManager.PlayerDecks[gains[countingforgains][0]].PlayerCards[gains[countingforgains][1]].sprite.rota)
                    {
                        case (1):
                            BoardElementsManager.PlayerDecks[gains[countingforgains][0]].PlayerCards[gains[countingforgains][1]].sprite.pos = new Point(BoardElementsManager.PlayerDecks[gains[countingforgains][0]].PlayerCards[gains[countingforgains][1]].sprite.pos.X + 2 * 8 * 6, BoardElementsManager.PlayerDecks[gains[countingforgains][0]].PlayerCards[gains[countingforgains][1]].sprite.pos.Y);
                            break;
                        case (2):
                            BoardElementsManager.PlayerDecks[gains[countingforgains][0]].PlayerCards[gains[countingforgains][1]].sprite.pos = new Point(BoardElementsManager.PlayerDecks[gains[countingforgains][0]].PlayerCards[gains[countingforgains][1]].sprite.pos.X, BoardElementsManager.PlayerDecks[gains[countingforgains][0]].PlayerCards[gains[countingforgains][1]].sprite.pos.Y - 2 * 8 * 6);
                            break;
                        case (3):
                            BoardElementsManager.PlayerDecks[gains[countingforgains][0]].PlayerCards[gains[countingforgains][1]].sprite.pos = new Point(BoardElementsManager.PlayerDecks[gains[countingforgains][0]].PlayerCards[gains[countingforgains][1]].sprite.pos.X - 2 * 8 * 6, BoardElementsManager.PlayerDecks[gains[countingforgains][0]].PlayerCards[gains[countingforgains][1]].sprite.pos.Y);
                            break;
                        default:
                            BoardElementsManager.PlayerDecks[gains[countingforgains][0]].PlayerCards[gains[countingforgains][1]].sprite.pos = new Point(BoardElementsManager.PlayerDecks[gains[countingforgains][0]].PlayerCards[gains[countingforgains][1]].sprite.pos.X + 2 * 8 * 6, BoardElementsManager.PlayerDecks[gains[countingforgains][0]].PlayerCards[gains[countingforgains][1]].sprite.pos.Y);
                            break;
                    }
                    */
                    foreach (GainForm g in BoardElementsManager.Gains)
                    {
                        if (g.isAnim)
                        {
                            BoardElementsManager.Updated = true;
                            BoardElementsManager.PlayerDecks[gains[countingforgains][0]].drawnCard = -1;
                            g.EndAnim();
                        }
                    }
                    countingforgains++;
                    State = "Gains";
                }
            }

            // This whole code is used to display the screen. When play number of player has been chosen and no one has won yet, this code compiles every sprite into a list and displays them one at a time
            if (State != "PlayerChoiceWait" && State != "WIN")
            {
                if (HUDManager.Updated || BoardElementsManager.Updated)
                {
                    SpritesToRenderHUD.Clear();
                    SpritesToRenderBoard.Clear();

                    HUDManager.Updated = false;
                    BoardElementsManager.Updated = false;

                    // Here we get all hud sprites
                    List<Sprite> bufferHud = new List<Sprite>();
                    bufferHud = HUDManager.UpdatePlayerHUD(SizeMultiplier, ActivePlayer, GM._players);
                    foreach (Sprite s in bufferHud)
                    {
                        SpritesToRenderHUD.Add(s);
                    }
                    // Here we get all board sprites
                    List<Sprite> bufferBoard = new List<Sprite>();
                    bufferBoard = BoardElementsManager.UpdateBoard();
                    foreach (Sprite s in bufferBoard)
                    {
                        SpritesToRenderBoard.Add(s);
                    }

                    // We add the gains if needed
                    foreach (GainForm g in BoardElementsManager.Gains)
                    {
                        if (g.isAnim)
                        {
                            BoardElementsManager.Updated = true;
                            List<Sprite> GainsBuffer = g.AnimateGain();
                            foreach (Sprite s in GainsBuffer)
                            {
                                SpritesToRenderBoard.Add(s);
                            }
                        }
                    }

                    // We add the dice choices buttons
                    if (State == "WaitDiceChoice")
                    {
                        SpritesToRenderBoard.Add(DiceButton[0]);
                        SpritesToRenderBoard.Add(DiceButton[1]);
                    }
                    if (State == "DiceAnimWait")
                    {
                        BoardElementsManager.Updated = true;
                    }

                    // We add all the shop cards
                    if (State == "ShopWait")
                    {
                        foreach (List<CardForm> ls in BoardElementsManager.Shop.ShopCardForms)
                        {
                            foreach (CardForm c in ls)
                            {
                                SpritesToRenderBoard.Add(c.sprite);
                            }
                        }
                        foreach (Sprite s in BoardElementsManager.Shop.ShopMonumentForms)
                        {
                            SpritesToRenderBoard.Add(s);
                        }
                        SpritesToRenderBoard.Add(BoardElementsManager.Shop.SkipButton);
                    }

                    // we add the info panel
                    if (showinfo)
                    {
                        SpritesToRenderBoard.Add(CardInfoPanel.backdrop);
                        SpritesToRenderBoard.Add(CardInfoPanel.card);
                        SpritesToRenderBoard.Add(CardInfoPanel.desc);

                        if (CardInfoPanel.IsBuy) { SpritesToRenderBoard.Add(CardInfoPanel.button); }
                        if (CardInfoPanel.IsBuy) { SpritesToRenderBoard.Add(CardInfoPanel.price.SpriteHandler); }
                    }

                    // And finaly we add all the potential additional buttons
                    foreach (Sprite s in AdditionnalButtons)
                    {
                        SpritesToRenderBoard.Add(s);
                    }

                    Invalidate();
                }
            }
            // else
            else
            {
                // if the users have yet to chose the amount of players, we diplay the buttons
                if(State != "WIN")
                {
                    SpritesToRenderBoard.Clear();
                    BoardElementsManager.Updated = false;
                    List<Sprite> bufferBoard = new List<Sprite>();
                    bufferBoard = BoardElementsManager.UpdateBoard();
                    foreach (Sprite s in bufferBoard)
                    {
                        SpritesToRenderBoard.Add(s);
                    }
                    Invalidate();
                }
                // if a player has won we instead display all the letter for the end screen
                else
                {
                    SpritesToRenderBoard = endanim.Anim();
                    Invalidate();
                }
            }
        }

        // This class paints every sprite using the list we've created before, using the special "Sprite" class I've made and rendering all of them using their "Render" method
        public void PaintSprites(object sender, PaintEventArgs e)
        {
            Backdrop.Render(e.Graphics);
            if (State != "PlayerChoiceWait")
            {
                foreach (Sprite s in SpritesToRenderHUD)
                {
                    s.Render(e.Graphics);
                }
            }
            foreach (Sprite s in SpritesToRenderBoard)
            {
                s.Render(e.Graphics);
            }
        }

        // This method initiates the players and al their cards
        public void InitPlayers()
        {
            MediaPlayer();

            GM._players = new List<Player>();

            for(int i = 0; i < nbPlayers; i++)
            {
                string name = "player " + (i + 1); // --> default name for each player "player 1" ect..
                List<Monument> monuments = new List<Monument>()
                {
                    new Monument ("Centre commercial", 10, 1, new List<TypeCard>(){TypeCard.restauration, TypeCard.magasin}, ActionMonument.BonusLootParType, ConditionMonument.toujours),
                    new Monument ("Gare", 4, 0, new List<TypeCard>() ,ActionMonument.AjoutDé,ConditionMonument.toujours),
                    new Monument ("Parc d'attractions", 16, 0, new List<TypeCard>() ,ActionMonument.RejouerTour,ConditionMonument.déDouble),
                    new Monument ("Tour radio", 22, 0, new List<TypeCard>() ,ActionMonument.RelanceDé,ConditionMonument.uneFoisParTour)
                };
                Dictionary<Card, int> terrainStart = new Dictionary<Card, int>()
                {
                    { GameManager.champDeBle,1},
                    { GameManager.ferme,0},
                    { GameManager.boulangerie,1},
                    { GameManager.cafe,0},
                    { GameManager.superette,0},
                    { GameManager.foret,0},
                    { GameManager.centreAffaires,0},
                    { GameManager.chaineTele,0},
                    { GameManager.stade,0},
                    { GameManager.fromagerie,0},
                    { GameManager.fabriqueMeuble,0},
                    { GameManager.mine,0},
                    { GameManager.restaurant,0},
                    { GameManager.verger,0},
                    { GameManager.fruitLegume,0}
                };
                GM._players.Add(new Player(name, 3, monuments, terrainStart));
            }

            BoardElementsManager.PlayerDecks = new PlayerDeckForm[nbPlayers];

            for (int i = 0; i < GM._players.Count; i++)
            {
                GM._players[i]._coins = 3;

                BoardElementsManager.Dices = new DiceForm[1];

                BoardElementsManager.PlayerDecks[i] = new PlayerDeckForm();
                BoardElementsManager.PlayerDecks[i].PlayerCards = new List<CardForm>();

                if (i == 0)
                {
                    foreach (Card c in GM._players[i].cards)
                    {
                        BoardElementsManager.PlayerDecks[i].PlayerCards.Add(new CardForm(c, false, false));
                    }
                }
                else
                {
                    foreach (Card c in GM._players[i].cards)
                    {
                        BoardElementsManager.PlayerDecks[i].PlayerCards.Add(new CardForm(c, true, false));
                        for (int j = 0; j < i; j++)
                        {
                            BoardElementsManager.PlayerDecks[i].PlayerCards[BoardElementsManager.PlayerDecks[i].PlayerCards.Count - 1].sprite.sprite.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        }
                    }
                }

                BoardElementsManager.PlayerDecks[i].IntendedRota = i;
            }
            HUDManager = new HUD(GM._players, this);
        }

        // This code is used to check for buttons interactions, using the button's position and size
        private bool ButtonCheck(Point pos, int SizeX, int SizeY, bool isCard = false)
        {
            if (MousePosition.X > pos.X + Location.X & MousePosition.X < pos.X + Location.X + SizeX & MousePosition.Y > pos.Y + Location.Y & MousePosition.Y < pos.Y + Location.Y + SizeY)
            {
                int a = pos.X + pos.Y + SizeX + SizeY;
                if (forHoverSound != a.ToString() && !isCard)
                {
                    forHoverSound = a.ToString();
                    PlayHoverSound();
                }
                if((Control.MouseButtons & MouseButtons.Left) != 0)
                {
                    if(isCard)
                        PlayCardSound();
                    else
                        PlayClickSound();
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        // ---------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------
        // ------------------------------------------ SOUND PLAYER -------------------------------------------
        // ---------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------


        private void DisplayMenu(object sender, EventArgs e)
        {
            PlayClickSound();
            if (menu)
            {
                menu = false;
                panelMenu.Visible = false;
            }
            else
            {
                menu = true;
                panelMenu.Visible = true;
            }
        }

        //SFX
        // These methods each play a unique sounds, andare pretty self explanatory
        private void Sound(string url)
        {
            MXP_SOUND.URL = url;
            MXP_SOUND.Ctlcontrols.play();
        }
        private void PlayDiceSound()
        {
            MXP_SOUND.URL = @"dice2.wav";
            MXP_SOUND.Ctlcontrols.play();
        }

        private void MediaPlayer()
        {
            MXP.URL = @"chillMusic.wav";
            MXP.Ctlcontrols.play();
        }

        private void PlayCardSound()
        {
            MXP_SOUND.URL = @"card.wav";
            MXP_SOUND.Ctlcontrols.play();
        }

        private void PlayMonumentSound()
        {
            MXP_SOUND.URL = @"monument.wav";
            MXP_SOUND.Ctlcontrols.play();
        }

        private void PlayEtablishment()
        {
            MXP_SOUND.URL = @"establishment.wav";
            MXP_SOUND.Ctlcontrols.play();
        }

        private void PlayHoverSound()
        {
            MXP_SOUND.URL = @"hover.wav";
            MXP_SOUND.Ctlcontrols.play();
        }

        private void PlayCoinSound()
        {
            MXP_SOUND.URL = @"coin.wav";
            MXP_SOUND.Ctlcontrols.play();
        }

        private void PlayClickSound()
        {
            MXP_SOUND.URL = @"clickButton.wav";
            MXP_SOUND.Ctlcontrols.play();
        }

        private void PlayWinSound()
        {
            MXP_SOUND.URL = @"winner.wav";
            MXP_SOUND.Ctlcontrols.play();
        }


        //change volumes
        private void LowerMainVolume(object sender, EventArgs e)
        {
            PlayClickSound();
            if (volumeGloal > 0) volumeGloal -= 10;
            AdjustVolume();
        }

        private void UpperMainVolume(object sender, EventArgs e)
        {
            PlayClickSound();
            if (volumeGloal < 100) volumeGloal += 10;
            AdjustVolume();
        }

        private void LowerSoundVolume(object sender, EventArgs e)
        {
            PlayClickSound();
            if (volumeSound > 0) volumeSound -= 10;
            AdjustVolume();
        }

        private void UpperSoundVolume(object sender, EventArgs e)
        {
            PlayClickSound();
            if (volumeSound < 100) volumeSound += 10;
            AdjustVolume();
        }

        private void LowerMusicVolume(object sender, EventArgs e)
        {
            PlayClickSound();
            if (volumeMusic > 0) volumeMusic -= 10;
            AdjustVolume();
        }

        private void UpperMusicVolume(object sender, EventArgs e)
        {
            PlayClickSound();
            if (volumeMusic < 100) volumeMusic += 10;
            AdjustVolume();
        }

        private void AdjustVolume()
        {
            if (main)
            {
                if (music) MXP.settings.volume = volumeMusic * volumeGloal / 100;
                else MXP.settings.volume = 0;
                if (sfx) MXP_SOUND.settings.volume = volumeSound * volumeGloal / 100;
                else MXP_SOUND.settings.volume = 0;
            }
            else
            {
                MXP.settings.volume = 0;
                MXP_SOUND.settings.volume = 0;
            }
            mainVolume.Text = (volumeGloal / 10).ToString();
            percentageMusic.Text = (volumeMusic / 10).ToString();
            percentageSound.Text = (volumeSound / 10).ToString();
        } // apply the modifications to the volume.s

        //turn off the volumes directly
        private void MasterVolume(object sender, EventArgs e)
        {
            if (main)
            {
                main = false;
                btn_masterVolume.Image = Image.FromFile(@"volumeOffImage.png");
            }
            else
            {
                main = true;
                btn_masterVolume.Image = Image.FromFile(@"volumeOnImage.png");
                PlayClickSound();
            }
            AdjustVolume();
            PlayClickSound();
        }

        private void MusicVolume(object sender, EventArgs e)
        {
            if (music)
            {
                music = false;
                btn_musicVolume.Image = Image.FromFile(@"musicOffImage.png");

            }
            else
            {
                music = true;
                btn_musicVolume.Image = Image.FromFile(@"musicOnImage.png");
            }
            AdjustVolume();
            PlayClickSound();
        }

        private void SfxVolume(object sender, EventArgs e)
        {
            if (sfx)
            {
                sfx = false;
                btn_sfxVolume.Image = Image.FromFile(@"sfxOffImage.png");
            }
            else
            {
                sfx = true;
                btn_sfxVolume.Image = Image.FromFile(@"sfxOnImage.png");
                PlayClickSound();
            }
            AdjustVolume();
        }

        private void Hover(object sender, EventArgs e)
        {
            PlayHoverSound();
        }

        // ---------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------
        // ------------------------------------------ SOUND PLAYER -------------------------------------------
        // ---------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------
    }
}
