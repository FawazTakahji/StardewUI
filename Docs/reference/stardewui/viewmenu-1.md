---
title: ViewMenu&lt;T&gt;
description: Generic menu implementation based on a root IView.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ViewMenu&lt;T&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI](index.md)  
Assembly: StardewUI.dll  

</div>

Generic menu implementation based on a root [IView](iview.md).

```cs
public class ViewMenu<T> : StardewValley.Menus.IClickableMenu, 
    System.IDisposable
```

### Type Parameters

**`T`**  


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ IClickableMenu ⇦ ViewMenu&lt;T&gt;

**Implements**  
[IDisposable](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable)

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ViewMenu&lt;T&gt;(Edges, Boolean)](#viewmenutedges-bool) | Initializes a new instance of [ViewMenu&lt;T&gt;](viewmenu-1.md). | 

### Fields

 | Name | Description |
| --- | --- |
| _childMenu | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| _dependencies | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| _parentMenu | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| allClickableComponents | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| behaviorBeforeCleanup | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| closeSound | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| currentlySnappedComponent | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| destroy | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| exitFunction | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| height | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| upperRightCloseButton | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| width | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| xPositionOnScreen | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| yPositionOnScreen | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 

### Properties

 | Name | Description |
| --- | --- |
| [DimmingAmount](#dimmingamount) | Amount of dimming between 0 and 1; i.e. opacity of the background underlay. | 
| Position | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| [View](#view) | The view to display with this menu. | 

### Methods

 | Name | Description |
| --- | --- |
| _ShouldAutoSnapPrioritizeAlignedElements() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| actionOnRegionChange(Int32, Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| AddDependency() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| [applyMovementKey(Int32)](#applymovementkeyint) | Initiates a focus search in the specified direction.<br><span class="muted" markdown>(Overrides IClickableMenu.applyMovementKey(Int32))</span> | 
| applyMovementKey(Keys) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| [areGamePadControlsImplemented()](#aregamepadcontrolsimplemented) | Returns whether or not the menu wants **exclusive** gamepad controls.<br><span class="muted" markdown>(Overrides IClickableMenu.areGamePadControlsImplemented())</span> | 
| automaticSnapBehavior(Int32, Int32, Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| cleanupBeforeExit() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| clickAway() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| [CreateView()](#createview) | Creates the view. | 
| customSnapBehavior(Int32, Int32, Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| [Dispose()](#dispose) |  | 
| [draw(SpriteBatch)](#drawspritebatch) | Draws the current menu content.<br><span class="muted" markdown>(Overrides IClickableMenu.draw(SpriteBatch))</span> | 
| draw(SpriteBatch, Int32, Int32, Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| drawBackground(SpriteBatch) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| drawBorderLabel(SpriteBatch, string, SpriteFont, Int32, Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| drawHorizontalPartition(SpriteBatch, Int32, Boolean, Int32, Int32, Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| drawMouse(SpriteBatch, Boolean, Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| drawVerticalIntersectingPartition(SpriteBatch, Int32, Int32, Int32, Int32, Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| drawVerticalPartition(SpriteBatch, Int32, Boolean, Int32, Int32, Int32, Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| drawVerticalUpperIntersectingPartition(SpriteBatch, Int32, Int32, Int32, Int32, Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| emergencyShutDown() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| exitThisMenu(Boolean) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| exitThisMenuNoSound() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| [FormatTooltip(IEnumerable&lt;ViewChild&gt;)](#formattooltipienumerableviewchild) | Formats a tooltip given the sequence of views from root to the lowest-level hovered child. | 
| gamePadButtonHeld(Buttons) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| gameWindowSizeChanged(Rectangle, Rectangle) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| GetChildMenu() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| getComponentWithID(Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| getCurrentlySnappedComponent() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| GetParentMenu() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| HasDependencies() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| initialize(Int32, Int32, Int32, Int32, Boolean) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| initializeUpperRightCloseButton() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| IsActive() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| IsAutomaticSnapValid(Int32, ClickableComponent, ClickableComponent) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| isWithinBounds(Int32, Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| [leftClickHeld(Int32, Int32)](#leftclickheldint-int) | Invoked on every frame in which a mouse button is down, regardless of the state in the previous frame.<br><span class="muted" markdown>(Overrides IClickableMenu.leftClickHeld(Int32, Int32))</span> | 
| moveCursorInDirection(Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| noSnappedComponentFound(Int32, Int32, Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| overrideSnappyMenuCursorMovementBan() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| [performHoverAction(Int32, Int32)](#performhoveractionint-int) | Invoked on every frame with the mouse's current coordinates.<br><span class="muted" markdown>(Overrides IClickableMenu.performHoverAction(Int32, Int32))</span> | 
| [populateClickableComponentList()](#populateclickablecomponentlist) | <span class="muted" markdown>(Overrides IClickableMenu.populateClickableComponentList())</span> | 
| readyToClose() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| [receiveGamePadButton(Buttons)](#receivegamepadbuttonbuttons) | Invoked whenever a controller button is newly pressed.<br><span class="muted" markdown>(Overrides IClickableMenu.receiveGamePadButton(Buttons))</span> | 
| [receiveKeyPress(Keys)](#receivekeypresskeys) | Invoked whenever a keyboard key is newly pressed.<br><span class="muted" markdown>(Overrides IClickableMenu.receiveKeyPress(Keys))</span> | 
| [receiveLeftClick(Int32, Int32, Boolean)](#receiveleftclickint-int-bool) | Invoked whenever the left mouse button is newly pressed.<br><span class="muted" markdown>(Overrides IClickableMenu.receiveLeftClick(Int32, Int32, Boolean))</span> | 
| [receiveRightClick(Int32, Int32, Boolean)](#receiverightclickint-int-bool) | Invoked whenever the right mouse button is newly pressed.<br><span class="muted" markdown>(Overrides IClickableMenu.receiveRightClick(Int32, Int32, Boolean))</span> | 
| [receiveScrollWheelAction(Int32)](#receivescrollwheelactionint) | Invoked whenever the mouse wheel is used. Only works with vertical scrolls.<br><span class="muted" markdown>(Overrides IClickableMenu.receiveScrollWheelAction(Int32))</span> | 
| [releaseLeftClick(Int32, Int32)](#releaseleftclickint-int) | Invoked whenever the left mouse button is just released, after being pressed/held on the last frame.<br><span class="muted" markdown>(Overrides IClickableMenu.releaseLeftClick(Int32, Int32))</span> | 
| RemoveDependency() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| [SetActive(Boolean)](#setactivebool) | Activates or deactivates the menu. | 
| SetChildMenu(IClickableMenu) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| setCurrentlySnappedComponentTo(Int32) | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| setUpForGamePadMode() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| shouldClampGamePadCursor() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| shouldDrawCloseButton() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| showWithoutTransparencyIfOptionIsSet() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| snapCursorToCurrentSnappedComponent() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| snapToDefaultClickableComponent() | <span class="muted" markdown>(Inherited from IClickableMenu)</span> | 
| [update(GameTime)](#updategametime) | Runs on every update tick.<br><span class="muted" markdown>(Overrides IClickableMenu.update(GameTime))</span> | 

### Events

 | Name | Description |
| --- | --- |
| [Close](#close) | Event raised when the menu is closed. | 

## Details

### Constructors

#### ViewMenu&lt;T&gt;(Edges, bool)

Initializes a new instance of [ViewMenu&lt;T&gt;](viewmenu-1.md).

```cs
public ViewMenu<T>(StardewUI.Layout.Edges gutter, bool forceDefaultFocus);
```

##### Parameters

**`gutter`** &nbsp; [Edges](layout/edges.md)  
Gutter edges, in which no content should be drawn. Used for overscan, or general aesthetics.

**`forceDefaultFocus`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Whether to always focus (snap the cursor to) the default element, even if the menu was triggered by keyboard/mouse.

-----

### Properties

#### DimmingAmount

Amount of dimming between 0 and 1; i.e. opacity of the background underlay.

```cs
public float DimmingAmount { get; set; }
```

##### Property Value

[Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)

##### Remarks

Underlay is only drawn when game options do not force clear backgrounds.

-----

#### View

The view to display with this menu.

```cs
public T View { get; }
```

##### Property Value

`T`

-----

### Methods

#### applyMovementKey(int)

Initiates a focus search in the specified direction.

```cs
public override void applyMovementKey(int directionValue);
```

##### Parameters

**`directionValue`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
An integer value corresponding to the direction; one of 0 (up), 1 (right), 2 (down) or 3 (left).

-----

#### areGamePadControlsImplemented()

Returns whether or not the menu wants **exclusive** gamepad controls.

```cs
public override bool areGamePadControlsImplemented();
```

##### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)

  Always `false`.

##### Remarks

This implementation always returns `false`. Contrary to what the name in Stardew's code implies, this setting is not required for [receiveGamePadButton(Buttons)](viewmenu-1.md#receivegamepadbuttonbuttons) to work; instead, when enabled, it suppresses the game's default mapping of button presses to clicks, and would therefore require reimplementing key-repeat and other basic behaviors. There is no reason to enable it here.

-----

#### CreateView()

Creates the view.

```cs
protected virtual T CreateView();
```

##### Returns

`T`

  The created view.

##### Remarks

Subclasses will generally create an entire tree in this method and store references to any views that might require content updates.

-----

#### Dispose()



```cs
public void Dispose();
```

-----

#### draw(SpriteBatch)

Draws the current menu content.

```cs
public override void draw(Microsoft.Xna.Framework.Graphics.SpriteBatch b);
```

##### Parameters

**`b`** &nbsp; [SpriteBatch](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html)  
The target batch.

-----

#### FormatTooltip(IEnumerable&lt;ViewChild&gt;)

Formats a tooltip given the sequence of views from root to the lowest-level hovered child.

```cs
protected virtual string FormatTooltip(System.Collections.Generic.IEnumerable<StardewUI.ViewChild> path);
```

##### Parameters

**`path`** &nbsp; [IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<[ViewChild](viewchild.md)>  
Sequence of all elements, and their relative positions, that the mouse coordinates are currently within.

##### Returns

[string](https://learn.microsoft.com/en-us/dotnet/api/system.string)

  The tooltip string to display, or `null` to not show any tooltip.

##### Remarks

The default implementation reads the string value of the _last_ (lowest-level) view with a non-empty [Tooltip](iview.md#tooltip), and breaks lines longer than 640px, which is the default vanilla tooltip width.

-----

#### leftClickHeld(int, int)

Invoked on every frame in which a mouse button is down, regardless of the state in the previous frame.

```cs
public override void leftClickHeld(int x, int y);
```

##### Parameters

**`x`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The mouse's current X position on screen.

**`y`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The mouse's current Y position on screen.

-----

#### performHoverAction(int, int)

Invoked on every frame with the mouse's current coordinates.

```cs
public override void performHoverAction(int x, int y);
```

##### Parameters

**`x`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The mouse's current X position on screen.

**`y`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The mouse's current Y position on screen.

##### Remarks

Essentially the same as [update(GameTime)](viewmenu-1.md#updategametime) but slightly more convenient for mouse hover/movement effects because of the arguments provided.

-----

#### populateClickableComponentList()



```cs
public override void populateClickableComponentList();
```

##### Remarks

Always a no-op for menus in StardewUI.

-----

#### receiveGamePadButton(Buttons)

Invoked whenever a controller button is newly pressed.

```cs
public override void receiveGamePadButton(Microsoft.Xna.Framework.Input.Buttons b);
```

##### Parameters

**`b`** &nbsp; [Buttons](https://docs.monogame.net/api/Microsoft.Xna.Framework.Input.Buttons.html)  
The button that was pressed.

-----

#### receiveKeyPress(Keys)

Invoked whenever a keyboard key is newly pressed.

```cs
public override void receiveKeyPress(Microsoft.Xna.Framework.Input.Keys key);
```

##### Parameters

**`key`** &nbsp; [Keys](https://docs.monogame.net/api/Microsoft.Xna.Framework.Input.Keys.html)  
The key that was pressed.

-----

#### receiveLeftClick(int, int, bool)

Invoked whenever the left mouse button is newly pressed.

```cs
public override void receiveLeftClick(int x, int y, bool playSound);
```

##### Parameters

**`x`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The mouse's current X position on screen.

**`y`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The mouse's current Y position on screen.

**`playSound`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Currently not used.

-----

#### receiveRightClick(int, int, bool)

Invoked whenever the right mouse button is newly pressed.

```cs
public override void receiveRightClick(int x, int y, bool playSound);
```

##### Parameters

**`x`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The mouse's current X position on screen.

**`y`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The mouse's current Y position on screen.

**`playSound`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Currently not used.

-----

#### receiveScrollWheelAction(int)

Invoked whenever the mouse wheel is used. Only works with vertical scrolls.

```cs
public override void receiveScrollWheelAction(int value);
```

##### Parameters

**`value`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
A value indicating the desired vertical scroll direction; negative values indicate "down" and positive values indicate "up".

-----

#### releaseLeftClick(int, int)

Invoked whenever the left mouse button is just released, after being pressed/held on the last frame.

```cs
public override void releaseLeftClick(int x, int y);
```

##### Parameters

**`x`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The mouse's current X position on screen.

**`y`** &nbsp; [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)  
The mouse's current Y position on screen.

-----

#### SetActive(bool)

Activates or deactivates the menu.

```cs
public void SetActive(bool active);
```

##### Parameters

**`active`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Whether the menu should be active (displayed). If this is `false`, then the menu will be closed if already open; if `true`, it will be opened if not already open.

-----

#### update(GameTime)

Runs on every update tick.

```cs
public override void update(Microsoft.Xna.Framework.GameTime time);
```

##### Parameters

**`time`** &nbsp; [GameTime](https://docs.monogame.net/api/Microsoft.Xna.Framework.GameTime.html)  
The current [GameTime](https://docs.monogame.net/api/Microsoft.Xna.Framework.GameTime.html) including the time elapsed since last update tick.

-----

### Events

#### Close

Event raised when the menu is closed.

```cs
public event EventHandler<System.EventArgs>? Close;
```

##### Event Type

[EventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler-1)<[EventArgs](https://learn.microsoft.com/en-us/dotnet/api/system.eventargs)>

-----

