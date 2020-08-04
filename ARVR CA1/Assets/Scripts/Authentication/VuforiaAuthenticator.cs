using System.IO;
using UnityEngine;
using Vuforia;

/// <summary>
/// This script initializes the Vuforia license key during runtime.
/// This script is created to exclude the license key from version controls systems.
/// Specify the relative file path to the license key and remember to exclude it from version control
/// </summary>
namespace Authentication
{
    public class LicenseKey : ScriptableObject
    {
        /// The license key file path relative to Application.dataPath
        private const string LicenseKeyRelativePath = "/Secrets/vuforia";

        // Set the Vuforia License Key when the application is launched
        [RuntimeInitializeOnLoadMethod]
        private static void SetLicenseKey()
        {
            var config = VuforiaConfiguration.Instance.Vuforia;
            config.DelayedInitialization = true;
            config.LicenseKey = ReceiveLicenseKey();
        }

        private static string ReceiveLicenseKey()
        { 
            string licenseFilePath = string.Concat(Application.dataPath, LicenseKeyRelativePath);

            return File.ReadAllText(licenseFilePath);
        }
    }
}