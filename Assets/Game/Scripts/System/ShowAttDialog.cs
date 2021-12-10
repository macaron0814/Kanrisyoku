using System.Runtime.InteropServices;

public class ShowAttDialog
{
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern int requestIDFA();
    //public staticにしているのd外部ファイルで「ShowAttDialog.RequestIDFA()」とすれば呼び出せます
    public static void RequestIDFA()
    {
        requestIDFA();
    }
#endif
}