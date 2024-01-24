using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AIAction
{
    protected AIContext _context;
    public AIContext Context => _context;

    List<Consideration> _considerations = new();


    public AIAction() {}

    public AIAction(AIAction action, AIContext context)
    {
        _considerations = action._considerations;
        _context = context;
    }

    public void AddConsideration(Consideration consideration)
    {
        _considerations.Add(consideration);
    }

    public float Score(Combat combat)
    {
        float score = 1f;
        float compensationFactor = 1.0f - (1.0f / _considerations.Count);

        #if DEBUG_AI
            handler.DebugStr += $"{GetType().Name} : {_context}\n";
        #endif
        foreach (var consideration in _considerations)
        {
            float considerationScore = consideration.Score(combat, _context);

            #if DEBUG_AI
                handler.DebugStr += $"\t {consideration.GetType().Name}: {considerationScore}\n";
            #endif

            if (considerationScore == 0)
                return 0;

            float modification = (1.0f - considerationScore) * compensationFactor;
            considerationScore = considerationScore + (modification * considerationScore);
            score *= considerationScore;
        }
        #if DEBUG_AI
            handler.DebugStr += "\n";
        #endif

        return score;
    }

    public abstract IEnumerable<AIAction> GenerateConcreteActions(Combat combat);
}
