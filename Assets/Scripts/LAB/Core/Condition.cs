using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [System.Serializable]
    public class Condition
    {
        [SerializeField] private string name;
        [SerializeField] private string[] parameters;

        public bool CheckCondition(IEnumerable<IEvaluator> evaluators)
        {
            foreach (var evaluator in evaluators)
            {
                var result = evaluator.Evaluate(name, parameters);
                
                switch (result)
                {
                    case null:
                        continue;
                    case false:
                        return false;
                }
            }
            return true;
        }
    }
}
