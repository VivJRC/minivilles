using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MinivilleBuildFinal.Enum;

namespace MinivilleBuildFinal.Controls
{
    // This class is used to keep track of the shop. It holds the information of what cards need to be displayed in the shop
    class ShopForm
    {
        public int animTimer = 0;
        List<CardForm> animCard = new List<CardForm>();
        int totalcount = 0;

        //public Card[] CardTypes;
        public bool[] Monument;

        public List<List<CardForm>> ShopCardForms;
        public Sprite[] ShopMonumentForms;

        public Sprite SkipButton = new Sprite(Image.FromFile("sprites/Skip.png"), new Point(48, 48), 0);

        public ShopForm()
        {
            List<CardForm> allCardType = new List<CardForm>();
            

            allCardType.Add(new CardForm(GameManager.champDeBle, true, false, true));
            allCardType.Add(new CardForm(GameManager.ferme, true, false, true));
            allCardType.Add(new CardForm(GameManager.boulangerie, true, false, true));
            allCardType.Add(new CardForm(GameManager.cafe, true, false, true));
            allCardType.Add(new CardForm(GameManager.superette, true, false, true));
            allCardType.Add(new CardForm(GameManager.foret, true, false, true));
            allCardType.Add(new CardForm(GameManager.centreAffaires, true, false, true));
            allCardType.Add(new CardForm(GameManager.chaineTele, true, false, true));
            allCardType.Add(new CardForm(GameManager.stade, true, false, true));
            allCardType.Add(new CardForm(GameManager.fromagerie, true, false, true));
            allCardType.Add(new CardForm(GameManager.fabriqueMeuble, true, false, true));
            allCardType.Add(new CardForm(GameManager.mine, true, false, true));
            allCardType.Add(new CardForm(GameManager.restaurant, true, false, true));
            allCardType.Add(new CardForm(GameManager.verger, true, false, true));
            allCardType.Add(new CardForm(GameManager.fruitLegume, true, false, true));

            ShopCardForms = new List<List<CardForm>>();
            for (int i = 0; i < allCardType.Count; i++)
            {
                ShopCardForms.Add(new List<CardForm>());

                for (int j = 0; j < 6; j++)
                {
                    if(j == 4 && allCardType[i].CardType._color == ColorCard.purple) { break; }
                    ShopCardForms[i].Add(new CardForm(allCardType[i].CardType, allCardType[i].isSmall, allCardType[i].isNumber, true));
                    totalcount++;
                }
            }

            ShopMonumentForms = new Sprite[4];
            ShopMonumentForms[0] = new Sprite(Image.FromFile("sprites/Cards/Monument/Mon1No.png"), new Point(24, 576 + 1638), 0);
            ShopMonumentForms[1] = new Sprite(Image.FromFile("sprites/Cards/Monument/Mon2No.png"), new Point(96, 576 + 1638), 0);
            ShopMonumentForms[2] = new Sprite(Image.FromFile("sprites/Cards/Monument/Mon3No.png"), new Point(168, 576 + 1638), 0);
            ShopMonumentForms[3] = new Sprite(Image.FromFile("sprites/Cards/Monument/Mon4No.png"), new Point(240, 576 + 1638), 0);

        }

        /*
        public ShopForm()
        {
            int nbcards = 15;

            ShopCardForms = new List<CardForm>[nbcards];
            for(int i = 0; i < ShopCardForms.Length; i++)
            {
                ShopCardForms[i] = new List<CardForm>();
                for(int j = 0; j < 4; j++)
                {
                    ShopCardForms[i].Add(new CardForm(new Card("Champs de blé", ColorCard.blue), true, false));
                }
            }
        }*/

        // This class initiates the shop's animtion by placing the cards at their correct spot below the screen. It is made to be modular and adapt
        // to the total amount of card types that exist in total
        public void InitShopAnim(Player p)
        {
            test = 0;
            animCard.Clear();
            int nbcardperrow;
            int nbcol;
            if (ShopCardForms.Count <= 15)
            {
                nbcardperrow = 5;
            }
            else
            {
                nbcardperrow = 6;
            }
            nbcol = (ShopCardForms.Count + 1) / nbcardperrow;
            if ((ShopCardForms.Count + 1) % nbcardperrow != 0) { nbcol += 1; }

            int StartX = 192;
            int StartY = 2040 - (ShopCardForms.Count * 72);

            int TotalXSpace = 14 * 8 * 6;
            int TotalYSpace = 9 * 8 * 6;

            int cardSpacingX = TotalXSpace / nbcardperrow;
            int cardSpacingY = TotalYSpace / nbcol;

            for (int i = 0; i < ShopCardForms.Count; i++)
            {
                Point pos = new Point(StartX + (cardSpacingX * (i % nbcardperrow)), StartY + (cardSpacingY * (i / nbcardperrow)));

                for (int j = 0; j < ShopCardForms[i].Count; j++)
                {
                    ShopCardForms[i][j].sprite.pos = new Point(pos.X, pos.Y + (j * 7));
                    animCard.Add(ShopCardForms[i][j]);
                }
            }

            int m = 0;
            foreach (bool b in p.monuments)
            {
                if (b)
                {
                    ShopMonumentForms[m].sprite = Image.FromFile(String.Format("sprites/Cards/Monument/Mon{0}Yes.png", m + 1));
                }
                else
                {
                    ShopMonumentForms[m].sprite = Image.FromFile(String.Format("sprites/Cards/Monument/Mon{0}No.png", m + 1));
                }
                m++;
            }

            animTimer = 0;
        }

        int test = 0;
        // This is used to animated the shop by taking each card and displacing them one after the other
        public void RaiseShopAnim()
        {
            for (int i = 0; i < animCard.Count; i++)
            {
                if (animTimer > i && animTimer < 126 - (totalcount - i))
                {
                    animCard[i].sprite.pos = new Point(animCard[i].sprite.pos.X, animCard[i].sprite.pos.Y - 18);
                }
            }
            foreach (Sprite s in ShopMonumentForms)
            {
                s.pos = new Point(s.pos.X, s.pos.Y - 13);
            }
            animTimer++;
            test++;
        }
        // This is used to animated the shop by taking each card and displacing them one after the other
        public void CleanShopAnim()
        {
            foreach (Sprite s in ShopMonumentForms)
            {
                s.pos = new Point(s.pos.X, s.pos.Y + 13);
            }
            for (int i = 0; i < animCard.Count; i++)
            {
                if (animTimer > i + 126 && animTimer < 252 - (totalcount - i))
                {
                    animCard[animCard.Count - i - 1].sprite.pos = new Point(animCard[animCard.Count - i - 1].sprite.pos.X, animCard[animCard.Count - i - 1].sprite.pos.Y + 18);
                }
            }
            animTimer++;
        }
        // This method is used when the player can buy a card and decided to do so. It updates the shop and return which card has been bought
        public CardForm buyCard(Card cardbought, Player activePlayer, PlayerHUD hud, GameManager gm)
        {
            foreach (List<CardForm> c in ShopCardForms)
            {
                if (c.Count != 0)
                {
                    if (c[0].CardType == cardbought)
                    {
                        CardForm cardformbought = new CardForm(c[0].CardType, true, false);
                        hud.cardForms.Add(new CardForm(c[0].CardType, true, false));
                        activePlayer.cards.Add(c[0].CardType);
                        gm.Establishment(c[0].CardType._name, activePlayer);

                        c.RemoveAt(c.Count - 1);
                        return cardformbought;
                    }
                }
            }
            return null;
        }
    }
}
