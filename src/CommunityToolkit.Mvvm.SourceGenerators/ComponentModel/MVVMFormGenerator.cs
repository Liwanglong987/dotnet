// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Mvvm.SourceGenerators.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CommunityToolkit.Mvvm.SourceGenerators.ComponentModel;

/// <summary>
/// 
/// </summary>
public class MVVMFormGenerator : IIncrementalGenerator
{
    const string formMetadataName = "CommunityToolkit.Mvvm.ComponentModel.Form";
    const string formAttributeMetadataName = "CommunityToolkit.Mvvm.ComponentModel.MVVMFormAttribute";

    /// <summary>
    /// 1. Get all attributed class
    /// 2. If it is not inherited from Form, report Disgnostics.
    /// 3. If it is, Generator will add a Generic Type DataContext 
    ///    function for it.
    /// </summary>
    /// <param name="context"></param>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // IncrementalValuesProvider<GeneratorAttributeSyntaxContext> classProvider = ;

        context.RegisterSourceOutput(
            context.SyntaxProvider
            .ForAttributeWithMetadataName(formAttributeMetadataName,
            (node, _) =>
            {
                return node is ClassDeclarationSyntax;
            },
            (context, token) =>
            {
                return context;
            }),

            static (context, classProvider) =>
            {
                AttributeData mvvmFormAttributeNode = classProvider.Attributes
                                                      .Single(attribute => attribute.AttributeClass != null &&
                                                              attribute.AttributeClass.HasFullyQualifiedMetadataName(formAttributeMetadataName));
                if (mvvmFormAttributeNode != default && classProvider.TargetSymbol is ITypeSymbol typeSymbol)
                {
                    string source = string.Empty;
                    INamedTypeSymbol? baseType = typeSymbol.BaseType;
                    while (baseType != null)
                    {
                        if (baseType.HasFullyQualifiedMetadataName(formMetadataName))
                        {
                            ISymbol targetSymbol = classProvider.TargetSymbol;
                            source = "namespace " + typeSymbol.ContainingNamespace.ToDisplayString()
                            + ";\r\npublic partial class " + typeSymbol.Name + "\r\n{\r\n    public object VMDataContext\r\n    {\r\n        get\r\n        {\r\n            return this.DataContext;\r\n        }\r\n        set\r\n        {\r\n            this.DataContext = value;\r\n        }\r\n    }\r\n}";

                            context.AddSource($"{typeSymbol.Name}.g.cs", source);
                            break;
                        }
                        else
                        {
                            baseType = baseType.BaseType;
                        }
                    }

                    if (source == string.Empty)
                    {
                        //should report error
                        //context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(,),, "attribute can only used on Form Class"));
                    }
                }
            });

    }
}
