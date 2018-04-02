package com.youxigu.df;

import android.os.Bundle;
import android.util.Log;
import android.widget.Toast;

import com.iflytek.cloud.InitListener;
import com.iflytek.cloud.RecognizerListener;
import com.iflytek.cloud.RecognizerResult;
import com.iflytek.cloud.SpeechConstant;
import com.iflytek.cloud.SpeechError;
import com.iflytek.cloud.SpeechSynthesizer;
import com.iflytek.cloud.SpeechUtility;
import com.iflytek.cloud.SpeechRecognizer;

import com.iflytek.cloud.SynthesizerListener;
import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

import org.json.JSONArray;
import org.json.JSONObject;
import org.json.JSONTokener;

public class MainActivity extends UnityPlayerActivity {

    public SpeechRecognizer speechRecognizer;
    public SpeechSynthesizer speechSynthesizer;
    private String ttsSpeakerName = "yefang";
    private String ttsSpeakerPitch = "50";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        //注意这里的appid为
        SpeechUtility.createUtility(getApplicationContext(),"appid=58880d30");

        initRecognizer();
    }

    //初始化
    private void initRecognizer(){
        speechRecognizer = SpeechRecognizer.createRecognizer(getApplicationContext(),mInitListener);

        speechSynthesizer = SpeechSynthesizer.createSynthesizer(getApplicationContext(),mInitListener);
    }

    public InitListener mInitListener = new InitListener() {
        @Override
        public void onInit(int i) {
            UnityPlayer.UnitySendMessage("Manager", "Result", "init success!");
        }
    };

    public void setTTSSpeaker(String targetName) {
        ttsSpeakerName = targetName;
    }

    public void setTTSPitch(String targetPitch) {
        ttsSpeakerPitch = targetPitch;
    }

    public void doTTS(String ttsStr){
        UnityPlayer.UnitySendMessage("MotionManager", "IsSpeaking", "true");

        //设置发音人
        speechSynthesizer.setParameter(SpeechConstant.VOICE_NAME,ttsSpeakerName);
        //设置音调
        speechSynthesizer.setParameter(SpeechConstant.PITCH,ttsSpeakerPitch);
        //设置音量
        speechSynthesizer.setParameter(SpeechConstant.VOLUME,"50");
        int code = speechSynthesizer.startSpeaking(ttsStr, mTTSListener);
    }

    private SynthesizerListener mTTSListener = new SynthesizerListener() {
        @Override
        public void onSpeakBegin() {

        }

        @Override
        public void onBufferProgress(int i, int i1, int i2, String s) {

        }

        @Override
        public void onSpeakPaused() {

        }

        @Override
        public void onSpeakResumed() {

        }

        @Override
        public void onSpeakProgress(int i, int i1, int i2) {

        }

        @Override
        public void onCompleted(SpeechError speechError) {
            UnityPlayer.UnitySendMessage("MotionManager", "IsSpeaking", "false");
        }

        @Override
        public void onEvent(int i, int i1, int i2, Bundle bundle) {

        }
    };

    //开始听写
    public void startSpeechListener(){

        UnityPlayer.UnitySendMessage("Manager", "Result", "");

        speechRecognizer.setParameter(SpeechConstant.DOMAIN, "iat");
        speechRecognizer.setParameter(SpeechConstant.LANGUAGE, "zh_cn");
        speechRecognizer.setParameter(SpeechConstant.ACCENT, "mandarin");
        speechRecognizer.startListening(mRecognizerListener);
    }

    public RecognizerListener mRecognizerListener = new RecognizerListener(){

        @Override
        public void onBeginOfSpeech() {
            // TODO Auto-generated method stub
            //UnityPlayer.UnitySendMessage("Manager", "Result", "onBeginOfSpeech");
        }

        @Override
        public void onEndOfSpeech() {
            // TODO Auto-generated method stub
            //UnityPlayer.UnitySendMessage("Manager", "Result", "onEndOfSpeech");
            //startSpeechListener();
            UnityPlayer.UnitySendMessage("Manager", "SpeechEnd","");
        }

        @Override
        public void onError(SpeechError arg0) {
            // TODO Auto-generated method stub
            //UnityPlayer.UnitySendMessage("Manager", "Result", "onError");
        }

        @Override
        public void onEvent(int arg0, int arg1, int arg2, Bundle arg3) {
            // TODO Auto-generated method stub
            //UnityPlayer.UnitySendMessage("Manager", "Result", "onEvent");
        }

        @Override
        public void onResult(RecognizerResult recognizerResult, boolean isLast) {
            //UnityPlayer.UnitySendMessage("Manager", "Result", "listener");
            printResult(recognizerResult);

            //if(isLast)
            //startSpeechListener();
        }

        @Override
        public void onVolumeChanged(int arg0, byte[] arg1) {
            //UnityPlayer.UnitySendMessage("Manager", "Result", "onVolumeChanged");
            // TODO Auto-generated method stub
        }
    };

    //解析
    private void printResult(RecognizerResult results) {
        String json = results.getResultString();

        StringBuffer ret = new StringBuffer();
        try {
            JSONTokener tokener = new JSONTokener(json);
            JSONObject joResult = new JSONObject(tokener);

            JSONArray words = joResult.getJSONArray("ws");
            for (int i = 0; i < words.length(); i++) {
                // 转写结果词，默认使用第一个结果
                JSONArray items = words.getJSONObject(i).getJSONArray("cw");
                JSONObject obj = items.getJSONObject(0);
                ret.append(obj.getString("w"));
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        //将解析结果“"result:" + ret.toString()”发送至“Manager”这个GameObject，中的“Result”函数
        UnityPlayer.UnitySendMessage("Manager", "Result", ret.toString());
    }

    public void ShowToast(final String mStr2Show){

        UnityPlayer.UnitySendMessage("Manager", "Result", "toast");

        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                Toast.makeText(getApplicationContext(),mStr2Show,Toast.LENGTH_LONG).show();
            }
        });
    }
}

