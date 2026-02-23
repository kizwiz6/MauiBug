# MauiBug
## .NET 10 MAUI Trimming Regression - MediaElement

This repository provides a minimal reproduction of a regression in the **.NET 10 (10.0.41)** build pipeline. The IL Trimmer is aggressively stripping essential constructors and JNI-callback methods from the `CommunityToolkit.Maui.MediaElement` library, even when explicit preservation rules are present.

## The Bug
When building for Android with trimming enabled, the app crashes because the trimmer removes the constructor for `MediaElement`. This manifests as a `XamlParseException` wrapping a `MissingMethodException`.

In larger production apps, this same behaviour causes `AbstractMethodError` crashes during video playback because the trimmer snipes interface methods (like `onAudioSessionIdChanged`) required by the Android Media3/ExoPlayer bridge.

## How to Reproduce
1. Open this solution in Visual Studio 2022 (Preview) with **.NET 10** installed.
2. Set the build configuration to **Debug** (Trimming is forced in the `.csproj` for this repro).
3. Deploy to a physical Android device or Emulator.
4. On the Main Page, tap **"Go to Video Page"**.
5. **Result:** The app will crash immediately.

## Diagnostic Evidence
The following stack trace was captured by wrapping `InitializeComponent()` in a diagnostic try-catch:

```text
=== TRIMMER CRASH DETECTED ===
Microsoft.Maui.Controls.Xaml.XamlParseException: Position 15:10. Arg_NoDefCTor, CommunityToolkit.Maui.Views.MediaElement
 ---> System.MissingMethodException: Arg_NoDefCTor, CommunityToolkit.Maui.Views.MediaElement
   at System.RuntimeType.CreateInstanceDefaultCtor(Boolean publicOnly, Boolean wrapExceptions)
   at Microsoft.Maui.Controls.Xaml.CreateValuesVisitor.Visit(ElementNode node, INode parentNode)

Environment
MAUI Version: 10.0.41
CommunityToolkit.Maui.MediaElement: 8.0.0
Target Framework: net10.0-android
Link Mode: Full
Trim Mode: partial (Full trim mode causes even deeper XAML parser failures)
