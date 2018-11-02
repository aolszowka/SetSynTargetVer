// -----------------------------------------------------------------------
// <copyright file="SetSynTargetVer.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2018. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SetSynTargetVer
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public static class SetSynTargetVer
    {
        internal static XNamespace msbuildNS = @"http://schemas.microsoft.com/developer/msbuild/2003";

        public static void SetTargetRuntimeLevel(XDocument projXml, string targetRuntimeLevel)
        {
            // Find all TargetRuntimeLevel Tags
            IEnumerable<XElement> targetRuntimeLevelTags = projXml.Descendants(msbuildNS + "TargetRuntimeLevel");

            foreach (XElement targetRuntimeLevelTag in targetRuntimeLevelTags)
            {
                targetRuntimeLevelTag.Value = targetRuntimeLevel;
            }
        }

        public static void SetLanguageCompatibilityLevel(XDocument projXml, string languageCompatibilityLevel)
        {
            // Find all LanguageCompatibilityLevel Tags
            IEnumerable<XElement> languageCompatibilityLevelTags = projXml.Descendants(msbuildNS + "LanguageCompatibilityLevel");

            foreach (XElement languageCompatibilityLevelTag in languageCompatibilityLevelTags)
            {
                languageCompatibilityLevelTag.Value = languageCompatibilityLevel;
            }
        }

        internal static int ProcessDirectory(string targetDirectory, string targetRuntime, string languageCompatibilityLevel)
        {
            // Get All Synproj Files
            IEnumerable<string> synprojFiles = Directory.EnumerateFiles(targetDirectory, "*.synproj", SearchOption.AllDirectories);

            Parallel.ForEach(synprojFiles, synprojFile =>
            {
                XDocument projXml = XDocument.Load(synprojFile);

                // Make a string copy to see if anything changed; in theory
                // we could have used the "Changed" event but because of the
                // way we always reset the values we need to instead compare
                // the end result
                string deepProjXmlCopy = projXml.ToString();

                if (!string.IsNullOrWhiteSpace(targetRuntime))
                {
                    SetTargetRuntimeLevel(projXml, targetRuntime);
                }

                if (!string.IsNullOrWhiteSpace(languageCompatibilityLevel))
                {
                    SetLanguageCompatibilityLevel(projXml, languageCompatibilityLevel);
                }

                bool hasChanges = projXml.ToString() != deepProjXmlCopy;

                if (hasChanges)
                {
                    projXml.Save(synprojFile);
                }
            }
            );

            return 0;
        }
    }
}
