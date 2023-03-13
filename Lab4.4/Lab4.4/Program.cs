using System;

public delegate void ActionEventHandler(object sender, EventArgs e);

public class Workflow
{
    public event ActionEventHandler ActionStarted;
    public event ActionEventHandler ActionCompleted;
    public event EventHandler WorkflowCompleted;

    private Action[] actions;
    private int currentActionIndex;

    public Workflow(Action[] actions)
    {
        this.actions = actions;
        currentActionIndex = 0;
    }

    public void Run()
    {
        ExecuteAction(actions[currentActionIndex]);
    }

    private void Execute;
    private void ExecuteAction(Action action)
    {
        OnActionStarted(action);
        action.Execute();

        OnActionCompleted(action);

        currentActionIndex++;

        if (currentActionIndex < actions.Length)
        {
            ExecuteAction(actions[currentActionIndex]);
        }
        else
        {
            OnWorkflowCompleted();
        }
    }

    protected virtual void OnActionStarted(Action action)
    {
        ActionStarted?.Invoke(action, EventArgs.Empty);
    }

    protected virtual void OnActionCompleted(Action action)
    {
        ActionCompleted?.Invoke(action, EventArgs.Empty);
    }

    protected virtual void OnWorkflowCompleted()
    {
        WorkflowCompleted?.Invoke(this, EventArgs.Empty);
    }
}

public abstract class Action
{
    public string Name { get; set; }
    public abstract void Execute();
}

public class StartAction : Action
{
    public override void Execute()
    {
        Console.WriteLine("Starting workflow...");
    }
}

public class EndAction : Action
{
    public override void Execute()
    {
        Console.WriteLine("Workflow completed.");
    }
}

public class IntermediateAction : Action
{
    public override void Execute()
    {
        Console.WriteLine($"Executing action '{Name}'...");
    }
}

public class Program
{
    public static void Main()
    {
        var startAction = new StartAction();
        var intermediateAction1 = new IntermediateAction { Name = "Action 1" };
        var intermediateAction2 = new IntermediateAction { Name = "Action 2" };
        var endAction = new EndAction();
        var actions = new Action[] { startAction, intermediateAction1, intermediateAction2, endAction };

        var workflow = new Workflow(actions);

        workflow.ActionStarted += (sender, e) =>
        {
            Console.WriteLine($"Starting action '{((Action)sender).Name}'...");
        };

        workflow.ActionCompleted += (sender, e) =>
        {
            Console.WriteLine($"Completed action '{((Action)sender).Name}'.");
        };

        workflow.WorkflowCompleted += (sender, e) =>
        {
            Console.WriteLine("Workflow completed successfully.");
        };

        workflow.Run();

        Console.ReadLine();
    }
}