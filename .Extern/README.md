
# InputDialogLibAndroid

This repository contains two separate components used together in a Unity project:
1. **InputDialogLibAndroid** — an Android Library used as a native plugin.
2. *(To be added)* Local server instructions.

---

## 📦 Android Library (InputDialogLibAndroid)

This is an Android Library module built with `com.android.library`. It is intended to be used as a native plugin in Unity.

### 🔧 How to Build and Deploy

To build the library and copy the resulting `.aar` into the Unity plugin folder, run the following command in the terminal **from this directory**:

```bash
./gradlew deployToPackageLocal
```

**Directory:**
```
Packages/com.gexetr.rembug/.Extern/InputDialogLibAndorid
```

### 📁 Output

Once the build completes, the `.aar` will be automatically copied into the corresponding Unity plugin folder.

---

## 🔜 Local Server (Coming Soon)

You will be able to run a local server to simulate or communicate with the plugin logic.
Details will be added in the second part of this README.

---

## 🧹 Git Ignore

This project uses a `.gitignore` that excludes build artifacts such as:

- `*.aar`
- `build/`
- `.gradle/`
- `.idea/`
- Temporary files

---

## 💬 Notes

- The library is not an executable application.
- It is packaged as a plugin for Unity and works in tandem with a local server.
