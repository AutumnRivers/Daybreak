# Chat API
## Sending A Basic Message
![](../img/ChatAPIExample1.jpg)
```cs
SendChatText("You would tell me if it wasn't, right?", 6f, "Autumn")
```
`SendChatText(string message, float delay, [string character])`

No bells, no whistles. Inside the scripting tools for the Custom Campaign editor, use the above method to send a simple text message to the network chat, as a character.

## Handling Choices
![](../img/ChatAPIChoicesExample.png)
```cs
ShowPlayerChoices("Of course I'm prepared!", "On second thought...", "")

while(ChatIsActive())
	Sleep()
endwhile

declare(int, $lastChoice, LastChoice())

if($lastChoice == 0)
	SendChatText("Locking down the system, then.", 3f, "Autumn")
    # ...
else
	SendChatText("Take your time.", 2f, "Autumn")
    # ...
endif
```
`ShowPlayerChoices(string choice1, string choice2, [string choice3])`

Shows the player 2-3 choices that they can pick from. In order to prevent the game from moving forward during choice selection, you must use sleep the current script while `ChatIsActive()` is true. After the player makes a choice, chat will no longer be active, and their choice will be saved to `LastChoice()`.

In the example above, `On second thought...` is equal to `1` for the last choice index.