using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MinivilleBuildFinal.Controls
{
    // This class takes care of the hud elements. Contrarily to Form1 and BoardElements, this class doesn't need states, since it always displays the same thing
    class HUD : Panel
    {
        Form1 form;

        List<CardForm> cardforms;
        List<Sprite> HUDSPRITE;
        List<Sprite> CURRENTPLAYERHUD;

        public bool Updated = true;

        private Size size = new Size(160, 24);
        private Point pos = new Point(0, 144);

        public PlayerHUD[] PlayerHUDs; // Contains the player HUDs
        int ActivePlayer; // ID of the active player
        //List<Card>[] PlayerCards; // List of each player's deck
        bool[][] PlayerMonument; // bool[player][monument]

        public int HUDOFFSET = 0; // for cleaning

        // This instantiates the HUD with each player's cards, money and monument
        public HUD(List<Player> players, Form1 f)
        {
            form = f;

            Name = "HUD";
            BackColor = Color.Gray;
            Anchor = AnchorStyles.None;
            Dock = DockStyle.None;

            PlayerHUDs = new PlayerHUD[players.Count];
            for (int i = 0; i < PlayerHUDs.Length; i++)
            {
                cardforms = new List<CardForm>();
                for (int j = 0; j < players[i].cards.Count; j++)
                {
                    cardforms.Add(new CardForm(players[i].cards[j], true, false));
                }

                PlayerHUDs[i] = new PlayerHUD(players[i].isActive, players[i]._coins, cardforms, players[i].monuments, i);
                switch (players.Count)
                {
                    case (2):
                        PlayerHUDs[i].SpaceForEachPlayer = 421;
                        PlayerHUDs[i].CurrentSprite = PlayerHUDs[i].BackdropSprite2;
                        break;
                    case (3):
                        PlayerHUDs[i].SpaceForEachPlayer = 259;
                        PlayerHUDs[i].CurrentSprite = PlayerHUDs[i].BackdropSprite3;
                        break;
                    case (4):
                        PlayerHUDs[i].SpaceForEachPlayer = 181;
                        PlayerHUDs[i].CurrentSprite = PlayerHUDs[i].BackdropSprite4;
                        break;
                    default:
                        PlayerHUDs[i].SpaceForEachPlayer = 181;
                        PlayerHUDs[i].CurrentSprite = PlayerHUDs[i].BackdropSprite4;
                        break;
                }
            }

            ActivePlayer = 0;
            // PlayerCards =
            PlayerMonument = new bool[4][];
            for (int i = 0; i < PlayerMonument.Length; i++)
            {
                PlayerMonument[i] = new bool[4];
            }

        }

        // Called by Form1, this updates the HUD and returns all the sprites that need to be rendered
        public List<Sprite> UpdatePlayerHUD(int SizeMultiplier, int activePlayer, List<Player> players)
        {
            ActivePlayer = activePlayer;

            HUDSPRITE = new List<Sprite>();

            // For eahc PlayerHUD...
            for (int i = 0; i < PlayerHUDs.Length; i++)
            {
                // it sets their size and sprites
                PlayerHUDs[i].Size = new Size(PlayerHUDs[i].size.Width * SizeMultiplier, PlayerHUDs[i].size.Height * SizeMultiplier);
                int temp;
                switch (players.Count)
                {
                    case (2):
                        temp = 80;
                        break;
                    case (3):
                        temp = 53;
                        break;
                    case (4):
                        temp = 40;
                        break;
                    default:
                        temp = 40;
                        break;
                }
                PlayerHUDs[i].Location = new Point(i * temp * SizeMultiplier, 120 * SizeMultiplier);

                CURRENTPLAYERHUD = new List<Sprite>();

                // ...and all the player's cards
                cardforms = new List<CardForm>();
                for (int j = 0; j < players[i].cards.Count; j++)
                {
                    cardforms.Add(new CardForm(players[i].cards[j], true, false));
                }

                int PlayerMoney;

                // It also updates the money of each player
                if (form.State == "GainsWait" || form.State == "GainsInit" || form.State == "Gains" || form.State == "DiceInit" || form.State == "DiceAnimWait" || form.State == "ThrowDice")
                {
                    PlayerMoney = PlayerHUDs[i].money;
                }
                else
                {
                    PlayerMoney = players[i]._coins;
                }
                // It then calls the PlayerHUDs to let them update themselves and gets the list of all the sprites that compose each HUD
                if (i == ActivePlayer)
                {
                    CURRENTPLAYERHUD = PlayerHUDs[i].UpdatePlayerHUD(SizeMultiplier, true, PlayerMoney, cardforms, players[i].monuments);
                }
                else
                {
                    CURRENTPLAYERHUD = PlayerHUDs[i].UpdatePlayerHUD(SizeMultiplier, false, PlayerMoney, cardforms, players[i].monuments);
                }

                // And it then returns all these sprites to Form1
                foreach (Sprite s in CURRENTPLAYERHUD)
                {
                    HUDSPRITE.Add(s);
                }

                if (PlayerHUDs[i].TargetInterval != PlayerHUDs[i].CurrentInterval)
                {
                    Updated = true;
                }
            }

            Size = new Size(size.Width * SizeMultiplier, size.Height * SizeMultiplier);
            Location = new Point(pos.X * SizeMultiplier, pos.Y * SizeMultiplier);

            if(HUDOFFSET != 0)
            {
                foreach (Sprite s in HUDSPRITE)
                {
                    s.pos.Y += HUDOFFSET;
                }
            }

            return HUDSPRITE;
        }
    }
}
