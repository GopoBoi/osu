﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osuTK;
using osuTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Graphics.Sprites;

namespace osu.Game.Graphics.UserInterface
{
    public class PageTabControl<T> : OsuTabControl<T>
    {
        protected override TabItem<T> CreateTabItem(T value) => new PageTabItem(value);

        public PageTabControl()
        {
            Height = 30;
        }

        public class PageTabItem : TabItem<T>
        {
            private const float transition_duration = 100;

            private readonly Box box;

            protected readonly SpriteText Text;

            protected Color4 BoxColour
            {
                get => box.Colour;
                set => box.Colour = value;
            }

            public PageTabItem(T value)
                : base(value)
            {
                AutoSizeAxes = Axes.X;
                RelativeSizeAxes = Axes.Y;

                Children = new Drawable[]
                {
                    Text = new OsuSpriteText
                    {
                        Margin = new MarginPadding { Top = 8, Bottom = 8 },
                        Origin = Anchor.BottomLeft,
                        Anchor = Anchor.BottomLeft,
                        Text = (value as Enum)?.GetDescription() ?? value.ToString(),
                        Font = OsuFont.GetFont(size: 14)
                    },
                    box = new Box
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 5,
                        Scale = new Vector2(1f, 0f),
                        Colour = Color4.White,
                        Origin = Anchor.BottomLeft,
                        Anchor = Anchor.BottomLeft,
                    },
                    new HoverClickSounds()
                };

                Active.BindValueChanged(active => Text.Font = Text.Font.With(Typeface.Exo, weight: active.NewValue ? FontWeight.Bold : FontWeight.Medium), true);
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                BoxColour = colours.Yellow;
            }

            protected override bool OnHover(HoverEvent e)
            {
                if (!Active.Value)
                    slideActive();
                return true;
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                if (!Active.Value)
                    slideInactive();
            }

            private void slideActive()
            {
                box.ScaleTo(new Vector2(1f), transition_duration);
            }

            private void slideInactive()
            {
                box.ScaleTo(new Vector2(1f, 0f), transition_duration);
            }

            protected override void OnActivated() => slideActive();

            protected override void OnDeactivated() => slideInactive();
        }
    }
}
