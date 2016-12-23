using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class ConfigFile {
    private Hashtable keyPairs = new Hashtable();
    private String iniFilePath;

    private struct SectionPair
    {
        public String Section;
        public String Key;
    }

    public ConfigFile(String iniPath)
    {
        TextReader iniFile = null;
        String strLine = null;
        String currentRoot = null;
        String[] keyPair = null;
        iniFilePath = iniPath;
        if (File.Exists(iniPath))
        {
            try
            {
                iniFile = new StreamReader(iniPath);
                strLine = iniFile.ReadLine();
                while (strLine != null)
                {
                    strLine = strLine.Trim();
                    if (strLine != "" && !strLine.StartsWith("#"))
                    {
                        if (strLine.StartsWith("[") && strLine.EndsWith("]"))
                        {
                            currentRoot = strLine.Substring(1, strLine.Length - 2);
                        }
                        else
                        {
                            keyPair = strLine.Split(new char[] { '=' }, 2);

                            SectionPair sectionPair;
                            String value = null;
                            if (currentRoot == null)
                                currentRoot = "ROOT";
                            sectionPair.Section = currentRoot.ToUpper();
                            sectionPair.Key = keyPair[0].ToUpper();
                            if (keyPair.Length > 1)
                                value = keyPair[1];
                            keyPairs.Add(sectionPair, value);
                        }
                    }
                    strLine = iniFile.ReadLine();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (iniFile != null)
                    iniFile.Close();
            }
        }
        else
            throw new FileNotFoundException("Unable to locate " + iniPath);
    }

    public ConfigFile(String iniPath, byte[] buffer)
    {
        TextReader iniFile = null;
        MemoryStream ms = null;
        String strLine = null;
        String currentRoot = null;
        String[] keyPair = null;
        iniFilePath = iniPath;
        if (buffer != null && buffer.Length > 0)
        {
            try
            {
                ms = new MemoryStream(buffer);
                ms.Seek(0, SeekOrigin.Begin);
                iniFile = new StreamReader(ms);
                strLine = iniFile.ReadLine();
                while (strLine != null)
                {
                    strLine = strLine.Trim();
                    if (strLine != "" && !strLine.StartsWith("#"))
                    {
                        if (strLine.StartsWith("[") && strLine.EndsWith("]"))
                        {
                            currentRoot = strLine.Substring(1, strLine.Length - 2);
                        }
                        else
                        {
                            keyPair = strLine.Split(new char[] { '=' }, 2);

                            SectionPair sectionPair;
                            String value = null;
                            if (currentRoot == null)
                                currentRoot = "ROOT";
                            sectionPair.Section = currentRoot.ToUpper();
                            sectionPair.Key = keyPair[0].ToUpper();
                            if (keyPair.Length > 1)
                                value = keyPair[1];
                            keyPairs.Add(sectionPair, value);
                        }
                    }
                    strLine = iniFile.ReadLine();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (iniFile != null)
                {
                    iniFile.Close();
                }
                if (ms != null)
                {
                    ms.Close();
                    ms = null;
                }
                if (iniFile != null)
                {
                    iniFile.Close();
                    iniFile = null;
                }
            }
        }
        else
            throw new FileNotFoundException("Unable to locate " + iniPath);
    }

    public String GetSetting(String sectionName, String settingName)
    {
        SectionPair sectionPair;
        sectionPair.Section = sectionName.ToUpper();
        sectionPair.Key = settingName.ToUpper();

        if (keyPairs.Contains(sectionPair))
        {
            return (String)keyPairs[sectionPair];
        }
        return String.Empty;
    }

    public String[] EnumSection(String sectionName)
    {
        ArrayList tmpArray = new ArrayList();

        foreach (SectionPair pair in keyPairs.Keys)
        {
            if (pair.Section == sectionName.ToUpper())
                tmpArray.Add(pair.Key);
        }

        return (String[])tmpArray.ToArray(typeof(String));
    }

    public void AddSetting(String sectionName, String settingName, String settingValue)
    {
        SectionPair sectionPair;
        sectionPair.Section = sectionName.ToUpper();
        sectionPair.Key = settingName.ToUpper();

        if (keyPairs.ContainsKey(sectionPair))
            keyPairs.Remove(sectionPair);

        keyPairs.Add(sectionPair, settingValue);
    }

    public void AddSetting(String sectionName, String settingName)
    {
        AddSetting(sectionName, settingName, null);
    }

    public void DeleteSetting(String sectionName, String settingName)
    {
        SectionPair sectionPair;
        sectionPair.Section = sectionName.ToUpper();
        sectionPair.Key = settingName.ToUpper();

        if (keyPairs.ContainsKey(sectionPair))
            keyPairs.Remove(sectionPair);
    }

    public void SaveSettings(String newFilePath)
    {
        ArrayList sections = new ArrayList();
        String tmpValue = "";
        String strToSave = "";

        foreach (SectionPair sectionPair in keyPairs.Keys)
        {
            if (!sections.Contains(sectionPair.Section))
                sections.Add(sectionPair.Section);
        }
        foreach (String section in sections)
        {
            strToSave += ("[" + section + "]\r\n");
            foreach (SectionPair sectionPair in keyPairs.Keys)
            {
                if (sectionPair.Section == section)
                {
                    tmpValue = (String)keyPairs[sectionPair];
                    if (tmpValue != null)
                        tmpValue = "=" + tmpValue;
                    strToSave += (sectionPair.Key + tmpValue + "\r\n");
                }
            }
            strToSave += "\r\n";
        }
        try
        {
            TextWriter tw = new StreamWriter(newFilePath);
            tw.Write(strToSave);
            tw.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void SaveSettings()
    {
        SaveSettings(iniFilePath);
    }
}
