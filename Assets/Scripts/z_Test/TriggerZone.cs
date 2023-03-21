using System;
using Modules.MonoBehaviour;
using UnityEngine;

namespace z_Test
{
    public class TriggerZone : SwitchableMonoBehaviour
    {
    }

    public class BuildingProgress
    {
        private Upgrade[] _upgrades;

        public void Calculate(int currentBonus)
        {
            var bonus = currentBonus;
            foreach (Upgrade upgrade in _upgrades)
            {
                bonus = (int)upgrade.CalculateGain(bonus);
            }
        }
    }

    public abstract class Upgrade
    {
        public abstract float CalculateGain(int currentGain);
        public abstract void Buy();
    }

    public class SimpleUpgrade : Upgrade
    {
        private readonly int _multiplier;

        private bool _bought;

        public SimpleUpgrade(int multiplier)
        {
            _multiplier = multiplier;
        }

        public override float CalculateGain(int currentGain) => _bought ? _multiplier * currentGain : 0;

        public override void Buy() =>
            _bought = true;
    }

    public class BuildingDependedUpgrade : Upgrade
    {
        private readonly BuildingMaster _buildingMaster;
        private readonly float _gainMultiplier;
        private int _enhanceMultiplier;
        private bool _bought;

        public BuildingDependedUpgrade(BuildingMaster buildingMaster, float gainMultiplier)
        {
            _buildingMaster = buildingMaster;
            _gainMultiplier = gainMultiplier;
            _enhanceMultiplier = 1;
        }

        public override float CalculateGain(int currentGain) => _bought
            ? currentGain + _buildingMaster.BuildingsNumber * _gainMultiplier * _enhanceMultiplier
            : 0;

        public override void Buy() =>
            _bought = true;

        public void MultiplyEnhanceMultiplier(int multiplier)
        {
            _enhanceMultiplier *= _enhanceMultiplier;
        }
    }

    public class Enhancer : Upgrade
    {
        private readonly BuildingDependedUpgrade _buildingDependedUpgrade;
        private readonly int _multiplier;

        public Enhancer(BuildingDependedUpgrade buildingDependedUpgrade, int multiplier)
        {
            _buildingDependedUpgrade = buildingDependedUpgrade;
            _multiplier = multiplier;
        }

        public override float CalculateGain(int currentGain) =>
            currentGain;

        public override void Buy() =>
            _buildingDependedUpgrade.MultiplyEnhanceMultiplier(_multiplier);
    }

    public class BuildingMaster
    {
        private Building[] _buildings;

        public int BuildingsNumber => _buildings.Length;
    }

    public class Building
    {
    }
}