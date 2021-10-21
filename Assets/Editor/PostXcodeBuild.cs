using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.Collections.Generic;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

public class PostXcodeBuild
{
#if UNITY_IOS
    private struct InfoplistInfo
    {
        public string key;
        public string value;
        public InfoplistInfo(string str1, string str2)
        {
            key = str1;
            value = str2;
        }
    };

    private struct LocalizationInfo
    {
        public string lang;
        public bool isdefault;
        public InfoplistInfo[] infoplist;
        public LocalizationInfo(string langstr, bool flg, InfoplistInfo[] info)
        {
            lang = langstr;
            infoplist = info;
            isdefault = flg;
        }
    };
    private struct CommonInfoPlistInfo
    {
        public InfoplistInfo[] infoplist;
        public CommonInfoPlistInfo(InfoplistInfo[] info)
        {
            infoplist = info;
        }
    };

    private static LocalizationInfo[] localizationInfo = {
        new LocalizationInfo("en", false, new InfoplistInfo[]
        {new InfoplistInfo("CFBundleDisplayName",            "Origami STG"),
         new InfoplistInfo("NSUserTrackingUsageDescription", "Please set to Allow to avoid displaying inappropriate advertisements"),
         //new InfoplistInfo("NSCameraUsageDescription",       "Please set to Allow to use camera for displaying preview and getting camera parameters")
        }),
        new LocalizationInfo("ja", true, new InfoplistInfo[]
        {new InfoplistInfo("CFBundleDisplayName",            "走れ！中間管理職"),
         new InfoplistInfo("NSUserTrackingUsageDescription", "不適切な広告の表示を避けるために”トラッキングを許可”に設定してください"),
         //new InfoplistInfo("NSCameraUsageDescription",       "カメラのプレビュー表示とパラメータ取得のためにカメラ使用を許可設定にしてください。")
        })
    };


    private static string[] skadnetworkitems = new string[] { "cstr6suwn9.skadnetwork" };


    static void createInfoPlistString(string pjdirpath, LocalizationInfo localizationinfo)
    {
        string dirpath = Path.Combine(pjdirpath, "Unity-iPhone Tests");

        if (!Directory.Exists(Path.Combine(dirpath, string.Format("{0}.lproj", localizationinfo.lang))))
        {
            Directory.CreateDirectory(Path.Combine(dirpath, string.Format("{0}.lproj", localizationinfo.lang)));
        }
        string plistpath = Path.Combine(dirpath, string.Format("{0}.lproj/InfoPlist.strings", localizationinfo.lang));
        StreamWriter w = new StreamWriter(plistpath, false);
        foreach (InfoplistInfo info in localizationinfo.infoplist)
        {
            string convertedval = System.Text.Encoding.UTF8.GetString(
                System.Text.Encoding.Convert(
                    System.Text.Encoding.Unicode,
                    System.Text.Encoding.UTF8,
                    System.Text.Encoding.Unicode.GetBytes(info.value)
                    )
            );
            w.WriteLine(string.Format(info.key + " = \"{0}\";", convertedval));
        }
        w.Close();
    }

    static void addknownRegions(string pjdirpath, LocalizationInfo[] info)
    {
        string strtmp = "";
        string pjpath = PBXProject.GetPBXProjectPath(pjdirpath);

        foreach (LocalizationInfo infotmp in info)
        {
            strtmp += "\t\t" + infotmp.lang + ",\n";
        }
        strtmp += "\t\t);\n";

        StreamReader r = new StreamReader(pjpath);
        string prjstr = "";
        string linetmp = "";
        while (r.Peek() >= 0)
        {
            linetmp = r.ReadLine();
            if (linetmp.IndexOf("knownRegions") != -1)
            {
                prjstr += linetmp + "\n";
                prjstr += strtmp;
                while (true)
                {
                    linetmp = r.ReadLine();
                    if (linetmp.IndexOf(");") != -1)
                    {
                        break;
                    }
                }
            }
            else
            {
                prjstr += linetmp + "\n";
            }
        }
        r.Close();
        StreamWriter sw = new StreamWriter(pjpath, false);
        sw.Write(prjstr);
        sw.Close();
    }

    static void addLocalizationInfoPlist(string pjdirpath, LocalizationInfo[] info)
    {
        string plistPath = Path.Combine(pjdirpath, "Info.plist");
        PlistDocument plist = new PlistDocument();

        plist.ReadFromFile(plistPath);
        var array = plist.root.CreateArray("CFBundleLocalizations");
        foreach (LocalizationInfo infotmp in info)
        {
            array.AddString(infotmp.lang);
        }
        var rootDict = plist.root;
        foreach (LocalizationInfo infotmp in info)
        {
            if (infotmp.isdefault)
            {
                foreach (InfoplistInfo pinfo in infotmp.infoplist)
                {
                    string convertedval = System.Text.Encoding.UTF8.GetString(
                        System.Text.Encoding.Convert(
                            System.Text.Encoding.Unicode,
                            System.Text.Encoding.UTF8,
                            System.Text.Encoding.Unicode.GetBytes(pinfo.value)
                    ));
                    rootDict.SetString(pinfo.key, convertedval);
                }
            }
        }
        plist.WriteToFile(plistPath);
    }

    static void addSkAdNetworkItems(string pjdirpath, string[] skadvallist)
    {
        string plistPath = Path.Combine(pjdirpath, "Info.plist");
        PlistDocument plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        if (skadvallist != null)
        {
            var array = plist.root.CreateArray("SKAdNetworkItems");
            foreach (string value in skadvallist)
            {
                PlistElementDict dict = array.AddDict();
                dict.SetString("SKAdNetworkIdentifier", value);
            }
        }
        plist.WriteToFile(plistPath);
    }

    static void addAppTrackingTransparency(string pathToBuiltProject)
    {
        // PBXProjectクラスというのを用いてAppTrackingTransparency.frameworkを追加していきます（ステップ３）
        string pbxProjectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
        PBXProject pbxProject = new PBXProject();
        pbxProject.ReadFromFile(pbxProjectPath);
        string targetGuid = pbxProject.GetUnityFrameworkTargetGuid();
        pbxProject.AddFrameworkToProject(targetGuid, "AppTrackingTransparency.framework", true);
        pbxProject.WriteToFile(pbxProjectPath);
    }

    static void addEnableBitcode(string pathToBuiltProject)
    {
        var projectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);

        var project = new PBXProject();
        project.ReadFromFile(projectPath);

        var target = project.TargetGuidByName("Unity-iPhone");
        project.SetBuildProperty(target, "ENABLE_BITCODE", "NO");

        project.WriteToFile(projectPath);
    }

    static void addNCMBFramework(string pathToBuiltProject)
    {
        string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
        PBXProject proj = new PBXProject();

        proj.ReadFromString(File.ReadAllText(projPath));
        string target = proj.GetUnityFrameworkTargetGuid();

        // システムのフレームワークを追加
        proj.AddFrameworkToProject(target, "WebKit.framework", true);
        proj.AddFrameworkToProject(target, "UserNotifications.framework", true);
        proj.AddFrameworkToProject(target, "AuthenticationServices.framework", true);

        //// Yosemiteでipaが書き出せないエラーに対応するための設定
        //proj.SetBuildProperty(target, "CODE_SIGN_RESOURCE_RULES_PATH", "$(SDKROOT)/ResourceRules.plist");

        //// フレームワークの検索パスを設定・追加
        //proj.SetBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");
        //proj.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(PROJECT_DIR)/Frameworks");

        // 書き出し
        proj.WriteToFile(projPath);
    }

    [PostProcessBuild]
    public static void SetXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget != BuildTarget.iOS)
        {
            return;
        }

        foreach (LocalizationInfo entry in localizationInfo)
        {
            /* add infoplist.string */
            createInfoPlistString(pathToBuiltProject, entry);
        }

        /* add knownregions to project */
        addknownRegions(pathToBuiltProject, localizationInfo);

        /* add localization to infoplist */
        addLocalizationInfoPlist(pathToBuiltProject, localizationInfo);

        /* add addSkAdNetworkItems */
        addSkAdNetworkItems(pathToBuiltProject, skadnetworkitems);

        /* add AppTrackingTransparency */
        addAppTrackingTransparency(pathToBuiltProject);

        /* add Enable Bitcode */
        //addEnableBitcode(pathToBuiltProject);

        /* add NCMBFramework*/
        //addNCMBFramework(pathToBuiltProject);
    }
#endif
}