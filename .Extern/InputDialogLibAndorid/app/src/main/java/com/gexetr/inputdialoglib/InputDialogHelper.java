package com.gexetr.inputdialoglib;

import android.app.Activity;
import android.content.Intent;

import com.unity3d.player.UnityPlayer;

public class InputDialogHelper {
    public static void ShowInputDialog() {
        Activity activity = UnityPlayer.currentActivity;
        Intent intent = new Intent(activity, InputOverlayActivity.class);
        activity.startActivity(intent);
    }
}
