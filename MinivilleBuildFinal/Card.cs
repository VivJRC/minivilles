using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinivilleBuildFinal.Enum;

namespace MinivilleBuildFinal
{

    internal class Card
    {
        public string _name;
        public int _cost;
        public List<int> _rollToActivate;
        public TypeCard _type;
        public ColorCard _color;
        public List<Propertie> _properties;
        int _quantite;
        TypeCard _multiplicateurType;
        

        public Card(string name, int cost, List<int> roll, TypeCard type, ColorCard color, int quantite, TypeCard multiplicateurType , List<Propertie> properties)
        {
            _name = name;
            _cost = cost;
            _rollToActivate = roll;
            _type = type;
            _color = color;
            _quantite = quantite;
            _multiplicateurType = multiplicateurType;
            _properties = properties;
        }
        public Card(string name, int cost, List<int> roll, TypeCard type, ColorCard color, int quantite, TypeCard multiplicateurType)
        {
            _name = name;
            _cost = cost;
            _rollToActivate = roll;
            _type = type;
            _color = color;
            _quantite = quantite;
            _multiplicateurType = multiplicateurType;
            _properties = new List<Propertie>();
        }
        public Card(string name, int cost, List<int> roll, TypeCard type, ColorCard color, int quantite)
        {
            _name = name;
            _cost = cost;
            _rollToActivate = roll;
            _type = type;
            _color = color;
            _quantite = quantite;
            _multiplicateurType = TypeCard.aucun;
            _properties = new List<Propertie>();
        }
        private void ExchangeEstablishments(List<Player> players, Player Owner)
        {
            //Find Current Player's number
            bool numPlayerFound = false;
            int ownerPlayerNumber = 0;
            while (!numPlayerFound)
            {
                if (players[ownerPlayerNumber] == Owner) numPlayerFound = true;
                else ownerPlayerNumber++;
            }

            //Show Players
            Console.WriteLine("\nLISTE DES JOUEURS");
            foreach (Player p in players)
            {
                if (!(p._name == Owner._name)) { Console.WriteLine(p._name); }
            }

            //Select Player
            bool thatAnwserWorks = false;
            string nomPlayer;
            int numPlayerTargeted = 0;
            while (true)
            {
                Console.WriteLine("\nQUESTION\nAvec qui veux-tu échanger un de tes bâtiments ?");
                nomPlayer = Console.ReadLine();
                for (int i = 0; i < players.Count; i++)
                {
                    Player player = players[i];
                    if (player._name == nomPlayer && !(player._name == Owner._name)) { thatAnwserWorks = true; numPlayerTargeted = i; }
                }
                if (thatAnwserWorks) { break; }
            }

            //SHOW OWNER'S ESTABLISHMENTS
            List<string> batsOwner = new List<string>();
            Console.WriteLine("\nYOUR ESTABLISHMENTS :");
            foreach (KeyValuePair<Card, int> elementDico in Owner._terrain)
            {if (elementDico.Value > 0) { Console.WriteLine("{0} x{1}", elementDico.Key._name, elementDico.Value); batsOwner.Add(elementDico.Key._name); } }

            //SHOW TARGETED PLAYER'S ESTABLISHMENTS
            List<string> batsTargetedGuy = new List<string>();
            Console.WriteLine("\nTARGETED PLAYER'S ESTABLISHMENTS :");
            foreach (KeyValuePair<Card, int> elementDico in players[numPlayerTargeted]._terrain)
            { if(elementDico.Value>0) { Console.WriteLine("{0} x{1}", elementDico.Key._name, elementDico.Value); batsTargetedGuy.Add(elementDico.Key._name); } }

            //Owner choose wich establishment he's giving away
            thatAnwserWorks = false;
            string establishmentToGive;
            thatAnwserWorks = false;
            while (true)
            {
                Console.WriteLine("\nQUESTION\nQuel établissement veux-tu lui donner ?");
                establishmentToGive = Console.ReadLine();
                if (batsOwner.Contains(establishmentToGive)){thatAnwserWorks = true;}
                if (thatAnwserWorks) { break; }
            }

            //Owner choose wich establishment he's going to take from him
            thatAnwserWorks = false;
            string establishmentToGet;
            thatAnwserWorks = false;
            while (true)
            {
                Console.WriteLine("\nQUESTION\nQuel établissement veux-tu lui prendre ?");
                establishmentToGet = Console.ReadLine();
                if (batsTargetedGuy.Contains(establishmentToGet)) { thatAnwserWorks = true; }
                if (thatAnwserWorks) { break; }
            }

            //Procced to exchange these thingies :3
            foreach (Card card in Owner._terrain.Keys) //The Owner Get His New thingie
            {
                if (card._name == establishmentToGet) { Owner._terrain[card]++; players[numPlayerTargeted]._terrain[card]--;  }
            }
            foreach (Card card in Owner._terrain.Keys) //The player chose an establishment
            {
                if (card._name == establishmentToGive) { Owner._terrain[card]--; players[numPlayerTargeted]._terrain[card]++; }
            }

            //RESUME
            Console.WriteLine("\nWHAT HAPPENED");
            Console.WriteLine("{0} gave the {1} to {2}, and {2} gave him the {3} in return.", Owner._name, establishmentToGive, players[numPlayerTargeted], establishmentToGet);
        }
        private void StealFromEveryone(int quantity, List<Player> players, Player Owner)
        {
            //Find Current Player's number
            bool numPlayerFound = false;
            int ownerPlayerNumber = 0;
            while (!numPlayerFound)
            {
                if (players[ownerPlayerNumber] == Owner) numPlayerFound = true;
                else ownerPlayerNumber++;
            }

            //Begin to steal
            int i = ownerPlayerNumber;
            for (int index = 0; index < players.Count; index++)
            {
                if (!(players[i] == Owner))
                {
                    if (players[i]._coins < quantity)
                    {
                        Console.WriteLine("{0} obtains {1} coins from {2}, using his {3}.", Owner._name, players[i]._coins, players[i]._name, this._name);
                        Owner._coins += players[i]._coins;
                        players[i]._coins = 0;
                    }
                    else
                    {
                        Console.WriteLine("{0} obtains {1} coins from {2}, using his {3}.", Owner._name, quantity, players[i]._name, this._name);
                        Owner._coins += quantity;
                        players[i]._coins -= quantity;
                    }
                }
                if (i == 0) { i = players.Count - 1; }
                else { i--; }
            }
        }
        private void StealPlayer(int quantity, List<Player> players, Player Owner)
        {
            int numPlayerTargeted = 0;
            string nomPlayer;
            bool thatAnwserWorks = false;
            while (true)
            {
                Console.WriteLine("A qui veux-tu voler {0} pièces ?", quantity);
                nomPlayer = Console.ReadLine();
                for (int i = 0;  i < players.Count; i++)
                {
                    Player player = players[i];
                    if (player._name == nomPlayer && !(player._name == Owner._name)) { thatAnwserWorks = true; numPlayerTargeted = i; }
                }
                if(thatAnwserWorks) { break; }
            }

            if (players[numPlayerTargeted]._coins < quantity)
            {
                Console.WriteLine("{0} obtains {1} coins from {2}, using his {3}.", Owner._name, players[numPlayerTargeted]._coins, players[numPlayerTargeted]._name, this._name);
                Owner._coins += players[numPlayerTargeted]._coins;
                players[numPlayerTargeted]._coins = 0;
            }
            else
            {
                Console.WriteLine("{0} obtains {1} coins from {2}, using his {3}.", Owner._name, quantity, players[numPlayerTargeted]._name, this._name);
                Owner._coins += quantity;
                players[numPlayerTargeted]._coins -= quantity;
            }
        }
        public List<int> Effect(Player currentPlayer, Player Owner, List<Player> players)
        {
            //Console.WriteLine("{0} used the effect of his {1} during {2}'s turn.", Owner._name, this._name, currentPlayer._name);
            int argentGagne = 0;
            switch (this._color)
            {
                case ColorCard.green:
                    int compteurType = 0;
                    if (_multiplicateurType == TypeCard.aucun) { compteurType = 1; }
                    {
                        foreach (var kvp in Owner._terrain)
                        {
                            Card key = kvp.Key;
                            int value = kvp.Value;
                            if (key._type == _multiplicateurType)
                            {
                                compteurType += kvp.Value;
                            }
                        }
                    }
                    argentGagne = 0;
                    argentGagne += this._quantite * compteurType;
                    for (int i = 0; i < Owner._monuments.Count; i++) { argentGagne = Owner._monuments[i].CheckIfBonus(argentGagne, this._type, currentPlayer._currentThrow); }
                    Owner._coins += argentGagne;
                    Console.WriteLine("{0} obtains {1} coins, using his {2}.", Owner._name, argentGagne, this._name);
                    break;

                case ColorCard.blue:
                    argentGagne = 0;
                    argentGagne += this._quantite;
                    for (int i = 0; i < Owner._monuments.Count; i++) { argentGagne = Owner._monuments[i].CheckIfBonus(argentGagne, this._type, currentPlayer._currentThrow); }
                    Owner._coins += argentGagne;
                    Console.WriteLine("{0} obtains {1} coins, using his {2}.", Owner._name, argentGagne, this._name);
                    break;

                case ColorCard.red:
                    argentGagne = 0;
                    argentGagne += this._quantite;
                    for (int i = 0; i < Owner._monuments.Count; i++) { argentGagne = Owner._monuments[i].CheckIfBonus(argentGagne, this._type, currentPlayer._currentThrow); }
                    if (currentPlayer._coins < argentGagne)
                    {
                        Console.WriteLine("{0} obtains {1} coins from {2}, using his {3}.", Owner._name, currentPlayer._coins, currentPlayer._name, this._name);
                        argentGagne = currentPlayer._coins;
                        Owner._coins += currentPlayer._coins;
                        currentPlayer._coins = 0;
                    }
                    else
                    {
                        Console.WriteLine("{0} obtains {1} coins from {2}, using his {3}.", Owner._name, argentGagne, currentPlayer._name, this._name);
                        Owner._coins += argentGagne;
                        currentPlayer._coins -= argentGagne;
                    }
                    break;

                case ColorCard.purple:
                    switch (this._properties[0])
                    {
                        case Propertie.Exchangeable:
                            this.ExchangeEstablishments(players, Owner);
                            break;

                        case Propertie.Targetable:
                            argentGagne = 0;
                            argentGagne += this._quantite;
                            for (int i = 0; i < Owner._monuments.Count; i++) { argentGagne = Owner._monuments[i].CheckIfBonus(argentGagne, this._type, currentPlayer._currentThrow); }
                            this.StealPlayer(argentGagne, players, Owner);
                            break;

                        case Propertie.FullTarget:
                            argentGagne = 0;
                            argentGagne += this._quantite;
                            for (int i = 0; i < Owner._monuments.Count; i++) { argentGagne = Owner._monuments[i].CheckIfBonus(argentGagne, this._type, currentPlayer._currentThrow); }
                            this.StealFromEveryone(argentGagne, players, Owner);
                            break;
                    }
                    break;
            }
            //Find Owner's number
            bool numPlayerFound = false;
            int ownerPlayerNumber = 0;
            while (!numPlayerFound)
            {
                if (players[ownerPlayerNumber] == Owner) numPlayerFound = true;
                else ownerPlayerNumber++;
            }

            //Find Card's Index
            int indexC = -1;
            int indexCard = -1;
            foreach (Card card in Owner._terrain.Keys)
            {
                indexC += 1;
                if (card._name == this._name)
                { 
                    indexCard = indexC;
                    break;
                }
            }

            List<int> toReturn = new List<int>() { ownerPlayerNumber, indexCard, argentGagne, 3};
            return toReturn;
        }
    }
}