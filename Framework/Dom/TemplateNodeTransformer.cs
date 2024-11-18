﻿namespace StardewUI.Framework.Dom;

/// <summary>
/// Transforms a <c>template</c> node based on the structure (attributes, children, etc.) of the instantiating node.
/// </summary>
/// <param name="template">The template node.</param>
public class TemplateNodeTransformer(SNode template) : INodeTransformer
{
    // Cache this because all transformers are checked in sequence, and the test for whether any individual transformer
    // should apply needs to be as fast as possible.
    private readonly string name =
        template.Attributes.FirstOrDefault(attr => attr.Name.Equals("name", StringComparison.OrdinalIgnoreCase))?.Value
        ?? "";

    /// <inheritdoc />
    public IReadOnlyList<SNode> Transform(SNode source)
    {
        if (!source.Tag.Equals(name))
        {
            return [source];
        }
        var attributes = source
            .Attributes.Where(attr => attr.Type == Grammar.AttributeType.Property)
            .ToDictionary(attr => attr.Name, StringComparer.InvariantCultureIgnoreCase);
        var namedOutletContents = source
            .ChildNodes.GroupBy(GetOutletName)
            .ToDictionary(
                g => g.Key,
                g => g.ToList() as IReadOnlyList<SNode>,
                StringComparer.InvariantCultureIgnoreCase
            );
        var defaultOutletContents = namedOutletContents.Remove("", out var nodes) ? nodes.ToList() : [];
        var result = template
            .ChildNodes.SelectMany(node =>
                TransformTemplateNode(node, attributes, defaultOutletContents, namedOutletContents)
            )
            .ToArray();
        return result;
    }

    private static IEnumerable<SNode> TransformTemplateNode(
        SNode templateNode,
        IReadOnlyDictionary<string, SAttribute> attributes,
        IReadOnlyList<SNode> defaultOutletContents,
        IReadOnlyDictionary<string, IReadOnlyList<SNode>> namedOutletContents
    )
    {
        if (templateNode.Tag.Equals("outlet", StringComparison.OrdinalIgnoreCase))
        {
            var nameAttribute = templateNode.Attributes.FirstOrDefault(attr =>
                attr.Name.Equals("name", StringComparison.OrdinalIgnoreCase)
            );
            if (nameAttribute is not null && nameAttribute.ValueType != Grammar.AttributeValueType.Literal)
            {
                Logger.LogOnce(
                    $"Ignoring <outlet> element '{templateNode.Element}' with non-literal 'name' value.",
                    LogLevel.Warn
                );
                return [];
            }
            return !string.IsNullOrEmpty(nameAttribute?.Value)
                ? namedOutletContents.GetValueOrDefault(nameAttribute.Value, [])
                : defaultOutletContents;
        }
        var transformedAttributes = templateNode
            .Attributes.Select(attr =>
                attr.ValueType == Grammar.AttributeValueType.TemplateBinding
                    ? attributes.TryGetValue(attr.Value, out var injectAttr)
                        ? injectAttr.WithName(attr.Name)
                        : null
                    : attr
            )
            .Where(attr => attr is not null)
            .Cast<SAttribute>()
            .ToArray();
        // TODO: Also transform events; event args could also accept template params, though the lexer/parser need to be
        // updated for this.
        var transformedEvents = templateNode.Element.Events;
        var transformedChildNodes = templateNode
            .ChildNodes.SelectMany(node =>
                TransformTemplateNode(node, attributes, defaultOutletContents, namedOutletContents)
            )
            .ToArray();
        var transformedElement = new SElement(templateNode.Tag, transformedAttributes, transformedEvents);
        return [new(transformedElement, transformedChildNodes)];
    }

    private static string GetOutletName(SNode node)
    {
        var attribute = node.Attributes.FirstOrDefault(attr =>
            attr.Type == Grammar.AttributeType.Structural
            && attr.Name.Equals("outlet", StringComparison.OrdinalIgnoreCase)
        );
        if (attribute is not null && attribute.ValueType != Grammar.AttributeValueType.Literal)
        {
            Logger.LogOnce(
                $"Ignoring *outlet attribute in node '{node.Element}' because it does not have a literal value.",
                LogLevel.Warn
            );
            return "";
        }
        return attribute?.Value ?? "";
    }
}