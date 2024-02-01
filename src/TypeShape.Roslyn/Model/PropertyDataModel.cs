﻿using Microsoft.CodeAnalysis;

namespace TypeShape.Roslyn;

/// <summary>
/// Represents the data model for either an instance property or an instance field.
/// </summary>
public readonly struct PropertyDataModel
{
    public PropertyDataModel(IPropertySymbol property)
    {
        PropertySymbol = property;
    }

    public PropertyDataModel(IFieldSymbol field)
    {
        PropertySymbol = field;
    }

    /// <summary>
    /// The declaring type of the property or field.
    /// </summary>
    public ITypeSymbol DeclaringType => PropertySymbol.ContainingType;

    /// <summary>
    /// Either an IPropertySymbol or an IFieldSymbol.
    /// </summary>
    public ISymbol PropertySymbol { get; }

    /// <summary>
    /// The declared name of the property or field.
    /// </summary>
    public string Name => PropertySymbol.Name;

    /// <summary>
    /// The type exposed by this property.
    /// </summary>
    public ITypeSymbol PropertyType => PropertySymbol switch
    {
        IPropertySymbol p => p.Type,
        var symbol => ((IFieldSymbol)symbol).Type,
    };

    /// <summary>
    /// Whether this model represents a field.
    /// </summary>
    public bool IsField => PropertySymbol is IFieldSymbol;

    /// <summary>
    /// Whether we can access the property getter.
    /// </summary>
    public required bool CanRead { get; init; }

    /// <summary>
    /// Whether we can access the property setter.
    /// </summary>
    public required bool CanWrite { get; init; }

    /// <summary>
    /// Whether the getter method output is non-nullable.
    /// </summary>
    public required bool IsGetterNonNullable { get; init; }

    /// <summary>
    /// Whether the setter method requires a non-nullable input.
    /// </summary>
    public required bool IsSetterNonNullable { get; init; }

    /// <summary>
    /// Whether the property or field is required.
    /// </summary>
    public bool IsRequired => PropertySymbol switch
    {
        IPropertySymbol p => p.IsRequired,
        var symbol => ((IFieldSymbol)symbol).IsRequired,
    };

    /// <summary>
    /// Whether the property is init-only.
    /// </summary>
    public bool IsInitOnly => PropertySymbol switch
    {
        IPropertySymbol p => p.SetMethod is { IsInitOnly: true },
        _ => false,
    };
}
