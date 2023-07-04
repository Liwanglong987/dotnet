// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace CommunityToolkit.Mvvm.ComponentModel;

/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class MVVMFormAttribute<TVM> : Attribute where TVM : ObservableObject
{
    //private readonly Type vMType;
    ///// <summary>
    ///// construct
    ///// </summary>
    //public MVVMFormAttribute()
    //{
    //    this.vMType = typeof(TVM);
    //}
}
