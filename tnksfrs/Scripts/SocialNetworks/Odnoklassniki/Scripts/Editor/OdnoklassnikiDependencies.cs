﻿// <copyright file="GPGSDependencies.cs" company="Google Inc.">
// Copyright (C) 2015 Google Inc. All Rights Reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
#if UNITY_ANDROID

using Google.JarResolver;
using UnityEditor;

/// <summary>
/// Play-Services Dependencies for Google Play Games.
/// </summary>
[InitializeOnLoad]
public static class OdnoklassnikiDependencies
{
    /// <summary>
    /// The name of your plugin.  This is used to create a settings file
    /// which contains the dependencies specific to your plugin.
    /// </summary>
    private static readonly string PluginName = "Odnoklassniki";

    /// <summary>Instance of the PlayServicesSupport resolver</summary>
    public static PlayServicesSupport svcSupport;

    /// <summary>
    /// Initializes static members of the <see cref="SampleDependencies"/> class.
    /// </summary>
    static OdnoklassnikiDependencies()
    {
        svcSupport = PlayServicesSupport.CreateInstance(
                                            PluginName,
                                            EditorPrefs.GetString("AndroidSdkRoot"),
                                            "ProjectSettings");

        RegisterDependencies();
    }

    /// <summary>
    /// Registers the dependencies.
    /// </summary>
    public static void RegisterDependencies()
    {
        svcSupport.DependOn("com.google.android.gms", "play-services-ads", "9+");
    }
}
#endif
