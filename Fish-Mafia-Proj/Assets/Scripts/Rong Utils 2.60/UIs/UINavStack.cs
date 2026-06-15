using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xrofng;

public class UINavStack : Singleton<UINavStack>
{
    private Stack<IStackableView> stack = new Stack<IStackableView>();

    public void NotifyShow(IStackableView view)
    {
        if (stack.Count > 0 && stack.Peek() == view)
            return;

        stack.Push(view);
    }

    public void Pop()
    {
        if (stack.Count == 0)
            return;

        IStackableView popped = stack.Pop();
        popped.Hide();

        if (stack.Count > 0)
            stack.Peek().Show();
    }

    public bool HasView => stack.Count > 0;
    public bool IsAtRoot => stack.Count == 1;
    public int StackCount => stack.Count;

    public void Clear()
    {
        while (stack.Count > 0)
        {
            stack.Pop().Hide();
        }
    }
}