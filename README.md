# Daybreak
After the midnight, comes the Daybreak.

---

**Daybreak** is a [BepInEx 5](https://github.com/BepInEx/BepInEx) plugin for [Midnight Protocol] that extends the capabilities of the game's custom campaigns.

---

## Usage

* Download [BepInEx 5.4.22](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.22) and extract it into the root folder of your *Midnight Protocol* installation folder (the folder with the executable)
* Run the game at least once to finalize the installation.
* Download the latest Daybreak release
* Place the downloaded `Daybreak_Midnight.dll` file into the `/BepInEx/plugins` folder of your Midnight Protocol installation.

For detailed instructions on how to use the new scripthing methods, please check the [Documentation](./docs/ExtendedAPI).

---

## Compiling From Source

### Requirements
* .NET Core SDK (Daybreak targets .NET Standard 2.0)
* A legal copy of [Midnight Protocol] (you'll need the `Assembly-CSharp.dll`!)
* BepInEx 5 (5.4.22 recommended)

### Setup
* Download the git repository
    * e.g., `git clone https://github.com/AutumnRivers/Daybreak`
* Copy the following file from `MidnightProtocol_Data/Managed` and place it in `Daybreak/lib`:
    * `Assembly-CSharp.dll`
* Congrats! You are now ready to build Daybreak however you wish

---

[Midnight Protocol]: https://store.steampowered.com/app/1162700/Midnight_Protocol/