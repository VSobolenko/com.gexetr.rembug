# Installation

- Via a git link in the PackageManager 
  ```
  https://github.com/VSobolenko/com.gexetr.remotedebug.git
  ```
- Editing of `Packages/manifest.json`
  ```
  "com.gexetr.rembug": "https://github.com/VSobolenko/com.remotedebug.git,
  ```
- Git Submodule
  ```sh
  git submodule add https://github.com/VSobolenko/com.gexetr.remotedebug Packages/com.gexetr.remotedebug
  ```
  
# Remote Debug Integration (Experimental)

> ⚠️ This project is experimental and currently under development. Use it with caution — bugs and changes are expected.  
> ✔️ Tested on **Unity 2022.3.46f1**.

---

## 📦 Overview

This Unity package integrates:

- An Android Java library for logging from Android devices.
- A C# console application that runs a local HTTP server.
- A unified development flow directly inside the Unity Editor.

All external tools are located in the `.Extern/` folder and are meant to be used **within Unity as a module**.

---

## 🚀 How to Use

### Log a Message from Android

Call the following from your Android code:
```csharp
LogRemote.Info("Hello from Android");
```

Replace the string with your custom log message.

---

## 🛠 Local HTTP Server

The `.Extern/Server/LocalHttpServer/` folder contains a **C# console server project**.

### 🧪 To run the server from Unity:

1. Go to **Tools > GCL > Run Local HTTP Server** in the Unity toolbar.
2. If the server executable is already built, it will be run immediately.
3. If not, Unity will automatically build it using `dotnet build` and then launch it.

> ✅ The server will always be run with **administrator privileges**.

---

## 📤 Deploy Android Library

Inside `.Extern/InputDialogLibAndorid/`, you can deploy the Android `.aar` plugin to Unity using:

```bash
./gradlew deployToPackageLocal
```

Make sure to run this command in the correct directory.

---

## 🧪 Development Notes

- This integration is actively evolving.
- You may encounter breaking changes or experimental behavior.
- Contributions and suggestions are welcome!

---
