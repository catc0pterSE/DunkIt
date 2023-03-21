using UnityEngine;

namespace z_Test
{
    public class Switcher : MonoBehaviour
    {
        /*[SerializeField] private GameObject obj;

        private void OnEnable()
        {
            StartCoroutine(TurnOffTurnOn());
        }

        private IEnumerator TurnOffTurnOn()
        {
            yield return new WaitForSeconds(2);
            obj.SetActive(false);
            yield return new WaitForSeconds(1);
            obj.SetActive(true);
        }*/
    }

    /*public class Player : IVisitable
    {
        private IResist[] _resists;

        public void Accept(IVisitor visitor)
        {
            _resists.Map(resist => resist.Accept(visitor));
        }
    }

    public interface IVisitable
    {
        public void Accept(IVisitor visitor);
    }
    
    public interface IResist : IVisitable
    {
    }

    public class FireResist : IResist
    {
        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class WaterResist : IResist
    {
        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }


    public interface IVisitor
    {
        public void Visit(FireResist fireResist);
        public void Visit(WaterResist waterResist);
    }

    public class FireResistVisitor : IVisitor
    {
        public void Visit(FireResist fireResist)
        {
            // do something
        }

        public void Visit(WaterResist waterResist)
        {
        }
    }

    public class WaterResistVisitor : IVisitor
    {
        public void Visit(FireResist fireResist)
        {
        }

        public void Visit(WaterResist waterResist)
        {
            // do something
        }
    }*/


    /*public class Water
    {
        public IWaterState CurrentWaterState { get; set; }

        public void Evaporate()
        {
            
        }
        
        public void Cool()
        {
            CurrentWaterState.Cool(this);
        }

        public void Heat()
        {
            CurrentWaterState.Heat(this);
        }
    }

    public interface IWaterState
    {
        public void Evaporate(Water water);
        
        public void Cool(Water water);
        public void Heat(Water water);
    }

    public class Ice : IWaterState
    {
        public void Evaporate(Water water)
        {
            
        }

        public void Cool(Water water)
        {
            
        }

        public void Heat(Water water)
        {
            water.CurrentWaterState = new Liquid();
        }
    }

    public class Steam: IWaterState
    {
        public void Evaporate(Water water)
        {
            
        }

        public void Cool(Water water)
        {
            water.CurrentWaterState = new Liquid();
        }

        public void Heat(Water water)
        {
            
        }
    }
    
    public class Liquid: IWaterState
    {
        public void Evaporate(Water water)
        {
            
        }

        public void Cool(Water water)
        {
            water.CurrentWaterState = new Ice();
        }

        public void Heat(Water water)
        {
            water.CurrentWaterState = new Steam();
        }
    }*/
}