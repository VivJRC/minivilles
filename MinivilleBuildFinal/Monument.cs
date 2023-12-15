using System;
using System.Collections.Generic;
using System.Linq;
//using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using MinivilleBuildFinal.Enum;

namespace MinivilleBuildFinal
{
    public enum ActionMonument
    {
        AjoutDé,
        RejouerTour,
        BonusLootParType,
        RelanceDé,
        none
    }
    public enum ConditionMonument
    {
        toujours,
        uneFoisParTour,
        déDouble,
        jamais
    }
    internal class Monument
    {
        public bool _built;
        public string _name;
        public int _cost;
        public ActionMonument _action;
        public ConditionMonument _condition;
        public int _quantity;
        public List<TypeCard> _multiplicateurType;
        bool _usedThisTurn = false;
        bool _thisTurnShouldRepeat = false;

        public Monument(string name, int cost, int quantity, List<TypeCard> multiplicateurType, ActionMonument action, ConditionMonument condition)
        {
            _built = false;
            _name = name;
            _cost = cost;
            _quantity = quantity;
            _multiplicateurType = multiplicateurType;
            _action = action;
            _condition = condition;
        }

        private bool ConditionWork(List<int> roll)  //MODULARITE DES CONDITIONS
        {
            switch (_condition)
            {
                case ConditionMonument.toujours: return true;
                case ConditionMonument.jamais: return false;
                case ConditionMonument.uneFoisParTour:
                    if (!this._usedThisTurn)
                    {
                        _usedThisTurn = true;
                        return true;
                    }
                    else { return false; }
                case ConditionMonument.déDouble:
                    if (roll.Count == 2)
                    {
                        if (roll[0] == roll[1]) { return true; }
                    }
                    return false;
            }
            return false;
        }
        public void StartBuild(Player Owner, int cost)  //CONSTRUIT ET GERE L'ACTION AjoutDé
        {
            Owner._coins -= cost;
            Console.WriteLine("{0} has bought the {1}", Owner._name, this._name);
            _built = true;
            if (this._action == ActionMonument.AjoutDé) { Owner._dice.Add(new Die()); }
        }
        public void DebutTour() //RESET LA CONDITION uneFoisParTour
        {
            if (_condition == ConditionMonument.uneFoisParTour) { _usedThisTurn = false; }
        }

        public int CheckIfBonus(int amount, TypeCard typeDeLaCarte, List<int> rollResults)  //ACTION BonusLootParType
        {
            if (this._built)
            {
                if (this._action == ActionMonument.BonusLootParType && this.ConditionWork(rollResults))
                {
                    if (this._multiplicateurType.Contains(typeDeLaCarte)) { amount += this._quantity; }
                }
            }
            return amount;
        }
        public bool CanResetDiceThrow(List<int> diceResult, int rollResult) //ACTION RelanceDé
        {
            bool toReturn = false;
            if (this._built && this._action == ActionMonument.RelanceDé && this.ConditionWork(diceResult))
            {
                string anwser;
                while (true)
                {
                    Console.WriteLine("Vous venez de faire {0}. Voulez-vous relancer les dés ?\noui -> o\nnon -> n", rollResult);
                    anwser = Console.ReadLine();
                    if (anwser == "o") { toReturn = true; break; }
                    else if (anwser == "n") { toReturn = false; break; }
                }
            }
            return toReturn;
        }

        public void CheckIfTurnWillRepeat(List<int> diceResult) //ACTION RejouerTour
        {
            _thisTurnShouldRepeat = false;
            if (this._built && this._action == ActionMonument.RejouerTour)
            {
                if (this.ConditionWork(diceResult))
                {
                    Console.WriteLine("Le tour va se répéter grâce à {0}", this._name); _thisTurnShouldRepeat = true;
                }
            }
        }
        public bool CheckIfTurnRepeat() //GERE L'ACTION RejouerTour
        {
            if (_thisTurnShouldRepeat) { _thisTurnShouldRepeat = false; return true; }
            else { return false; }
        }
    }
}
