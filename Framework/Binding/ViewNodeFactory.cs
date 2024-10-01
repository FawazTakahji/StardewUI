﻿using StardewUI.Framework.Converters;
using StardewUI.Framework.Dom;
using StardewUI.Framework.Sources;

namespace StardewUI.Framework.Binding;

/// <summary>
/// Default in-game view engine.
/// </summary>
/// <param name="viewFactory">Factory for creating views, based on their tag names.</param>
/// <param name="valueSourceFactory">The factory responsible for creating <see cref="IValueSource{T}"/> instances from
/// attribute data.</param>
/// <param name="valueConverterFactory">The factory responsible for creating
/// <see cref="IValueConverter{TSource, TDestination}"/> instances, used to convert bound values to the types required
/// by the target view or structural property.</param>
/// <param name="viewBinder">Binding service used to create <see cref="IViewBinding"/> instances that detect changes to
/// data or assets and propagate them to the bound <see cref="IView"/>.</param>
public class ViewNodeFactory(
    IViewFactory viewFactory,
    IValueSourceFactory valueSourceFactory,
    IValueConverterFactory valueConverterFactory,
    IViewBinder viewBinder
) : IViewNodeFactory
{
    public IViewNode CreateNode(SNode node)
    {
        var childNodes = node.ChildNodes.Select(CreateNode);
        var viewNode = new ViewNode(viewFactory, viewBinder, node.Element, childNodes);
        IViewNode result = viewNode;
        var structuralAttributes = StructuralAttributes.Get(node.Attributes);
        if (structuralAttributes.If is not null)
        {
            var condition = new UnaryCondition(valueSourceFactory, valueConverterFactory, structuralAttributes.If);
            result = new ConditionalNode(result, condition);
        }
        return result;
    }

    class StructuralAttributes
    {
        public IAttribute? Case { get; set; }
        public IAttribute? If { get; set; }
        public IAttribute? Repeat { get; set; }
        public IAttribute? Switch { get; set; }

        public static StructuralAttributes Get(IReadOnlyList<IAttribute> attributes)
        {
            var result = new StructuralAttributes();
            foreach (var attribute in attributes)
            {
                switch (attribute.Name)
                {
                    case "case":
                        result.Case = attribute;
                        break;
                    case "if":
                        result.If = attribute;
                        break;
                    case "repeat":
                        result.Repeat = attribute;
                        break;
                    case "switch":
                        result.Switch = attribute;
                        break;
                }
            }
            return result;
        }
    }
}
