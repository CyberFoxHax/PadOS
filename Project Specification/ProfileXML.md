[[_toc_]]

Profile XML is a datastructure for configuring input sumulation profiles.

# PadOS XML structure

Starting off the base syntax is XML

a very basic profile would look like this:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<Profile>
    <ButtonTrigger Button="RightShoulder" />
    <KeyboardAction Button="N" />
</Profile>
```

this defines a simple profile that maps RightShoulder to button N on the keyboard.

## Mappings

A mapping is a group of 1 Trigger and it's actions. 

An example mapping would look like this

```xml
<ButtonTrigger Button="LeftShoulder" />
<KeyboardAction Button="P" />

<ButtonTrigger Button="RightShoulder" />
<KeyboardAction Button="N" />
```

There is an implicit grouping going on. This means that a trigger can have multiple assigned actions. The above example example defines 2 "mappings".

Should you need more actions on a mapping you can add more actions. (But you can't add more triggers)

```xml
<trigger />
<action />
<action />
<action />
```


## Variables

It's fairly common to have repeated values in a profile, you can create variables to create reusable values.

Note: You need to declare the variable __before__ using it.

```xml
<Variable Name="MyDelay" Value="1000" />
<Variable Name="MyRepeat" Value="150" />
```

to reference the variable

```xml
<ButtonTrigger Button="LeftShoulder" />
<RepeatAction Delay="{MyDelay}" Interval="{MyRepeat}">
    <KeyboardAction Button="Backspace" />
</RepeatAction>
```

# Trigger Types

Here is the list of all available triggers.

## `<ButtonTrigger Button="A" />`

The simplest form of trigger that listens to a button on the controller.

available buttons:

 * `A`
 * `B`
 * `X`
 * `Y`
 * `Start`
 * `Back`
 * `DPadUp`
 * `DPadDown`
 * `DPadLeft`
 * `DPadRight`
 * `LeftThumb`
 * `RightThumb`
 * `LeftShoulder`
 * `RightShoulder`
 * `Guide`

## `<AnalogueTrigger Axis="LeftThumbX" Value="-0.5" Frequency="10" />`


This will listen to the analogue sticks.

### Axis
Axis will define which analogue you want to use, there are a total of 6 axes.

 * RightThumbX
 * RightThumbY
 * LeftThumbX
 * LeftThumbY
 * RightTrigger
 * LeftTrigger

### Value

For value you define the threshold until the trigger is fired. -0.5 means move the stick halfway to the *left*

* -X is Left
* +X is Right
* -Y is Down
* +Y is Up

Frequency means the trigger will continue to trigger at specified internal in Milliseconds

## `<ComboTrigger>`

A composite trigger. You can set up a combination of trigger that need to be activated all at once, E.g:

```xml
<ComboTrigger>
    <ButtonTrigger Button="LeftShoulder" />
    <ButtonTrigger Button="RightShoulder" />
    <AnalogueTrigger Axis="LeftTrigger" />
    <AnalogueTrigger Axis="RightTrigger" />
    <ButtonTrigger Button="Start" />
    <ButtonTrigger Button="Back" />
</ComboTrigger>
<SystemAction Key="Reboot" />
```

In this case push all the listed buttons above and will launch a reboot.

## `<SequenceTrigger>`

A composite trigger. You can set up a combination of trigger that need to be activated in order, E.g:

```xml
<SequenceTrigger>
    <ButtonTrigger Button="Up" />
    <ButtonTrigger Button="Up" />
    <ButtonTrigger Button="Down" />
    <ButtonTrigger Button="Down" />
    <ButtonTrigger Button="Left" />
    <ButtonTrigger Button="Right" />
    <ButtonTrigger Button="Left" />
    <ButtonTrigger Button="Right" />
    <ButtonTrigger Button="B" />
    <ButtonTrigger Button="A" />
</SequenceTrigger>
<SystemAction Key="Reboot" />
```

In this case push all the listed buttons above in order. And the system will reboot.


## `<HoldSwitch>`

A composite trigger that allows you to switch between actions based on hold time. Only works for buttons at the moment.

Usage:

```xml
<HoldSwitch>
    <ButtonTrigger Button="RightThumb" />
</HoldSwitch>
<KeyboardAction Button="Alt+Tab" HoldSwitch.Timeout="0" />
<KeyboardAction Button="Alt+F4" HoldSwitch.Timeout="1000" />
```

In this example you can press the right thumb down and it'll do Alt+Tab, or you can hold and it'll do Alt+F4. Note the extra `HoldSwitch.Timeout` attribute. This is required hold time in milliseconds.


# Actions

Here's a list of all available Actions

## `<KeyboardAction Button="Shift+N" />`

This action will enter a key combination on the keyboard.

Field syntax is `{modifier}+{button}` wher `{modifier}` can be `Ctrl`, `Alt` or `Shift`. `{Button}` is any key on the keyboard (case insensitive).

Note: typing `n` or `N` makes no difference. If you wish the type an uppercase `N` you need to use the `Shift+N` combination, just like you would on a real keyboard.

Available buttons (aside from the full alphanumeric set)

 * `Shift`
 * `Ctrl`
 * `Alt`
 * `Tab`
 * `Up`
 * `Down`
 * `Left`
 * `Right`
 * `Delete`
 * `Backspace`
 * `Enter`
 * `Escape`
 * `Space`
 * `Win`

## `<MouseAction Move="X" Speed="15" />`

This will control the mouse.

```xml
<MouseAction Button="Left" Position="100 100" />
<MouseAction Scroll="Y" Speed="20" />
<MouseAction Move="X" Speed="15" />
<MouseAction Position="100 100" />
<MouseAction Button="Left" />
```

### Attribute: `Button`

Defines button name or index. You can use either name or number listed below.

 1. Left
 2. Middle
 2. Scroll
 3. Right
 4. Back
 5. Forward

### Attribute: `Position`

Define where on the screen the mouse should move to.

Example: Move mouse to x: 150, y: 150 in pixels
```xml
<MouseAction Position="150 150" />
```

Example: Move mouse to screen center
```xml
<MouseAction Position="50% 50%" />
```


### Attribute: `Move`

Used together with `Speed`: Moves the mouse relatively by `Speed`. Values:

 * X
 * Y

Example: Move the mouse to left

```xml
<MouseAction Move="X" Speed="-15" />
```

### Attribute: `Scroll`

Scroll at Axis. Values:

 * X
 * Y

### Attribute: `Speed`

Used together with `Scroll`. How many units should be scrolled. Use a negative value to scroll in opposite direction.

## `<RepeatAction>` (Not implemented)

Special action that will execute actions you define within.

```xml
<RepeatAction Delay="1000" Interval="500">
    <!-- actions -->
</RepeatAction>
```

# `<Plugin Filename="PadOS.Plugin.DesktopInput.dll" />`

Special block that allows you to load a dll written in .NET

you need to implement this interface:

```csharp
interface IInputSimulatorPlugin {
    string Key { get; }
    bool Enabled { get; set; }
}
```

## `Key`

Custom name you can reference in your XML. Not implemented!

## `Enabled`

Getter/Setter to enable and disable plugin. You'll need to make sure your plugin can correctly turn on and off so PadOS can manage it's runtime.