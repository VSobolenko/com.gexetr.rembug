package com.gexetr.inputdialoglib;

import android.app.Activity;
import android.app.AlertDialog;
import android.os.Bundle;
import android.view.WindowManager;
import android.widget.EditText;

import com.unity3d.player.UnityPlayer;

public class InputOverlayActivity extends Activity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        getWindow().setFlags(
                WindowManager.LayoutParams.FLAG_NOT_TOUCH_MODAL,
                WindowManager.LayoutParams.FLAG_NOT_TOUCH_MODAL
        );
        getWindow().setBackgroundDrawableResource(android.R.color.transparent);

        final EditText input = new EditText(this);
        input.setText("192.168.");

        new AlertDialog.Builder(this)
                .setTitle("IP Configuration")
                .setMessage("Enter server ip address:")
                .setView(input)
                .setCancelable(false)
                .setPositiveButton("OK", (dialog, which) -> {
                    String userInput = input.getText().toString();
                    UnityPlayer.UnitySendMessage("Gexetr.RemBug.LogRemote", "ReceivePositiveMessage", userInput);
                    finish();
                })
                .setNegativeButton("Cancel", (dialog, which) -> {
                    String userInput = input.getText().toString();
                    //UnityPlayer.UnitySendMessage("Gexetr.RemBug.LogRemote", "ReceiveNegativeMessage", userInput);
                    finish();
                })
                .show();
    }
}
