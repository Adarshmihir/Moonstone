using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
    [System.Serializable]
    public class Condition
    {
        [SerializeField] private Disjunction[] and;
        
        public bool CheckCondition(IEnumerable<IEvaluator> evaluators)
        {
            return and.All(dis => dis.CheckCondition(evaluators));
        }
        
        [System.Serializable]
        private class Disjunction
        {
            [SerializeField] private Predicate[] or;

            public bool CheckCondition(IEnumerable<IEvaluator> evaluators)
            {
                return or.Any(predicate => predicate.CheckCondition(evaluators));
            }
        }
        
        [System.Serializable]
        private class Predicate
        {
            [SerializeField] private string name;
            [SerializeField] private string[] parameters;
            [SerializeField] private bool negate;

            public bool CheckCondition(IEnumerable<IEvaluator> evaluators)
            {
                return evaluators.Select(evaluator => evaluator.Evaluate(name, parameters)).All(result => result != negate);
            }
        }
    }
}
