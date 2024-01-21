using System;
using System.Collections.Generic;
using System.Text;

namespace Daybreak_Midnight.Static
{
    public static class ConstantVariables
    {
        public const string DOUBLE_NEWLINE = "\n\n";
        public const int BASE_FONT_SIZE = 36;

        public const string MANUAL_ENTRY = $"<h1>{Daybreak.PLUGIN_NAME} v{Daybreak.PLUGIN_VERSION}</h1>" + "\n\n" +
            "Developed by <b>Autumn Rivers</b> using <b>BepInEx 5</b>." + "\n\n" +
            $"<h2>What is {Daybreak.PLUGIN_NAME}?</h2>" + "\n\n" +
            $"{Daybreak.PLUGIN_NAME} is a modification for <i>Midnight Protocol</i> that extends the custom campaign capabilities. " +
            "It can be used for a variety of features, such as using custom music, and chat logs." + "\n\n" +
            $"<h2>Who can use {Daybreak.PLUGIN_NAME}?</h2>" + "\n\n" +
            "Anyone! It is licensed under the MIT License, and free for anyone to use as they so wish.";

        public const string BGM_MANUAL = "<h1>Custom Background Music</h1>\n\n" +
            $"With {Daybreak.PLUGIN_NAME}, you can use custom background music in your scripts for your custom campaigns. " +
            "<b>If you are going to use custom music that is <i>not</i> yours, be sure to get permission from the artist!</b>\n\n" +
            "<h2>Usage</h2>\n\n" +
            "In scripting for your custom campaign, custom music is played just as custom SFX; [PlaySFX]. " +
            "However, there is a difference. In order for your sound to be recognized as music, it <i>must</i> start with 'mus_' in the filename!\n\n" +
            "For example; [mus_example.wav] in your custom campaign's \"/Audio\" folder. [PlaySFX('mus_example.wav')]\n\n" +
            "<h2>Gotchas</h2>\n\n" +
            "This implementation is, admittedly, not perfect. Enabling this will automatically disable MP's default background music, " +
            "which means the rest of your campaign without custom music would become silent. You can fix this with the following command: " +
            "[PlaySFX('resetbgm')].\n\n" +
            "This will reset the background music back to TENSION.";

        public const string OPENING_WARNING = $"{Daybreak.PLUGIN_NAME} is successfully installed. " +
            "You likely won't get support from the game devs if something breaks.\n\n" +
            "Please check the <b>Manual</b> for more information.";

        public const string NETWORK_CHAT_PATH = "CameraController/CameraRig/Main Camera/UICamera/MainHUD/NetworkChat(Clone)/";
    }
}
