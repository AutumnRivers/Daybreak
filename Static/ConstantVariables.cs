using System;
using System.Collections.Generic;
using System.Text;

namespace Daybreak_Midnight.Static
{
    public static class ConstantVariables
    {
        public const string DOUBLE_NEWLINE = "\n\n";
        public const int BASE_FONT_SIZE = 36;

        public const string CONSOLE_PATH = "CameraController/CameraRig/Main Camera/UICamera/ConsoleCanvas/Console(Clone)";

        public const string MANUAL_ENTRY = $"<h1>{Daybreak.PLUGIN_NAME} v{Daybreak.PLUGIN_VERSION}</h1>" + "\n\n" +
            "Developed by <b>Autumn Rivers</b> using <b>BepInEx 5</b>." + "\n\n" +
            $"<h2>What is {Daybreak.PLUGIN_NAME}?</h2>" + "\n\n" +
            $"{Daybreak.PLUGIN_NAME} is a modification for <i>Midnight Protocol</i> that extends the custom campaign capabilities. " +
            "It can be used for a variety of features, such as using custom music, and chat logs." + "\n\n" +
            $"<h2>Who can use {Daybreak.PLUGIN_NAME}?</h2>" + "\n\n" +
            "Anyone! It is licensed under the MIT License, and free for anyone to use as they so wish.";

        public const string OPENING_WARNING = $"{Daybreak.PLUGIN_NAME} is successfully installed. " +
            "You likely won't get support from the game devs if something breaks.\n\n" +
            "Please check the <b>Daybreak API</b> selection for more information.";
    }

    public static class APIDocs
    {
        public const string CHAT1 = "<h1>Chat API - Basics</h1>\n\n" +
            "The <b>Chat API</b> opens up the possibility to use the text chat in custom campaigns. " +
            "You can use this for dialogue while the player is in a network!\n\n" +
            "<h2>Sending A Message</h2>\n\n" +
            "USAGE: [SendChatText(string Message, float Delay, [string Character])]\n" +
            "'Message' - A <i>string</i> indicating the message contents.\n" +
            "'Delay' - A <i>float</i> indicating how long the game should wait to send the message.\n" +
            "'Character' - A <i>string</i> indicating a character name. If left blank, defaults to 'System'.";

        public const string CHAT_CHOICES = "<h1>Chat API - Choices</h2>\n\n" +
            "The <b>Chat API</b> allows you to also have multiple choices that the player can pick from. " +
            "You could create entire branching paths with these!\n\n" + 
            "<h2>Sending A Choice</h2>\n\n" +
            "USAGE [ShowPlayerChoices(string choice1, [string choice2, string choice3])]\n" +
            "You can have up to <b>three choices</b>. When the choice is made, you will be able to tell what the player chose with " +
            "the <b>LastChoice()</b> method. This will return an <i>int</i> of the player's choice.\n\n" +
            "It is <b>RECOMMENDED</b> to read the out-of-game documentation for more information.";

        public const string BGM_MANUAL = "<h1>Custom Background Music</h1>\n\n" +
            $"With {Daybreak.PLUGIN_NAME}, you can use custom background music in your scripts for your custom campaigns. " +
            "<b>If you are going to use custom music that is <i>not</i> yours, be sure to get permission from the artist!</b>\n\n" +
            "<h2>Playing Music</h2>\n\n" +
            "USAGE: [PlayCustomMusic(string filename)]\n" +
            "Filename - The name of the <b>WAV</b> file to play, similar to <u>PlaySFX</u>. " +
            "(e.g., an audio file named 'coolSong.wav' would be played as [PlayCustomMusic(\"coolSong\")].\n\n" +
            "<h2>Resetting BGM</h2>\n\n" +
            "PlayCustomMusic stops <i>Midnight Protocol's</i> usual background music handler when activated. " +
            "This means that, if you do not reset your BGM, then your campaign will be completely silent from thereonout.\n\n" +
            "By running <b>ResetBGM()</b> in your script, then the background music will reset back to TENSION, " +
            "and Midnight Protocol will once again be in control of the pre-defined music.";

        public const string SYSOP_API_DOCS1 = "<h1>SysOp API</h1>\n\n" +
            "The <b>SysOP API</b> allows you to interact with how SysOps move and target nodes. " +
            "Mixing this with the <b>Variables API</b> may allow you to create some unique SysOp logic...\n\n" +
            "<h2>Setting SysOp IDs</h2>\n\n" +
            "<b>IMPORTANT:</b> You should only ever run this in <i>ON MAP LOADED</i>!\n\n" +
            "USAGE: [SetSysOpId(string nodeAddress, string sysOpId)]\n" +
            "Saves the SysOp at the nodeAddress under sysOpId. For use with the SysOp API.\n" +
            "nodeAddress - The node address of where the SysOp currently is.\n" +
            "sysOpId - The unique ID for the SysOp.\n\n" +
            "<h2>Changing SysOp's Path</h2>\n\n" +
            "USAGE: [SetSysOpTarget(string nodeAddress, string sysOpId)]\n" +
            "Forces the SysOp to target the nodeAddress, similar to how they target encryption/financial nodes.\n" +
            "nodeAddress - The node address to target.\n" +
            "sysOpId - The unique SysOp ID to target.\n\n" +
            "<h2>Teleporting SysOps</h2>\n\n" +
            "USAGE: [TeleportSysOpToNode(string nodeAddress, string sysOpId)]\n" +
            "Instantly teleports a SysOp to the target nodeAddress.";

        public const string VARIABLES_API_DOCS1 = "<h1>Variables API</h1>\n\n" +
            "The <b>Variables API</b> allows you to save global variables that persist throughout the netowrk.\n\n" +
            "<h2>Setting Global Variables</h2>\n\n" +
            "<b>NOTE:</b> It is RECOMMENDED to set your global variables in <i>ON MAP LOADED</i>, but not required.\n\n" +
            "USAGE: [SetGlobalVariable(string name, string value) / SetCounter(string name, int value)]\n" +
            "<b>SetGlobalVariable</b> - Sets a global string under [name] with the value of [value].\n" +
            "<b>SetCounter</b> - Sets a global int under [name] with the value of [value].\n\n" +
            "<h2>Reading Global Variables</h2>\n\n" +
            "USAGE: [GetGlobalVariable(string name), GetCounter(string name)]\n" +
            "<b>GetGlobalVariable</b> - Returns a string of the global variable under [name].\n" +
            "<b>GetCounter</b> - Returns an int of the global counter under [name].";

        public const string VARIABLES_API_COUNTER = "<h1>Variables API - Counters</h1>\n\n" +
            "<b>Counters</b> store an int for you to use, and can only be incremented or decremented by 1.\n\n" +
            "<h2>AddCounter(string name)</h2>\n" +
            "Adds <b>1</b> to the counter under [name]. Returns an int of the new value.\n\n" +
            "<h2>SubtractCounter(string name)</h2>\n" +
            "Subtracts <b>1</b> to the counter under [name]. Returns an int of the new value.";
    }
}
