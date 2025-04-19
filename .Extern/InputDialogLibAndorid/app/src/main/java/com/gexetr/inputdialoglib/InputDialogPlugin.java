package com.gexetr.inputdialoglib;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Intent;
import android.os.Handler;
import android.os.Looper;
import android.widget.EditText;

import com.unity3d.player.UnityPlayer;

public class InputDialogPlugin {
    public static void showInputDialog(final String unityGameObject, final String unityMethod, final String title, final String message) {
        final Activity activity = UnityPlayer.currentActivity;

        // Свернуть Unity-приложение (Unity уходит в Application.pause)
        activity.moveTaskToBack(true);

        new Handler(Looper.getMainLooper()).post(() -> {
            final EditText input = new EditText(activity);

            new AlertDialog.Builder(activity)
                    .setTitle(title)
                    .setMessage(message)
                    .setView(input)
                    .setCancelable(false)
                    .setPositiveButton("OK", (dialog, which) -> {
                        String userInput = input.getText().toString();

                        // Вернуть Unity-приложение из фона (без перезапуска)
                        Intent intent = new Intent(activity, activity.getClass());
                        intent.addFlags(Intent.FLAG_ACTIVITY_REORDER_TO_FRONT | Intent.FLAG_ACTIVITY_SINGLE_TOP | Intent.FLAG_ACTIVITY_CLEAR_TOP);
                        activity.startActivity(intent);

                        UnityPlayer.UnitySendMessage(unityGameObject, unityMethod, userInput);
                    })
                    .setNegativeButton("Cancel", (dialog, which) -> {
                        Intent intent = new Intent(activity, activity.getClass());
                        intent.addFlags(Intent.FLAG_ACTIVITY_REORDER_TO_FRONT | Intent.FLAG_ACTIVITY_SINGLE_TOP | Intent.FLAG_ACTIVITY_CLEAR_TOP);
                        activity.startActivity(intent);

                        UnityPlayer.UnitySendMessage(unityGameObject, unityMethod, "");
                    })
                    .show();
        });
    }
}
