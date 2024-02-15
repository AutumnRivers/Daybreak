# Variables API
## Setting And Changing Global Variables
```cs
# OnMapLoad
SetGlobalVariable("prepared", "no")

# OnEnter
# Using the ChatAPI example...
if($lastChoice == 0)
	SendChatText("Locking down the system, then.", 3f, "Autumn")
	SetGlobalVariable("prepared", "yes")
# ...
```
`SetGlobalVariable(string name, string value)`

Global variables can be used across the network. You can use this for any number of reasons. Using `SetGlobalVariable` on an existing global variable will simply overwrite it. You can get a global variable's value with `GetGlobalVariable("variableName")`.

## Counters
Counters are similar to global variables, but are numbers. Counters can be set and overridden like global variables, but can also be incremented and decremented by 1.
### Setting A Counter
```cs
SetCounter("counter", 0)
```
`SetCounter(string name, int value)`

Sets a counter to the value. Overrides the counter if the name already exists. You can get the current value of a counter with `GetCounter(string name)`.

### Incrementing/Decrementing Counters
```cs
AddCounter("counter")
#...
SubtractCounter("counter")
```
`AddCounter(string name)` / `SubtractCounter(string name)`

Adds/subtracts the current value of the counter by one.

* If the counter does not exist, `AddCounter` will run the equivalent of `SetCounter(name, 1)`.
* If the counter does not exist, `SubtractCounter` will run the equivalent of `SetCounter(name, 0)`.