using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MinivilleBuildFinal.Controls
{
    // This class is used to keep track of each player's HUD. It hold the information of it's player's cards, money and everything needed to display the proper HUD
    class PlayerHUD : Panel
    {
        public Image CurrentSprite;
        public Image BackdropSprite4;
        public Image BackdropSprite3;
        public Image BackdropSprite2;
        public Image MoneySprite;

        public Size size = new Size(40, 24);
        public Point pos;
        public int CurrentInterval;
        public int TargetInterval;
        public int SpaceForEachPlayer = 181;

        int PlayerID;
        bool isActivePlayer;
        public int money;
        public List<CardForm> cardForms;
        bool[] Monument;

        NumberForm PlayerNumberForm;
        NumberForm[] PlayerMoneyForm;
        MonumentForm[] PlayerMonumentForm;
        public PlayerHUD(bool isactiveplayerP, int moneyP, List<CardForm> cardformsP, bool[] monumentP, int playerID)
        {
            BackdropSprite4 = Image.FromFile("Sprites/PlayerHudBackDrop4.png");
            BackdropSprite3 = Image.FromFile("Sprites/PlayerHudBackDrop3.png");
            BackdropSprite2 = Image.FromFile("Sprites/PlayerHudBackDrop2.png");
            MoneySprite = Image.FromFile("Sprites/Money.png");

            Name = "playerHUD";
            BackColor = Color.LightGray;
            Anchor = AnchorStyles.None;
            Dock = DockStyle.None;

            isActivePlayer = isactiveplayerP;
            money = moneyP;
            cardForms = cardformsP;
            Monument = monumentP;
            PlayerID = playerID;

            TargetInterval = (SpaceForEachPlayer) / (cardForms.Count - 1);
            CurrentInterval = TargetInterval;

            int state;
            if (isActivePlayer)
            {
                state = 1;
            }
            else
            {
                state = 0;
            }

            PlayerNumberForm = new NumberForm(playerID, state);

            PlayerMoneyForm = new NumberForm[2];
            PlayerMoneyForm[0] = new NumberForm((money - (money % 10)) / 10, 1);
            PlayerMoneyForm[1] = new NumberForm(money % 10, 1);

            PlayerMonumentForm = new MonumentForm[4];
            PlayerMonumentForm[0] = new MonumentForm();
            PlayerMonumentForm[1] = new MonumentForm();
            PlayerMonumentForm[2] = new MonumentForm();
            PlayerMonumentForm[3] = new MonumentForm();
        }

        // This class updates the players HUD based on the information it's given and returns a list of every sprite to render
        public List<Sprite> UpdatePlayerHUD(int SizeMultiplier, bool isactiveplayerP, int moneyP, List<CardForm> cardformsP, bool[] monumentP)
        {
            isActivePlayer = isactiveplayerP;
            money = moneyP;

            List<Sprite> RenderSprites = new List<Sprite>();

            RenderSprites.Add(new Sprite(CurrentSprite, Location, 0));

            int state;
            if (isActivePlayer)
            {
                state = 2;
            }
            else
            {
                state = 0;
            }
            // Player Number
            PlayerNumberForm.ChangeNumber(PlayerID + 1, state);
            if (PlayerNumberForm.State == 2)
            {
                PlayerNumberForm.SpriteHandler.pos = new Point(Location.X, Location.Y - (8 * SizeMultiplier));
            }
            else
            {
                PlayerNumberForm.SpriteHandler.pos = Location;
            }
            RenderSprites.Add(PlayerNumberForm.SpriteHandler);

            // Player Money
            PlayerMoneyForm[0].ChangeNumber((money - (money % 10)) / 10, 1);
            PlayerMoneyForm[0].SpriteHandler.pos = new Point(Location.X + 16 * SizeMultiplier, Location.Y);
            RenderSprites.Add(PlayerMoneyForm[0].SpriteHandler);
            PlayerMoneyForm[1].ChangeNumber(money % 10, 1);
            PlayerMoneyForm[1].SpriteHandler.pos = new Point(Location.X + 24 * SizeMultiplier, Location.Y);
            RenderSprites.Add(PlayerMoneyForm[1].SpriteHandler);
            RenderSprites.Add(new Sprite(MoneySprite, new Point(Location.X + 32 * SizeMultiplier, Location.Y), 0));

            TargetInterval = (SpaceForEachPlayer) / (cardForms.Count - 1);

            if (CurrentInterval > TargetInterval + 1 || CurrentInterval < TargetInterval - 1)
            {
                CurrentInterval -= (CurrentInterval - TargetInterval) / 2;
            }
            else
            {
                CurrentInterval = TargetInterval;
            }

            // Player Cards
            for (int i = 0; i < cardForms.Count - 1; i++)
            {
                if ((CurrentInterval * i) > SpaceForEachPlayer)
                {
                    cardForms[i].sprite.pos = new Point(Location.X + 6 + SpaceForEachPlayer - 1, Location.Y + 72);
                }
                else
                {
                    cardForms[i].sprite.pos = new Point(Location.X + 6 + (CurrentInterval * i), Location.Y + 72);
                }
                RenderSprites.Add(cardForms[i].sprite);
            }
            cardForms[cardForms.Count - 1].sprite.pos = new Point(Location.X + 6 + SpaceForEachPlayer - 1, Location.Y + 72);
            RenderSprites.Add(cardForms[cardForms.Count - 1].sprite);

            // Player Monuments
            for (int i = 0; i < PlayerMonumentForm.Length; i++)
            {
                PlayerMonumentForm[i].isBought = Monument[i];
                if (PlayerMonumentForm[i].isBought)
                {
                    PlayerMonumentForm[i].sprite.sprite = PlayerMonumentForm[i].bought;
                }
                else
                {
                    PlayerMonumentForm[i].sprite.sprite = PlayerMonumentForm[i].notBought;
                }
                PlayerMonumentForm[i].sprite.pos = new Point(Location.X + 12 + (18 * i), Location.Y + 54);
                RenderSprites.Add(PlayerMonumentForm[i].sprite);
            }

            return RenderSprites;
        }
    }
}
