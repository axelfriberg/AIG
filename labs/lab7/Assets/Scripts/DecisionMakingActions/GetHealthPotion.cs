using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using System;
using UnityEngine;

namespace Assets.Scripts.DecisionMakingActions
{
    public class GetHealthPotion : WalkToTargetAndExecuteAction
    {
        
        public GetHealthPotion(AutonomousCharacter character, GameObject target) : base("GetHealthPotion",character,target)
        {
            
        }

        public override bool CanExecute()
        {
            if (!base.CanExecute())
                return false;
            else
                return true;
        }

        public override bool CanExecute(WorldModel worldModel)
        {
            if (!base.CanExecute(worldModel))
                return false;
            else
                return true;
        }

        public override void Execute()
        {
            base.Execute();
            this.Character.GameManager.GetHealthPotion(this.Target);
        }

        public override void ApplyActionEffects(WorldModel worldModel)
        {
            base.ApplyActionEffects(worldModel);
            worldModel.SetProperty(Properties.HP, this.Character.GameManager.characterData.MaxHP);
            //disables the target object so that it can't be reused again
            worldModel.SetProperty(this.Target.name, false);
        }

        public override int getHvalue(WorldModel state) {
            int maxHP = this.Character.GameManager.characterData.MaxHP;
            int currentHP = this.Character.GameManager.characterData.HP;
            if (currentHP > 0.8 * maxHP) {
                return 8;
            } else if (currentHP > 0.5 * maxHP) {
                return 5;
            } else if (currentHP > 0.3 * maxHP) {
                return 3;
            } else {
                return 1;
            }
        }
    }
}
