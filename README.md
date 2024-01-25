# Fluxity

<img src="https://img.shields.io/badge/unity-2021.3-green.svg?style=flat-square" alt="unity 2021.3">

AIR's Flux pattern implementation for Unity.

## What is it and why should I use it?

Fluxity is a Unity-based implementation of the Flux pattern proposed by Facebook/Meta for SPAs, in the form of
a single-*scene* application.

Written in C#, it is intended as an alternative to the MVC pattern; a clean solution to sharing state between
all the different view types that can exist in a Unity application (UI, 2D & 3D scene objects, etc.)

### Core concept

Simplicity and speed in setting up and binding together a Flux architecture in your scene, and one that can have additional state, effects, and/or presenters added quickly and easily.

## Differences from Standard Pattern

Some adjustments have made to the basic concepts and names to fit it into the Unity environment and C# language.
The table below specifies any changes to the basic concept names with following explanations:

| Flux Pattern      | Fluxity Equivalent    |
| -----------       | -----------           |
| Store             | Store                 |
| Dispatcher        | Dispatcher            |
| Action            | Command               |
| Controller-View   | FeatureObserver / FeatureObserverAggregate |
| View              | -                     |

### Commands

Flux pattern Actions are renamed to be Commands in Fluxity to make a cleaner distinction from C# Actions, and to emphasize
that they are only used via the Dispatcher.

### FeatureObserver

FeatureObserver allows for registering on state change or at will polling of the state of the feature. They are the backbone of creating `Display` type calls.

The FeatureObserverAggregate is designed for the special case where you want to route the callback from a number of Features all to the same `Action`.

### Views

Fluxity does not explicitly implement any Views, as the form the view takes is entirely dependent on the type of Unity
object being used; this hands-off approach to the pattern implementation allows Fluxity to be used with any Unity object or package that can be abstracted into a view.

## Dependencies

- [AIR.Flume](https://github.com/AnImaginedReality/Flume) - Unity dependency injection package (Git package).

## Installation / Minimum required Setup

The required library is distributed as a git package ([How to install package from git URL](https://docs.unity3d.com/Manual/upm-ui-giturl.html))

### Git URL

```
https://github.com/ActuatorDigital/Fluxity.git?path=Packages/Fluxity
```

The minimum required to install and use Fluxity in a scene is laid out in the following simple steps:

1. Ensure Flume package is installed via Package Manager
2. Install library via Package Manager -> Git URL
3. Create State, commands, effects, and reducers for that state
4. Create/open scene
5. Set up Flume service container/installer
6. Create a Fluxity initializer and add to scene
7. Create reducer/effect bindings in the Initializer
8. Register Fluxity and State feature with the Flume service installer

Any additional Flux content can be added simply after this:

1. Create state
2. Register state during RegisterFluxity in service installer
3. Create commands, effects, reducers
4. Create bindings for reducers/effects in the Initializer

### IL2CPP builds

Requires Flume and Fluxity need to be referenced in a link.xml file

```xml
<linker>
  <assembly fullname="AIR.Flume" preserve="all"></assembly>
  <assembly fullname="AIR.Fluxity" preserve="all"></assembly>
</linker>
```

## Code Example

In this example we'll give a quick show of code required to control the spinning of a 3D object from a UI via Fluxity.

<details>
<summary>Show code sample</summary>

### State used

```cs
public struct SpinState
{
    public bool DoSpin;
    public float DegreesPerSecond;
}
```

### Commands used

```cs
public class StartSpinCommand : ICommand
{
    public float DegreesPerSecond { get; set; }
}

public class StopSpinCommand : ICommand
{
}
```

### Reducers used

```cs
public static class SpinStateReducers
{
    public static SpinState StartSpin(SpinState state, StartSpinCommand command)
    {
        return new SpinState {
            DegreesPerSecond = command.DegreesPerSecond,
            DoSpin = true,
        };
    }

    public static SpinState StopSpin(SpinState state, StopSpinCommand command)
    {
        return new SpinState { DoSpin = false };
    }
}
```

### Fluxity Initializer

```cs
public class SpinExampleInitializer : FluxityInitializer
{
    public override void RegisterFluxity(FluxityRegisterContext context)
    {
        context
            .Feature(new SpinState())
                .Reducer<StartSpinCommand>(SpinnerReducers.StartSpin)
                .Reducer<StopSpinCommand>(SpinnerReducers.StopSpin)
            ;
    }

    protected override void RegisterServices(FlumeServiceContainer container)
    {
    }

    protected override void PostInitialize(IDispatcher dispatcher)
    {
        // Initial state that object starts out spinning.
        var command = new StartSpinCommand { DegreesPerSecond = 270f };
        dispatcher.Dispatch(command);
    }
}
```

### Spinning Object Presenter

```cs
public class SpinningObjectPresenter : MonoBehaviour
{
    [SerializeField] private SpinnerView uSpinnerView;

    private FeatureObserver<SpinState> _spinStateBinding;

    public override void CreateBindings()
    {
        _spinStateBinding = Bind<SpinState>();
    }

    public override void Display()
    {
        var currentState = _spinStateBinding.State;
        uSpinnerView.SetSpinRate(currentState.DegreesPerSecond);
        if (currentState.DoSpin)
            uSpinnerView.StartSpin();
        else
            uSpinnerView.StopSpin();
    }
}
```

### Spinning Object View

```cs
public class SpinnerView : MonoBehaviour
{
    private bool _isSpinning = false;
    private float _degreesPerSecond = 0;

    public void Update()
    {
        if (!_isSpinning)
            return;
        
        var rotationDelta = _degreesPerSecond * Time.deltaTime;
        transform.Rotate(0, rotationDelta, 0);
    }

    public void SetSpinRate(float degreesPerSecond) => _degreesPerSecond = degreesPerSecond;

    public void StartSpin() => _isSpinning = true;

    public void StopSpin() => _isSpinning = false;
}
```

### Button Views

```cs
public class StartSpinButtonView : MonoBehaviour
{
    [SerializeField] private Text uButtonText;
    [SerializeField] private Button uButton;
    [SerializeField] private float uDegreesPerSecond = 45f;

    private DispatcherHandle _dispatcherHandle;

    public void Start()
    {
        _dispatcherHandle = new DispatcherHandle();
        uButtonText.text = "Start";
        uButton.onClick.AddListener(OnClick);
    }

    public void OnDestroy() => uButton.onClick.RemoveListener(OnClick);

    private void OnClick()
    {
        var command = new StartSpinCommand { DegreesPerSecond = uDegreesPerSecond };
        _dispatcherHandle.Dispatch(command);
    }
}

public class StopSpinButtonView : MonoBehaviour
{
    [SerializeField] private Text uButtonText;
    [SerializeField] private Button uButton;

    private DispatcherHandle _dispatcherHandle;

    public void Start()
    {
        _dispatcherHandle = new DispatcherHandle();
        uButtonText.text = "Stop";
        uButton.onClick.AddListener(OnClick);
    }

    public void OnDestroy() => uButton.onClick.RemoveListener(OnClick);

    private void OnClick()
    {
        _dispatcherHandle.Dispatch(new StopSpinCommand());
    }
}
```

</details>
Additional examples, including for features such as Effects, can be found in the folder `Assets/Examples`.

## Editor

Fluxity provides editor windows to aid in debugging.

### Fluxity Runtime Stores

This window gives a readonly view of the current state in the stores. Located via `Window->Fluxity->Runtime Stores`.
This is runtime only.

![No Stores can be shown when not playing](img/Stores-Not-Runtime.png?raw=true "No Stores can be shown when not playing.")

With stores registered, the window shows all available stores.

![When playing all registered shows show in left panel](img/Stores-Many-Avail.png?raw=true "When playing all registered shows show in left panel.")

Clicking a store in the left panel shows its values on the right.

![With a Feature selected we see a readonly view of its value(s)](img/Feature-Complex-Recursive.png?raw=true "With a Feature selected we see a readonly view of its value(s).")

### Fluxity Runtime Bindings

This window shows a table of all currently bound reducers and effects. This will be empty when not in playmode. The Column headers can be clicked to sort the table. Located via `Window->Fluxity->Runtime Bindings`.
This is runtime only.

!["Reducer and Effect binding, showing during play mode."](img/runtine-bindings-window.png?raw=true "Reducer and Effect binding, showing during play mode.")

### Fluxity History Window

This window acts as middleware to Fluxity. It keeps a copy of every command that is dispatched. This will attempt to keep last history even when playmode is exited. Located via `Window->Fluxity->Runtime History`.

!["Timestamps of each dispatched command, showing during play mode."](img/fluxity_runtime_history.png?raw=true "Timestamps of each dispatched command, showing during play mode.")

It will also log commands with timestamps to file if enabled. They will appear in your project's `Logs`, named `fluxitycommandhistory_<TIME STAMP>.txt`.

### Fluxity Preferences

Found in `Edit->Preferences->Fluxity`. Presently controls the number of commands kept in the recent history queue, and if the history should be logged to file.

!["Fluxity Preferences."](img/fluxity_preferences.png?raw=true "Fluxity Preferences.")
