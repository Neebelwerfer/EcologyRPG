using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public abstract class NPCBehaviour : ScriptableObject
{
    public float UpdatePerSecond;

    State currState;

    float timeSinceLastUpdate = 0f;

    public void ChangeState(EnemyNPC character, State newState)
    {
        currState?.OnExit(character);
        currState = newState;
        currState.OnEnter(character);
    }

    public abstract void Init(EnemyNPC character);

    public void UpdateBehaviour(EnemyNPC character)
    {
        currState.Update(character);
    }
}

public class State
{
    public string Name;
    DecisionTree DecisionTree;
    Action<EnemyNPC> OnEnterAction;
    Action<EnemyNPC> OnExitAction;

    public State(string name)
    {
        Name = name;
    }

    public virtual void OnEnter(EnemyNPC character)
    {
        OnEnterAction?.Invoke(character);
    }

    public virtual void OnExit(EnemyNPC character)
    {
        OnExitAction?.Invoke(character);
    }

    public void SetOnEnterAction(Action<EnemyNPC> action)
    {
        OnEnterAction = action;
    }

    public void SetOnExitAction(Action<EnemyNPC> action)
    {
        OnExitAction = action;
    }

    public void SetDecisionTree(DecisionTree decisionTree)
    {
        DecisionTree = decisionTree;
    }

    public void Update(EnemyNPC character)
    {
        DecisionTree.Traverse(character);
    }
}

public class DecisionTree
{
    Node RootNode;

    public void SetRootNode(Node rootNode)
    {
        RootNode = rootNode;
    }

    public void Traverse(EnemyNPC character)
    {
        RootNode.Traverse(character);
    }
}

public abstract class Node
{
    public abstract void Traverse(EnemyNPC character);
}

public class ActionNode : Node
{
    Action<EnemyNPC> Action;

    public ActionNode(Action<EnemyNPC> action)
    {
        Action = action;
    }

    public override void Traverse(EnemyNPC character)
    {
        Action.Invoke(character);
    }
}

public class DecisionNode : Node
{
    Node TrueNode;
    Node FalseNode;

    Predicate<EnemyNPC> Condition;

    public DecisionNode(Predicate<EnemyNPC> condition, Node trueNode, Node falseNode)
    {
        Condition = condition;
        TrueNode = trueNode;
        FalseNode = falseNode;
    }

    public override void Traverse(EnemyNPC character)
    {
        if (Condition.Invoke(character))
        {
            TrueNode.Traverse(character);
        } 
        else
        {
            FalseNode.Traverse(character);
        }
    }
}