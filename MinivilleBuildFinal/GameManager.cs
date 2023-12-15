using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography.X509Certificates;
using MinivilleBuildFinal.Enum;

namespace MinivilleBuildFinal
{
    class GameManager
    {
        public List<Player> _players;
        public Dictionary<Card, int> _deck;
        public EffectManager _effectManager;
        string winner;

        // POUR AJOUTER UNE NOUVELLE CARTE, IL FAUT CREER UN NOUVEL ELEMENT DE CLASS CARD COMME CI DESSOUS ET L'AJOUTER A "FORCOUNTINGPURPOSES";

        public static Card champDeBle = new Card("Champs de blé", 1, new List<int>() { 1 }, TypeCard.agriculture, ColorCard.blue, 1);
        public static Card ferme = new Card("Ferme", 1, new List<int>() { 2 }, TypeCard.elevage, ColorCard.blue, 1);
        public static Card boulangerie = new Card("Boulangerie", 1, new List<int>() { 2, 3 }, TypeCard.magasin, ColorCard.green, 1);
        public static Card cafe = new Card("Café", 2, new List<int>() { 3 }, TypeCard.restauration, ColorCard.red, 1);
        public static Card superette = new Card("Supérette", 2, new List<int>() { 4 }, TypeCard.magasin, ColorCard.green, 3);
        public static Card foret = new Card("Forêt", 3, new List<int>() { 5 }, TypeCard.ressources, ColorCard.blue, 1);
        public static Card centreAffaires = new Card("Centre d'affaires", 8, new List<int>() { 6 }, TypeCard.spe, ColorCard.purple, 0, TypeCard.aucun, new List<Propertie>() { Propertie.Exchangeable });
        public static Card chaineTele = new Card("Chaîne de télévision", 7, new List<int>() { 6 }, TypeCard.spe, ColorCard.purple, 5, TypeCard.aucun, new List<Propertie>() { Propertie.Targetable });
        public static Card stade = new Card("Stade", 6, new List<int>() { 6 }, TypeCard.spe, ColorCard.purple, 2, TypeCard.aucun, new List<Propertie>() { Propertie.FullTarget });
        public static Card fromagerie = new Card("Fromagerie", 5, new List<int>() { 7 }, TypeCard.industrie, ColorCard.green, 3, TypeCard.elevage);
        public static Card fabriqueMeuble = new Card("Fabrique de meubles", 3, new List<int>() { 8 }, TypeCard.industrie, ColorCard.green, 3, TypeCard.ressources);
        public static Card mine = new Card("Mine", 6, new List<int>() { 9 }, TypeCard.ressources, ColorCard.blue, 5);
        public static Card restaurant = new Card("Restaurant", 3, new List<int>() { 9, 10 }, TypeCard.restauration, ColorCard.red, 2);
        public static Card verger = new Card("Verger", 3, new List<int>() { 10 }, TypeCard.agriculture, ColorCard.blue, 3);
        public static Card fruitLegume = new Card("Marché de fruits et légumes", 2, new List<int>() { 11, 12 }, TypeCard.marche, ColorCard.green, 2, TypeCard.agriculture);

        public List<Card> FORCOUNTINGPURPOSES = new List<Card> { champDeBle , ferme , boulangerie , cafe, superette, foret, centreAffaires, chaineTele, stade, fromagerie, fabriqueMeuble, mine, restaurant, verger, fruitLegume };


        public enum StateMachine
        {
            Menu = 0,
            Game = 1,
            End = 2
        }
        StateMachine state;

        //Timer timer = new Timer(); // when I'll use WindowsForm
        public GameManager()
        {
            _players = new List<Player>();
            _effectManager = new EffectManager();
            _deck = new Dictionary<Card, int>()
                {   { champDeBle,6},
                    { ferme,6},
                    { boulangerie,6},
                    { cafe,6},
                    { superette,6},
                    { foret,6},
                    { centreAffaires,4},
                    { chaineTele,4},
                    { stade,4},
                    { fromagerie,6},
                    { fabriqueMeuble,6},
                    { mine,6},
                    { restaurant,6},
                    { verger,6},
                    { fruitLegume,6}
                };

            state = new StateMachine();
            state = StateMachine.Menu;
        }

        public void RunGame()
        {
            switch (state)
            {
                case StateMachine.Menu:
                    Menu();//DEMANDER LE NB JOUEUR ET LES NOMS
                    state = StateMachine.Game;
                    break;

                case StateMachine.Game:
                    Game();//PARTIE
                    state = StateMachine.End;
                    break;

                case StateMachine.End:
                    //RECAP DE LA PARTIE
                    break;
            }
        }

        public void Menu()
        {
            // Display the menu
            Console.WriteLine("MENU");
            // Display the buttons for the player to answer
            Console.WriteLine("How many players are there ? From 2 to 4");
            int nbPlayer = int.Parse(Console.ReadLine()); // TEMPORARY


            for (int i = 0; i < nbPlayer; i++)
            {
                Dictionary<Card, int> terrainStart = new Dictionary<Card, int>()
                {
                    { champDeBle,1},
                    { ferme,0},
                    { boulangerie,1},
                    { cafe,0},
                    { superette,0},
                    { foret,0},
                    { centreAffaires,0},
                    { chaineTele,0},
                    { stade,0},
                    { fromagerie,0},
                    { fabriqueMeuble,0},
                    { mine,0},
                    { restaurant,0},
                    { verger,0},
                    { fruitLegume,0}
                };
                List<Monument> monuments = new List<Monument>()
                {
                    new Monument ("Centre commercial", 10, 1, new List<TypeCard>(){TypeCard.restauration, TypeCard.magasin}, ActionMonument.BonusLootParType, ConditionMonument.toujours),
                    new Monument ("Gare", 4, 0, new List<TypeCard>() ,ActionMonument.AjoutDé,ConditionMonument.toujours),
                    new Monument ("Parc d'attractions", 16, 0, new List<TypeCard>() ,ActionMonument.RejouerTour,ConditionMonument.déDouble),
                    new Monument ("Tour radio", 22, 0, new List<TypeCard>() ,ActionMonument.RelanceDé,ConditionMonument.uneFoisParTour)
                };
                string name = "player " + (i + 1); // --> default name for each player "player 1" ect..
                Player player = new Player(name, 3, monuments, terrainStart);
                _players.Add(player);
                foreach (Card establishment in terrainStart.Keys) // all the cards given to the player are removed from the deck
                {
                    _deck[establishment] -= terrainStart[establishment];
                }
            }

            Console.WriteLine("Les cartes ont été distribuées, on peut commencer la partie");
        }

        public void DisplayChoice(Player currentPlayer)
        {
            Console.WriteLine("\nESTABLISHEMENT");
            foreach (Card card in _deck.Keys)                                         // show all the establishments
            {
                if (_deck[card] > 0)
                {
                    string text = card._name + " x " + _deck[card] + " |";
                    for (int l = 0; l < card._rollToActivate.Count; l++)
                    {
                        text += card._rollToActivate[l] + " ";
                    }
                    text += "|  (" + card._cost + " coins) " + (currentPlayer._coins >= card._cost).ToString();
                    Console.WriteLine(text);
                }
            }
            Console.WriteLine("\nMONUMENTS");
            foreach (Monument monument in currentPlayer._monuments)                 // show all the monuments
            {
                if (!monument._built)
                {
                    Console.WriteLine("{0} ({1} coins) {2}", monument._name, monument._cost, currentPlayer._coins >= monument._cost);
                }
            }
            Console.WriteLine("");
        }
        public void Establishment(string answer, Player currentPlayer)
        {
            Console.WriteLine("Write the name of the establishment");
            foreach (Card card in _deck.Keys) //The player chose an establishment
            {
                if (card._name == answer)
                {
                    _deck[card]--;
                    currentPlayer._coins -= card._cost;
                    currentPlayer._terrain[card]++;
                    Console.WriteLine("{0} has bought the {1}", currentPlayer._name, answer);
                    break;
                }
            }
        }
        public void Monument(string answer, Player currentPlayer)
        {
            foreach (Monument monument in currentPlayer._monuments) // The player chose a monument
            {
                if (monument._name == answer)
                {
                    int index = currentPlayer._monuments.FindIndex(monum => monum._name == answer);
                    currentPlayer._monuments[index].StartBuild(currentPlayer, monument._cost);
                }
            }
        }
        public void Game()
        {
            Console.WriteLine("\nGAME \n");
            // show a button to stop ? -> back to menu
            bool end = false;
            while (!end)     // while nobody won
            {
                for (int i = 0; i < _players.Count; i++) // all players in order
                {
                    bool turnWillRepeat = true;
                    while (turnWillRepeat)
                    {
                        Console.Clear();
                        Player currentPlayer = _players[i];
                        Console.WriteLine("-----------------");
                        Console.WriteLine("{0}'s Turn", currentPlayer._name);                         //turn the table
                        Console.WriteLine("-----------------");
                        int roll = currentPlayer.ThrowDie();
                        Console.WriteLine("\nDICE\n{0} rolled a {1}", currentPlayer._name, roll);             //show roll
                        Console.WriteLine("\nEFFECTS");
                        _effectManager.Effect(roll, currentPlayer, _players); // I don't have the effectManager yet, but all I got to do is call it
                                                                              // PROBLEM : I don't have any idea how to display anything
                        Console.WriteLine("\nYOUR ESTABLISHMENTS :");
                        foreach (KeyValuePair<Card, int> elementDico in currentPlayer._terrain)
                        { if (elementDico.Value > 0) { Console.WriteLine("{0} x{1}", elementDico.Key._name, elementDico.Value); } }


                        bool choice = false;
                        DisplayChoice(currentPlayer);
                        Console.WriteLine("\nYOU HAVE {0} COINS\n", currentPlayer._coins);
                        while (!choice) // while the player hasn't bought anything
                        {
                            Console.WriteLine("YOUR CHOICE\nWhat do you want to buy {0} ?", currentPlayer._name);
                            // button buy (???)
                            Console.WriteLine("Do you wanna buy an establishment or a monument or nothing ? (e/m/n)");

                            string answer = Console.ReadLine(); //saisie clavier des enfers
                            if (answer == "n")
                            {
                                choice = true;
                            }
                            else if (answer == "e")
                            {
                                Establishment(answer, currentPlayer);
                                choice = true;
                            }
                            else if (answer == "m")
                            {
                                Monument(answer, currentPlayer);
                                choice = true;
                            }
                        }
                        //Ask monuments if turn will repeat
                        turnWillRepeat = false;
                        foreach (Monument mo in currentPlayer._monuments)
                        {
                            if (!turnWillRepeat) { turnWillRepeat = mo.CheckIfTurnRepeat(); }
                        }

                        // check wether the player has all their monuments or not
                        if (currentPlayer.Win())
                        {
                            end = true;
                            winner = currentPlayer._name;
                            turnWillRepeat = false;
                        }

                        //reset all the "only once per turn" effect of all his monuments
                        foreach (Monument mo in currentPlayer._monuments)
                        {
                            mo.DebutTour();
                        }
                    }
                }
            }
        }
        public void End()
        {
            Console.WriteLine("Congratulation to {0}", winner);
        }
    }
}
