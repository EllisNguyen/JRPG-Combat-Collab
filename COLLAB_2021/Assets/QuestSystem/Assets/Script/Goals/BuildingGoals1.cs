using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System.Linq;



    public class BuildingGoals1 : Quest.QuestGoal

    {
        public string Building;
        public override string GetDescription()
        {
            return $"Do a {Building}";
        }
        public override void Initilize()
        {
            base.Initilize();

        }
        private void OnBuilding()
        {
            CurrentAmount++;
            Evaluate();
        }

    }
    
    


