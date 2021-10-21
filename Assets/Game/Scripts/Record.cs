using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Record
{
    public static void ClearRecord()
    {
        iOSRankingUtility.ReportProgress("first_run", 100.0f);
    }
}
