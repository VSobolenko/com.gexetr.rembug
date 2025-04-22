mergeInto(LibraryManager.library, {
    ShowTextInput: function (gameObjectNamePtr, positiveMethodPtr, negativeMethodPtr) {
        const gameObjectName = UTF8ToString(gameObjectNamePtr);
        const positiveMethod = UTF8ToString(positiveMethodPtr);
        const negativeMethod = UTF8ToString(negativeMethodPtr);

        window.focusLost = true;

        setTimeout(() => {
            const result = prompt("Enter IP:", "192.168.43.236");
            window.focusLost = false;

            if (result !== null) {
                SendMessage(gameObjectName, positiveMethod, result);
            } else {
                SendMessage(gameObjectName, negativeMethod, "cancel");
            }
        }, 100);
    }
});