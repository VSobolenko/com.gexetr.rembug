package com.gexetr.inputdialoglib;

import android.app.Activity;
import android.content.Intent;

import com.unity3d.player.UnityPlayer;

public class InputOverlayLauncher {
    public static void ShowInputOverlay(String gameObject, String positiveMethod, String negativeMethod) {
        Activity activity = UnityPlayer.currentActivity;
        Intent intent = new Intent(activity, OverlayInputDialogActivity.class);
        intent.putExtra("game_object", gameObject);
        intent.putExtra("callback_method_positive", positiveMethod);
        intent.putExtra("callback_method_negative", negativeMethod);
        activity.startActivity(intent);
    }
}
