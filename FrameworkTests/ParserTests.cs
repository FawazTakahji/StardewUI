﻿using StardewUI.Framework.Dom;
using StardewUI.Framework.Grammar;

namespace StarML.Tests;

public class ParserTests
{
    public record TagExpectation(
        string Name,
        SAttribute[]? Attributes = null,
        SEvent[]? Events = null,
        bool IsClosingTag = false
    );

    public static TheoryData<string, TagExpectation[]> Data =>
        new()
        {
            {
                @"<lane orientation=""vertical"" align-content=""middle end"">
                    <image width={<ImageWidth} sprite={@Mods/focustense.StardewUITest/Sprites/Header} />
                    <label font=""dialogue"" text={{HeaderText}} />
                    <checkbox is-checked={<>Checked}/>
                </lane>",

                [
                    new("lane", [new("orientation", "vertical"), new("align-content", "middle end")]),
                    new(
                        "image",
                        [
                            new("width", "ImageWidth", ValueType: AttributeValueType.InputBinding),
                            new(
                                "sprite",
                                "Mods/focustense.StardewUITest/Sprites/Header",
                                ValueType: AttributeValueType.AssetBinding
                            ),
                        ]
                    ),
                    new("image", IsClosingTag: true),
                    new(
                        "label",
                        [new("font", "dialogue"), new("text", "HeaderText", ValueType: AttributeValueType.InputBinding)]
                    ),
                    new("label", IsClosingTag: true),
                    new("checkbox", [new("is-checked", "Checked", ValueType: AttributeValueType.TwoWayBinding)]),
                    new("checkbox", IsClosingTag: true),
                    new("lane", IsClosingTag: true),
                ]
            },
            {
                @"<label font=""small"" *repeat={<>Items} text={{DisplayName}} />",

                [
                    new(
                        "label",
                        [
                            new("font", "small"),
                            new("repeat", "Items", AttributeType.Structural, AttributeValueType.TwoWayBinding),
                            new("text", "DisplayName", ValueType: AttributeValueType.InputBinding),
                        ]
                    ),
                ]
            },
            {
                @"<textinput text={<>^Name} />",

                [
                    new(
                        "textinput",
                        [
                            new(
                                "text",
                                "Name",
                                ValueType: AttributeValueType.TwoWayBinding,
                                ContextRedirect: new ContextRedirect.Distance(1)
                            ),
                        ]
                    ),
                ]
            },
            {
                @"<label text={{<^^^Name}} />",

                [
                    new(
                        "label",
                        [
                            new(
                                "text",
                                "Name",
                                ValueType: AttributeValueType.InputBinding,
                                ContextRedirect: new ContextRedirect.Distance(3)
                            ),
                        ]
                    ),
                ]
            },
            {
                @"<label text={{<:^^Name}} />",

                [
                    new(
                        "label",
                        [
                            new(
                                "text",
                                "Name",
                                ValueType: AttributeValueType.OneTimeBinding,
                                ContextRedirect: new ContextRedirect.Distance(2)
                            ),
                        ]
                    ),
                ]
            },
            {
                @"<checkbox is-checked={>~Foo.Enabled} />",

                [
                    new(
                        "checkbox",
                        [
                            new(
                                "is-checked",
                                "Enabled",
                                ValueType: AttributeValueType.OutputBinding,
                                ContextRedirect: new ContextRedirect.Type("Foo")
                            ),
                        ]
                    ),
                ]
            },
            {
                @"<checkbox is-checked={:~Foo.Enabled} />",

                [
                    new(
                        "checkbox",
                        [
                            new(
                                "is-checked",
                                "Enabled",
                                ValueType: AttributeValueType.OneTimeBinding,
                                ContextRedirect: new ContextRedirect.Type("Foo")
                            ),
                        ]
                    ),
                ]
            },
            {
                @"<button click=|HandleClick(Foo, ""Bar"", ^^Baz, ~Quux.Abc)| />",

                [
                    new(
                        "button",
                        [],
                        [
                            new(
                                "click",
                                "HandleClick",
                                [
                                    new(ArgumentExpressionType.ContextBinding, "Foo"),
                                    new(ArgumentExpressionType.Literal, "Bar"),
                                    new(ArgumentExpressionType.ContextBinding, "Baz", new ContextRedirect.Distance(2)),
                                    new(ArgumentExpressionType.ContextBinding, "Abc", new ContextRedirect.Type("Quux")),
                                ]
                            ),
                        ]
                    ),
                ]
            },
            {
                @"<button click=|^^HandleClick(Foo, $Bar)| />",

                [
                    new(
                        "button",
                        [],
                        [
                            new(
                                "click",
                                "HandleClick",
                                [
                                    new(ArgumentExpressionType.ContextBinding, "Foo"),
                                    new(ArgumentExpressionType.EventBinding, "Bar"),
                                ],
                                new ContextRedirect.Distance(2)
                            ),
                        ]
                    ),
                ]
            },
            {
                @"<button click=|~Foo.HandleClick(Bar)| />",

                [
                    new(
                        "button",
                        [],
                        [
                            new(
                                "click",
                                "HandleClick",
                                [new(ArgumentExpressionType.ContextBinding, "Bar")],
                                new ContextRedirect.Type("Foo")
                            ),
                        ]
                    ),
                ]
            },
            // We don't test or need to test this in the Lexer, but it's useful to have one test combining both
            // attributes and event bindings to make sure the parser doesn't get confused about what the lexer is
            // emitting.
            {
                @"<checkbox layout=""stretch"" change=|HandleChange(""Foo"", Bar)| label-text={<>Baz} click=|^HandleClick()| />",

                [
                    new(
                        "checkbox",
                        [
                            new("layout", "stretch"),
                            new("label-text", "Baz", ValueType: AttributeValueType.TwoWayBinding),
                        ],
                        [
                            new(
                                "change",
                                "HandleChange",
                                [
                                    new(ArgumentExpressionType.Literal, "Foo"),
                                    new(ArgumentExpressionType.ContextBinding, "Bar"),
                                ]
                            ),
                            new("click", "HandleClick", [], new ContextRedirect.Distance(1)),
                        ]
                    ),
                ]
            },
        };

    [Theory]
    [MemberData(nameof(Data))]
    public void ParsedSyntax(string markup, TagExpectation[] tags)
    {
        var reader = new DocumentReader(markup);
        foreach (var tag in tags)
        {
            Assert.True(reader.NextTag());
            Assert.Equal(tag.Name, reader.Tag.Name.ToString());
            Assert.Equal(tag.IsClosingTag, reader.Tag.IsClosingTag);
            var attributeEnumerator = (tag.Attributes ?? []).AsEnumerable().GetEnumerator();
            var eventEnumerator = (tag.Events ?? []).AsEnumerable().GetEnumerator();
            TagMember memberType;
            while ((memberType = reader.NextMember()) != TagMember.None)
            {
                switch (memberType)
                {
                    case TagMember.Attribute:
                        Assert.True(attributeEnumerator.MoveNext());
                        var attribute = attributeEnumerator.Current;
                        Assert.Equal(attribute.Name, reader.Attribute.Name.ToString());
                        Assert.Equal(attribute.Value, reader.Attribute.Value.ToString());
                        Assert.Equal(attribute.ValueType, reader.Attribute.ValueType);
                        Assert.Equal(
                            attribute.ContextRedirect,
                            ContextRedirect.FromParts(reader.Attribute.ParentDepth, reader.Attribute.ParentType)
                        );
                        break;
                    case TagMember.Event:
                        Assert.True(eventEnumerator.MoveNext());
                        var @event = eventEnumerator.Current;
                        Assert.Equal(@event.Name, reader.Event.EventName.ToString());
                        Assert.Equal(@event.HandlerName, reader.Event.HandlerName.ToString());
                        Assert.Equal(
                            @event.ContextRedirect,
                            ContextRedirect.FromParts(reader.Event.ParentDepth, reader.Event.ParentType)
                        );
                        var argumentEnumerator = @event.Arguments.GetEnumerator();
                        while (true)
                        {
                            bool hasExpectedArgument = argumentEnumerator.MoveNext();
                            bool hasActualArgument = reader.NextArgument();
                            Assert.Equal(hasExpectedArgument, hasActualArgument);
                            if (!hasExpectedArgument)
                            {
                                break;
                            }
                            Assert.Equal(argumentEnumerator.Current.Type, reader.Argument.ExpressionType);
                            Assert.Equal(argumentEnumerator.Current.Expression, reader.Argument.Expression.ToString());
                            Assert.Equal(
                                argumentEnumerator.Current.ContextRedirect,
                                ContextRedirect.FromParts(reader.Argument.ParentDepth, reader.Argument.ParentType)
                            );
                        }
                        break;
                }
            }
        }
        Assert.True(reader.Eof);
    }

    [Fact]
    public void TestDocumentParsing()
    {
        string markup =
            @"<lane orientation=""vertical"" align-content=""middle end"">
                <image width={{<ImageWidth}} sprite={{@Mods/focustense.StardewUITest/Sprites/Header}} />
                <label font=""dialogue"" text={{HeaderText}} />
                <checkbox is-checked={{<>Checked}}/>
            </lane>";

        var document = Document.Parse(markup);

        Assert.Equal("lane", document.Root.Tag);
        Assert.Collection(
            document.Root.Attributes,
            attr => Assert.Equal(new("orientation", "vertical"), attr),
            attr => Assert.Equal(new("align-content", "middle end"), attr)
        );
        Assert.Collection(
            document.Root.ChildNodes,
            node =>
            {
                Assert.Equal("image", node.Tag);
                Assert.Collection(
                    node.Attributes,
                    attr => Assert.Equal(new("width", "ImageWidth", ValueType: AttributeValueType.InputBinding), attr),
                    attr =>
                        Assert.Equal(
                            new(
                                "sprite",
                                "Mods/focustense.StardewUITest/Sprites/Header",
                                ValueType: AttributeValueType.AssetBinding
                            ),
                            attr
                        )
                );
            },
            node =>
            {
                Assert.Equal("label", node.Tag);
                Assert.Collection(
                    node.Attributes,
                    attr => Assert.Equal(new("font", "dialogue"), attr),
                    attr => Assert.Equal(new("text", "HeaderText", ValueType: AttributeValueType.InputBinding), attr)
                );
            },
            node =>
            {
                Assert.Equal("checkbox", node.Tag);
                Assert.Collection(
                    node.Attributes,
                    attr =>
                        Assert.Equal(new("is-checked", "Checked", ValueType: AttributeValueType.TwoWayBinding), attr)
                );
            }
        );
    }
}
